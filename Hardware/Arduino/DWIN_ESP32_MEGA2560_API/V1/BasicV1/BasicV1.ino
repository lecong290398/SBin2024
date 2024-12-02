#include <Arduino.h>
#include <Dwin2.h>
#include <WiFi.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>
#include <AESLib.h>
#include <Base64.h>
#pragma region Constants ESP32_DWIN_MEGA2056
// Rx Tx ESP gpio connected to DWin Display
#define RX_PIN 16
#define TX_PIN 17

// Rx Tx ESP gpio connected to MEGA 2560
#define RXD3 19
#define TXD3 20

// Class for controlling UI elements of the display
DWIN2 dwc;

// Status Bin Trash
int sensorStatusBinMetal = 0; // 1 FULL || 0 Available || -1 Lỗi
int sensorStatusBinPlastic = 0;
int sensorStatusBinOther = 0;

// count Trash
int countMetalTrash = 0;
int countPlasticTrash = 0;
int countOtherTrash = 0;

// Lenh HMI gui xuong
#define Cmd_BatDauQuyTrinh "Cmd_BatDauQuyTrinh"
#define Cmd_KetThucQuyTrinh "Cmd_KetThucQuyTrinh" // Dung gui len/gui xuong

// Lenh gui HMI
#define Cmd_CountRac "Cmd_CountRac"
#define Cmd_DayRacKimLoai "Cmd_DayRacKimLoai"
#define Cmd_DayRacNhua "Cmd_DayRacNhua"
#define Cmd_DayRacKhongXacDinh "Cmd_DayRacKhongXacDinh"

int isPutTrash = false;
int isEndTrash = false;
// Callback function to receive a response from the display
void dwinEchoCallback(DWIN2 &d);

#pragma endregion

#pragma region Constants API
// Thông tin WiFi
const char *ssid = "LeCong";        // Tên WiFi
const char *password = "123123123"; // Mật khẩu WiFi
const char *deviceID = "1";         // ID của thiết bị
// Địa chỉ URL API
const char *apiUrlTokenAuth = "https://app.sbin.edu.vn/api/TokenAuth/Authenticate";                                          // URL API lấy token
const char *apiUrlTransactionBins = "https://app.sbin.edu.vn/api/services/app/TransactionBins/CreateDevice_TransactionBins"; // URL API tạo transaction bins
const char *apiUrlUpdateDeviceForBin = "https://app.sbin.edu.vn/api/services/app/Devices/EditStatusBinTrashDevice";          // URL API cập nhật trạng thái thiết bị
// Thông tin user
const char *user = "linhtrungsbin01";       // Tên đăng nhập
const char *pass = "123qwe";                // Mật khẩu
unsigned long previousMillis = 0;           // Lưu trữ thời gian lần cuối hàm GetToken được gọi
unsigned long interval = 0;                 // Khoảng thời gian giữa các lần lấy token (tính bằng mili giây)
unsigned long CallAPICreateTransaction = 0; // Lưu trữ thời gian lần cuối hàm CreateDeviceTransactionBins được gọi
unsigned long transactionStatusId = 1;
bool isProcessing = false;        // Prevent simultaneous tasks
bool isOffline = false;           // Current mode of operation (true: offline, false: online)
unsigned long timeoutWifi = 1500; // 3 seconds timeout
// Object chứa token
struct TokenData
{
    String accessToken;
    String refreshToken;
    long expireInSeconds;
};
TokenData _tokenData; // Default constructor
#pragma endregion

void setup()
{
    Serial.begin(115200);
    Serial.printf("-------- Start DWIN communication RUN --------\n");
    Serial.printf("-----------------------------------------------\n");
    // Initialize timers, tasks, serial communication, and other settings
    dwc.begin();
    // Set callback for receiving responses
    dwc.setUartCbHandler(dwinEchoCallback);
    // Disable echo for commands and responses
    dwc.setEcho(false);
    Serial.printf("-------- Start Mega 2560 communication RUN --------\n");
    Serial.printf("-----------------------------------------------\n");
    // Initialize Serial2 for communication with the display broad control MEGA 2650
    Serial2.begin(9600, SERIAL_8N1, RXD3, TXD3);
    // Connect to Wi-Fi and set mode to offline if failed and get token from server if connected to Wi-Fi successfully
    checkConnection();
}

