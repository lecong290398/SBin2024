
#define dirPin 7 // STEP 1 DÙNG CHO BĂNG TẢI
#define stepPin 29

#define dirPin2 5 // STEP 2 DÙNG CHO TAY GẠT RÁC KIM LOẠI
#define stepPin2 23

#include <Servo.h>
Servo myservo;
int pos = 0;

// constants won't change. They're used here to set pin numbers:
const int buttonPin = 49; // CẢM BIẾN RÁC THẢI NHỰA
const int S_KIMLOAI = 48; // CẢM BIẾN RÁC THẢI KIM LOẠI
const int HOME = 47;      // CẢM BIẾN GỐC CỦA SERVO 1
const int HUMAN = 21;     // CẢM BIẾN NGƯỜI ĐỂ BẬT ĐÈN
const int RAC1 = A0;      // CẢM BIẾN ĐẦY THÙNG RÁC 1
const int RAC2 = A1;      // CẢM BIẾN ĐẦY THÙNG RÁC 2
const int RAC3 = A3;      // CẢM BIẾN ĐẦY THÙNG RÁC 3
const int VT_RAC1 = A4;   // CẢM BIẾN RÁC ĐÃ RƠI VÀO THÙNG RÁC 1
const int VT_RAC2 = A6;   // CẢM BIẾN RÁC ĐÃ RƠI VÀO THÙNG RÁC 2
const int VT_RAC3 = A7;   // CẢM BIẾN RÁC ĐÃ RƠI VÀO THÙNG RÁC 3
const int ledPin = A8;    // ĐÈN KHI CÓ NGƯỜI

// variables will change:
int buttonState = 0; // variable for reading the pushbutton status

// Phan config các hàm con chuẩn

#define TayGat_Step stepPin2 // STEP 2 DÙNG CHO TAY GẠT RÁC KIM LOẠI
#define TayGat_Dir dirPin2
#define TayGat_Dir_VeGoc HIGH
#define TayGat_Dir_DayRac LOW
#define TayGat_Rev 1600 // stepsPerRevolution
#define TayGat_Delay 500
#define TayGat_Goc HOME
#define TayGat_ChamGoc HIGH
#define TayGat_MaxXung 1600

static void TayGatVeGoc()
{
    // Set chiều tay gạt quay về gốc
    digitalWrite(TayGat_Dir, TayGat_Dir_VeGoc);

    // Di chuyển tay gạt về gốc
    while (true)
    {
        // Check đụng cảm biến thì dừng
        if (digitalRead(TayGat_Goc) == TayGat_ChamGoc)
        {
            break;
        }

        digitalWrite(TayGat_Step, HIGH);
        delayMicroseconds(TayGat_Delay);
        digitalWrite(TayGat_Step, LOW);
        delayMicroseconds(TayGat_Delay);
    }
}
static void TayGatDayRac()
{
    // Set chiều tay gạt quay về gốc
    digitalWrite(TayGat_Dir, TayGat_Dir_DayRac);

    for (int i = 0; i < TayGat_MaxXung; i++)
    {
        digitalWrite(TayGat_Step, HIGH);
        delayMicroseconds(TayGat_Delay);
        digitalWrite(TayGat_Step, LOW);
        delayMicroseconds(TayGat_Delay);
    }
}

#define BangTai_Step stepPin // STEP 1 DÙNG CHO BĂNG TẢI
#define BangTai_Dir dirPin
#define BangTai_Dir_RacKimLoai HIGH     // Hướng chạy qua trái về thùng rác kim loại
#define BangTai_Dir_RacKhongXacDinh LOW // Hướng chạy qua phải về thùng rác không xác định
#define BangTai_Rev 1600                // stepsPerRevolution
#define BangTai_Delay 500

