using System;
using System.IO.Ports; // RS-232 통신용 SerialPort 클래스
using System.Net;
using System.Net.Sockets; // Ethernet UDP 통신용 Socket
using System.Threading; // Thread.Sleep() 사용
using System.Windows.Forms; // Winform UI 관련

/*
    1. enum (열거형)
        - 상태나 옵션을 명확하게 표현할 때 사용
        - 문자열 비교보다 타입 안정성이 있고, 자동 완성으로 실수 방지

    2. '?.' 이란
        - null-safe 호출로 객체가 null일 경우 호출을 무시한다.

    3. 이벤트 호출 시 object sender & EventArgs e
        - sender : 이벤트를 발생시킨 객체
        - e : 이벤트와 관련된 데이터

    4. 이벤트 호출 시 += 연산자 (델리게이트)
    
 */

namespace FAPlus.LightControlller.Communication
{
    public partial class LightCommunicationForm : Form
    {
        Socket socket; // Ethernet UDP 통신용 소켓 
        IPEndPoint endPoint; // 목적지 IP와 port
        SerialPort serialPort; //RS-232 통신용 객체

        Panel[] channelPanels; // 채널 패널 배열
        Panel[] functionPanels; // 기능 패널 배열
        CheckBox[] checkChannels; // 채널 체크박스 
        NumericUpDown[] numericControls; // 밝기 조절 컨트롤

        public LightCommunicationForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // UI 컨트롤들을 배열에 매핑
            channelPanels = new Panel[] { panel1, panel2, panel3, panel4 };
            functionPanels = new Panel[] { panel5, panel6, panel7, panel8 };
            checkChannels = new CheckBox[] { Ch_1, Ch_2, Ch_3, Ch_4 };
            numericControls = new NumericUpDown[] { Ch1_LSize, Ch2_LSize, Ch3_LSize, Ch4_LSize };

            // ComboBox 통신 방식 항목
            comboCommType.Items.AddRange(Enum.GetNames(typeof(CommMode))); // 여러 개 항목을 한 번에 추가
            comboCommType.SelectedIndex = 0; // 기본값 Ethernet 
            comboCommType.SelectedIndexChanged += ComboCommType_SelectedIndexChanged; // 콤보박스 항목이 변경될 때 발생하는 이벤트

            // 밝기 조절 이벤트 바인딩
            Ch1_LSize.ValueChanged += Ch_LSize_ValueChanged;
            Ch2_LSize.ValueChanged += Ch_LSize_ValueChanged;
            Ch3_LSize.ValueChanged += Ch_LSize_ValueChanged;
            Ch4_LSize.ValueChanged += Ch_LSize_ValueChanged;

            InitSocket(); // 소켓 초기화
            InitializeLights(); // 조명 상태 초기화

        } // 폼 로드 시 초기 설정(자동 호출 메서드)

        // =======================================================================
        // 통신 방식에 따라 Enum으로 구분 및 통신 방식 변경 이벤트
        public enum CommMode
        {
            Ethernet, // index 0
            RS232     // index 1

        } // 사용자 정의 열거형
        CommMode currentMode = CommMode.Ethernet; // 현재 통신 모드를 Ethernet으로 초기화
        private void ComboCommType_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentMode = (CommMode)comboCommType.SelectedIndex;

