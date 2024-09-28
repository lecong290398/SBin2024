#include <Arduino.h>
#include <Dwin2.h>
#include <WiFi.h>
#include <HardwareSerial.h>

// Rx Tx ESP gpio connected to DWin Display
#define RX_PIN 16
#define TX_PIN 17

// Class for controlling UI elements of the display
DWIN2 dwc;
HardwareSerial mySerialESP_MEGA(1); // Sử dụng Serial1 (RX=21, TX=19 trên ESP32)

//Status Bin Trash
int sensorStatusMetal = 0; // 1 FULL || 0 Available || -1 Lỗi
int sensorStatusPlastic = 0; 
int sensorStatusOther = 0; 

// count Trash
int countStatusMetal = 0; 
int countStatusPlastic = 0; 
int countStatusOther = 0; 


// Callback function to receive a response from the display
void dwinEchoCallback(DWIN2 &d);

void setup() {
    Serial.begin(115200);
    while (Serial.available()) {}
    Serial.printf("-------- Start DWIN communication demo --------\n");
    Serial.printf("-----------------------------------------------\n");
    // Initialize timers, tasks, serial communication, and other settings
    dwc.begin();
    // Set callback for receiving responses
    dwc.setUartCbHandler(dwinEchoCallback);
    // Disable echo for commands and responses
    dwc.setEcho(false);
    Serial.printf("\n----- Dwin Display Common commands end -----\n");

    Serial.printf("\n----- mySerialESP_MEGA  commands RUN -----\n");
    mySerialESP_MEGA.begin(115200, SERIAL_8N1, 16, 17); // Khởi tạo Serial1 (RX=16, TX=17)
    Serial.printf("\n----- mySerialESP_MEGA  commands end -----\n");
}

void loop() {
    // Get the current page number
    uint8_t pageNum = dwc.getPage();
    Serial.printf("Current page: %d\n", pageNum);
    processPage(pageNum);
    delay(700);

 
}

// Hàm để đọc giá trị từ pin của Arduino qua Serial
int readValueFromArduino(int pin) {
  mySerialESP_MEGA.println(pin); // Gửi yêu cầu đọc pin
  delay(100); // Đợi một chút để Arduino có thời gian xử lý
  if (mySerialESP_MEGA.available()) {
    return mySerialESP_MEGA.parseInt(); // Đọc giá trị từ Arduino
  }
  return -1; // Trả về -1 nếu không có dữ liệu
}

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


// Function to process different pages
void processPage(int pageNum) {
    switch(pageNum) {
        case 1:
            // Read value from Arduino
            sensorStatusMetal= readValueFromArduino(A0); // Read from pin A0
            if (sensorStatusMetal != -1) {
                Serial.print("Received value from pin A0: ");
                Serial.println(sensorStatusMetal); // Print received value
            } else {
                Serial.println("No data received.");
            }

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
            countStatusMetal = 0;
            countStatusPlastic = 0;
            countStatusOther = 0; 
            break;
    }
}

// Callback function for DWIN echo
void dwinEchoCallback(DWIN2 &d) {
    Serial.print("Echo  dwinEchoCallback:::");
    Serial.println(d.getDwinEcho());
}

#pragma endregion