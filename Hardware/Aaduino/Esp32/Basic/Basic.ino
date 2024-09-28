#include <WiFi.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>

#pragma region Config Variables
// Thông tin WiFi
const char* ssid = "HomeLCTM";
const char* password = "1@qweQAZ";

// Địa chỉ URL API
const char* apiUrlTokenAuth = "https://admin.sbin.edu.vn/api/TokenAuth/Authenticate";
const char* apiUrlTransactionBins = "https://admin.sbin.edu.vn/api/services/app/TransactionBins/CreateDevice_TransactionBins";
const char* apiUrlUpdateDeviceForBin = "https://admin.sbin.edu.vn/api/services/app/Devices/UpdateDeviceForBin";
// Thông tin user
const char* user = "linhtrungsbin01";
const char* pass = "123qwe";

// Object chứa token
struct TokenData {
  String accessToken;
  String refreshToken;
  long expireInSeconds;
};

int isUpdateStatusDevice = 0;

#pragma endregion

#pragma region Function Prototypes
// Hàm kết nối WiFi
void connectWiFi() {
  Serial.print("Connecting to WiFi...");
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("Connected to WiFi.");
}

// Hàm khởi tạo token từ JSON
TokenData createTokenFromJson(const String& jsonString) {
  TokenData tokenData;
  StaticJsonDocument<256> doc; // Giảm kích thước nếu không cần thiết

  if (deserializeJson(doc, jsonString)) {
    Serial.println("Failed to parse JSON.");
  } else {
    tokenData.accessToken = doc["result"]["accessToken"].as<String>();
    tokenData.refreshToken = doc["result"]["refreshToken"].as<String>();
    tokenData.expireInSeconds = doc["result"]["expireInSeconds"].as<long>();
  }

  return tokenData;
}

// Hàm gửi yêu cầu POST
TokenData GetToken() {
  TokenData tokenData;
  if (WiFi.status() == WL_CONNECTED) {
    HTTPClient http;
    http.begin(apiUrlTokenAuth);
    http.addHeader("Content-Type", "application/json");
    http.addHeader("accept", "text/plain");
    // JSON request data
    std::string requestData = "{\"userNameOrEmailAddress\":\"" + std::string(user) + "\",\"password\":\"" + std::string(pass) + "\"}";
    int httpResponseCode = http.POST(requestData.c_str());
    if (httpResponseCode > 0) {
      String response = http.getString();
      Serial.println("Response getToken:");
      Serial.println(response);
      tokenData = createTokenFromJson(response);
    } else {
      Serial.printf("Error on sending POST: %d\n", httpResponseCode);
    }
    http.end();
  } else {
    Serial.println("WiFi not connected.");
  }
  return tokenData;
}
#pragma endregion

void setup() {
  Serial.begin(115200);
  connectWiFi();
}

void loop() {
  // Chức năng chính không cần thiết
}
