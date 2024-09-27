#include <Arduino.h>
#include <Dwin2.h>
#include <WiFi.h>

// Rx Tx ESP gpio connected to DWin Display
#define RX_PIN 16
#define TX_PIN 17

// Class for controlling UI elements of the display
DWIN2 dwc;

// Thông tin mạng WiFi
const char* ssid = "HomeLCTM";
const char* password = "1@qweQAZ";
const char* serverName = "http://example.com/api/endpoint";

// Callback function to receive a response from the display
void dwinEchoCallback(DWIN2 &d);

void setup() {
    Serial.begin(115200);
    while (Serial.available()) {}
    Serial.printf("-------- Start DWIN communication demo --------\n");
    Serial.printf("-----------------------------------------------\n");

    const int delayTime = 1000;
    delay(delayTime);

    Serial.printf("\n----- Dwin Display Common commands start -----\n");
    // Initialize timers, tasks, serial communication, and other settings
    dwc.begin();
    // Set callback for receiving responses
    dwc.setUartCbHandler(dwinEchoCallback);
    // Disable echo for commands and responses
    dwc.setEcho(false);
    Serial.printf("\n----- Dwin Display Common commands end -----\n");

    // Kết nối WiFi
      WiFi.begin(ssid, password);
      Serial.println("Connecting to WiFi...");
      
      while (WiFi.status() != WL_CONNECTED) {
        delay(1000);
        Serial.println("Connecting...");
      }
      Serial.println("Connected to WiFi");
    
}
#pragma region API ROUTER

// Hàm tách biệt để gọi API và trả về phản hồi
String postDataToServer(String jsonData) {
  HTTPClient http;

  // Bắt đầu kết nối với URL API
  http.begin(serverName);
  http.addHeader("Content-Type", "application/json");

  // Gửi yêu cầu POST và nhận mã trạng thái HTTP
  int httpResponseCode = http.POST(jsonData);

  String payload = "";

  if (httpResponseCode > 0) {
    // Lấy chuỗi phản hồi từ server
    payload = http.getString();
  } else {
    Serial.print("Error on sending POST: ");
    Serial.println(httpResponseCode);
    payload = "Error"; // Trả về chuỗi "Error" khi có lỗi
  }

  // Đóng kết nối HTTP
  http.end();

  return payload; // Trả về chuỗi phản hồi hoặc "Error"
}

#pragma endregion

#pragma region DWIN CONTROL 

// Function to handle common operations for pages 4 and 5
void handlePage4or5(uint16_t address1, uint16_t address2 , int idPage , int time ) {
    dwc.setAddress(address1, address2);
    dwc.setUiType(INT);
    dwc.setStartVal(20);
    dwc.setLimits(0, time, false);
   for (int i = time; i >= 0; i--) {
        dwc.sendData(i);
        delay(1000);
        if(i == 1)
        {
           dwc.setPage(idPage);
        }
    }
}

void loop() {
      delay(700);
    // Get the current page number
    uint8_t pageNum = dwc.getPage();
    Serial.printf("Current page: %d\n", pageNum);
    processPage(pageNum);
}
// Function to process different pages
void processPage(int pageNum) {
    switch(pageNum) {
        case 1:
            // Set addresses and send data for page 1
            dwc.setAddress(0x9010, 0x1010);
            dwc.setUiType(ASCII);
            dwc.setColor(0xF9C5);
            dwc.sendData("Full");

            dwc.setAddress(0x9020, 0x1020);
            dwc.setUiType(ASCII);
            dwc.setColor(0x058E);
            dwc.sendData("Available");

            dwc.setAddress(0x9030, 0x1030);
            dwc.setUiType(ASCII);
            dwc.setColor(0x058E);
            dwc.sendData("Available");
            break;
        case 2:
            // Delay for 5 seconds and set page to 3
            delay(5000);
            dwc.setPage(3);
            break;
        case 3:
            // Set address and send integer data for page 3
            dwc.setAddress(0x3010, 0x1310);
            dwc.setUiType(INT);
            dwc.sendData(7);
            delay(500);
            // Set address and send integer data for page 3
            dwc.setAddress(0x3020, 0x1320);
            dwc.setUiType(INT);
            dwc.sendData(8);
            delay(500);

           // Set address and send integer data for page 3
            dwc.setAddress(0x3030, 0x1330);
            dwc.setUiType(INT);
            dwc.sendData(0);
            break;
        case 4:
            dwc.setAddress(0x9910, 0x1099);
            dwc.setUiType(ASCII);
            dwc.sendData("https://youtu.be/-bwemq005vw?si=Zv0bqV6s0lPpvimZ");
            // Handle operations for page 4
            handlePage4or5(0x4010, 0x1410,5,15);
            break;
        case 5:
            // Handle operations for page 5
            handlePage4or5(0x5010, 0x1510,0,10);
            break;
        default:
            // Default case does nothing
            break;
    }
}

// Callback function for DWIN echo
void dwinEchoCallback(DWIN2 &d) {
    Serial.print("Echo  dwinEchoCallback:::");
    Serial.println(d.getDwinEcho());
}

#pragma endregion