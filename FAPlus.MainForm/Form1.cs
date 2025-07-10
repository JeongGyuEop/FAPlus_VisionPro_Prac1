using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.PMAlign;
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
        private CogRectangle roiRegion; // ROI로 사용할 크기조절만 가능 네모 영역
        private CogCoordinateAxes coordinateAxes; // 중심 좌표 축(X축, Y축, 회전)을 표현
        private CogPMAlignTool pmAlignTool = new CogPMAlignTool(); // VisionPro에서 패턴 매칭(템플릿 매칭)을 수행하는 핵심 툴
        private ImageList imageList = new ImageList();
        private List<string> imageFiles = new List<string>(); // 이미지 폴더에서 불러온 파일 경로들의 리스트

        private ICogImage currentImage; // 현재 화면에 표시된 이미지이자 검사 대상 이미지

        private int currentImageIndex = 0; // 현재 보여지고 있는 이미지가 리스트 내에서 몇 번째인지 저장
        private bool check = false; // 이미지 넘길 때 자동으로 Check_pattern()을 수행할지 여부를 결정하는 플래그

        public Form1() {  InitializeComponent(); }

        private void Form1_Load(object sender, EventArgs e)
        {
            imageList.ImageSize = new Size(100, 100); // 썸네일 크기 설정 (가로 100px, 세로 100px)

            imageListView.View = View.LargeIcon; // ListView를 썸네일 보기 모드 설정
            imageListView.LargeImageList = imageList; // ListView에 이미지 리스트 연결

            imageListView.SelectedIndexChanged += ImageListView_SelectedIndexChanged; // 사용자가 썸네일을 클릭했을 때 이벤트

            Train.Enabled = false;
            toolRun.Enabled = false;
        }

        // ========================================================================================================
        // 버튼 클릭 이벤트
        private void LoadImage_Click(object sender, EventArgs e)
        {
            /*
                using 문은 C#의 IDisposable 객체를 안전하게 사용하고 자동으로 해제해주는 문법
                FolderBrowserDialog는 윈도우 리소스를 사용하는 객체이기 때문에
                using을 사용하여 사용 후 자동으로 Dispose()가 호출되도록 한다.
            */
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                /*
                    folderBrowserDialog.ShowDialog()는 사용자가 폴더를 선택한 후 누른 버튼이 무엇인지 반환
                    (DialogResult.OK 는 "확인" 버튼을 눌렸을 때를 의미한다.)
                */
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    // 사용자가 선택한 폴더 경로를 가져온다.(예: "C:\Images")
                    string selectedFolder = folderBrowserDialog.SelectedPath;

                    // 배열 형태의 문자열 리스트 : 지원하는 이미지 파일 확장자 목록
                    // 소문자로 비교하기 위해 확장자를 전부 소문자로 정의한다.
                    string[] supportedExtensions = new[] { ".bmp", ".jpg", ".jpeg", ".png" };

                    /*
                        Directory.GetFiles() : 선택한 폴더 내 모든 파일 경로 문자열 배열 반환
                        Path.GetExtension() : 파일의 확장자만 추출 (예: ".jpg")
                        Where(...) : LINQ 문법. 조건에 맞는 파일만 필터링
                        ToList() : IEnumerable<string>을 List<string>으로 변환
                    */
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
                        // 첫 이미지 표시
                        LoadImageByIndex(currentImageIndex, false);
                        imageListView.Items[currentImageIndex].Selected = true; // 선택 상태도 표시

                        InitDisplay();
                    }
                }
            }

            /*
                showImage는 VisionPro의 CogDisplay 객체
                이전에 그려진 그래픽 요소를 제거하여 새로운 이미지가 깨끗하게 표시되도록 한다.
                StaticGraphics : 고정된 그래픽 (예: 학습 영역, 박스, 텍스트 등)
                InteractiveGraphics : 마우스로 조작 가능한 ROI 등의 객첵
             */
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

        private void RoiBtn_Click(object sender, EventArgs e)
        {
            InitDisplay();
            CheckTrained();

            Train.Enabled = true;

            roiRegion = new CogRectangle(); // CogRectangle은 크기와 위치 변경만 가능한 사각형 객체
            coordinateAxes = new CogCoordinateAxes(); // 중심 좌표를 시각적으로 표현하는 객체 (십자축 모양)

            LoadImageByIndex(currentImageIndex, check);

            showImage.Fit(true); // 디스플레이에 이미지 크기를 딱 맞게 맞춤
            check = false; // 학습 후 검사 여부를 결정하는 플래그. ROI 설정 시 검사 off


            // ROI 영역 설정 ----------------------------------------------------------
            // SetCenterWidthHeight(X, Y, width, height) -> 중심 좌표(150, 150), 가로 세로 길이(300, 300)
            roiRegion.SetCenterWidthHeight(150, 150, 300, 300); 

            coordinateAxes.OriginX = roiRegion.CenterX; // ROI의 중심 x좌표
            coordinateAxes.OriginY = roiRegion.CenterY; // ROI의 중심 y좌표

            roiRegion.GraphicDOFEnable = CogRectangleDOFConstants.All; // ROI의 변형 자유도 설정 (이동, 크기 조절 허용)

            roiRegion.Interactive = true; // ROI 객체를 마우스로 조절 가능하게 함.

            roiRegion.Changed += RoiRegion_Changed; // ROI 변경 시 중심축도 따라가도록 이벤트 등록


            // 디스플레이에 ROI와 중심좌표 추가 ---------------------------------------
            showImage.InteractiveGraphics.Clear(); // 기존의 InteractiveGraphics를 모두 제거하여 깨끗한 상태로 만듦
            showImage.StaticGraphics.Clear();

            // ROI 영역을 화면에 추가 (태그명: "ROI", 상호작용 가능하게 true)
            showImage.InteractiveGraphics.Add(roiRegion, "ROI", true);

            // 중심 좌표축을 화면에 추가 (태그명: "centerPoint", 상호작용 가능)
            showImage.StaticGraphics.Add(coordinateAxes, "centerPoint");


        } // ROI 영역을 설정하기 위한 버튼

        private void Train_Click(object sender, EventArgs e)
        {
            // ROI와 중심점이 제대로 설정되었는지 확인
            if (roiRegion == null || coordinateAxes == null)
            {
                MessageBox.Show("ROI 영역과 중심 점이 설정되지 않았습니다. 먼저 설정해주세요.");
                return;
            }

            // ---------------------- 학습 설정 ------------------------------------
            // 현재 디스플레이에 표시된 이미지를 PMAlign 패턴 학습 이미지로 설정
            pmAlignTool.Pattern.TrainImage = currentImage;

            //pmAlignTool.Pattern.untrain;

            // 학습할 패턴의 기준 좌표 원점을 사용자가 지정한 좌표축 중심으로 설정
            pmAlignTool.Pattern.Origin.TranslationX = coordinateAxes.OriginX;
            pmAlignTool.Pattern.Origin.TranslationY = coordinateAxes.OriginY;
            //pmAlignTool.Pattern.Origin.Rotation = coordinateAxes.Rotation;

            // 학습에 사용할 ROI 영역 지정 (사각형)
            pmAlignTool.Pattern.TrainRegion = roiRegion;

            try
            {
                // ----------------------- 회전 허용 범위 설정 ----------------------------------
                // LowHigh 설정은 회전 각도의 최소~최대 범위를 수동 지정한다는 뜻
                pmAlignTool.RunParams.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh;

                // 회전 허용 범위를 -π ~ +π (즉, -180도 ~ +180도)로 지정
                pmAlignTool.RunParams.ZoneAngle.Low =  CogMisc.RadToDeg(-180);  // -180도
                pmAlignTool.RunParams.ZoneAngle.High = CogMisc.RadToDeg(180);  // +180도

                // ----------------------- 패턴 학습 수행 --------------------------------------
                // 설정한 이미지와 ROI, 기준 원점 정보를 바탕으로 패턴 학습을 수행
                pmAlignTool.Pattern.TrainAlgorithm = CogPMAlignTrainAlgorithmConstants.PatMax;
                pmAlignTool.Pattern.Train();
                MessageBox.Show("패턴 학습이 완료되었습니다.");

                if(pmAlignTool.Pattern.Trained)
                {
                    Train.Enabled = false;
                    toolRun.Enabled = true;
                    ClearDisplay(showImage, currentImage);
                }

                // ----------------------- 학습 결과 표시 ---------------------------------------
                // 학습된 패턴을 이미지(ROI 영역 내 부분)를 TrainDisplay에 표시
                ClearDisplay(trainDisplay, pmAlignTool.Pattern.GetTrainedPatternImage());

                // 패턴의 외곽선 및 구조 정보를 Coarse 형태로 표시 (보통 Cyan으로 표현)
                trainDisplay.StaticGraphics.AddList(pmAlignTool.Pattern.CreateGraphicsCoarse(CogColorConstants.Cyan), "coarsePattern");

            }
            catch (Exception ex) {  MessageBox.Show("패턴 학습 실패: " + ex.Message);  }

        } // 이미지 Train을 위한 버튼

        private void CheckRun_Click(object sender, EventArgs e)
        {
            // 검사 실행 전 필수 체크 : 패턴이 학습되지 않은 경우 검사 불가
            if (pmAlignTool.Pattern.Trained == false)
            {
                MessageBox.Show("패턴을 먼저 학습하세요.");
                return;
            }

            // check는 다음 이미지 넘길 때 자동 검사할지 여부를 판단하는 플래그
            check = true;

            // 실제 패턴 매칭 검사 메소드 호출
            Check_pattern();

        } // 트레인 이미지 검사 버튼

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

        private void SearchRegion_Click(object sender, EventArgs e)
        {
            InitDisplay();
            CheckTrained();

            Train.Enabled = false;

            int imageWidth = currentImage.Width;
            int imageHeight = currentImage.Height;

            double centerX = imageWidth / 2.0;
            double centerY = imageHeight / 2.0;

            CogRectangle searchRegion = new CogRectangle();
            searchRegion.SetCenterWidthHeight(centerX, centerY, imageWidth, imageHeight);

            searchRegion.GraphicDOFEnable = CogRectangleDOFConstants.All;
            searchRegion.Interactive = true;
            searchRegion.Color = CogColorConstants.Cyan;

            // 디스플레이에 표시
            ClearDisplay(showImage, currentImage);
            showImage.InteractiveGraphics.Add(searchRegion, "searchRegion", true);

            pmAlignTool.SearchRegion = searchRegion;
        } // 검색영역 버튼


        // ========================================================================================================
        // 호출 함수
        private void LoadImageByIndex(int imageIndex, bool check)
        {
            /*
                using 문을 사용하는 이유
                CogImageFile은 IDisposable을 구현하고 있는 객체로,
                이미지 파일을 열고 닫는 리소스를 사용하므로
                using을 통해 자동으로 Dispose()가 호출되도록 한다.
            */
            using (CogImageFile cogImageFile = new CogImageFile())
            {
                // 선택된 이미지 경로를 열어서 읽기 모드로 설정ClearDisplay(showImage, true);
                cogImageFile.Open(imageFiles[imageIndex], CogImageFileModeConstants.Read);

                // 읽어온 이미지 파일 중 첫 번째 이미지 객체를 currentImage로 저장
                // 대부분의 경우 1개의 이미지만 존재하므로 [0] 사용
                currentImage = cogImageFile[0];
            }

            if (check) { Check_pattern(); }
            else
            {
                showImage.Fit(true);
                ClearDisplay(showImage, currentImage); 
            }

        } // 여러 장의 이미지를 로드하기 위한 함수

        private void Check_pattern()
        {
            Console.WriteLine(currentImageIndex);

            // ----------------- 패턴 매칭 검사 실행 ----------------------
            // 검사 대상 이미지를 PMAlignTool 에 설정
            pmAlignTool.InputImage = currentImage;

            pmAlignTool.LastRunRecordEnable = CogPMAlignLastRunRecordConstants.ResultsOrigin|
                                              CogPMAlignLastRunRecordConstants.ResultsMatchRegion;
            pmAlignTool.LastRunRecordDiagEnable = CogPMAlignLastRunRecordDiagConstants.All;
            using (Form a = new Form())
            {
                a.Width = 800;
                a.Height = 1200;
                using (CogPMAlignEditV2 kiki = new CogPMAlignEditV2())
                {
                    kiki.Subject = pmAlignTool;
                    kiki.Dock = DockStyle.Fill;
                    a.Controls.Add(kiki);
                    a.ShowDialog();
                }
            }

            pmAlignTool.CurrentRecordEnable = CogPMAlignCurrentRecordConstants.All;

            // ===============================================================================
            //pmAlignTool.LastRunRecordDiagEnable = CogPMAlignLastRunRecordDiagConstants.ResultsMatchFeatures;
            // ===============================================================================

            // PMAlignTool 실행 -> 내부적으로 학습된 패턴을 이미지에서 찾음
            pmAlignTool.Run();

            // 결과가 0개이면 매칭 실패 -> 이후 처리하지 않고 종료
            if (pmAlignTool.Results.Count == 0) {
                resultDisplay.Record = null;
                resultDisplay.StaticGraphics.Clear();
                resultDisplay.Image = currentImage;
                MessageBox.Show("매칭 실패");
                return;
            }

            // 가장 높은 점수를 가진 첫 번째 결과 사용
            var pmResult = pmAlignTool.Results[0];
             
            // -------------------- 결과 시각화 --------------------------------
            var temp = pmAlignTool.CreateLastRunRecord().SubRecords["InputImage"];

            //var temp = pmAlignTool.CreateLastRunRecord().RecordKey["LastRun"];

            resultDisplay.Record = temp;
             
            // ------------------- 검사 결과 텍스트로 표시 ----------------------
            // 점수, 위치, 회전 각도 추출
            double score = pmResult.Score;
            double x = pmResult.GetPose().TranslationX;
            double y = pmResult.GetPose().TranslationY;
            double rotationDeg = pmResult.GetPose().Rotation * 180.0 / Math.PI;

            // 결과를 라벨에 표시 (소수점 포멧 포함)
            resultLabel.Text = $"Score: {score:F2}, X: {x:F1}, Y: {y:F1}, R: {rotationDeg:F1}°";

        } // PMAlign 검사 함수

        private void InitDisplay()
        {
            /*
                이미지가 하나도 로드되지 않은 상태일 경우 사용자에게 알림
                imageFiles는 이미지 파일 경로들이 저장된 리스트
            */
            if (imageFiles.Count == 0)
            {
                MessageBox.Show("이미지를 먼저 불러오세요.");
                return;
            }

            toolRun.Enabled = false; // 검사 버튼 비활성화

            ClearDisplay(trainDisplay);
            ClearDisplay(resultDisplay);

            resultLabel.Text = ""; // 검사 결과 텍스트 초기화
        } // ROI 영역 설정 || 검색 영역 설정에 대한 공통 부분 처리 함수

        private void CheckTrained()
        {
            if (pmAlignTool.Pattern.Trained)
            {
                pmAlignTool.Pattern.TrainImage = null;
                check = false;

                // 학습이 되어 있음
                Console.WriteLine("기존의 패턴이 초기화되었습니다.");
            }
            else
            {
                toolRun.Enabled = false; // 검사 버튼 비활성화

                // 학습이 되어 있지 않음
                Console.WriteLine("학습된 패턴이 없습니다.");
            }
        } // Train 확인 여부 호출 함수

        private void ClearDisplay(CogDisplay display, ICogImage currentImage = null) // 디스플레이 초기화 함수
        {
            display.StaticGraphics.Clear();
            display.InteractiveGraphics.Clear();
            display.Image = currentImage != null ? currentImage : null;
            display.Fit(true);
        }


        // ========================================================================================================
        // 변화 이벤트
        private void ImageListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (imageListView.SelectedIndices.Count == 0) return;

            currentImageIndex = imageListView.SelectedIndices[0];
            LoadImageByIndex(currentImageIndex, check);

            // 선택된 항목이 화면에 보이도록 자동 스크롤
            imageListView.EnsureVisible(currentImageIndex);

        } // 썸네일 이미지의 선택된 인덱스가 변경될 때 이벤트

        private void RoiRegion_Changed(object sender, CogChangedEventArgs e)
        {

            // coordinateAxes : 십자축 모양의 좌표계 표시 객체 (ROI의 중심 위치로 좌표축 원점 이동)
            coordinateAxes.OriginX = roiRegion.CenterX; // ROI의 중심 x좌표
            coordinateAxes.OriginY = roiRegion.CenterY; // ROI의 중심 y좌표

            // 디스플레이 갱신
            showImage.StaticGraphics.Remove("centerPoint"); // 이전 좌표축 제거

            // 중심 좌표축을 화면에 추가 (태그명: "centerPoint", 상호작용 가능)
            showImage.StaticGraphics.Add(coordinateAxes, "centerPoint");


        } // ROI 영역 이동 및 변화에 대한 이벤트

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

        private void button2_Click(object sender, EventArgs e)
        {
            using (Form a = new Form())
            {
                a.Width = 800;
                a.Height = 1200;
                using (CogPMAlignEditV2 kiki = new CogPMAlignEditV2())
                {
                    kiki.Subject = pmAlignTool;
                    kiki.Dock = DockStyle.Fill;
                    a.Controls.Add(kiki);
                    a.ShowDialog();
                }
            }
        }
    }
}