using FAPlus.MainForm.Service;
using System;
using System.Windows.Forms;

namespace FAPlus.AquisitionCamera
{
    public partial class AquisitionCameraForm : Form
    {
        private readonly CameraService _camera;

        public AquisitionCameraForm(CameraService camera)
        {
            InitializeComponent(); // WinForm UI 구성
            _camera = camera;
            _camera.LoadVpp(@"vpp\camera.vpp");
            if (!_camera.IsConnected) _camera.Connect();

            BoardTypeLabel.Text = _camera.FrameGrabberName ?? "(프레임그래버 없음)";

            // 비디오 포맷 콤보 박스는 항상 채움
            VideoFormatCombo.Items.Clear();
            VideoFormatCombo.Items.AddRange(_camera.VideoFormats);


            if (_camera.IsConfigured)
            {
                VideoFormatCombo.SelectedIndexChanged -= VideoFormatCombo_SelectedIndexChanged;
                VideoFormatCombo.SelectedItem = _camera.VideoFormat;
                VideoFormatCombo.SelectedIndexChanged += VideoFormatCombo_SelectedIndexChanged;

                // 이제서야 Live 시작 (구성이 보장된 뒤)
                _camera.StartLive(cogDisplay1);

                if (_camera.Exposure.HasValue) exposureUpDown.Value = (decimal)_camera.Exposure.Value;
                if (_camera.Brightness.HasValue) brightnessUpDown.Value = (decimal)_camera.Brightness.Value;
                if (_camera.Contrast.HasValue) contrastUpDown.Value = (decimal)_camera.Contrast.Value;
            }

            //>>>>>>>>>
            //_camera.StartLive(cogDisplay1);
            //>>>>>>>>>

        } // 폼 로드 시 초기화

        //=====================
        private void OkButton_Click(object sender, EventArgs e)
        {
            if (!_camera.IsConfigured)
            {
                MessageBox.Show("비디오 포맷을 선택하여 카메라 설정을 완료하세요.");
                return;
            }
            this.DialogResult = DialogResult.OK;
            
            _camera.SaveVpp(@"vpp\camera.vpp");
            Close();
        } // 카메라 저장 버튼
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        } // 취소 버튼

        //=====================
        private void VideoFormatCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("비디오 포맷 설정 호출");
            // LiveDisplay 중지
            //>>>>>>>>>
            _camera.StopLoop();
            //>>>>>>>>>
            _camera.StopLive(cogDisplay1);
            _camera.SetVideoFormat(VideoFormatCombo.SelectedItem?.ToString());
            _camera.StartLive(cogDisplay1); // VideoFormat 변경되면 카메라 라이브 미리보기

            // 파라미터 초기값 표시
            if (_camera.Exposure.HasValue) exposureUpDown.Value = (decimal)_camera.Exposure.Value;
            if (_camera.Brightness.HasValue) brightnessUpDown.Value = (decimal)_camera.Brightness.Value;
            if (_camera.Contrast.HasValue) contrastUpDown.Value = (decimal)_camera.Contrast.Value;

        } // 비디오 포맷 값 변경 시 이벤트
        private void exposureUpDown_ValueChanged(object sender, EventArgs e) 
            => _camera.Exposure = (double)exposureUpDown.Value; // 노출값이 변경될 때 호출되는 이벤트 핸들러
        private void brightnessUpDown_ValueChanged(object sender, EventArgs e)
            => _camera.Brightness = (double)brightnessUpDown.Value; // 밝기 값이 변경될 때 호출되는 이벤트 핸들러
        private void contrastUpDown_ValueChanged(object sender, EventArgs e)
            => _camera.Contrast = (double)contrastUpDown.Value; // 대비 값이 변경될 때 호출되는 이벤트 핸들러 

        //====================
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _camera.StopLive(cogDisplay1);
            base.OnFormClosed(e);
        }

    }
}