static void BangTaiChay()
{
    digitalWrite(BangTai_Step, HIGH);
    delayMicroseconds(TayGat_Delay);
    digitalWrite(BangTai_Step, LOW);
    delayMicroseconds(TayGat_Delay);
}
static void BangTaiHuongThungRacKimLoai()
{
    digitalWrite(BangTai_Dir, BangTai_Dir_RacKimLoai);
}
static void BangTaiHuongThungRacKhongXacDinh()
{
    digitalWrite(BangTai_Dir, BangTai_Dir_RacKhongXacDinh);
}

#pragma region RC SERVO

#define RcServo_Pin 51
#define RcServo_GocMin 125
#define RcServo_GocMax 170
#define RcServo_Step 1
#define RcServo_Delay 55

// Hàm chung để điều chỉnh góc servo theo hướng (tăng hoặc giảm)
static void RcServoDieuChinhGoc(int gocMucTieu, bool tangGoc)
{
    int gocRcServo = myservo.read(); // Chỉ đọc một lần góc hiện tại

    // Điều chỉnh góc dựa trên biến tangGoc
    while ((tangGoc && gocRcServo < gocMucTieu) || (!tangGoc && gocRcServo > gocMucTieu))
    {
        // Tăng hoặc giảm góc dựa trên hướng di chuyển
        gocRcServo += (tangGoc ? RcServo_Step : -RcServo_Step);

        // Chuẩn hóa để góc không vượt quá giới hạn
        if (gocRcServo > RcServo_GocMax)
        {
            gocRcServo = RcServo_GocMax;
        }
        else if (gocRcServo < RcServo_GocMin)
        {
            gocRcServo = RcServo_GocMin;
        }

        // Cập nhật góc cho servo
        myservo.write(gocRcServo);

        // Sử dụng hệ thống timer thay vì delay() nếu cần tối ưu hơn
        delay(RcServo_Delay);
    }
}
// Hàm điều chỉnh về góc tối thiểu
static void RcServoVeGocMin()
{
    RcServoDieuChinhGoc(RcServo_GocMin, false); // false -> Giảm góc
}

// Hàm tăng góc đến giá trị tối đa
static void RcServoTangGoc()
{
    RcServoDieuChinhGoc(RcServo_GocMax, true); // true -> Tăng góc
}

#pragma endregion

#pragma region HUMAN

#define CamBienNguoi HUMAN
#define CamBienNguoi_On HIGH
#define DenCoNguoi ledPin
#define DenCoNguoi_On HIGH
#define DenCoNguoi_Off LOW
//- khi cảm biến có người (pin 46)  thì bật đèn lên (A8)
bool denDangBat = false;                   // Trạng thái hiện tại của đèn
unsigned long thoiGianBatDen = 0;          // Biến lưu thời gian khi đèn được bật
const unsigned long delayThoiGian = 10000; // 10 giây (10,000ms)

static void CoNguoiBatDen()
{

    // Đọc giá trị từ cảm biến
    bool coNguoi = digitalRead(CamBienNguoi) == CamBienNguoi_On;

    if (coNguoi)
    {
        // Nếu có người, bật đèn và cập nhật thời gian
        if (!denDangBat)
        {
            digitalWrite(DenCoNguoi, DenCoNguoi_On);
            denDangBat = true;
        }
        thoiGianBatDen = millis(); // Cập nhật thời gian bật đèn
    }
    else if (denDangBat && (millis() - thoiGianBatDen >= delayThoiGian))
    {
        // Nếu không còn người và đã đủ 10 giây thì tắt đèn
        digitalWrite(DenCoNguoi, DenCoNguoi_Off);
        denDangBat = false;
    }
}
#pragma endregion

#define CamBienRac VT_RAC1
#define CamBienRac_On LOW
static bool CoRac()
{
    return digitalRead(CamBienRac) == CamBienRac_On;
}

#define CamBienKimLoai S_KIMLOAI
#define CamBienKimLoai_On LOW
static bool RacKimLoai()
{
    return digitalRead(CamBienKimLoai) == CamBienKimLoai_On;
}