void loop()
{
    // Gọi hàm GetToken
    HandlerToken();
    // Get the current page number
    uint8_t pageNum = dwc.getPage();
    // Process the current page
    processPage(pageNum);
    // Delay for a while
    delay(200);
}

// -------------------------------------------------------------------------------------------------------------------

#pragma region DWIN CONTROL
// Function to handle common operations for pages 4 and 5
void handlePage4or5(uint16_t address1, uint16_t address2, int idPage, int time)
{
    dwc.setAddress(address1, address2);
    dwc.setUiType(INT);
    dwc.setStartVal(20);
    dwc.setLimits(0, time, false);
    for (int i = time; i >= 0; i--)
    {
        dwc.sendData(i);
        delay(1000);
        if (i == 1)
        {
            dwc.setPage(idPage);
        }
    }
}

// Function to handle the status of the trash bin
void handlerStatusBinTrash(const String &commandString)
{
    if (commandString.startsWith(Cmd_DayRacKimLoai))
    {
        Serial.println("GET Cmd_DayRacKimLoai");
        sensorStatusBinMetal = 1;
    }
    else if (commandString.startsWith(Cmd_DayRacNhua))
    {
        Serial.println("GET Cmd_DayRacNhua");
        sensorStatusBinPlastic = 1;
    }
    else if (commandString.startsWith(Cmd_DayRacKhongXacDinh))
    {
        Serial.println("GET Cmd_DayRacKhongXacDinh");
        sensorStatusBinOther = 1;
    }
    else
    {
        Serial.println("Unrecognized command.");
        return; // Exit if no command matches
    }

    // Update bin statuses
    updateBinStatus(sensorStatusBinPlastic, sensorStatusBinMetal, sensorStatusBinOther);
}

// Function to handle count of the trash bin
void handlerCountBinTrash(String commandString)
{
    if (commandString.startsWith(Cmd_CountRac))
    {
        commandString.trim(); // Loại bỏ ký tự xuống dòng "\n" ở cuối chuỗi
        // Chia chuỗi theo dấu ';'
        String values[4]; // Mảng lưu các giá trị sau khi chia
        int index = 0;
        // Lặp qua chuỗi và chia bằng ký tự ';'
        int startIndex = 0;
        int endIndex = commandString.indexOf(';');
        while (endIndex != -1)
        {
            values[index++] = commandString.substring(startIndex, endIndex);
            startIndex = endIndex + 1;
            endIndex = commandString.indexOf(';', startIndex);
        }
        // Thêm giá trị cuối cùng sau dấu chấm phẩy
        values[index] = commandString.substring(startIndex);
        // In kết quả
        for (int i = 0; i < 4; i++)
        {
            Serial.print("Value at index ");
            Serial.print(i);
            Serial.print(": ");
            Serial.println(values[i]);
            // Xử lý giá trị
            if (i == 1)
            {
                dwc.setAddress(0x3010, 0x1310);
                dwc.setUiType(INT);
                int countMetal = values[i].toInt();
                dwc.sendData(countMetal);
                countMetalTrash = countMetal;
            }
            if (i == 2)
            {
                dwc.setAddress(0x3020, 0x1320);
                dwc.setUiType(INT);
                int countPlastic = values[i].toInt();
                dwc.sendData(countPlastic);
                countPlasticTrash = countPlastic;
            }
            if (i == 3)
            {
                dwc.setAddress(0x3030, 0x1330);
                dwc.setUiType(INT);
                int countOther = values[i].toInt();
                dwc.sendData(countOther);
                countOtherTrash = countOther;
            }
        }
    }
}
#pragma endregion

// -------------------------------------------------------------------------------------------------------------------

#pragma region Status

void setStatusMetal()
{
    // Set address and send ASCII data for page 1
    dwc.setAddress(0x9010, 0x1010);
    dwc.setUiType(ASCII);
    if (sensorStatusBinMetal)
    { // Updated variable name
        dwc.setColor(0xF9C5);
        dwc.sendData("Full");
    }
    else
    {
        dwc.setColor(0x058E);
        dwc.sendData("Available");
    }
}
// Function to set status for plastic bin
void setStatusPlastic()
{
    // Set address and send ASCII data for page 1
    dwc.setAddress(0x9020, 0x1020);
    dwc.setUiType(ASCII);
    //  Check if the bin is full
    if (sensorStatusBinPlastic)
    {
        dwc.setColor(0xF9C5);
        dwc.sendData("Full");
    }
    else
    {
        dwc.setColor(0x058E);
        dwc.sendData("Available");
    }
}
// Function to set status for other bin
void setStatusOther()
{
    // Set address and send ASCII data for page 1
    dwc.setAddress(0x9030, 0x1030);
    dwc.setUiType(ASCII);
    //  Check if the bin is full
    if (sensorStatusBinOther)
    { // Updated variable name
        dwc.setColor(0xF9C5);
        dwc.sendData("Full");
    }
    else
    {
        dwc.setColor(0x058E);
        dwc.sendData("Available");
    }
}


