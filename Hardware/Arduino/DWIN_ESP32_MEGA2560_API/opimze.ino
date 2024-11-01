#include <Arduino.h>
#include <Dwin2.h>
#include <WiFi.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>
#include <FreeRTOS.h>
#include <task.h>

#define RX_PIN 16
#define TX_PIN 17
#define RXD3 19
#define TXD3 20

DWIN2 dwc;

// Khai báo các biến trạng thái của cảm biến và đếm rác từng loại
int sensorStatusBinMetal = 0, sensorStatusBinPlastic = 0, sensorStatusBinOther = 0;
int countMetalTrash = 0, countPlasticTrash = 0, countOtherTrash = 0;
int isPutTrash = 0, isEndTrash = 0;
bool offlineMode = true; // Chế độ offline

// Thông tin kết nối WiFi và API
const char* ssid = "HomeLCTM";
const char* password = "1@qweQAZ";
const char* apiUrlTokenAuth = "https://app.sbin.edu.vn/api/TokenAuth/Authenticate";
const char* apiUrlTransactionBins = "https://app.sbin.edu.vn/api/services/app/TransactionBins/CreateDevice_TransactionBins";
const char* apiUrlUpdateDeviceForBin = "https://app.sbin.edu.vn/api/services/app/Devices/EditStatusBinTrashDevice";

// Biến lưu trữ thông tin token và thời gian refresh token
unsigned long previousMillis = 0, interval = 0;
struct TokenData {
    String accessToken;
    String refreshToken;
    long expireInSeconds;
};

TokenData _tokenData = {"", "", 0};

// Cấu hình khởi tạo
void setup() {
    Serial.begin(115200);
    dwc.begin(); // Khởi tạo giao tiếp với màn hình DWIN
    dwc.setUartCbHandler(dwinEchoCallback); // Cài đặt callback xử lý dữ liệu từ DWIN
    dwc.setEcho(false);
    Serial2.begin(9600, SERIAL_8N1, RXD3, TXD3);

    // Nếu không ở chế độ offline, tạo task kết nối WiFi
    if (!offlineMode) {
        xTaskCreate(WiFiTask, "WiFiTask", 2048, NULL, 1, NULL);
    }

    // Tạo các task xử lý trang và quản lý token
    xTaskCreate(PageProcessingTask, "PageProcessingTask", 2048, NULL, 1, NULL);
    xTaskCreate(TokenManagementTask, "TokenManagementTask", 2048, NULL, 1, NULL);

    vTaskStartScheduler(); // Khởi động bộ lập lịch FreeRTOS
}

void loop() {
    // Không cần làm gì trong loop, các task đã được FreeRTOS xử lý
}

// Task kết nối WiFi
void WiFiTask(void *pvParameters) {
    connectWiFi();
    vTaskDelete(NULL); // Xóa task khi không còn cần thiết
}

// Task xử lý trang
void PageProcessingTask(void *pvParameters) {
    while (true) {
        uint8_t pageNum = dwc.getPage();
        processPage(pageNum); // Xử lý nội dung trang hiện tại
        vTaskDelay(200 / portTICK_PERIOD_MS); // Delay 200ms
    }
}

// Task quản lý token
void TokenManagementTask(void *pvParameters) {
    while (true) {
        if (!offlineMode) {
            refreshToken(); // Refresh token nếu không ở chế độ offline
        }
        vTaskDelay(1000 / portTICK_PERIOD_MS); // Delay 1s
    }
}

// Kết nối WiFi
void connectWiFi() {
    Serial.print("Connecting to WiFi...");
    WiFi.begin(ssid, password);
    while (WiFi.status() != WL_CONNECTED) {
        vTaskDelay(500 / portTICK_PERIOD_MS); // Delay 500ms
        Serial.print(".");
    }
    Serial.println("Connected to WiFi.");
    _tokenData = getToken(); // Lấy token khi đã kết nối WiFi
    interval = _tokenData.expireInSeconds * 1000;
    previousMillis = millis();
}

// Refresh token khi cần
void refreshToken() {
    unsigned long currentMillis = millis();
    if (currentMillis - previousMillis >= interval || _tokenData.accessToken.isEmpty()) {
        previousMillis = currentMillis;
        _tokenData = getToken();
        interval = _tokenData.expireInSeconds * 1000;
        Serial.println("GETTING new token...");
        Serial.println(_tokenData.accessToken);
        Serial.println(_tokenData.expireInSeconds);
    }
}

// Xử lý các trang dựa vào số trang hiện tại
void processPage(int pageNum) {
    String command = Serial2.readStringUntil('\r');
    switch (pageNum) {
        case 1:
            updateBinStatusDisplay();
            break;
        case 2:
            handlePageTwo(command);
            break;
        case 3:
            handleCountBinTrash(command);
            break;
        case 4:
            finalizeTrashProcess();
            break;
        case 5:
            startPageCountdown(5);
            break;
        default:
            resetTrashCounts();
            break;
    }
}

// Reset đếm rác và trạng thái các thùng
void resetTrashCounts() {
    countMetalTrash = 0;
    countPlasticTrash = 0;
    countOtherTrash = 0;
    isPutTrash = 0;
    isEndTrash = 0;

    sensorStatusBinMetal = 0;
    sensorStatusBinPlastic = 0;
    sensorStatusBinOther = 0;

    updateBinStatusDisplay(); // Cập nhật hiển thị trạng thái thùng rác

    Serial.println("Trash counts and bin statuses have been reset.");
}