#define CamBienNhua buttonPin
#define CamBienNhua_On LOW
static bool RacNhua()
{
    return digitalRead(CamBienNhua) == CamBienNhua_On;
}

int countRacKimLoai = 0;
int countRacNhua = 0;
int countRacKhongXacDinh = 0;

static void ResetCountRac()
{
    countRacKimLoai = 0;
    countRacNhua = 0;
    countRacKhongXacDinh = 0;
}
static void Cong1RacKimLoai()
{
    countRacKimLoai++;
    GuiCmdCountRac();
}
static void Cong1RacNhua()
{
    countRacNhua++;
    GuiCmdCountRac();
}
static void Cong1RacKhongXacDinh()
{
    countRacKhongXacDinh++;
    GuiCmdCountRac();
}

#define CamBienThungRacKimLoai VT_RAC2
#define CamBienThungRacKimLoai_Off HIGH
int prevCamBienThungRacKimLoai = CamBienThungRacKimLoai_Off;
int currCamBienThungRacKimLoai;

static bool ThungRacKimLoaiCoRac()
{
    currCamBienThungRacKimLoai = digitalRead(CamBienThungRacKimLoai);
    // Cảm biến off
    if (currCamBienThungRacKimLoai == CamBienThungRacKimLoai_Off)
    {
        // Check xem trạng thái lần trước có on không
        // => nếu có là có rác rơi vào
        if (prevCamBienThungRacKimLoai != CamBienThungRacKimLoai_Off)
        {
            prevCamBienThungRacKimLoai = currCamBienThungRacKimLoai;
            return true;
        }
        else
        {
            prevCamBienThungRacKimLoai = currCamBienThungRacKimLoai;
        }
    }

    // Cảm biên on => ghi nhơ trạng thái
    else
    {
        prevCamBienThungRacKimLoai = currCamBienThungRacKimLoai;
    }

    return false;
}

#define CamBienThungRacKhongXacDinh VT_RAC3
#define CamBienThungRacKhongXacDinh_Off HIGH
int prevCamBienThungRacKhongXacDinh = CamBienThungRacKhongXacDinh_Off;
int currCamBienThungRacKhongXacDinh;

static bool ThungRacKhongXacDinhCoRac()
{
    currCamBienThungRacKhongXacDinh = digitalRead(CamBienThungRacKhongXacDinh);
    // Cảm biến off
    if (currCamBienThungRacKhongXacDinh == CamBienThungRacKhongXacDinh_Off)
    {
        // Check xem trạng thái lần trước có on không
        // => nếu có là có rác rơi vào
        if (prevCamBienThungRacKhongXacDinh != CamBienThungRacKhongXacDinh_Off)
        {
            prevCamBienThungRacKhongXacDinh = currCamBienThungRacKhongXacDinh;
            return true;
        }
        else
        {
            prevCamBienThungRacKhongXacDinh = currCamBienThungRacKhongXacDinh;
        }
    }
    // Cảm biên on => ghi nhơ trạng thái
    else
    {
        prevCamBienThungRacKhongXacDinh = currCamBienThungRacKhongXacDinh;
    }

    return false;
}

bool daGuiDayRacKimLoai = false;
bool daGuiDayRacNhua = false;
bool daGuiDayRacKhongXacDinh = false;
bool daGuiCmdKetThucQuyTrinh = false;

#define CamBienRacKimLoaiDay RAC1
#define CamBienRacKimLoaiDay_On LOW
static bool RacKimLoaiDay()
{
    if (digitalRead(CamBienRacKimLoaiDay) == CamBienRacKimLoaiDay_On)
    {
        return true;
    }
    else
    {
        daGuiDayRacKimLoai = false;
        return false;
    }
}

