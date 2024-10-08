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
const int HUMAN = 46;     // CẢM BIẾN NGƯỜI ĐỂ BẬT ĐÈN
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
#define RcServo_GocMin 0
#define RcServo_GocMax 180
#define RcServo_Step 1
#define RcServo_Delay 10

static void RcServoVeGocMin()
{
    int gocRcServo = myservo.read();
    do
    {
        // Giảm dần góc servo theo step
        gocRcServo -= RcServo_Step;
        // Chuẩn hóa đẻ góc không bao giờ bé hơn min
        if (gocRcServo < RcServo_GocMin)
        {
            gocRcServo = RcServo_GocMin;
        }
        myservo.write(gocRcServo);
        delay(RcServo_Delay);
    } while (gocRcServo > RcServo_GocMin);
}
static void RcServoTangGoc()
{
    int gocRcServo = myservo.read();

    // Tăng dần góc servo theo step
    gocRcServo += RcServo_Step;
    // Chuẩn hóa đẻ góc không bao giờ lớn hơn max
    if (gocRcServo > RcServo_GocMax)
    {
        gocRcServo = RcServo_GocMax;
    }
    myservo.write(gocRcServo);
    delay(RcServo_Delay);
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

// Lenh HMI gui xuong
#define Cmd_BatDauQuyTrinh "Cmd_BatDauQuyTrinh"
#define Cmd_KetThucQuyTrinh "Cmd_KetThucQuyTrinh" // Dung gui len/gui xuong

// Lenh gui HMI
#define Cmd_CountRac "Cmd_CountRac"
#define Cmd_DayRacKimLoai "Cmd_DayRacKimLoai"
#define Cmd_DayRacNhua "Cmd_DayRacNhua"
#define Cmd_DayRacKhongXacDinh "Cmd_DayRacKhongXacDinh"

#define Cmd_test "Cmd_test"

static void CheckHmiCmd()
{
    if (Serial3.available())
    { // nếu PC có gửi dữ liệu đến

        String text = Serial3.readStringUntil('\r'); // đọc các giá trị đó cho đến khi gặp kí tự xuống dòng là \n

        // Echo báo đã nhận lệnh
        String echo = String("Rcv:") + text + String(":hihi") + String(text.length());
        Serial.println(echo);
        Serial3.println(echo);

        if (text == Cmd_BatDauQuyTrinh)
        {

            Serial.println(Cmd_BatDauQuyTrinh);

            lenhHmi = LenhHmi_Run;
            ResetThoiGianKhongCoRac();
            ResetCountRac();
            ResetDaGui();
        }
        else if (text == Cmd_KetThucQuyTrinh)
        {
            Serial.println(Cmd_KetThucQuyTrinh);
            GuiCmdCountRac();
            lenhHmi = LenhHmi_NoOp;
        }
        else
        {
            Serial.println("Unknow command");
            Serial.println(text);
            Serial.println(text.length());
        }
    }
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

    String result = Cmd_KetThucQuyTrinh;
    Serial.println(result);
    Serial3.println(result);
    daGuiCmdKetThucQuyTrinh = true;
}
static void GuiCmdCountRac()
{
    String result = Cmd_CountRac + String(";") + countRacKimLoai + String(";") + countRacNhua + String(";") + countRacKhongXacDinh;
    Serial.println(result);
    Serial3.println(result);
}

static void ResetDaGui()
{
    daGuiDayRacKimLoai = false;
    daGuiDayRacNhua = false;
    daGuiDayRacKhongXacDinh = false;
    daGuiCmdKetThucQuyTrinh = false;
}
static void GuiCmdDayRacKimLoai()
{
    if (daGuiDayRacKimLoai)
    {
        return;
    }

    String result = Cmd_DayRacKimLoai + String(";") + countRacKimLoai;
    Serial.println(result);
    Serial3.println(result);
    daGuiDayRacKimLoai = true;
}
static void GuiCmdDayRacNhua()
{
    if (daGuiDayRacNhua)
    {
        return;
    }

    String result = Cmd_DayRacNhua + String(";") + countRacNhua;
    Serial.println(result);
    Serial3.println(result);
    daGuiDayRacNhua = true;
}
static void GuiCmdDayRacKhongXacDinh()
{
    if (daGuiDayRacKhongXacDinh)
    {
        return;
    }

    String result = Cmd_DayRacKhongXacDinh + String(";") + countRacKhongXacDinh;
    Serial.println(result);
    Serial3.println(result);
    daGuiDayRacKhongXacDinh = true;
}

void setup()
{
    Serial.begin(9600);
    Serial3.begin(9600);

    myservo.attach(RcServo_Pin); // CON SERVO NÈ

    // initialize the LED pin as an output:
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

    digitalWrite(dirPin, HIGH); // SET CHIEU QUAY MẶT ĐỊNH CỦA STEP
    digitalWrite(dirPin2, LOW);

    //- bật máy lên thì cho step 2 chạy về vị trí gốc chạm vào cảm biến home ( pin 47) thì dừng lại+ RC servo ( pin 51) ở góc 0 độ.
    TayGatVeGoc();
    myservo.write(RcServo_GocMin);

    ResetCountRac();
    ResetDaGui();
    ResetThoiGianKhongCoRac();
}

void loop()
{
    //- khi cảm biến có người (pin 46)  thì bật đèn lên (A8)
    CoNguoiBatDen();

    // Kiểm tra tín hiệu từ HMI
    CheckHmiCmd();

    // Không có lệnh của hmi gửi lệnh mở nắp xuống esp
    if (!KiemTraTrangThaiChay())
    {
        return;
    }

    // Không có rác
    if (!CoRac())
    {
        //- sau khi chờ 1 khoản thời gian mà cảm biến (pin A4)
        // không xác định có rác trong hộc phân loại thì kết thúc quy trình
        // và gửi tính hiệu về esp để hmi hiển thị cái QR code
        KiemTraThoiGianKetThucQuyTrinh();
    }

    // Xử lý khi có rác
    // Chờ 2 s đe xác định loại rác;
    delay(100);

    // Check rac đầy và xử lý
    //- khi cảm biến(pin A0 A1 A3) xác định rác đầy thì báo về server
    // và không cho RC servo(pin 51) mở ra và báo trên màn hình loại rác này đã đầy.
    if (RacKimLoaiDay())
    {
        GuiCmdDayRacKimLoai();
        RcServoVeGocMin();
    }
    //- khi cảm biến(pin A0 A1 A3) xác định rác đầy thì báo về server
    // và không cho RC servo(pin 51) mở ra và báo trên màn hình loại rác này đã đầy.
    if (RacNhuaDay())
    {
        GuiCmdDayRacNhua();
        RcServoVeGocMin();
    }
    //- khi cảm biến(pin A0 A1 A3) xác định rác đầy thì báo về server
    // và không cho RC servo(pin 51) mở ra và báo trên màn hình loại rác này đã đầy.
    if (RacKhongXacDinhDay())
    {
        GuiCmdDayRacKhongXacDinh();
        RcServoVeGocMin();
    }

    //+nếu cảm biến phát hiện là kim loại ( pin 48 )
    if (RacKimLoai() && RacNhua() && !RacKimLoaiDay())
    {
        Serial.println("Rac kim loai");
        if (CoRac())
        {

            // RC servo chuyển thành góc lớn dần ( bé hơn 180 độ)
            // đến khi nào cảm biến ( pin A4) xác nhận rác đã rơi ra ngoài
            // thì sau đó thì chuyển về góc 0 độ
            do
            {
                RcServoTangGoc();
            } while (CoRac());
            RcServoVeGocMin();

            // sau khi rác rơi xuống băng tải thì step 1 chạy sang bên trái
            // đến khi cảm biến rác đã rơi vào thùng(pin A6) on rồi off
            // là xong 1 quy trình phân loại
            BangTaiHuongThungRacKimLoai();
            do
            {
                BangTaiChay();
            } while (!ThungRacKimLoaiCoRac());

            Cong1RacKimLoai();

            // Sau xử lý xong thì bắt đầu đếm thời gian không có rác
            ResetThoiGianKhongCoRac();
        }
    }
    //+ tương tự cảm biến rác thải nhựa ( pin 49)
    else if (RacNhua() && RacNhuaDay() && !RacKimLoai())
    {
        if (CoRac())
        {
            // RC servo chuyển thành góc lớn dần ( bé hơn 180 độ)
            // đến khi nào cảm biến ( pin A4) xác nhận rác đã rơi ra ngoài
            // thì sau đó thì chuyển về góc 0 độ
            do
            {
                RcServoTangGoc();
            } while (CoRac());
            RcServoVeGocMin();

            // step 1 không cần di chuyển step 2 từ vị trí home di chuyển 1 số xung rồi quay về vị trí home
            TayGatDayRac();

            Cong1RacNhua();

            // Day rac xong ve home lai
            TayGatVeGoc();

            // Sau xử lý xong thì bắt đầu đếm thời gian không có rác
            ResetThoiGianKhongCoRac();
        }
    }
    //+ nếu cả 2 không xác định được loại rác nhưng cảm biến ( pin A4) xác nhận có rác
    else if (!RacKimLoai() && !RacNhua() && !RacKhongXacDinhDay())
    {
        if (CoRac())
        {
            // RC servo chuyển thành góc lớn dần ( bé hơn 180 độ)
            // đến khi nào cảm biến ( pin A4) xác nhận rác đã rơi ra ngoài
            // thì sau đó thì chuyển về góc 0 độ
            do
            {
                RcServoTangGoc();
            } while (CoRac());
            RcServoVeGocMin();

            // sau khi rác rơi xuống băng tải thì step 1 chạy sang bên phải đến khi cảm biến rác đã rơi
            BangTaiHuongThungRacKhongXacDinh();
            do
            {
                BangTaiChay();
            } while (!ThungRacKhongXacDinhCoRac());

            Cong1RacKhongXacDinh();

            // Sau xử lý xong thì bắt đầu đếm thời gian không có rác
            ResetThoiGianKhongCoRac();
        }
    }
}