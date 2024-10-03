#include <WiFi.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>

#pragma region Config Variables
// Thông tin WiFi
const char *ssid = "HomeLCTM";
const char *password = "1@qweQAZ";

// Địa chỉ URL API
const char *apiUrlTokenAuth = "https://admin.sbin.edu.vn/api/TokenAuth/Authenticate";
const char *apiUrlTransactionBins = "https://admin.sbin.edu.vn/api/services/app/TransactionBins/CreateDevice_TransactionBins";
const char *apiUrlUpdateDeviceForBin = "https://admin.sbin.edu.vn/api/services/app/Devices/UpdateDeviceForBin";
// Thông tin user
const char *user = "linhtrungsbin01";
const char *pass = "123qwe";
unsigned long previousMillis = 0; // Lưu trữ thời gian lần cuối hàm GetToken được gọi
const long interval = 900000;     // 15 phút (900000ms = 15 * 60 * 1000)

// Object chứa token
struct TokenData
{
  String accessToken;
  String refreshToken;
  long expireInSeconds;
};

TokenData _tokenData = null;
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

  //Get token
  HandlerToken();                          // Gọi hàm GetToken
  Serial.println("Fetching new token...");
  Serial.println(_tokenData.accessToken); // In ra token
}

void HandlerToken()
{
  // Kiểm tra nếu thời gian đã trôi qua đủ (15 phút)
  if (currentMillis - previousMillis >= interval || _tokenData.accessToken == null)
  {
    previousMillis = currentMillis; // Cập nhật thời gian lần cuối hàm GetToken được gọi
    _tokenData = GetToken();        // Gọi hàm GetToken
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

void loop()
{
    //Get token
  HandlerToken();                          // Gọi hàm GetToken
  Serial.println("Fetching new token...");
  Serial.println(_tokenData.accessToken); // In ra token

}
