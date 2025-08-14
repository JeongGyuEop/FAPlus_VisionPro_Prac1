using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.ImageFile;
using FAPlus.AquisitionCamera;
using FAPlus.MainForm.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FAPlus.MainForm
{
    public partial class Form1 : Form
    {
        private PatternToolService _patternService = new PatternToolService();
        private readonly CameraService _camera = new CameraService();
        private readonly RegionManager _regionManager = new RegionManager();

        private ImageList imageList = new ImageList();
        private List<string> imageFiles = new List<string>(); // 이미지 폴더에서 불러온 파일 경로들의 리스트

        private ICogImage currentImage; // 현재 화면에 표시된 이미지이자 검사 대상 이미지

        private int currentImageIndex = 0; // 현재 보여지고 있는 이미지가 리스트 내에서 몇 번째인지 저장
        private bool check = false; // 이미지 넘길 때 자동으로 Check_pattern()을 수행할지 여부를 결정하는 플래그
        private bool firstOpenCamera  = true;
        private bool isLiveInspectionRunning = false;

        //========================
        // 폼 Load & Close
        public Form1() {  InitializeComponent(); }
        private void Form1_Load(object sender, EventArgs e)
        {
            imageList.ImageSize = new Size(100, 100); // 썸네일 크기 설정 (가로 100px, 세로 100px)

            imageListView.View = View.LargeIcon; // ListView를 썸네일 보기 모드 설정
            imageListView.LargeImageList = imageList; // ListView에 이미지 리스트 연결

            imageListView.SelectedIndexChanged += ImageListView_SelectedIndexChanged; // 사용자가 썸네일을 클릭했을 때 이벤트
            _regionManager.RoiChanged += (roi, axes) =>
            {
                showImage.StaticGraphics.Remove("centerPoint");
                showImage.StaticGraphics.Add(axes, "centerPoint");
            };

            toolRun.Enabled = false;
            Acq_Once.Enabled = false;
            liveOnOffBtn.Enabled = false;
            SearchRegion.Enabled = false;
            roi_Btn.Enabled = false;
            panel1.Enabled = false;
            Check_Stop.Enabled = false;
            PatternVppSave.Enabled = false;
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e) => _camera.Dispose();

        // =======================
        // 이미지 파일 로드
        private void LoadImage_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    // 사용자가 선택한 폴더 경로를 가져온다.(예: "C:\Images")
                    string selectedFolder = folderBrowserDialog.SelectedPath;

                    // 배열 형태의 문자열 리스트 : 지원하는 이미지 파일 확장자 목록
                    // 소문자로 비교하기 위해 확장자를 전부 소문자로 정의한다.
                    string[] supportedExtensions = new[] { ".bmp", ".jpg", ".jpeg", ".png" };

                    imageFiles = Directory.GetFiles(selectedFolder)
                                          .Where(file => supportedExtensions.Contains(Path.GetExtension(file).ToLower()))
                                          .ToList();

                    currentImageIndex = 0; // 처음 이미지를 보여주기 위해 현재 인덱스를 0으로 초기화

                    // 이미지 리스트와 ListView 초기화
                    imageList.Images.Clear();
                    imageListView.Items.Clear();

                    // 파일들을 하나씩 순회하며 썸네일 생성 및 ListView에 추가
                    for(int i=0; i<imageFiles.Count; i++)
                    {
                        using (Image img = Image.FromFile(imageFiles[i]))
                        {
                            Image thumb = img.GetThumbnailImage(100, 100, () => false, IntPtr.Zero);
                            imageList.Images.Add(thumb);
                        }

                        imageListView.Items.Add(new ListViewItem(Path.GetFileName(imageFiles[i]),i));
                    }

                    if (imageFiles.Count > 0)
                    {
                        panel1.Enabled = true;

                        // 첫 이미지 표시
                        LoadImageByIndex(currentImageIndex, false);
                        imageListView.Items[currentImageIndex].Selected = true; // 선택 상태도 표시

                        Acq_Once.Enabled = false;
                        liveOnOffBtn.Enabled = false;
                        SearchRegion.Enabled = true;
                        roi_Btn.Enabled = true;
                        connectCamera.Text = "카메라 연결";

                        firstOpenCamera = true;

                        CheckInit_Display();
                    }

                    
                }
            }

            showImage.StaticGraphics.Clear();
            showImage.InteractiveGraphics.Clear();

        }// 이미지를 로드하는 버튼
        private void NextImage_Click(object sender, EventArgs e)
        {
            imageListView.SelectedItems.Clear();
            currentImageIndex++;
            if(currentImageIndex > imageFiles.Count - 1)
            {
                currentImageIndex = 0;
            }
            imageListView.Items[currentImageIndex].Selected = true; 

        } // 다음 이미지를 보여주기 위한 버튼
        private void BeforeImage_Click(object sender, EventArgs e)
        {
            imageListView.SelectedItems.Clear();
            currentImageIndex--;
            if (currentImageIndex < 0)
            {
                currentImageIndex = imageFiles.Count - 1;
            }
            imageListView.Items[currentImageIndex].Selected = true;

        } // 이전 이미지를 보여주기 위한 버튼
        private void LoadImageByIndex(int imageIndex, bool check)
        {
            using (CogImageFile cogImageFile = new CogImageFile())
            {
                // 선택된 이미지 경로를 열어서 읽기 모드로 설정ClearDisplay(showImage, true);
                cogImageFile.Open(imageFiles[imageIndex], CogImageFileModeConstants.Read);

                // 읽어온 이미지 파일 중 첫 번째 이미지 객체를 currentImage로 저장
                // 대부분의 경우 1개의 이미지만 존재하므로 [0] 사용
                currentImage = cogImageFile[0];
            }
                
            if (check) { Check_pattern(currentImage); }
            else
            {
                showImage.Fit(true);
                ClearGraphic_Display(showImage, currentImage); 
            }

        } // 여러 장의 이미지를 로드하기 위한 함수
        private void ImageListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (imageListView.SelectedIndices.Count == 0) return;

            currentImageIndex = imageListView.SelectedIndices[0];
            LoadImageByIndex(currentImageIndex, check);

        // 선택된 항목이 화면에 보이도록 자동 스크롤
        imageListView.EnsureVisible(currentImageIndex);

        } // 썸네일 이미지의 선택된 인덱스가 변경될 때 이벤트
       
        // =======================
        // 타이머 설정
        private void AutoPlay_Click(object sender, EventArgs e)
        {
            if (imageFiles == null || imageFiles.Count == 0) return;

            playTimer.Enabled = false;
            beforeImage.Enabled = false;
            nextImage.Enabled = false;

            setTime.Interval = (int)(playTimer.Value * 1000);

            // 타이머 시작
            setTime.Start();

        } // Auto 검사를 위한 Auto Play 버튼
        private void StopAutoPlay_Click(object sender, EventArgs e)
        {
            setTime.Stop();
            playTimer.Enabled = true;
            beforeImage.Enabled = true;
            nextImage.Enabled = true;
        } // Auto 검사를 중지하기 위한 Stop Auto Play 버튼
        private void SetTime_Tick(object sender, EventArgs e)
        {
            try
            {
                // 중복 방지 -> 추후에 변경
                setTime.Stop();

                // 다음 이미지 인덱스로 이동
                currentImageIndex++;

                // 이미지 끝까지 간 경우 다시 처음으로
                if (currentImageIndex >= imageFiles.Count) { currentImageIndex = 0; }

                // ListView 선택 갱신 (자동 스크롤 포함)
                imageListView.SelectedItems.Clear();
                imageListView.Items[currentImageIndex].Selected = true;
            }
            catch (Exception ex) // 예외 처리
            {
                MessageBox.Show("오류 발생: " + ex.Message);
            }
            finally
            {
                setTime.Start(); // 타이머 재시작 (AtuoPlay 유지)
            }
        } // Auto Play 타이머가 일정 시간마다 실행될 때 호출되는 이벤트 핸들러

        // =======================
        // 디스플레이 설정
        private void ClearGraphic_Display(CogDisplay display, ICogImage currentImage = null) // 디스플레이 그래픽 초기화 함수
        {
            display.StaticGraphics.Clear();
            display.InteractiveGraphics.Clear();
            display.Image = currentImage != null ? currentImage : null;
            display.Fit(true);
        }
        private void CheckInit_Display()
        {
            toolRun.Enabled = false; // 검사 버튼 비활성화

            ClearGraphic_Display(trainDisplay);
            ClearGraphic_Display(resultDisplay);

            resultLabel.Text = ""; // 검사 결과 텍스트 초기화
        } // ROI, 검색 영역 설정에 대한 검사 디스플레이 초기화

        // ======================
        // 카메라
        private void connectCamera_Click(object sender, EventArgs e)
        {
            _camera.StopLive(showImage);
            liveOnOffBtn.Text = "라이브 시작";

            // 설정 폼을 서비스와 함께 띄움
            var configured = false;
            using (var dlg = new AquisitionCameraForm(_camera, firstOpenCamera))
            {
                var result = dlg.ShowDialog(this);
                configured = (result == DialogResult.OK) && _camera.IsConfigured;
            }
            
            if (!configured) return;

            Acq_Once.Enabled = liveOnOffBtn.Enabled = true;
            connectCamera.Text = "카메라 연결 수정";
            imageListView.Clear();
            panel1.Enabled = false;
            imageFiles.Clear();
            firstOpenCamera = false;
        }
        private void liveOnOffBtn_Click(object sender, EventArgs e)
        {
            if (!check)
            {
                if (showImage.LiveDisplayRunning)
                {
                    _camera.StopLive(showImage);
                    liveOnOffBtn.Text = "라이브 시작";
                    Acq_Once.Enabled = true;
                }
                else
                {
                    _camera.StartLive(showImage);
                    liveOnOffBtn.Text = "라이브 종료";
                    Acq_Once.Enabled = false;
                    SearchRegion.Enabled = false;
                    roi_Btn.Enabled = false;
                }
            }
            else
            {
                if (isLiveInspectionRunning)
                {
                    _camera.StopLoop();
                    liveOnOffBtn.Text = "연속 촬영 검사 시작";
                    isLiveInspectionRunning = false;
                }
                else
                {
                    _camera.StartLoop();
                    liveOnOffBtn.Text = "연속 촬영 검사 종료";
                    isLiveInspectionRunning = true;
                }
            }
        }
        private void AcqOnce_Click(object sender, EventArgs e)
        {   
            // 라이브 검사가 진행중일 때 경우
            if (check)
            {
                if (isLiveInspectionRunning)
                {
                    _camera.StopLoop();
                    isLiveInspectionRunning = false;
                    liveOnOffBtn.Text = "연속 촬영 검사 시작";
                }
                _camera.CheckAcquireOnce();
            }
            else
            {
                if (showImage.LiveDisplayRunning)
                {
                    _camera.StopLive(showImage);
                }
                SearchRegion.Enabled = roi_Btn.Enabled = true;
                liveOnOffBtn.Text = "라이브 시작";
                var img = _camera.AcquireOnce();
                showImage.Image = img;
                currentImage = img;
            }
        }

        // =======================
        // 패턴 Load & Save
        private void PatternVppLoad_Click(object sender, EventArgs e)
        {
            _patternService.LoadVpp(@"vpp\pattern.vpp");
            if (!_patternService.IsTrained)
            {
                MessageBox.Show("저장된 패턴이 없습니다. ROI와 검색 영역을 설정한 후 패턴을 학습하세요.");
                return;
            }
            else
            {
                // TrainDisplay에 트레인 이미지와 Coarse 피처 렌더
                trainDisplay.StaticGraphics.Clear();
                trainDisplay.Image = _patternService.GetTrainedPatternImage(); 
                var coarse = _patternService.CreateTrainedPatternCoarseGraphics(CogColorConstants.Cyan); 
                if (coarse != null) trainDisplay.StaticGraphics.AddList(coarse, "coarsePattern");

                PatternVppSave.Enabled = false;
                toolRun.Enabled = true;
            }
        } // 패턴 저장 버튼
        private void PatternVppSave_Click(object sender, EventArgs e)
        {
            // ROI와 중심점이 제대로 설정되었는지 확인
            if (_regionManager.Roi == null || _regionManager.Axes == null)
            {
                MessageBox.Show("ROI 영역과 중심 점이 설정되지 않았습니다. 먼저 설정해주세요.");
                return;
            }

            var trainOk = _patternService.Train(currentImage, _regionManager.Roi, _regionManager.Axes);
            if (!trainOk) { MessageBox.Show("패턴 학습 실패"); return; }
            MessageBox.Show("패턴 학습 완료");

            if (_patternService.IsTrained)
            {
                toolRun.Enabled = true;
                ClearGraphic_Display(showImage, currentImage);

                _patternService.SaveVpp(@"vpp\pattern.vpp");
                MessageBox.Show("패턴 저장 완료");
            }

            ClearGraphic_Display(trainDisplay, _patternService.GetTrainedPatternImage());
            var coarse = _patternService.CreateTrainedPatternCoarseGraphics(CogColorConstants.Cyan);
            if (coarse != null) trainDisplay.StaticGraphics.AddList(coarse, "coarsePattern");

        } // 패턴 로드 버튼

        // =======================
        // 영역 설정
        private void SearchRegion_Click(object sender, EventArgs e)
        {
            if (currentImage == null)
            {
                MessageBox.Show("이미지를 먼저 촬영하거나 불러오세요.");
                return;
            }

            CheckInit_Display();
            CheckTrained();

            int imageWidth = currentImage.Width;
            int imageHeight = currentImage.Height;
            double centerX = currentImage.Width / 2.0;
            double centerY = currentImage.Height / 2.0;

            var searchRegion = _regionManager.CreateSearchRegion(centerX, centerY, imageWidth, imageHeight);
            if (searchRegion != null)
            {
                ClearGraphic_Display(showImage, currentImage);
                showImage.InteractiveGraphics.Add(searchRegion, "searchRegion", true);
            }

        } // 검색영역 버튼
        private void RoiBtn_Click(object sender, EventArgs e)
        {
            if (currentImage == null)
            {
                MessageBox.Show("이미지를 먼저 촬영하거나 불러오세요.");
                return;
            }

            CheckInit_Display();
            CheckTrained();

            showImage.Fit(true); // 디스플레이에 이미지 크기를 딱 맞게 맞춤
            check = false; // 학습 후 검사 여부를 결정하는 플래그. ROI 설정 시 검사 off

            // ROI 영역 설정
            var (roiRegion, coordinateAxes) = _regionManager.CreateRoi(150, 150, 300, 300);
            showImage.InteractiveGraphics.Add(roiRegion, "ROI", true); // ROI 영역을 화면에 추가 
            showImage.StaticGraphics.Add(coordinateAxes, "centerPoint"); // 중심 좌표축을 화면에 추가

            PatternVppSave.Enabled = true;
        } // ROI 영역을 설정하기 위한 버튼

        // =======================
        // 검사 설정
        private void CheckRun_Click(object sender, EventArgs e)
        {
            // 검사 실행 전 필수 체크 : 패턴이 학습되지 않은 경우 검사 불가
            if (!_patternService.IsTrained)
            {
                MessageBox.Show("패턴을 먼저 학습하세요.");
                return;
            }

            if (showImage.LiveDisplayRunning)
            {
                showImage.StopLiveDisplay();
            }

            // check는 다음 이미지 넘길 때 자동 검사할지 여부를 판단하는 플래그
            check = true;
            _camera.ImageReceived -= Check_pattern;
            _camera.ImageReceived += Check_pattern;

            loadImage.Enabled = false;
            roi_Btn.Enabled = false;
            SearchRegion.Enabled = false;
            connectCamera.Enabled = false;
            liveOnOffBtn.Text = "연속 촬영 검사 시작";
            Acq_Once.Text = "1회 촬영 검사";
            toolRun.Enabled = false;
            Check_Stop.Enabled = true;

            // 실제 패턴 매칭 검사 메소드 호출
            Check_pattern(currentImage);

        } // 트레인 이미지 검사 버튼
        private void Check_pattern(ICogImage image)
        {
            if (!check || !IsHandleCreated) return;
            if (InvokeRequired)
            {
                BeginInvoke((Action<ICogImage>)Check_pattern, image);
                return;
            }

            // ----------------- 패턴 매칭 검사 실행 ----------------------
            // 검사 대상 이미지를 PMAlignTool 에 설정
            var result = _patternService.Match(image);

            if (result == null || result.Score < 0.7) OkNg.Text = "NG";
            else OkNg.Text = "OK";

            // -------------------- 결과 시각화 --------------------------------
            _patternService.ShowResult(resultDisplay);

            // ------------------- 검사 결과 텍스트로 표시 ----------------------
            // 점수, 위치, 회전 각도 추출
            resultLabel.Text = _patternService.FormatResult(result);

        } // PMAlign 검사 함수
        private void CheckTrained()
        {
            if (_patternService.IsTrained)
            {
                _patternService.ResetPattern();
                check = false;
                Console.WriteLine("기존의 패턴이 초기화되었습니다.");
            }
            else
            {
                toolRun.Enabled = false;
                Console.WriteLine("학습된 패턴이 없습니다.");
            }
        } // Train 확인 여부 호출 함수
        private void Check_Stop_Click(object sender, EventArgs e)
        {
            check = false;
            _camera.StopLoop();
            _camera.ImageReceived -= Check_pattern;

            resultDisplay.Image = null;
            liveOnOffBtn.Text = "라이브 시작";
            Acq_Once.Text = "1회 촬영";
            OkNg.Text = "";
            resultLabel.Text = "";
            loadImage.Enabled = true;
            roi_Btn.Enabled = true;
            SearchRegion.Enabled = true;
            connectCamera.Enabled = true;
            toolRun.Enabled = true;
            Check_Stop.Enabled = false;
        }

    }
}