#define CamBienRacNhuaDay RAC2
#define CamBienRacNhuaDay_On LOW
static bool RacNhuaDay()
{
    if (digitalRead(CamBienRacNhuaDay) == CamBienRacNhuaDay_On)
    {
        return true;
    }
    else
    {
        daGuiDayRacNhua = false;
        return false;
    }
}

#define CamBienRacKhongXacDinhDay RAC3
#define CamBienRacKhongXacDinhDay_On LOW
static bool RacKhongXacDinhDay()
{
    if (digitalRead(CamBienRacKhongXacDinhDay) == CamBienRacKhongXacDinhDay_On)
    {
        return true;
    }
    else
    {
        daGuiDayRacKhongXacDinh = false;
        return false;
    }
}

#define LenhHmi_NoOp 0                      // Trạng thái không có lệnh gì
#define LenhHmi_Run 1                       // HMI báo chuyển trạng thái mở nắp
#define LenhHmi_Run_MaxTimeKhongCoRac 10000 // Thời gian tối đa liên tục không có rác
int lenhHmi = LenhHmi_NoOp;
unsigned long gioBatDauNhanLenhMoNap = 0;

static bool KiemTraTrangThaiChay()
{
    return lenhHmi == LenhHmi_Run;
}

static void KiemTraThoiGianKetThucQuyTrinh()
{
    if (abs(millis() - gioBatDauNhanLenhMoNap) > LenhHmi_Run_MaxTimeKhongCoRac)
    {
        GuiCmdKetThucQuyTrinh();
    }
}

// Khai báo lệnh HMI
constexpr const char *Cmd_BatDauQuyTrinh = "Cmd_BatDauQuyTrinh";
constexpr const char *Cmd_KetThucQuyTrinh = "Cmd_KetThucQuyTrinh"; // Dùng để gửi lên/gửi xuống
// Khai báo lệnh gửi HMI
constexpr const char *Cmd_CountRac = "Cmd_CountRac";
constexpr const char *Cmd_DayRacKimLoai = "Cmd_DayRacKimLoai";
constexpr const char *Cmd_DayRacNhua = "Cmd_DayRacNhua";
constexpr const char *Cmd_DayRacKhongXacDinh = "Cmd_DayRacKhongXacDinh";

#pragma region HMI Command Processing

static void CheckHmiCmd()
{
    if (Serial3.available())
    {
        String text = Serial3.readStringUntil('\r'); // Đọc dữ liệu từ Serial3

        // Gọi hàm để echo lại lệnh đã nhận
        EchoReceivedCommand(text);

        // Xử lý lệnh từ HMI
        if (text == Cmd_BatDauQuyTrinh)
        {
            ProcessStartCommand();
        }
        else if (text == Cmd_KetThucQuyTrinh)
        {
            ProcessEndCommand();
        }
        else
        {
            LogUnknownCommand(text);
        }
    }
}

static void EchoReceivedCommand(const String &text)
{
    String echo = String("Rcv:") + text + String(":hihi") + String(text.length());
    Serial.println(echo);
    Serial3.println(echo);
}

static void ProcessStartCommand()
{
    Serial.println(Cmd_BatDauQuyTrinh);
    lenhHmi = LenhHmi_Run;
    ResetThoiGianKhongCoRac();
    ResetCountRac();
    ResetDaGui();
}

static void ProcessEndCommand()
{
    Serial.println(Cmd_KetThucQuyTrinh);
    GuiCmdCountRac();
    lenhHmi = LenhHmi_NoOp;
}

static void LogUnknownCommand(const String &text)
{
    Serial.println("Unknown command");
    Serial.println(text);
    Serial.println(text.length());
}

static void ResetThoiGianKhongCoRac()
{
    gioBatDauNhanLenhMoNap = millis();
}

static void GuiCmdKetThucQuyTrinh()
{
    if (daGuiCmdKetThucQuyTrinh)
    {
        return;
    }

    SendCommand(Cmd_KetThucQuyTrinh);
    daGuiCmdKetThucQuyTrinh = true;
}

