using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.ToolBlock;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace FAPlus.MainForm.Service
{
    public class CameraService : IDisposable
    {
        private CogAcqFifoTool;
        private ICogFrameGrabber _frameGrabber;
        private ICogAcqFifo _acqFifo;
        private ICogAcqExposure _exposure;
        private ICogAcqBrightness _brightness;
        private ICogAcqContrast _contrast;
        private bool _roop = false;

        public event Action<ICogImage> ImageReceived; // 이미지 획득 이벤트

        public bool IsConnected => _frameGrabber != null;
        public bool IsConfigured => _acqFifo != null;
        public string FrameGrabberName => _frameGrabber.Name;
        public string[] VideoFormats
        {
            get
            {     
                if (_frameGrabber == null) return Array.Empty<string>();
                var videoFormat_arr = new string[_frameGrabber.AvailableVideoFormats.Count];
                _frameGrabber.AvailableVideoFormats.CopyTo(videoFormat_arr, 0);
                return videoFormat_arr;
            }
        }
        public string VideoFormat { get; private set; }

        // ========== 노출/대비/밝기 설정
        public double? Exposure 
        {
            get { return _exposure.Exposure; }
            private set { _exposure.Exposure = value.Value; } 
        }
        public double? Brightness 
        { 
            get { return _brightness?.Brightness; }  
            private set { _brightness.Brightness = value.Value; } 
        }
        public double? Contrast 
        {
            get { return _contrast?.Contrast; }
            set { _contrast.Contrast = value.Value; } 
        }

        // =========== 카메라 연결
        public void Connect(string vf = null)
        {
            if (_acqFifo != null) return;
            var frameGrabbers = new CogFrameGrabbers();

            if (frameGrabbers.Count < 1) throw new Exception("프레임그래버가 없습니다.");
            _frameGrabber = frameGrabbers[0];

            Console.WriteLine("프레임그래버 설정");
        }
        public void SetVideoFormat(string videoFormat)  
        {  
            if (_frameGrabber == null) throw new InvalidOperationException("Connect 먼저 호출");
            
            _roop = false;
            if (_acqFifo != null)
            {
                _acqFifo.Complete -= OnComplete; 
                if (_acqFifo is IDisposable d) d.Dispose(); 
                _acqFifo = null;
            }

            _acqFifo = _frameGrabber.CreateAcqFifo(videoFormat, CogAcqFifoPixelFormatConstants.Format8Grey, 0, true);
            CacheParams();
            VideoFormat = videoFormat;
        }
        private void CacheParams()
        {
            _exposure = _acqFifo.OwnedExposureParams;
            _brightness = _acqFifo.OwnedBrightnessParams;
            _contrast = _acqFifo.OwnedContrastParams;
        }

        // ========== 라이브 디스플레이
        public ICogImage AcquireOnce()
        {
            if (_roop) return null;
            int trig;
            return _acqFifo.Acquire(out trig);
        } // 1회 촬영
        public void StartLive(CogDisplay display)
        {
            if (_acqFifo == null) throw new InvalidOperationException("Connect 먼저 호출");
            if (!display.LiveDisplayRunning) display.StartLiveDisplay(_acqFifo);
        }
        public void StopLive(CogDisplay display)
        {
            if (display.LiveDisplayRunning) display.StopLiveDisplay();
        }

        // ========== 1회 촬영 & 연속 촬영(검사용)
        public void CheckAcquireOnce()
        {
            if (_roop) return;      // 또는 예외/로그
            int trig;
            var img = _acqFifo.Acquire(out trig);
            ImageReceived?.Invoke(img);
        } // 1회 촬영
        public void StartLoop()
        {
            if (_acqFifo == null) return;
            _acqFifo.Complete -= OnComplete;
            _acqFifo.Complete += OnComplete;
            _roop = true;
            _acqFifo.StartAcquire();
        }
        public void StopLoop()
        {
            _roop = false;
            if (_acqFifo != null)
            {
                _acqFifo.Complete -= OnComplete;
            }
        }
        private void OnComplete(object s, CogCompleteEventArgs e)  
        {
            var fifo = _acqFifo;           
            if (fifo == null) return;
            int t, n;
            var img = fifo.CompleteAcquire(e.Ticket, out t, out n);
            ImageReceived?.Invoke(img);
            if (_roop) fifo.StartAcquire();
        }

        // ========== vpp 파일 저장 & 로드
        public void SaveVpp(string path)
        {
            Console.WriteLine("vpp 저장 호출");

            if (_acqFifo == null) throw new InvalidOperationException("카메라가 구성되지 않았습니다.");

            var acqTool = new CogAcqFifoTool();
            acqTool.Operator = _acqFifo; // VideoFormat, Exposure, Brightness, Contrast 등 상태 직렬화
             
            // ToolBlock에 담기
            var toolBlock = new CogToolBlock();
            toolBlock.Tools.Add(acqTool);

            CogSerializer.SaveObjectToFile(toolBlock, path, typeof(BinaryFormatter));
        }
        public void LoadVpp(string path)
        {
            Console.WriteLine("vpp 로드 호출");

            if (!File.Exists(path))
                throw new FileNotFoundException("지정한 vpp 파일을 찾을 수 없습니다.", path);

            // 1) vpp 파일 로드
            var obj = CogSerializer.LoadObjectFromFile(path, typeof(BinaryFormatter));

            // 2) CogToolBlock으로 변환
            var toolBlock = obj as CogToolBlock;
            if (toolBlock == null)
                throw new InvalidOperationException("vpp 파일에 CogToolBlock이 없습니다.");

            // 3) CogAcqFifoTool 찾기
            CogAcqFifoTool acqTool = null;

            // Tools 컬렉션에서 첫 번째 CogAcqFifoTool 찾기
            foreach (ICogTool tool in toolBlock.Tools)
            {
                if (tool is CogAcqFifoTool fifoTool)
                {
                    acqTool = fifoTool;
                    break;
                }
            }

            if (acqTool == null)
                throw new InvalidOperationException("vpp 파일에 CogAcqFifoTool이 포함되어 있지 않습니다.");

            // 4) _acqFifo에 연결
            _acqFifo = acqTool.Operator;

            VideoFormat = _acqFifo.VideoFormat;
            _exposure = _acqFifo.OwnedExposureParams;
            _brightness = _acqFifo.OwnedBrightnessParams;
            _contrast = _acqFifo.OwnedContrastParams;

            Console.WriteLine("[VPP에서 불러온 카메라 설정]");
            Console.WriteLine($"VideoFormat : {_acqFifo.VideoFormat}");
            Console.WriteLine($"Exposure    : {Exposure}");
            Console.WriteLine($"Brightness  : {Brightness}");
            Console.WriteLine($"Contrast    : {Contrast}");
        }

        // ========== 자원해제
        public void Dispose()
        {
            StopLoop();
            if (_acqFifo is IDisposable d) d.Dispose();
            _acqFifo = null;
            if (_frameGrabber is IDisposable d2) d2.Dispose();
            _frameGrabber = null;
        }
    }
}
