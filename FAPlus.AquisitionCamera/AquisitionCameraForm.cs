using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.Exceptions;
using System;
using System.Windows.Forms;

namespace FAPlus.AquisitionCamera
{
    public partial class AquisitionCameraForm : Form
    {
        private CogAcqFifoTool fifoTool;               // VisionPro에서 이미지 획득을 위한 고수준 도구형 객체 (내부에 ICogAcqFifo(Operator)포함)
        private ICogFrameGrabber mFrameGrabber = null; // 하드웨어 카메라 보드와 연결을 담당하는 프레임그래버 객체
        private ICogAcqFifo mAcqFifo = null;           // 카메라에서 이미지를 획득하는 저수준 획득 객체
        private int acqCount = 0;                      // 이미지 획득 횟수 기록 (GC 트리거용)
        private ICogAcqBrightness mBrightness;         // 카메라의 밝기(Brightness) 파라미터 제어 객체
        private ICogAcqContrast mContrast;             // 카메라의 대비(Contrast) 파라미터 제어 객체
        private ICogAcqExposure mExposure;             // 카메라의 노출 시간(Exposure Time)을 제어하는 객체
        private string selectedFormat = "";            // 이전에 선택했던 비디오 포맷 저장용

        public AquisitionCameraForm()
        {
            //this.FormClosing += Form1_FormClosing;
            //this.FormClosed += Form1_FormClosed;
        }  // 기본 생성자

        public AquisitionCameraForm(bool init)
        {
            InitializeComponent(); // WinForm UI 구성
            InitializeAcquisition(init);
        } // 폼 로드 시 초기화

        private void InitializeAcquisition(bool init)
        {
            try
            {
                if (init) // 만약 처음 창이 열리는 것이면
                {
                    CogFrameGrabbers mFrameGrabbers = new CogFrameGrabbers(); // 현재 시스템에 연결된 모든 프레임그래버 목록 가져옴
                    mFrameGrabber = mFrameGrabbers[0]; // 첫 번째 프레임그래버를 사용하여 객체 초기화

                    fifoTool = new CogAcqFifoTool(); // 이미지 획득용 도구 객체 생성 (고수준)
                    mAcqFifo = fifoTool.Operator; // 내부 Operator(ICogAcqFifo) 획득기 설정

                    // 내부 Operator가 null 이거나, 프레임그래버가 없으면 예외처리
                    if (mFrameGrabbers.Count < 1) 
                        throw new CogAcqNoFrameGrabberException("프레임그래버를 찾을 수 없습니다.");

                    BoardTypeLabel.Text = mAcqFifo.FrameGrabber.Name; // 연결된 프레임그래버(카메라보드) 이름 표시

                    // 사용 가능한 비디오 포맷 목록을 콤보박스에 추가
                    VideoFormatCombo.Items.Clear();
                    foreach (string format in mFrameGrabber.AvailableVideoFormats)
                        VideoFormatCombo.Items.Add(format);

                    // 첫 번째 포맷 선택
                    selectedFormat = mFrameGrabber.AvailableVideoFormats[0];

                    // 초기 UI 상태 설정
                    SetControlEnabled(false);
                } 
                else // 카메라 연결 Form이 2번 이상 열린 경우
                {
                    VideoFormatCombo.SelectedItem = selectedFormat;

                    // 초기 UI 상태 설정
                    SetControlEnabled(true);
                }


                // 콤보박스 선택 이벤트 등록
                VideoFormatCombo.SelectedIndexChanged -= VideoFormatCombo_SelectedIndexChanged;
                VideoFormatCombo.SelectedIndexChanged += VideoFormatCombo_SelectedIndexChanged;

                // Fifo 생성 및 UI 설정
                CreateAcqFifoWithParams(selectedFormat);
                UpdateCameraParamsUI();

            }
            catch (CogAcqException ex)
            {
                MessageBox.Show("카메라가 연결되지 않았습니다: " + ex.Message);
            }

        } // 프로그램 시작 시 카메라 연결 및 초기화

        private void SetControlEnabled(bool enabled)
        {
            exposureUpDown.Enabled = enabled;
            brightnessUpDown.Enabled = enabled;
            contrastUpDown.Enabled = enabled;
        } // 코드 중복 제거

        private void CreateAcqFifoWithParams(string videoFormat)
        {
            try
            {
                // LiveDisplay 중지
                if (cogDisplay1.LiveDisplayRunning)
                {
                    cogDisplay1.StopLiveDisplay();
                }

                // 기존 AcqFifo가 있다면 해제
                if (mAcqFifo is IDisposable disposable)
                {
                    disposable.Dispose();
                    mAcqFifo = null;
                }

                // 새 FIFO 생성
                mAcqFifo = mFrameGrabber.CreateAcqFifo(
                    videoFormat,
                    CogAcqFifoPixelFormatConstants.Format8Grey,
                    0,
                    true
                );
            }
            catch (CogAcqCannotCreateFifoException ex)
            {
                MessageBox.Show("FIFO 생성 실패: " + ex.Message);
            }
        }