// Thực hiện đếm ngược cho một trang
void startPageCountdown(int countdownTime) {
    static unsigned long lastUpdateTime = 0;
    static int remainingTime = countdownTime;

    unsigned long currentMillis = millis();

    if (currentMillis - lastUpdateTime >= 1000) {
        lastUpdateTime = currentMillis;

        dwc.setAddress(0x5010, 0x1510);
        dwc.setUiType(INT);
        dwc.sendData(remainingTime);

        remainingTime--;

        if (remainingTime < 0) {
            dwc.setPage(0);
            remainingTime = countdownTime;
        }
    }
}

// Kết thúc quy trình xử lý rác
void finalizeTrashProcess() {
    if (!isEndTrash) {
        Serial2.println("Cmd_KetThucQuyTrinh");
        createTransaction(countPlasticTrash, countMetalTrash, countOtherTrash);
        isEndTrash = 1;
    }
}

// Tạo giao dịch rác khi kết nối WiFi thành công
void createTransaction(int plasticQuantity, int metalQuantity, int otherQuantity) {
    if (WiFi.status() == WL_CONNECTED && !offlineMode) {
        HTTPClient http;
        http.begin(apiUrlTransactionBins);

        String tokenHeader = "Bearer " + _tokenData.accessToken;
        http.addHeader("Content-Type", "application/json");
        http.addHeader("accept", "text/plain");
        http.addHeader("Authorization", tokenHeader);

        StaticJsonDocument<256> jsonDoc;
        jsonDoc["plasticQuantity"] = plasticQuantity;
        jsonDoc["metalQuantity"] = metalQuantity;
        jsonDoc["otherQuantity"] = otherQuantity;
        jsonDoc["deviceId"] = deviceID;
        jsonDoc["transactionStatusId"] = transactionStatusId;

        String jsonPayload;
        serializeJson(jsonDoc, jsonPayload);

        int httpResponseCode = http.POST(jsonPayload);

        if (httpResponseCode > 0) {
            String response = http.getString();
            Serial.println("Server Response: " + response);

            StaticJsonDocument<256> responseDoc;
            if (deserializeJson(responseDoc, response)) {
                Serial.println("Error parsing JSON response");
            } else {
                bool success = responseDoc["success"].as<bool>();
                String QRData = responseDoc["result"].as<String>();

                if (success) {
                    Serial.println("Transaction created successfully.");
                    setQRCode(QRData);
                } else {
                    Serial.println("Transaction creation failed.");
                }
            }
        } else {
            Serial.print("Connection error: ");
            Serial.println(httpResponseCode);
        }
        http.end();
    } else {
        handleOfflineMode();
    }
}

// Đặt mã QR trên màn hình DWIN
void setQRCode(const String &qrData) {
    dwc.setAddress(0x9910, 0x1099);
    dwc.setUiType(ASCII);
    dwc.sendData(qrData.isEmpty() ? "999" : qrData);
}

// Chế độ offline khi không có kết nối
void handleOfflineMode() {
    Serial.println("Offline mode active, transaction not sent.");
    setQRCode("999");
    startPageCountdown(10);
}

// Xử lý trang 2 theo lệnh nhận được
void handlePageTwo(const String &command) {
    if (sensorStatusBinMetal && sensorStatusBinPlastic && sensorStatusBinOther)
        dwc.setPage(0);
    if (!isPutTrash && command.startsWith("Cmd_BatDauQuyTrinh"))
        startTrashProcess();
    if (command.startsWith("Cmd_CountRac"))
        handleCountBinTrash(command);
    if (command.startsWith("Cmd_KetThucQuyTrinh"))
        endTrashProcess();
}

// Xử lý đếm số lượng rác
void handleCountBinTrash(const String &command) {
    String values[4];
    parseCommand(command, values, ";");
    countMetalTrash = updateTrashCount(0x3010, values[1]);
    countPlasticTrash = updateTrashCount(0x3020, values[2]);
    countOtherTrash = updateTrashCount(0x3030, values[3]);
}

int updateTrashCount(uint16_t address, const String &value) {
    int count = value.toInt();
    dwc.setAddress(address, address + 0x1000);
    dwc.setUiType(INT);
    dwc.sendData(count);
    return count;
}

void endTrashProcess() {
    Serial2.println("Cmd_KetThucQuyTrinh");
    if (!countMetalTrash && !countPlasticTrash && !countOtherTrash)
        dwc.setPage(5);
    else
        dwc.setPage(4);
}

// Cập nhật trạng thái thùng rác trên màn hình
void updateBinStatusDisplay() {
    setBinStatus(0x9010, sensorStatusBinMetal);
    setBinStatus(0x9020, sensorStatusBinPlastic);
    setBinStatus(0x9030, sensorStatusBinOther);
}

void setBinStatus(uint16_t address, int status) {
    dwc.setAddress(address, address + 0x1000);
    dwc.setUiType(ASCII);
    dwc.setColor(status ? 0xF9C5 : 0x058E);
    dwc.sendData(status ? "Full" : "Available");
}

TokenData getToken() {
    TokenData tokenData;
    HTTPClient http;
    http.begin(apiUrlTokenAuth);
    http.addHeader("Content-Type", "application/json");
    String jsonPayload = "{\"userNameOrEmailAddress\":\"linhtrungsbin01\",\"password\":\"123qwe\"}";
    int responseCode = http.POST(jsonPayload);
    if (responseCode > 0) {
        String response = http.getString();
        StaticJsonDocument<256> doc;
        if (!deserializeJson(doc, response)) {
            tokenData.accessToken = doc["result"]["accessToken"].as<String>();
            tokenData.expireInSeconds = doc["result"]["expireInSeconds"].as<long>();
        }
    }
    http.end();
    return tokenData;
}

// Callback xử lý dữ liệu từ DWIN
void dwinEchoCallback(DWIN2 &d) {
    // Thực hiện logic callback
}
