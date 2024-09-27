#include <Arduino.h>
#include <Dwin2.h>
#include <freertos/FreeRTOS.h>
#include <freertos/task.h>

// Rx Tx ESP gpio connected to DWin Display
#define RX_PIN 16
#define TX_PIN 17

// Class for controlling UI elements of the display
DWIN2 dwc;

// Callback function to receive a response from the display
void dwinEchoCallback(DWIN2 &d);

// Task handles
TaskHandle_t displayTaskHandle = NULL;
TaskHandle_t pageTaskHandle = NULL;

void setup() {
    Serial.begin(115200);
    while (Serial.available()) {} // Wait until the serial buffer is empty
    Serial.printf("-------- Start DWIN communication demo --------\n");
    Serial.printf("-----------------------------------------------\n");

    const int delayTime = 1000;
    vTaskDelay(pdMS_TO_TICKS(delayTime));

    Serial.printf("\n----- Dwin Display Common commands start -----\n");
    // Initialize timers, tasks, serial communication, and other settings
    dwc.begin();
    // Set callback for receiving responses
    dwc.setUartCbHandler(dwinEchoCallback);
    // Disable echo for commands and responses
    dwc.setEcho(false);

    // Create FreeRTOS tasks
    xTaskCreate(displayTask, "Display Task", 2048, NULL, 1, &displayTaskHandle);
    xTaskCreate(pageTask, "Page Task", 2048, NULL, 1, &pageTaskHandle);
}

void displayTask(void *pvParameters) {
    while (1) {
        vTaskDelay(pdMS_TO_TICKS(700));
        // Get the current page number
        uint8_t pageNum = dwc.getPage();
        Serial.printf("Current page: %d\n", pageNum);
        xTaskNotify(pageTaskHandle, pageNum, eSetValueWithOverwrite);
    }
}

void pageTask(void *pvParameters) {
    uint32_t pageNum;
    while (1) {
        xTaskNotifyWait(0x00, 0xFFFFFFFF, &pageNum, portMAX_DELAY);
        processPage(pageNum);
    }
}

// Function to handle common operations for pages 4 and 5
void handlePage4or5(uint16_t address1, uint16_t address2, int idPage, int time) {
    dwc.setAddress(address1, address2);
    dwc.setUiType(INT);
    dwc.setStartVal(20);
    dwc.setLimits(0, time, false);
    for (int i = time; i >= 0; i--) {
        dwc.sendData(i);
        vTaskDelay(pdMS_TO_TICKS(1000));
        if (i == 1) {
            dwc.setPage(idPage);
        }
    }
}

// Function to process different pages
void processPage(int pageNum) {
    switch (pageNum) {
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
            vTaskDelay(pdMS_TO_TICKS(5000));
            dwc.setPage(3);
            break;
        case 3:
            // Set address and send integer data for page 3
            dwc.setAddress(0x3010, 0x1310);
            dwc.setUiType(INT);
            dwc.sendData(7);
            vTaskDelay(pdMS_TO_TICKS(500));

            dwc.setAddress(0x3020, 0x1320);
            dwc.setUiType(INT);
            dwc.sendData(8);
            vTaskDelay(pdMS_TO_TICKS(500));

            dwc.setAddress(0x3030, 0x1330);
            dwc.setUiType(INT);
            dwc.sendData(0);
            break;
        case 4:
            // Set address and send URL for page 4
            dwc.setAddress(0x9910, 0x1099);
            dwc.setUiType(ASCII);
            dwc.sendData("https://youtu.be/-bwemq005vw?si=Zv0bqV6s0lPpvimZ");
            // Handle operations for page 4
            handlePage4or5(0x4010, 0x1410, 5, 15);
            break;
        case 5:
            // Handle operations for page 5
            handlePage4or5(0x5010, 0x1510, 0, 10);
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

void loop() {
    // Empty loop as tasks are handled by FreeRTOS
}