        private void UpdateCameraParamsUI()
        {
            // ----- 노출 값 설정 -----
            // 노출 설정 불가능할 경우 라벨로 알림 표시
            mExposure = mAcqFifo.OwnedExposureParams; // 노출 파라미터 객체 얻기
            if (mExposure != null)
            {
                exposureUpDown.Value = (decimal)mExposure.Exposure;
            }
            else
            {
                exposureUpDown.Visible = false;
                lblNoExposure.Visible = false;
            }

            // ----- 밝기 값 설정 -----
            // 밝기 설정 불가능할 경우 라벨로 알림 표시
            mBrightness = mAcqFifo.OwnedBrightnessParams; // 밝기 파라미터 객체 얻기
            if (mBrightness != null)
            {
                brightnessUpDown.Value = (decimal)mBrightness.Brightness;
            }
            else
            {
                brightnessUpDown.Visible = false;
                lblNoBrightness.Visible = true;
            }

            // ----- 대비 값 설정 -----
            // 대비 설정 불가능할 경우 라벨로 알림 표시
            mContrast = mAcqFifo.OwnedContrastParams; // 대비 파라미터 객체 얻기
            if (mContrast != null)
            {
                contrastUpDown.Value = (decimal)mContrast.Contrast;
            }
            else
            {
                contrastUpDown.Visible = false;
                lblNoContrast.Visible = true;
            }
        }


        //==================================================================================================
        // 촬영 이벤트
        private void AcqOnce_Click(object sender, EventArgs e)
        {
            try
            {
                // 라이브 디스플레이가 남아있지 않은 상태여야 안전하게 해제 가능
                if (cogDisplay1.LiveDisplayRunning) cogDisplay1.StopLiveDisplay();

                // 기존 AcqFifo가 있다면 해제
                (mAcqFifo as IDisposable)?.Dispose();

                // 사용 가능한 비디오 포맷 중 첫 번째 선택
                string videoFormat = mFrameGrabber.AvailableVideoFormats[0];
                CreateAcqFifoWithParams(videoFormat);

                // 이미지 획득 및 디스플레이에 표시
                int triggerNum;
                cogDisplay1.Image = mAcqFifo.Acquire(out triggerNum);

                acqCount++;
                if (acqCount >= 5)
                {
                    GC.Collect(); // 메모리 정리
                    acqCount = 0;
                }
            }
            catch (CogException cogEx)
            {
                MessageBox.Show("이미지 획득 중 오류 발생 : " + cogEx.Message);
            }

        } // 1회 촬영 버튼을 클릭한 경우

        private void LiveOn_Btn(object sender, EventArgs e)
        {
            if (cogDisplay1.LiveDisplayRunning)
            {
                Acq_Once.Enabled = true;
                SetControlEnabled(false);

                // 라이브 디스플레이 정지
                cogDisplay1.StopLiveDisplay();
            }
            else
            {
                Acq_Once.Enabled = false; 
                SetControlEnabled(true);

                // 라이브 디스플레이 시작
                cogDisplay1.StartLiveDisplay(mAcqFifo);
            }
        } // Live On 버튼 클릭 시


        //==================================================================================================
        // 콤보 박스 & 속성 값 변경 시 이벤트
        private void VideoFormatCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 선택된 비디오 포맷으로 AcqFifo 생성
            selectedFormat = VideoFormatCombo.SelectedItem.ToString();

            CreateAcqFifoWithParams(selectedFormat);
            UpdateCameraParamsUI();

            Acq_Once.Enabled = true;

            cogDisplay1.StartLiveDisplay(mAcqFifo);

            // 컨트롤 상태 변경
            SetControlEnabled(true);
            Acq_Once.Enabled = false;
        } // 비디오 포맷 콤보박스 변경될 때 호출되는 이벤트 핸들러

        private void exposureUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (mExposure == null) return;

            if (exposureUpDown.Value <= 1000 && exposureUpDown.Value >= 0)
            {
                mExposure.Exposure = (double)exposureUpDown.Value;
            }
            else
            {
                exposureUpDown.Value = 8M;
                mExposure.Exposure = 8;
            }
        } // 노출 값이 변경될 때 호출되는 이벤트 핸들러

        private void brightnessUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (mBrightness == null) return;

            if (brightnessUpDown.Value <= 1 && brightnessUpDown.Value >= 0)
            {
                mBrightness.Brightness = (double)brightnessUpDown.Value;
            }
            else
            {
                brightnessUpDown.Value = 0.06M;
                mBrightness.Brightness = 0.06;
            }
        } // 밝기 값이 변경될 때 호출되는 이벤트 핸들러

        private void contrastUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (mContrast == null) return;

            if (contrastUpDown.Value <= 1 && contrastUpDown.Value >= 0)
            {
                mContrast.Contrast = (double)contrastUpDown.Value;
            }
            else   
            {
                contrastUpDown.Value = 0.2M;
                mContrast.Contrast = 0.2;
            }
        } // 대비 값이 변경될 때 호출되는 이벤트 핸들러 


        //==================================================================================================
        // 자원 해제
        //private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    // LiveDisplay가 실행 중이면 중지
        //    if (cogDisplay1.LiveDisplayRunning)
        //    {
        //        cogDisplay1.StopLiveDisplay();
        //    }
        //} // 폼 종료 이전에 실행

        //private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    try
        //    {
        //        // AcqFifo 해제
        //        if (mAcqFifo is IDisposable disposable)
        //        {
        //            disposable.Dispose();
        //            mAcqFifo = null;
        //        }

        //        // 프레임그래버 연결 해제
        //        mFrameGrabber?.Disconnect(false);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("폼 종료 중 오류 발생 : " + ex.Message);
        //    }

        //} // 폼 종료 시 


        public ICogAcqFifo PublicAcqFifo => mAcqFifo;            // FIFO 접근용
        //public ICogFrameGrabber PublicFrameGrabber => mFrameGrabber; // FrameGrabber 접근용

    }
}