static void GuiCmdCountRac()
{
    String result = String(Cmd_CountRac) + ";" + countRacKimLoai + ";" + countRacNhua + ";" + countRacKhongXacDinh;
    SendCommand(result);
}

static void SendCommand(const String &command)
{
    Serial.println(command);
    Serial3.println(command);
}

#pragma endregion

#pragma region Full Trash Process

static void ResetDaGui()
{
    daGuiDayRacKimLoai = false;
    daGuiDayRacNhua = false;
    daGuiDayRacKhongXacDinh = false;
    daGuiCmdKetThucQuyTrinh = false;
}

static void GuiCmdDayRac(String cmdType, bool &daGuiFlag, int countRac)
{
    if (daGuiFlag)
    {
        return;
    }

    String result = cmdType + ";" + String(countRac);
    Serial.println(result);
    Serial3.println(result);
    daGuiFlag = true;
}

static void GuiCmdDayRacKimLoai()
{
    GuiCmdDayRac(Cmd_DayRacKimLoai, daGuiDayRacKimLoai, countRacKimLoai);
}

static void GuiCmdDayRacNhua()
{
    GuiCmdDayRac(Cmd_DayRacNhua, daGuiDayRacNhua, countRacNhua);
}

static void GuiCmdDayRacKhongXacDinh()
{
    GuiCmdDayRac(Cmd_DayRacKhongXacDinh, daGuiDayRacKhongXacDinh, countRacKhongXacDinh);
}

#pragma endregion

void setup()
{
    // Khởi tạo giao tiếp Serial
    Serial.begin(9600);
    Serial3.begin(9600);

    // Khởi tạo Servo
    KhoiTaoServo();

    // Khởi tạo LED và nút bấm
    KhoiTaoDenVaNut();

    // Khởi tạo cảm biến
    KhoiTaoCamBien();

    // Khởi tạo Stepper Motors
    KhoiTaoStepper();

    // Đặt hệ thống về trạng thái ban đầu
    TayGatVeGoc();                 // Đưa tay gạt về vị trí gốc
       myservo.write(RcServo_GocMax);
 // Đưa servo về góc 180 độ

    // Reset các trạng thái ban đầu
    ResetCountRac();
    ResetDaGui();
    ResetThoiGianKhongCoRac();
}

#pragma region Setup Function

// Hàm khởi tạo servo
void KhoiTaoServo()
{
    myservo.attach(RcServo_Pin); // Gắn chân điều khiển servo
}

// Hàm khởi tạo đèn LED và nút bấm
void KhoiTaoDenVaNut()
{
    pinMode(ledPin, OUTPUT);
    pinMode(buttonPin, INPUT);
}

// Hàm khởi tạo các cảm biến
void KhoiTaoCamBien()
{
    pinMode(S_KIMLOAI, INPUT);
    pinMode(HOME, INPUT);
    pinMode(HUMAN, INPUT);
    pinMode(RAC1, INPUT);
    pinMode(RAC2, INPUT);
    pinMode(RAC3, INPUT);
    pinMode(VT_RAC1, INPUT);
    pinMode(VT_RAC2, INPUT);
    pinMode(VT_RAC3, INPUT);
}

// Hàm khởi tạo stepper motors
void KhoiTaoStepper()
{
    pinMode(stepPin, OUTPUT);
    pinMode(dirPin, OUTPUT);
    pinMode(stepPin2, OUTPUT);
    pinMode(dirPin2, OUTPUT);

    // Đặt chiều quay mặc định của stepper
    digitalWrite(dirPin, HIGH); // Stepper 1 quay theo chiều kim đồng hồ
    digitalWrite(dirPin2, LOW); // Stepper 2 quay ngược chiều kim đồng hồ
}

#pragma endregion

