#include <Arduino.h>
#include <Dwin2.h>
#include <WiFi.h>

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

// Lenh HMI gui xuong
#define Cmd_BatDauQuyTrinh "Cmd_BatDauQuyTrinh"
#define Cmd_KetThucQuyTrinh "Cmd_KetThucQuyTrinh"

// Lenh gui HMI
#define Cmd_CountRac "Cmd_CountRac"
#define Cmd_DayRacKimLoai "Cmd_DayRacKimLoai"
#define Cmd_DayRacNhua "Cmd_DayRacNhua"
#define Cmd_DayRacKhongXacDinh "Cmd_DayRacKhongXacDinh"

int isPutTrash = 0;
int isEndTrash = 0;
String command = "";

// Queue to hold data between tasks
QueueHandle_t commandQueue;

void dwinEchoCallback(DWIN2 &d);

// Task prototypes
void TaskMega2560Communication(void *pvParameters);
void TaskUpdateUI(void *pvParameters);

void setup()
{
  Serial.begin(115200);
  Serial.printf("-------- Start DWIN communication RUN --------\n");
  dwc.begin();
  dwc.setUartCbHandler(dwinEchoCallback);
  dwc.setEcho(false);

  Serial.printf("-------- Start Mega 2560 communication RUN --------\n");
  Serial2.begin(9600, SERIAL_8N1, RXD3, TXD3);

  // Create queue to hold commands from Mega 2560
  commandQueue = xQueueCreate(5, sizeof(String));

  // Create tasks
  xTaskCreate(TaskMega2560Communication, "TaskMega2560Communication", 2048, NULL, 1, NULL);
  xTaskCreate(TaskUpdateUI, "TaskUpdateUI", 2048, NULL, 1, NULL);
}

void loop()
{
  // No need to put code in loop, everything is handled by FreeRTOS tasks
}

void TaskMega2560Communication(void *pvParameters)
{
  while (1)
  {
    // Read command from Mega 2560
    String commandStatusBinTrash = Serial2.readStringUntil('\r');
    
    // Process the command and send to the queue
    if (!commandStatusBinTrash.isEmpty())
    {
      xQueueSend(commandQueue, &commandStatusBinTrash, portMAX_DELAY);
    }
    vTaskDelay(100 / portTICK_PERIOD_MS); // Non-blocking delay
  }
}

void TaskUpdateUI(void *pvParameters)
{
  String command;
  while (1)
  {
    // Wait for command from queue
    if (xQueueReceive(commandQueue, &command, portMAX_DELAY) == pdTRUE)
    {
      // Process command
      if (command.startsWith(Cmd_DayRacKimLoai))
      {
        sensorStatusBinMetal = 1;
      }
      else if (command.startsWith(Cmd_DayRacNhua))
      {
        sensorStatusBinPlastic = 1;
      }
      else if (command.startsWith(Cmd_DayRacKhongXacDinh))
      {
        sensorStatusBinOther = 1;
      }

      // Update UI for bins
      setStatusMetal();
      setStatusPlastic();
      setStatusOther();
    }
    vTaskDelay(200 / portTICK_PERIOD_MS); // Delay to avoid overwhelming updates
  }
}

void setStatusMetal()
{
  dwc.setAddress(0x9010, 0x1010);
  dwc.setUiType(ASCII);
  if (sensorStatusBinMetal)
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

void setStatusPlastic()
{
  dwc.setAddress(0x9020, 0x1020);
  dwc.setUiType(ASCII);
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

void setStatusOther()
{
  dwc.setAddress(0x9030, 0x1030);
  dwc.setUiType(ASCII);
  if (sensorStatusBinOther)
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

void dwinEchoCallback(DWIN2 &d)
{
  Serial.print("Echo dwinEchoCallback::: ");
  Serial.println(d.getDwinEcho());
}