#pragma endregion

// -------------------------------------------------------------------------------------------------------------------

#pragma region PAGE
// Function to process different pages
void processPage(int pageNum)
{
    String commandMega2560 = Serial2.readStringUntil('\r');
    handlerStatusBinTrash(commandMega2560);

    switch (pageNum)
    {
    case 1:
        processPage1();
        break;
    case 2:
        processPage2(commandMega2560);
        break;
    case 3:
        processPage3(commandMega2560);
        break;
    case 4:
        processPage4(commandMega2560);
        break;
    case 5:
        processPage5();
        break;
    default:
        resetVariables();
        break;
    }
}

void resetVariables()
{
    countMetalTrash = 0;
    countPlasticTrash = 0;
    countOtherTrash = 0;
    isPutTrash = false;
    isEndTrash = false;
    isProcessing = false;
}

void processPage1()
{
    setStatusMetal();
    setStatusPlastic();
    setStatusOther();
}

// -------------------------------------------------------------------------------------------------------------------
// Hàm kiểm tra và xử lý khi tất cả các thùng rác đều đầy
void checkAllBinsFull()
{
    if (sensorStatusBinMetal == 1 && sensorStatusBinPlastic == 1 && sensorStatusBinOther == 1)
    {
        Serial.print("Page ID :2 - FULL ALL TRASH BIN");
        dwc.setPage(0);
    }
}

// Hàm bắt đầu quá trình xử lý rác
void startTrashProcess()
{
    if (isPutTrash == false)
    {
        Serial2.println(Cmd_BatDauQuyTrinh);
        isPutTrash = true;
        Serial.print("Send Cmd_BatDauQuyTrinh");
    }
}

// Hàm xử lý lệnh đếm rác
void handleTrashCountingCommand(const String &commandMega2560)
{
    if (commandMega2560.startsWith(Cmd_CountRac))
    {
        handlerCountBinTrash(commandMega2560);
        Serial.println("Page ID :2 - handlerCountBinTrash to Mega2560");
        Serial.println(countMetalTrash);
        Serial.println(countPlasticTrash);
        Serial.println(countOtherTrash);
        dwc.setPage(3);
    }
}

// Hàm kết thúc quá trình xử lý
void finishTrashProcess(const String &commandMega2560)
{
    if (commandMega2560.startsWith(Cmd_KetThucQuyTrinh))
    {
        Serial.print("Page ID :2 - Get and Send Cmd_KetThucQuyTrinh to Mega2560");
        Serial2.println(Cmd_KetThucQuyTrinh);
        if (countMetalTrash == 0 && countPlasticTrash == 0 && countOtherTrash == 0)
        {
            Serial.println("Page ID :2 - NO TRASH IN BIN");
            dwc.setPage(5);
        }
        else if (sensorStatusBinMetal == 1 && sensorStatusBinPlastic == 1 && sensorStatusBinOther == 1)
        {
            Serial.println("Page ID :2 - FULL ALL TRASH BIN");
            dwc.setPage(5);
        }
        else
        {
            dwc.setPage(4);
        }
    }
}

void processPage2(const String &commandString)
{
    if (isProcessing)
    {
        Serial.println("Processing in progress. Please wait...");
        return;
    }

    isProcessing = true;

    checkAllBinsFull();
    startTrashProcess();
    handleTrashCountingCommand(commandString);
    finishTrashProcess(commandString);

    isProcessing = false;
}

// -------------------------------------------------------------------------------------------------------------------

void processPage3(const String &commandMega2560)
{
    if (commandMega2560.startsWith(Cmd_CountRac))
    {
        handlerCountBinTrash(commandMega2560);
        Serial.println("Page ID :3 - handlerCountBinTrash to Mega2560");
        Serial.println(countMetalTrash);
        Serial.println(countPlasticTrash);
        Serial.println(countOtherTrash);
    }
    if (commandMega2560.startsWith(Cmd_KetThucQuyTrinh))
    {
        Serial.print("Page ID :3 - Timeout GET Cmd_KetThucQuyTrinh to Mega2560");
        dwc.setPage(4);
    }
}

