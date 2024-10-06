#include <WiFi.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>
#include <ESPAsyncWebServer.h>

#pragma region Config Variables
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
// Object chứa token
struct TokenData
{
  String accessToken;
  String refreshToken;
  long expireInSeconds;
};
TokenData _tokenData; // Default constructor
#pragma endregion

#pragma region Function Prototypes
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
  if (WiFi.status() == WL_CONNECTED)
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
    Serial.println("WiFi not connected");
  }
}

// Hàm tạo transaction bins
void CreateDeviceTransactionBins(int plasticQuantity, int metalQuantity, int otherQuantity, int transactionStatusId = 1)
{
  if (WiFi.status() == WL_CONNECTED)
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
    http.end();
  }
  else
  {
    Serial.println("Không kết nối được tới WiFi");
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

#pragma region DEMO RESET

// Biến lưu trữ trạng thái rác
int percentPlastis = 50;
int percentMetal = 30;
int percentOther = 20;
AsyncWebServer server(80);

// Hàm tạo HTML cho trang web
String generateHTML()
{
  String html = "<html>\
                  <head>\
                    <title>ESP32 Reset Values</title>\
                  </head>\
                  <body>\
                    <h2>ESP32 Reset Trash Status</h2>\
                    <p>Percent Plastis: " +
                String(percentPlastis) + "%</p>\
                    <p>Percent Metal: " +
                String(percentMetal) + "%</p>\
                    <p>Percent Other: " +
                String(percentOther) + "%</p>\
                    <button onclick=\"resetValues()\">Reset Values</button>\
                    <script>\
                      function resetValues() {\
                        fetch('/reset').then(() => {\
                          window.location.reload();\
                        });\
                      }\
                    </script>\
                  </body>\
                </html>";
  return html;
}

// Hàm khởi tạo server
void initServer()
{
  // Cung cấp trang HTML khi truy cập root "/"
  server.on("/", HTTP_GET, [](AsyncWebServerRequest *request)
            {
    String html = generateHTML();  // Gọi hàm tạo HTML
    request->send(200, "text/html", html); });

  // Xử lý yêu cầu reset các giá trị về 0
  server.on("/reset", HTTP_GET, [](AsyncWebServerRequest *request)
            {
    resetValues();  // Gọi hàm reset
    request->send(200, "text/plain", "Values Reset to 0"); });

  // Khởi động server
  server.begin();
  Serial.println("HTTP server started");
}

// Hàm reset các giá trị về 0
void resetValues()
{
  percentPlastis = 0;
  percentMetal = 0;
  percentOther = 0;
  Serial.println("Values have been reset to 0");
}

#pragma endregion

void setup()
{
  // Khởi tạo Serial
  Serial.begin(115200);
  // Kết nối WiFi
  connectWiFi();
  // Khởi tạo server và cung cấp trang web
  initServer();
}

void loop()
{
  // Gọi hàm GetToken
  HandlerToken();

  // Gọi hàm CreateDeviceTransactionBins
  if (_tokenData.accessToken != nullptr && CallAPICreateTransaction == 0)
  {
    Serial.println("CreateDevice Transaction Bins.............");
    CreateDeviceTransactionBins(3, 2, 1);
    Serial.println("Update Transaction Bins.............");
    callUpdateStatusAPI(0, 1, 0);
    // Gán CallAPICreateTransaction = 1 để không gọi lại hàm CreateDeviceTransactionBins
    CallAPICreateTransaction = 1;
  }

  delay(2000);
}
