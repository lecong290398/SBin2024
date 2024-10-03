#include <WiFi.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>

#pragma region Config Variables
// Thông tin WiFi
const char *ssid = "HomeLCTM";
const char *password = "1@qweQAZ";
const char *deviceID = "1"; // ID của thiết bị
// Địa chỉ URL API
const char *apiUrlTokenAuth = "https://app.sbin.edu.vn/api/TokenAuth/Authenticate";
const char *apiUrlTransactionBins = "https://app.sbin.edu.vn/api/services/app/TransactionBins/CreateDevice_TransactionBins";
const char *apiUrlUpdateDeviceForBin = "https://app.sbin.edu.vn/api/services/app/Devices/UpdateDeviceForBin";
// Thông tin user
const char *user = "linhtrungsbin01";
const char *pass = "123qwe";
unsigned long previousMillis = 0; // Lưu trữ thời gian lần cuối hàm GetToken được gọi
unsigned long interval = 0;       // Khoảng thời gian giữa các lần lấy token (tính bằng mili giây)

// Object chứa token
struct TokenData
{
  String accessToken;
  String refreshToken;
  long expireInSeconds;
};

TokenData _tokenData; // Default constructor
int isUpdateStatusDevice = 0;

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

// Hàm khởi tạo token từ JSON
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

void CreateDeviceTransactionBins()
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
    String jsonData = "{\"plastisQuantity\":19,\"metalQuantity\":18,\"ortherQuantity\":1,\"deviceId\":1,\"transactionStatusId\":1}";
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

void setup()
{
  Serial.begin(115200);
  connectWiFi();
}
int CallON = 0;
void loop()
{
  // Gọi hàm GetToken
  HandlerToken();
  if (_tokenData.accessToken != nullptr && CallON == 0)
  {
     Serial.println("CreateDevice Transaction Bins.............");
      CreateDeviceTransactionBins();
      CallON = 1;
  }
  delay(2000);
}
