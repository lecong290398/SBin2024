#include <Arduino.h>
#include <Dwin2.h>
#include <WiFi.h>

// Rx Tx ESP gpio connected to DWin Display
#define RX_PIN 16
#define TX_PIN 17

//Mega 2650
#define RXD3 19
#define TXD3 20

// Class for controlling UI elements of the display
DWIN2 dwc;

//Status Bin Trash
int sensorStatusMetal = 0; // 1 FULL || 0 Available || -1 Lỗi
int sensorStatusPlastic = 0; 
int sensorStatusOther = 0; 

// count Trash
int countStatusMetal = 0; 
int countStatusPlastic = 0; 
int countStatusOther = 0; 

// Lenh HMI gui xuong
#define Cmd_BatDauQuyTrinh "Cmd_BatDauQuyTrinh"
#define Cmd_KetThucQuyTrinh "Cmd_KetThucQuyTrinh" // Dung gui len/gui xuong

// Lenh gui HMI
#define Cmd_CountRac "Cmd_CountRac"
#define Cmd_DayRacKimLoai "Cmd_DayRacKimLoai"
#define Cmd_DayRacNhua "Cmd_DayRacNhua"
#define Cmd_DayRacKhongXacDinh "Cmd_DayRacKhongXacDinh

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

    Serial.printf("\n----- mySerialESP_MEGA  commands end -----\n");
    //MEGA 2560
    Serial2.begin(9600, SERIAL_8N1, RXD3, TXD3);

}

void loop() {
    // Get the current page number
    uint8_t pageNum = dwc.getPage();
    //Serial.printf("Current page: %d\n", pageNum);
    processPage(pageNum);
    delay(700);

 
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
  int isPutTrash = 0;
  String command= "";
// Function to process different pages
void processPage(int pageNum) {
    switch(pageNum) {
        case 1:
              command = Serial2.readStringUntil('\r'); 
                    Serial.print(command);

              if (command.startsWith("Cmd_DayRacKimLoai")) {
                    //Metal
                    dwc.setAddress(0x9010, 0x1010);
                    dwc.setUiType(ASCII);
                    dwc.setColor(0xF9C5);
                    dwc.sendData("LCONG");                       
              }

         
            // Platic
//dwc.setAddress(0x9020, 0x1020);
           // dwc.setUiType(ASCII);
          //  dwc.setColor(0x058E);
//dwc.sendData("Available");
            //Other
            //dwc.setAddress(0x9030, 0x1030);
           // dwc.setUiType(ASCII);
           // dwc.setColor(0x058E);
           // dwc.sendData("Available");
            
            break;
        case 2:
        if (isPutTrash == 0)
        {
             Serial2.println(Cmd_BatDauQuyTrinh);
             Serial.println(Cmd_BatDauQuyTrinh);
             isPutTrash = 1;
        }
             command = Serial2.readStringUntil('\r'); 
            if (command.startsWith("Cmd_CountRac")) {
                  command.trim(); // Loại bỏ ký tự xuống dòng "\n" ở cuối chuỗi

                  // Chia chuỗi theo dấu ';'
                  String values[4];  // Mảng lưu các giá trị sau khi chia
                  int index = 0;

                  // Lặp qua chuỗi và chia bằng ký tự ';'
                  int startIndex = 0;
                  int endIndex = command.indexOf(';');

                  while (endIndex != -1) {
                    values[index++] = command.substring(startIndex, endIndex);
                    startIndex = endIndex + 1;
                    endIndex = command.indexOf(';', startIndex);
                  }

                  // Thêm giá trị cuối cùng sau dấu chấm phẩy
                  values[index] = command.substring(startIndex);

                  // In kết quả
                  for (int i = 0; i < 4; i++) {
                    Serial.print("Value at index ");
                    Serial.print(i);
                    Serial.print(": ");
                    Serial.println(values[i]);


                  }
            }
            //delay(5000);
            //dwc.setPage(3);
            break;
        case 3:
            // Set address and send integer data for page 3
           // dwc.setAddress(0x3010, 0x1310);
           //dwc.setUiType(INT);
           // dwc.sendData(7);
           // delay(500);
            // Set address and send integer data for page 3
           // dwc.setAddress(0x3020, 0x1320);
            //dwc.setUiType(INT);
           // dwc.sendData(8);
            //delay(500);

           // Set address and send integer data for page 3
           // dwc.setAddress(0x3030, 0x1330);
           // dwc.setUiType(INT);
           // dwc.sendData(0);

            break;
        case 4:
            Serial2.println("Cmd_KetThucQuyTrinh");
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
            isPutTrash = 0;
            break;
    }
}

// Callback function for DWIN echo
void dwinEchoCallback(DWIN2 &d) {
    Serial.print("Echo  dwinEchoCallback:::");
    Serial.println(d.getDwinEcho());
}

#pragma endregion