void loop()
{
    CoNguoiBatDen(); // Kiểm tra cảm biến có người

    CheckHmiCmd(); // Kiểm tra lệnh từ HMI

    if (!KiemTraTrangThaiChay())
        return; // Nếu không có lệnh HMI, kết thúc sớm

    if (!CoRac()) // Nếu không có rác
    {
        KiemTraThoiGianKetThucQuyTrinh(); // Kiểm tra thời gian và kết thúc nếu cần
        return;
    }

    delay(200); // Chờ ngắn để xác định loại rác
                // Kiểm tra các loại rác đầy và xử lý chung

    KiemTraVaXuLyRacDay(); // Kiểm tra và xử lý rác đầy

    // Xử lý rác cụ thể
    if (RacKimLoai() && RacNhua())
    {
        if (!RacKimLoaiDay())
        {
            XuLyRacKimLoai();
        }
    }
    else if (RacNhua() && !RacNhuaDay())
    {
        if (!RacKimLoai())
        {
            XuLyRacNhua();
        }
    }
    else if (!RacKimLoai() && !RacNhua())
    {
        if (!RacKhongXacDinhDay())
        {
            XuLyRacKhongXacDinh();
        }
    }
}

#pragma region Process Trash

// Hàm kiểm tra và xử lý rác đầy
bool KiemTraVaXuLyRacDay()
{
    if (RacKimLoaiDay())
    {
        GuiCmdDayRacKimLoai();
        RcServoTangGoc();
        return true;
    }
    if (RacNhuaDay())
    {
        GuiCmdDayRacNhua();
        RcServoTangGoc();
        return true;
    }
    if (RacKhongXacDinhDay())
    {
        GuiCmdDayRacKhongXacDinh();
        RcServoTangGoc();
        return true;
    }
    return false;
}

// Hàm xử lý rác kim loại
void XuLyRacKimLoai()
{
    Serial.println("Rac kim loai");
    if (CoRac())
    {
        XuLyServoRac();                // Xử lý servo cho rác rơi ra ngoài
        BangTaiHuongThungRacKimLoai(); // Di chuyển băng tải
        do
        {
            BangTaiChay();
        } while (!ThungRacKimLoaiCoRac()); // Chờ cho rác rơi vào thùng
        Cong1RacKimLoai();         // Cập nhật số lượng rác
        ResetThoiGianKhongCoRac(); // Đặt lại thời gian không có rác
    }
}

// Hàm xử lý rác nhựa
void XuLyRacNhua()
{
    Serial.println("Rac nhưa");
    if (CoRac())
    {
        XuLyServoRac();            // Xử lý servo cho rác rơi ra ngoài
        TayGatDayRac();            // Di chuyển tay gạt rác nhựa
        Cong1RacNhua();            // Cập nhật số lượng rác
        TayGatVeGoc();             // Đưa tay gạt về góc ban đầu
        ResetThoiGianKhongCoRac(); // Đặt lại thời gian không có rác
    }
}

// Hàm xử lý rác không xác định
void XuLyRacKhongXacDinh()
{
    Serial.println("Rac khong xac dinh");

    if (CoRac())
    {
        XuLyServoRac();                     // Xử lý servo cho rác rơi ra ngoài
        BangTaiHuongThungRacKhongXacDinh(); // Di chuyển băng tải về hướng thùng không xác định
        do
        {
            BangTaiChay();
        } while (!ThungRacKhongXacDinhCoRac()); // Chờ cho rác rơi vào thùng
        Cong1RacKhongXacDinh();    // Cập nhật số lượng rác
        ResetThoiGianKhongCoRac(); // Đặt lại thời gian không có rác
    }
}

// Hàm xử lý servo cho rác rơi ra ngoài
void XuLyServoRac()
{
    do
    {
        RcServoVeGocMin(); // Đưa servo về góc 180 độ
    } while (CoRac()); // Lặp đến khi rác rơi ra ngoài
    RcServoTangGoc();
}
#pragma endregion