unsigned long lastCmdSentTime = 0;
bool isCmdSent = false;
void processPage4(const String &commandMega2560)
{
    if (!isEndTrash)
    {
        Serial2.println(Cmd_KetThucQuyTrinh);
        Serial.print("Page ID :4 - Send Cmd_KetThucQuyTrinh to Mega2560");
        lastCmdSentTime = millis(); // Cập nhật thời điểm gửi lệnh
        isCmdSent = true;
        isEndTrash = true;
    }

    if (commandMega2560.startsWith(Cmd_KetThucQuyTrinh))
    {
        createTransactionBins();
    }

    // Kiểm tra timeout
    if (isCmdSent && (millis() - lastCmdSentTime > 5000))
    {
        Serial.println("5 seconds passed without receiving Cmd_KetThucQuyTrinh, generating QR code.");
        generateAndSendQRCode(countPlasticTrash, countMetalTrash, countOtherTrash, sensorStatusBinPlastic, sensorStatusBinMetal, sensorStatusBinOther, dwc);
        isCmdSent = false; // Đặt lại cờ
        isProcessing = false;
    }
}

void processPage5()
{
    handlePage4or5(0x5010, 0x1510, 0, 5);
}

// Callback function for DWIN echo
void dwinEchoCallback(DWIN2 &d)
{
    Serial.print("Echo  dwinEchoCallback:::");
    Serial.println(d.getDwinEcho());
}

#pragma endregion

// -------------------------------------------------------------------------------------------------------------------

#pragma region API CONTROL
// Hàm kết nối WiFi
void checkConnection()
{
    Serial.print("Connecting to WiFi...");
    WiFi.begin(ssid, password);
    unsigned long startAttemptTime = millis();

    // Attempt to connect to Wi-Fi
    while (WiFi.status() != WL_CONNECTED && millis() - startAttemptTime < timeoutWifi)
    {
        delay(500);
        Serial.print(".");
    }

    // Check if connected
    if (WiFi.status() != WL_CONNECTED)
    {
        Serial.println("Switched to Offline mode.");
        isOffline = true;
    }
    else
    {
        isOffline = false;
        Serial.println("Switched to Online mode.");
        Serial.println("Fetching new token...");
        // Get token
        //  Gọi hàm GetToken lần đầu tiên để lấy token ban đầu
        _tokenData = GetToken();
        interval = _tokenData.expireInSeconds * 1000; // Chuyển expireInSeconds sang mili giây
        previousMillis = millis();                    // Lưu lại thời điểm token được nhận
    }
}

// Hàm lấy token từ server
void HandlerToken()
{
    if (isOffline == false)
    {
        unsigned long currentMillis = millis(); // Lấy thời gian hiện tại

        // Kiểm tra nếu thời gian đã trôi qua đủ hoặc token là nullptr
        if (currentMillis - previousMillis >= interval || _tokenData.accessToken == nullptr)
        {
            previousMillis = currentMillis;               // Cập nhật thời gian lần cuối hàm GetToken được gọi
            _tokenData = GetToken();                      // Gọi hàm GetToken để nhận token mới
            interval = _tokenData.expireInSeconds * 1000; // Cập nhật lại interval dựa trên expireInSeconds

            // In ra thông tin token
            Serial.println("GETTING new token...");
            Serial.println(_tokenData.accessToken);
            Serial.println(_tokenData.expireInSeconds);
        }
    }
}

// Hàm khởi tạo token từ JSON Token
TokenData createTokenFromJson(const String &jsonString)
{
    TokenData tokenData;
    StaticJsonDocument<256> doc; // Giảm kích thước nếu không cần thiết

    if (deserializeJson(doc, jsonString))
    {
        Serial.println("Failed to parse JSON.");
    }
    else
    {
        tokenData.accessToken = doc["result"]["accessToken"].as<String>();
        tokenData.refreshToken = doc["result"]["refreshToken"].as<String>();
        tokenData.expireInSeconds = doc["result"]["expireInSeconds"].as<long>();
    }
    return tokenData;
}

