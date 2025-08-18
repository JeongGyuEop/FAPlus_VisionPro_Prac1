using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.ToolBlock;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Xml.Schema;

namespace FAPlus.MainForm.Service
{
    public class CameraService : IDisposable
    {
        private CogAcqFifoTool cogAcqFifoTool;
        private bool _loop;
        public event Action<ICogImage> ImageReceived; // 이미지 획득 이벤트 

        // 프로퍼티
        private ICogAcqFifo Fifo => cogAcqFifoTool?.Operator;
        public bool IsConnected => Fifo?.FrameGrabber != null;
        public bool IsConfigured => Fifo != null;
        public string FrameGrabberName => Fifo?.FrameGrabber.Name;
        public string[] VideoFormats
        {
            get
            {
                var fg = Fifo?.FrameGrabber;
                if (fg == null) return Array.Empty<string>();
                var fg_arr = new string[fg.AvailableVideoFormats.Count];
                fg.AvailableVideoFormats.CopyTo(fg_arr, 0);
                return fg_arr;
            }
        }
        public string VideoFormat => Fifo?.VideoFormat;

        // ========== 노출/대비/밝기 설정(프로퍼티)
        public double? Exposure 
        {
            get => Fifo?.OwnedExposureParams?.Exposure;
            set 
            { 
                var p = Fifo?.OwnedExposureParams;
                if (p != null && value.HasValue) p.Exposure = value.Value;    
            }
        }
        public double? Brightness 
        {
            get => Fifo?.OwnedBrightnessParams?.Brightness;
            set
            {
                var p = Fifo?.OwnedBrightnessParams;
                if (p != null && value.HasValue) p.Brightness = value.Value;
            }
        }
        public double? Contrast 
        {
            get => Fifo?.OwnedContrastParams?.Contrast;
            set
            {
                var p = Fifo?.OwnedContrastParams;
                if (p != null && value.HasValue) p.Contrast = value.Value;
            }
        }

        // =========== 카메라 연결
        public void Connect()
        {
            if (cogAcqFifoTool == null) throw new InvalidOperationException("먼저 vpp를 로드하세요.");
            if (Fifo?.FrameGrabber != null) return; // 이미 연결

            var fgs = new CogFrameGrabbers();
            if (fgs.Count < 1) throw new Exception("프레임그래버가 없습니다.");

            var vf = Fifo?.VideoFormat ?? fgs[0].AvailableVideoFormats[0];

            var newFifo = fgs[0].CreateAcqFifo(vf, CogAcqFifoPixelFormatConstants.Format8Grey, 0, true);
            cogAcqFifoTool.Operator = newFifo;
        }
        public void SetVideoFormat(string videoFormat)  
        {  
            if (Fifo?.FrameGrabber == null) throw new InvalidOperationException("Connect 먼저 호출");
            
            _loop = false;
            Fifo.Complete -= OnComplete;

            var fg = Fifo.FrameGrabber;
            var newFifo = fg.CreateAcqFifo(videoFormat, CogAcqFifoPixelFormatConstants.Format8Grey, 0, true);
            cogAcqFifoTool.Operator = newFifo;
        }

        // ========== 라이브 디스플레이
        public ICogImage AcquireOnce()
        {
            if (_loop) return null;
            int trig;
            return Fifo.Acquire(out trig);
        } // 1회 촬영
        public void StartLive(CogDisplay display)
        {
            if (Fifo == null) throw new InvalidOperationException("Connect 먼저 호출");
            if (!display.LiveDisplayRunning) display.StartLiveDisplay(Fifo);
        }
        public void StopLive(CogDisplay display)
        {
            if (display.LiveDisplayRunning) display.StopLiveDisplay();
        }

        // ========== 1회 촬영 & 연속 촬영(검사용)
        public void CheckAcquireOnce()
        {
            if (_loop) return;      // 또는 예외/로그
            int trig;
            var img = Fifo.Acquire(out trig);
            ImageReceived?.Invoke(img);
        } // 1회 촬영
        public void StartLoop()
        {
            if (Fifo == null) return;
            Fifo.Complete -= OnComplete;
            Fifo.Complete += OnComplete;
            _loop = true;
            Fifo.StartAcquire();
        }
        public void StopLoop()
        {
            _loop = false;
            if (Fifo != null)
            {
                Fifo.Complete -= OnComplete;
            }
        }
        private void OnComplete(object s, CogCompleteEventArgs e)  
        {
            var fifo = Fifo;           
            if (fifo == null) return;
            int t, n;
            var img = fifo.CompleteAcquire(e.Ticket, out t, out n);
            ImageReceived?.Invoke(img);
            if (_loop) fifo.StartAcquire();
        }

        // ========== vpp 파일 저장 & 로드
        public void SaveVpp(string path)
        {
            if (cogAcqFifoTool == null || Fifo == null) throw new InvalidOperationException("카메라가 구성되지 않았습니다.");
            CogSerializer.SaveObjectToFile(cogAcqFifoTool, path, typeof(BinaryFormatter));
        }
        public void LoadVpp(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException("지정한 vpp 파일을 찾을 수 없습니다.", path);

            // 1) vpp 파일 로드
            var obj = CogSerializer.LoadObjectFromFile(path, typeof(BinaryFormatter));
            var acqTool = obj as CogAcqFifoTool;

            // 기존 객체 정리 후 교체
            cogAcqFifoTool?.Dispose();
            cogAcqFifoTool = acqTool;
        }

        // ========== 자원해제
        public void Dispose()
        {
            StopLoop();
            cogAcqFifoTool?.Dispose();
            cogAcqFifoTool = null;
        }
    }
}
