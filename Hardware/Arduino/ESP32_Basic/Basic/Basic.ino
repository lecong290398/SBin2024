#include <Arduino.h>
#include <Dwin2.h>
#include <WiFi.h>
#include <string>

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
}

void loop()
{
    // Get the current page number
    uint8_t pageNum = dwc.getPage();
    // Process the current page
    processPage(pageNum);
    // Delay for a while
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
    }
    else if (commandString.startsWith(Cmd_DayRacNhua))
    {
        Serial.print("GET Cmd_DayRacNhua");
        sensorStatusBinPlastic = 1; // Updated variable name
    }
    else if (commandString.startsWith(Cmd_DayRacKhongXacDinh))
    {
        Serial.print("GET Cmd_DayRacKhongXacDinh");
        sensorStatusBinOther = 1; // Updated variable name
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

#pragma region PAGE 1
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
            Serial.print("GET Cmd_KetThucQuyTrinh 22222");
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
            Serial.print("GET Cmd_KetThucQuyTrinh 33333");
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
            isEndTrash = 1;

            // API Create QR Code and Create Transaction Bin

            // Set address and send ASCII data for page 4
            dwc.setAddress(0x9910, 0x1099);
            dwc.setUiType(ASCII);
            // SET QR CODE
            dwc.sendData("https://youtu.be/-bwemq005vw?si=Zv0bqV6s0lPpvimZ");
            // Handle operations for page 4
            handlePage4or5(0x4010, 0x1410, 5, 10);
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