// Hàm gọi updateBinStatus
// Unified function to handle bin status updates
void updateBinStatus(int percentPlastics, int percentMetal, int percentOther)
{
    if (isOffline)
    {
        handleOfflineUpdateStatus(percentPlastics, percentMetal, percentOther);
    }
    else
    {
        handleOnlineUpdateStatus(percentPlastics, percentMetal, percentOther);
    }
}

void handleOnlineUpdateStatus(int percentPlastics, int percentMetal, int percentOther)
{
    if (!isProcessing)
    {
        isProcessing = true;
        HTTPClient http;
        http.begin(apiUrlUpdateDeviceForBin);
        String dataToken = "Bearer " + _tokenData.accessToken;
        http.addHeader("Content-Type", "application/json");
        http.addHeader("Authorization", dataToken);

        String jsonData = "{";
        jsonData += "\"id\":\"" + String(deviceID) + "\",";
        jsonData += "\"percentStatusPlastis\":" + String(percentPlastics) + ",";
        jsonData += "\"percentStatusMetal\":" + String(percentMetal) + ",";
        jsonData += "\"percentStatusOrther\":" + String(percentOther);
        jsonData += "}";

        int httpResponseCode = http.POST(jsonData);
        if (httpResponseCode > 0)
        {
            Serial.println("Bin status updated successfully!");
        }
        else
        {
            Serial.println("Failed to update bin status online.");
        }
        http.end();
        isProcessing = false;
    }
}
// Offline handler for bin status updates

void handleOfflineUpdateStatus(int percentPlastics, int percentMetal, int percentOther)
{
    Serial.println("Offline mode: Skipping API calls. Simulating bin status updates.");
    // Optionally store these updates in local memory or log them
}

// Hàm tạo transaction bins
// Unified function to create transaction bins
void createTransactionBins()
{
    if (isOffline)
    {
        generateAndSendQRCode(countPlasticTrash, countMetalTrash, countOtherTrash, sensorStatusBinPlastic, sensorStatusBinMetal, sensorStatusBinOther, dwc);
    }
    else
    {
        if (!isProcessing)
        {
            isProcessing = true;
            HTTPClient http;
            http.begin(apiUrlTransactionBins);
            String dataToken = "Bearer " + _tokenData.accessToken;
            http.addHeader("Content-Type", "application/json");
            http.addHeader("Authorization", dataToken);

            String jsonData = "{";
            jsonData += "\"plasticQuantity\":" + String(countPlasticTrash) + ",";
            jsonData += "\"metalQuantity\":" + String(countMetalTrash) + ",";
            jsonData += "\"otherQuantity\":" + String(countOtherTrash) + ",";
            jsonData += "\"deviceId\":" + String(deviceID) + ",";
            jsonData += "\"transactionStatusId\":" + String(transactionStatusId);
            jsonData += "}";

            int httpResponseCode = http.POST(jsonData);
            if (httpResponseCode > 0)
            {
                String response = http.getString();
                TokenData tokenData;
                StaticJsonDocument<256> doc; // Giảm kích thước nếu không cần thiết
                if (deserializeJson(doc, response))
                {
                    Serial.println("Failed to parse JSON. CreateDeviceTransactionBins");
                }
                else
                {
                    bool success = doc["success"].as<bool>();
                    String dataQR = doc["result"].as<String>();
                    Serial.println("QRData: ");
                    Serial.println(dataQR);
                    if (success)
                    {
                        Serial.println("CreateDeviceTransactionBins thành công");
                        // Handle operations for page 4
                        dwc.setAddress(0x9910, 0x1099);
                        dwc.setUiType(ASCII);
                        // SET QR CODE
                        dwc.sendData(dataQR);
                        handlePage4or5(0x4010, 0x1410, 5, 15);
                    }
                    else
                    {
                        Serial.println("CreateDeviceTransactionBins thất bại");
                        generateAndSendQRCode(countPlasticTrash, countMetalTrash, countOtherTrash, sensorStatusBinPlastic, sensorStatusBinMetal, sensorStatusBinOther, dwc);
                    }
                }
            }
            else
            {
                Serial.print("Lỗi kết nối: ");
                Serial.println(httpResponseCode);
                generateAndSendQRCode(countPlasticTrash, countMetalTrash, countOtherTrash, sensorStatusBinPlastic, sensorStatusBinMetal, sensorStatusBinOther, dwc);
            }
            http.end();
            isProcessing = false;
        }
    }
}

