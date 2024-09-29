#include <Servo.h>
#include <Arduino_FreeRTOS.h>

// Định nghĩa các pin
#define dirPin 30
#define stepPin 31
#define dirPin2 5
#define stepPin2 23

// Các cảm biến
const int buttonPin = 49;
const int S_KIMLOAI = 48;
const int HOME = 47;
const int HUMAN = 46;
const int RAC1 = A0;
const int RAC2 = A1;
const int RAC3 = A3;
const int VT_RAC1 = A4;
const int VT_RAC2 = A6;
const int VT_RAC3 = A7;
const int ledPin = A8;

// Đối tượng Servo
Servo myservo;
const int RcServo_Pin = 51;

// Trạng thái rác
enum {
    RACSANG,
    RACKIMLOAI,
    RACNHUA,
    RACKHONGXACDINH
};

volatile int countRacKimLoai = 0;
volatile int countRacNhua = 0;
volatile int countRacKhongXacDinh = 0;

// Lệnh HMI
enum {
    LENH_HMI_NO_OP,
    LENH_HMI_RUN
};
volatile int lenhHmi = LENH_HMI_NO_OP;

// Các hàm hỗ trợ
void TayGatVeGoc();
void TayGatDayRac();
void BangTaiChay();
void RcServoVeGocMin();
void CoNguoiBatDen();
void CheckHmiCmd();
void ResetCountRac();
void ResetDaGui();
void KiemTraThoiGianKetThucQuyTrinh();
void GuiCmdCountRac();
void GuiCmdDayRacKimLoai();
void GuiCmdDayRacNhua();
void GuiCmdDayRacKhongXacDinh();
void TaskCheckSensors(void *pvParameters);
void TaskControlServo(void *pvParameters);
void TaskHmiCommands(void *pvParameters);

void setup() {
    Serial.begin(9600);
    Serial3.begin(9600);
    myservo.attach(RcServo_Pin);
    pinMode(ledPin, OUTPUT);
    pinMode(buttonPin, INPUT);
    pinMode(S_KIMLOAI, INPUT);
    pinMode(HOME, INPUT);
    pinMode(HUMAN, INPUT);
    pinMode(RAC1, INPUT);
    pinMode(RAC2, INPUT);
    pinMode(RAC3, INPUT);
    pinMode(VT_RAC1, INPUT);
    pinMode(VT_RAC2, INPUT);
    pinMode(VT_RAC3, INPUT);
    pinMode(stepPin, OUTPUT);
    pinMode(dirPin, OUTPUT);
    pinMode(stepPin2, OUTPUT);
    pinMode(dirPin2, OUTPUT);

    // Khởi tạo
    digitalWrite(dirPin, HIGH);
    digitalWrite(dirPin2, LOW);
    TayGatVeGoc();
    myservo.write(0);
    ResetCountRac();
    ResetDaGui();

    // Tạo các tác vụ
    xTaskCreate(TaskCheckSensors, "CheckSensors", 1000, NULL, 1, NULL);
    xTaskCreate(TaskControlServo, "ControlServo", 1000, NULL, 1, NULL);
    xTaskCreate(TaskHmiCommands, "HmiCommands", 1000, NULL, 1, NULL);
}

void loop() {
    // FreeRTOS sẽ xử lý các tác vụ
}

void TaskCheckSensors(void *pvParameters) {
    while (true) {
        CoNguoiBatDen();
        if (RacKimLoai()) {
            countRacKimLoai++;
            GuiCmdCountRac();
        }
        vTaskDelay(200 / portTICK_PERIOD_MS); // Thay thế delay bằng vTaskDelay
    }
}

void TaskControlServo(void *pvParameters) {
    while (true) {
        if (lenhHmi == LENH_HMI_RUN) {
            // Điều khiển servo theo lệnh
            RcServoTangGoc(); // Hoặc gọi hàm khác tùy thuộc vào trạng thái
            vTaskDelay(100 / portTICK_PERIOD_MS);
        }
    }
}

void TaskHmiCommands(void *pvParameters) {
    while (true) {
        CheckHmiCmd();
        vTaskDelay(100 / portTICK_PERIOD_MS);
    }
}

void TayGatVeGoc() {
    digitalWrite(dirPin2, HIGH);
    while (digitalRead(HOME) != HIGH) {
        digitalWrite(stepPin2, HIGH);
        delayMicroseconds(500);
        digitalWrite(stepPin2, LOW);
        delayMicroseconds(500);
    }
}

void TayGatDayRac() {
    digitalWrite(dirPin2, LOW);
    for (int i = 0; i < 1600; i++) {
        digitalWrite(stepPin2, HIGH);
        delayMicroseconds(500);
        digitalWrite(stepPin2, LOW);
        delayMicroseconds(500);
    }
}

void CoNguoiBatDen() {
    if (digitalRead(HUMAN) == HIGH) {
        digitalWrite(ledPin, HIGH);
    } else {
        digitalWrite(ledPin, LOW);
    }
}

void ResetCountRac() {
    countRacKimLoai = 0;
    countRacNhua = 0;
    countRacKhongXacDinh = 0;
}

void ResetDaGui() {
    // Reset các biến đã gửi
}

void CheckHmiCmd() {
    if (Serial3.available()) {
        String command = Serial3.readStringUntil('\r');
        if (command == "Cmd_BatDauQuyTrinh") {
            lenhHmi = LENH_HMI_RUN;
        } else if (command == "Cmd_KetThucQuyTrinh") {
            lenhHmi = LENH_HMI_NO_OP;
        }
    }
}

void GuiCmdCountRac() {
    String result = "Cmd_CountRac;" + String(countRacKimLoai) + ";" + String(countRacNhua) + ";" + String(countRacKhongXacDinh);
    Serial3.println(result);
}
