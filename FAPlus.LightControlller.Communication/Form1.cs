using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace FAPlus.LightControlller.Communication
{
    public partial class Form1 : Form
    {
        byte start = 0x01;
        byte op_code;
        byte data_length;
        byte address;
        byte data;
        byte end = 0x04;

        byte[] buffer;

        // 1. 소켓 생성
        Socket socket; // Ethernet
        IPEndPoint endPoint; // Ethernet

        SerialPort serialPort; //RS-232

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Socket_Connect();
            //RS232_Connect(); // RS-232
        }

        private void Socket_Connect()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            endPoint = new IPEndPoint(IPAddress.Parse("192.168.0.230"), 50000);

        }

        private void RS232_Connect()
        {
            //serialPort = new SerialPort("COM1", 19200, Parity.None, 8, StopBits.One); // RS-232
            //serialPort.Open();// RS-232
        }

        private void Receive_Call()
        {
            byte[] response = new byte[10];
            int length = socket.Receive(response); // 응답 수신 (예: 0x06)

            Console.WriteLine("수신된 바이트 수: " + length);
            Console.WriteLine("받은 데이터(hex): " + response[0].ToString("X2"));
        }

        private void OnButton_CheckedChanged(object sender, EventArgs e)
        {
            buffer = new byte[]
            {
                start,       // Start
                0x00,       // OP (Write)
                0x01,       // DL
                0x34,       // Addr (Output Enable 주소 예시)
                0x01,       // Data (1 = Enable)
                end        // End
            };

            socket.SendTo(buffer, endPoint); // Ethernet
            //serialPort.Write(buffer, 0, buffer.Length); // RS-232

            Receive_Call(); // 응답 수신
        }

        private void OffButton_CheckedChanged(object sender, EventArgs e)
        {
            buffer = new byte[]
            {
                start,       // Start
                0x00,       // OP (Write)
                0x01,       // DL
                0x34,       // Addr (Output Enable 주소 예시)
                0x02,       // Data (1 = Enable)
                end        // End
            };              

            socket.SendTo(buffer, endPoint); // Ethernet
            //serialPort.Write(buffer, 0, buffer.Length); // RS-232

            Receive_Call(); // 응답 수신
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (socket != null) socket.Close();
            if (serialPort != null && serialPort.IsOpen) serialPort.Close();
        }

        //// 1. CSR 설정 패킷 (채널 선택)
        //buffer = new byte[]
        //{
        //    start,
        //    0x00,       // OP
        //    0x01,       // DL
        //    0x20,       // Addr
        //    0x01,       // Data
        //    end        // End
        //};

        //// 2. SVR 설정 패킷 (밝기 설정)
        //byte[] svrPacket = new byte[]
        //{
        //    0x01,       // Start
        //    0x00,       // OP
        //    0x01,       // DL
        //    0x28,       // Addr
        //    0xFF,       // Data
        //    0x04        // End
        //};

    }
}
   