void generateAndSendQRCode(int plasticQuantity, int metalQuantity, int otherQuantity, bool sensorStatusBinPlastic, bool sensorStatusBinMetal, bool sensorStatusBinOther, DWIN2 &dwc)
{
    // Tạo đối tượng JSON để giữ dữ liệu
    StaticJsonDocument<256> jsonDoc;
    jsonDoc["plasticQuantity"] = plasticQuantity;
    jsonDoc["metalQuantity"] = metalQuantity;
    jsonDoc["otherQuantity"] = otherQuantity;
    jsonDoc["sensorStatusBinPlastic"] = sensorStatusBinPlastic;
    jsonDoc["sensorStatusBinMetal"] = sensorStatusBinMetal;
    jsonDoc["sensorStatusBinOther"] = sensorStatusBinOther;
    jsonDoc["deviceId"] = deviceID;

    // Chuyển đổi đối tượng JSON thành chuỗi
    String qrCodeData;
    serializeJson(jsonDoc, qrCodeData);

    // In và gửi dữ liệu chuỗi JSON tới hiển thị
    Serial.println("Offline mode: Generating offline QR code.");
    Serial.println("QR Code Data: " + qrCodeData);
    String encryptedData;
    encryptQRCodeData(qrCodeData, encryptedData);

    // In kết quả
    Serial.println("Dữ liệu QR Code gốc:");
    Serial.println(qrCodeData);
    Serial.println("Dữ liệu QR Code đã mã hóa:");
    Serial.println(encryptedData);

    // Đặt địa chỉ và gửi dữ liệu đã mã hóa
    dwc.setAddress(0x9910, 0x1099); // Địa chỉ cho dữ liệu mã QR
    dwc.setUiType(ASCII);
    dwc.sendData(encryptedData);
    handlePage4or5(0x4010, 0x1410, 5, 15);
}

// Hàm gửi yêu cầu POST
TokenData GetToken()
{
    TokenData tokenData;
    if (WiFi.status() == WL_CONNECTED)
    {
        HTTPClient http;
        http.begin(apiUrlTokenAuth);
        http.addHeader("Content-Type", "application/json");
        http.addHeader("accept", "text/plain");
        // JSON request data
        std::string requestData = "{\"userNameOrEmailAddress\":\"" + std::string(user) + "\",\"password\":\"" + std::string(pass) + "\"}";
        int httpResponseCode = http.POST(requestData.c_str());
        if (httpResponseCode > 0)
        {
            String response = http.getString();
            Serial.println("Response getToken:");
            Serial.println(response);
            tokenData = createTokenFromJson(response);
        }
        else
        {
            Serial.printf("Error on sending POST: %d\n", httpResponseCode);
        }
        http.end();
    }
    else
    {
        Serial.println("WiFi not connected.");
    }
    return tokenData;
}

// Hàm mã hóa dữ liệu QR Code

void encryptQRCodeData(String &qrCodeData, String &encryptedData)
{
    // Khóa và IV
    const char *keyStr = "SbinSolution2024_8CFB2EC534E14D5";
    byte key[32];
    memcpy(key, keyStr, 32); // Sao chép đúng 32 byte

    byte iv[16] = {0}; // IV 16 byte toàn 0

    // Chuyển đổi chuỗi đầu vào thành byte
    int dataLength = qrCodeData.length();
    int paddedLength = ((dataLength + 15) / 16) * 16;
    byte inputData[paddedLength];
    memset(inputData, 0, paddedLength);                // Đệm 0 vào dữ liệu
    memcpy(inputData, qrCodeData.c_str(), dataLength); // Sao chép chuỗi đầu vào

    // Bộ đệm dữ liệu mã hóa
    byte encryptedDataBuffer[paddedLength];

    // Mã hóa AES CBC
    AES aes;
    aes.do_aes_encrypt(inputData, paddedLength, encryptedDataBuffer, key, 256, iv);

    // Tính toán độ dài chuỗi mã hóa Base64
    int base64Length = ((paddedLength + 2) / 3) * 4;

    // Mã hóa Base64
    char base64EncodedData[base64Length + 1];
    base64_encode(base64EncodedData, (char *)encryptedDataBuffer, paddedLength);
    base64EncodedData[base64Length] = '\0'; // Đảm bảo kết thúc bằng null

    // Gán dữ liệu đầu ra
    encryptedData = String(base64EncodedData);
}

#pragma endregion