            switch (currentMode)
            {
                case CommMode.Ethernet:
                    InitSocket();
                    serialPort?.Close(); // 기존에 열린 시리얼 포트가 있다면 닫음
                    break;

                case CommMode.RS232:
                    InitRS232();
                    socket?.Close(); // 기존에 소켓이 있다면 닫음
                    break;
            }
            
        } // 통신 방식 변경 이벤트

        // =======================================================================
        // Socket, RS232, 조명 초기화
        private void InitSocket()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            endPoint = new IPEndPoint(IPAddress.Parse("192.168.0.230"), 50000);
        } // 소켓 초기화
        private void InitRS232()
        {
            try
            {
                serialPort = new SerialPort("COM1", 19200, Parity.None, 8, StopBits.One);
                serialPort.Open();
                Console.WriteLine("RS-232 연결 성공");
            }
            catch (Exception ex)
            {
                MessageBox.Show("RS-232 연결 실패: " + ex.Message);
            }
        } // RS232 초기화
        private void InitializeLights()
        {
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    byte addr = (byte)(0x28 + i); // CH1~CH4 밝기 주소
                    SendPacket(addr, 0x00); // 밝기 0으로 설정
                    Thread.Sleep(10);
                    functionPanels[i].Enabled = false;
                }

                SendPacket(0x34, 0x02);  // 조명 OFF
                SendPacket(0x20, 0x00); // 채널 선택 초기화

            }
            catch (Exception ex)
            {
                Console.WriteLine("초기화 오류: " + ex.Message);
            }
        } // 조명 초기화

        // =======================================================================
        // 패킷 전송 및 송신 결과 확인
        private void SendPacket(byte address, byte data)
        {
            byte[] buffer = new byte[]
            {
                0x01,
                0x00,           
                0x01,           
                address,
                data,
                0x04
            };

            if (currentMode == CommMode.Ethernet)
            {
                socket?.SendTo(buffer, endPoint);
            }
            else if (currentMode == CommMode.RS232)
            {
                serialPort?.Write(buffer, 0, buffer.Length);
            }

        } // 패킷 전송
        private void Receive_Call()
        {
            byte[] response = new byte[10];

            if (currentMode == CommMode.Ethernet)
            {
                try
                {
                    socket.ReceiveTimeout = 500;
                    int length = socket.Receive(response);
                    Console.WriteLine($"[UDP 응답] 수신된 바이트 수: {length}, 데이터(hex): {BitConverter.ToString(response, 0, length)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("UDP 응답 수신 실패: " + ex.Message);
                }
            }
            else if (currentMode == CommMode.RS232)
            {
                try
                {
                    int length = serialPort.Read(response, 0, response.Length);
                    Console.WriteLine($"[RS-232 응답] 수신된 바이트 수: {length}, 데이터(hex): {BitConverter.ToString(response, 0, length)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("RS-232 응답 수신 실패: " + ex.Message);
                }
            }
        } // 송신 결과 확인

        // =======================================================================
        // Channel별 CheckBox 이벤트
        private void Ch_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                UpdateChannelSelection();
            }
        }
        private void UpdateChannelSelection()
        {
            byte value = 0x00;

            for (int i = 0; i < 4; i++)
            {
                if (checkChannels[i].Checked)
                {
                    value |= (byte)(1 << i);
                    functionPanels[i].Enabled = true;

                    // 밝기 설정 추가 (주소: 0x28 + i)
                    byte brightness = (byte)numericControls[i].Value;
                    byte addr = (byte)(0x28 + i);
                    SendPacket(addr, brightness);
                    Thread.Sleep(10); // 너무 빠르면 오류 발생할 수 있어 약간의 딜레이 추가
                }
                else functionPanels[i].Enabled = false;
            }

            SendPacket(0x20, value);

            Receive_Call(); // 응답 확인
        }

        // =======================================================================
        // 조명 On 이벤트
        private void OnBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton radio && radio.Checked)
            {
                if (byte.TryParse(radio.Tag?.ToString(), out byte channel))
                {
                    byte brightnessAddr = (byte)(0x28 + (channel - 1));
                    SendPacket(brightnessAddr, 0xFF); // 밝기 255

                    SendPacket(0x34, 0x01); // Output Enable: ON
                    Console.WriteLine($"CH{channel} 조명 ON");
                    Receive_Call();
                }
            }
        }

        // =======================================================================
        // 조명 Off 이벤트
        private void OffBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton radio && radio.Checked)
            {
                if (byte.TryParse(radio.Tag?.ToString(), out byte channel))
                {
                    SendPacket(0x34, 0x02); // Output Enable: OFF
                    Console.WriteLine($"CH{channel} 조명 OFF");
                    Receive_Call();
                }
            }
        }

        // =======================================================================
        // 조명 밝기값 변경 이벤트
        private void Ch_LSize_ValueChanged(object sender, EventArgs e)
        {
            if (sender is NumericUpDown numeric)
            {
                for (int i = 0; i < numericControls.Length; i++)
                {
                    if (numericControls[i] == numeric && checkChannels[i].Checked)
                    {
                        byte brightness = (byte)numeric.Value;
                        byte addr = (byte)(0x28 + i); // CH1: 0x28, CH2: 0x29, ...
                        SendPacket(addr, brightness);
                        Console.WriteLine($"CH{i + 1} 밝기 변경 → {brightness}");
                        Thread.Sleep(10); // 딜레이
                        break;
                    }
                }
            }
        }

        // =======================================================================
        // 자원해제
        private void End_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. 조명 OFF 및 초기화
                for (int i = 0; i < 4; i++)
                {
                    byte addr = (byte)(0x28 + i); // 밝기 주소
                    SendPacket(addr, 0x00); // 밝기 0
                    Thread.Sleep(10);
                }

                SendPacket(0x34, 0x02); // Output Enable OFF
                Thread.Sleep(10);
                SendPacket(0x20, 0x00); // 채널 선택 해제
                Thread.Sleep(10);

                Console.WriteLine("모든 채널 OFF 및 밝기 초기화 완료");

                // Ethernet Socket 해제
                socket?.Close();
                socket?.Dispose();

                // RS-232 해제
                if (serialPort != null)
                {
                    if (serialPort.IsOpen)
                        serialPort.Close();
                    serialPort.Dispose();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("종료 중 리소스 정리 오류: " + ex.Message);
            }
        }
    }
}


