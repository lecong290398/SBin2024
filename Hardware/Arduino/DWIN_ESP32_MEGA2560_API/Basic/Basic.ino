#include <Arduino.h>
#include <Dwin2.h>
#include <WiFi.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>

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

int isPutTrash = 0;
int isEndTrash = 0;
// Callback function to receive a response from the display
void dwinEchoCallback(DWIN2 &d);

#pragma endregion

#pragma region Constants API
// Thông tin WiFi
const char *ssid = "HomeLCTM";     // Tên WiFi
const char *password = "1@qweQAZ"; // Mật khẩu WiFi
const char *deviceID = "1";        // ID của thiết bị
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
unsigned long isOffline = 0;
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
    if (isOffline == 0)
    {
        // Kết nối WiFi
        Serial.printf("-------- Start WIFI RUN --------\n");
        connectWiFi();
    }else{
        Serial.printf("-------- Start OFFLINE MODE --------\n");
    }
}

void loop()
{
    if (isOffline == 0)
    {
        // Gọi hàm GetToken
        HandlerToken();
    }
    // Get the current page number
    uint8_t pageNum = dwc.getPage();
    // Process the current page
    processPage(pageNum);
    // Delay for a while
    delay(200);
}

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

// Function to handle status of the trash bin
void handlerStatusBinTrash(String commandString)
{
    // Check if the command is to empty the trash
    if (commandString.startsWith(Cmd_DayRacKimLoai))
    {
        Serial.print("GET Cmd_DayRacKimLoai");
        sensorStatusBinMetal = 1;
        // Call API to update status of the trash bin
        callUpdateStatusAPI(sensorStatusBinPlastic, sensorStatusBinMetal, sensorStatusBinOther);
    }
    else if (commandString.startsWith(Cmd_DayRacNhua))
    {
        Serial.print("GET Cmd_DayRacNhua");
        sensorStatusBinPlastic = 1; // Updated variable name
        // Call API to update status of the trash bin
        callUpdateStatusAPI(sensorStatusBinPlastic, sensorStatusBinMetal, sensorStatusBinOther);
    }
    else if (commandString.startsWith(Cmd_DayRacKhongXacDinh))
    {
        Serial.print("GET Cmd_DayRacKhongXacDinh");
        sensorStatusBinOther = 1; // Updated variable name
        // Call API to update status of the trash bin
        callUpdateStatusAPI(sensorStatusBinPlastic, sensorStatusBinMetal, sensorStatusBinOther);
    }
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

#pragma region PAGE
// Function to set status for metal bin
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
// Function to process different pages
void processPage(int pageNum)
{
    // Read the command from the Mega2560
    String commandMega2560 = Serial2.readStringUntil('\r');
    switch (pageNum)
    {
    case 1:
        // Set status for each bin
        setStatusMetal();
        setStatusPlastic();
        setStatusOther();
        break;
    case 2:
        Serial.println("Page 2");

        // Check if the command is to start the process
        if (isPutTrash == 0)
        {
            // Send command to start the process
            Serial2.println(Cmd_BatDauQuyTrinh);
            // Set the flag to indicate that the trash has been put
            isPutTrash = 1;
            Serial.print("Send Cmd_BatDauQuyTrinh");
        }
        if (commandMega2560.startsWith(Cmd_CountRac))
        {
            handlerCountBinTrash(commandMega2560);
            // Set address and send integer data for page 3
            dwc.setPage(3);
        }
        else if (commandMega2560.startsWith(Cmd_KetThucQuyTrinh))
        {
            Serial.print("GET Cmd_KetThucQuyTrinh Page ID 2");
            Serial2.println(Cmd_KetThucQuyTrinh);
            if (countMetalTrash == 0 && countPlasticTrash == 0 && countOtherTrash == 0)
            {
                dwc.setPage(5);
            }
            else
            {
                dwc.setPage(4);
            }
        }
        break;
    case 3:
        if (commandMega2560.startsWith(Cmd_CountRac))
        {
            handlerCountBinTrash(commandMega2560);
        }
        else if (commandMega2560.startsWith(Cmd_KetThucQuyTrinh))
        {
            Serial.print("GET Cmd_KetThucQuyTrinh Page ID 3");
            dwc.setPage(4);
        }
        break;
    case 4:
        if (isEndTrash == 0)
        {
            // Send command to end the process
            Serial2.println(Cmd_KetThucQuyTrinh);
            Serial.print("Send Cmd_KetThucQuyTrinh");
            // Set the flag to indicate that the trash has been put
            CreateDeviceTransactionBins(countPlasticTrash, countMetalTrash, countOtherTrash);
            isEndTrash = 1;
        }
        break;
    case 5:
        // Handle operations for page 5
        handlePage4or5(0x5010, 0x1510, 0, 5);
        break;
    default:
        // Reset all variables
        countMetalTrash = 0;
        countPlasticTrash = 0;
        countOtherTrash = 0;
        isPutTrash = 0;
        isEndTrash = 0;
        break;
    }
    // Handle status of the trash bin
    handlerStatusBinTrash(commandMega2560);
}

// Callback function for DWIN echo
void dwinEchoCallback(DWIN2 &d)
{
    Serial.print("Echo  dwinEchoCallback:::");
    Serial.println(d.getDwinEcho());
}

#pragma endregion

#pragma region API CONTROL

// Hàm kết nối WiFi
void connectWiFi()
{
    Serial.print("Connecting to WiFi...");
    WiFi.begin(ssid, password);
    while (WiFi.status() != WL_CONNECTED)
    {
        delay(500);
        Serial.print(".");
    }
    Serial.println("Connected to WiFi.");

    Serial.println("Fetching new token...");
    // Get token
    //  Gọi hàm GetToken lần đầu tiên để lấy token ban đầu
    _tokenData = GetToken();
    interval = _tokenData.expireInSeconds * 1000; // Chuyển expireInSeconds sang mili giây
    previousMillis = millis();                    // Lưu lại thời điểm token được nhận
}

// Hàm lấy token từ server
void HandlerToken()
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

// Hàm gọi API PUT
void callUpdateStatusAPI(int percentPlastics, int percentMetal, int percentOther)
{
    if (WiFi.status() == WL_CONNECTED && isOffline == 0)
    { // Kiểm tra xem đã kết nối WiFi chưa
        HTTPClient http;
        http.begin(apiUrlUpdateDeviceForBin); // URL API
        String dataToken = "Bearer " + _tokenData.accessToken;
        http.addHeader("Content-Type", "application/json");
        http.addHeader("accept", "text/plain");
        http.addHeader("Authorization", dataToken);
        // Tạo payload JSON
        String jsonData = "{";
        jsonData += "\"id\":\"" + String(deviceID) + "\",";
        jsonData += "\"percentStatusPlastis\":" + String(percentPlastics) + ",";
        jsonData += "\"percentStatusMetal\":" + String(percentMetal) + ",";
        jsonData += "\"percentStatusOrther\":" + String(percentOther);
        jsonData += "}";
        int httpResponseCode = http.POST(jsonData); // Gửi yêu cầu POST
        if (httpResponseCode > 0)
        {
            // Kiểm tra kết quả trả về
            String response = http.getString();
            Serial.println("HTTP Response code: " + String(httpResponseCode));
            Serial.println("Response: " + response);
        }
        else
        {
            Serial.println("Error on sending PUT: " + String(httpResponseCode));
        }
        http.end(); // Đóng kết nối
    }
    else
    {
        if (isOffline == 1)
        {
            Serial.println("Bạn đang ở mode Offline ~ callUpdateStatusAPI");
        }
        else
        {
            Serial.println("WiFi not connected");
        }
    }
}

// Hàm tạo transaction bins
void CreateDeviceTransactionBins(int plasticQuantity, int metalQuantity, int otherQuantity)
{
    if (WiFi.status() == WL_CONNECTED && isOffline == 0)
    {
        HTTPClient http;
        // Chuẩn bị kết nối tới server
        http.begin(apiUrlTransactionBins);
        // Đặt header
        String dataToken = "Bearer " + _tokenData.accessToken;
        http.addHeader("Content-Type", "application/json");
        http.addHeader("accept", "text/plain");
        http.addHeader("Authorization", dataToken);

        // Chuẩn bị dữ liệu JSON
        String jsonData = "{";
        jsonData += "\"plasticQuantity\":" + String(plasticQuantity) + ",";
        jsonData += "\"metalQuantity\":" + String(metalQuantity) + ",";
        jsonData += "\"otherQuantity\":" + String(otherQuantity) + ",";
        jsonData += "\"deviceId\":" + String(deviceID) + ",";
        jsonData += "\"transactionStatusId\":" + String(transactionStatusId);
        jsonData += "}";

        // Gửi yêu cầu POST
        int httpResponseCode = http.POST(jsonData);
        // API Create QR Code and Create Transaction Bin
        // Set address and send ASCII data for page 4
        String dataQR = "999";
        // Xử lý phản hồi từ server
        if (httpResponseCode > 0)
        {
            String response = http.getString();
            Serial.println("Phản hồi từ server: apiUrlTransactionBins ");
            Serial.println(response);
            TokenData tokenData;
            StaticJsonDocument<256> doc; // Giảm kích thước nếu không cần thiết
            if (deserializeJson(doc, response))
            {
                Serial.println("Failed to parse JSON. CreateDeviceTransactionBins");
            }
            else
            {
                bool success = doc["success"].as<bool>();
                String QRData = doc["result"].as<String>();
                Serial.println("QRData: ");
                Serial.println(QRData);
                if (success)
                {
                    Serial.println("CreateDeviceTransactionBins thành công");
                    dataQR = QRData;
                }
                else
                {
                    Serial.println("CreateDeviceTransactionBins thất bại");
                }
            }
        }
        else
        {
            Serial.print("Lỗi kết nối: ");
            Serial.println(httpResponseCode);
        }
        // Handle operations for page 4
        dwc.setAddress(0x9910, 0x1099);
        dwc.setUiType(ASCII);
        // SET QR CODE
        dwc.sendData(dataQR);
        handlePage4or5(0x4010, 0x1410, 5, 10);
        http.end();
    }
    else
    {
        if (isOffline == 1)
        {
            Serial.println("Bạn đang ở mode Offline ~ CreateDeviceTransactionBins");
        }
        else
        {
            Serial.println("Không kết nối được tới WiFi");
        }
    }
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

#pragma endregion