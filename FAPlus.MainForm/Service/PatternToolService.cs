using Cognex.VisionPro;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.ToolBlock;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FAPlus.MainForm.Service
{
    public sealed class PatternToolService : IDisposable
    {
        private CogPMAlignTool cogPMAlignTool = new CogPMAlignTool();

        // 프로퍼티 생성
        public CogPMAlignTool Tool
        {
            get => cogPMAlignTool;
            private set
            {
                if (cogPMAlignTool == value) return;
                cogPMAlignTool?.Dispose();
                cogPMAlignTool = value ?? new CogPMAlignTool();
            }
        }

        // ========== 트레인 설정, 트레인, 트레인 결과
        public bool IsTrained => Tool?.Pattern?.Trained ?? false; // 트레인 여부
        public ICogImage TrainImage
        {
            get => Tool?.Pattern?.TrainImage;
            set { if (Tool?.Pattern != null) Tool.Pattern.TrainImage = value; }
        }
        public CogRectangle TrainRegion
        {
            get => Tool.Pattern.TrainRegion as CogRectangle;
            set { if (Tool?.Pattern != null) Tool.Pattern.TrainRegion = value; }
        }
        public CogRectangle SearchRegion
        {
            get => Tool?.SearchRegion as CogRectangle;
            set { if (Tool != null) Tool.SearchRegion = value; }
        }
        public double? OriginX
        {
            get => Tool?.Pattern?.Origin?.TranslationX;
            set { if (Tool?.Pattern?.Origin != null && value.HasValue) Tool.Pattern.Origin.TranslationX = value.Value; }
        }
        public double? OriginY
        {
            get => Tool?.Pattern?.Origin?.TranslationY;
            set { if (Tool?.Pattern?.Origin != null && value.HasValue) Tool.Pattern.Origin.TranslationY = value.Value; }
        }
        public CogPMAlignZoneConstants AngleConfig
        {
            get => Tool.RunParams.ZoneAngle.Configuration;
            set => Tool.RunParams.ZoneAngle.Configuration = value;
        }
        public double AngleLowDeg
        {
            get => CogMisc.DegToRad(Tool.RunParams.ZoneAngle.Low) * 180.0 / Math.PI;
            set => Tool.RunParams.ZoneAngle.Low = CogMisc.RadToDeg(value * Math.PI / 180.0);
        }
        public double AngleHighDeg
        {
            get => CogMisc.DegToRad(Tool.RunParams.ZoneAngle.High) * 180.0 / Math.PI;
            set => Tool.RunParams.ZoneAngle.High = CogMisc.RadToDeg(value * Math.PI / 180.0);
        }
        public bool Train(ICogImage image, CogRectangle roiRegion, CogCoordinateAxes coordinateAxes)
        {
            if (Tool?.Pattern == null) return false;

            TrainImage = image; // PMAlign 패턴 학습 이미지 설정
            TrainRegion = roiRegion; // 학습 ROI 영역 설정
            OriginX = coordinateAxes?.OriginX; // 학습 패턴의 기준 좌표 원점 설정
            OriginY = coordinateAxes?.OriginY; // 학습 패턴의 기준 좌표 원점 설정

            AngleConfig = CogPMAlignZoneConstants.LowHigh; // 회전 각도의 최대,최소 수동설정
            AngleLowDeg = -180; // -180도
            AngleHighDeg = 180; // 180도

            Tool.Pattern.TrainAlgorithm = CogPMAlignTrainAlgorithmConstants.PatMax; // 패턴 학습 알고리즘 선택
            Tool.Pattern.Train(); // 패턴 학습

            return IsTrained;
        } // 패턴 트레인

        // ========== 패턴 초기화
        public void ResetPattern()
        {
            if (Tool?.Pattern == null) return;
            Tool.Pattern.TrainImage = null;
        }

        // ========== 패턴 매칭
        public CogPMAlignResult Match(ICogImage image)
        {
            if (Tool == null) return null;

            Tool.InputImage = image; // 검사 대상 이미지를 PMAlignTool에 설정
            Tool.CurrentRecordEnable = CogPMAlignCurrentRecordConstants.All; // 모든 실행 결과 기록 (입력 이미지, 매칭 결과 등)
            Tool.RunParams.SaveMatchInfo = true; // 매칭된 특징점 정보 저장 (ResultMathchFeatures 사용 시 필수)
            // 진단 기록에 입력 이미지 참조 + 매치 특징점 포함
            Tool.LastRunRecordDiagEnable = CogPMAlignLastRunRecordDiagConstants.InputImageByReference |
                                                     CogPMAlignLastRunRecordDiagConstants.ResultsMatchFeatures;
            // 마지막 실행 기록에 원점 좌표 + 매칭 영역 포함
            Tool.LastRunRecordEnable = CogPMAlignLastRunRecordConstants.ResultsOrigin |
                                                 CogPMAlignLastRunRecordConstants.ResultsMatchRegion;

            Tool.Run(); // 툴 실행 -> 내부적으로 학습된 패턴을 이미지에서 찾음.

            if (Tool.Results == null || Tool.Results.Count == 0) return null; // 매치 결과가 없으면 null 반환
            return Tool.Results[0]; // 패턴과 일치하는 결과의 가장 첫번째 값을 반환
        }  // 패턴 매칭 검사 실행

        // ========== 결과 표시
        public ICogImage GetTrainedPatternImage() => Tool?.Pattern?.GetTrainedPatternImage();
        public CogGraphicCollection CreateTrainedPatternCoarseGraphics(CogColorConstants color) => Tool?.Pattern?.CreateGraphicsCoarse(color); // 트레인 영역 표시
        public string FormatResult(CogPMAlignResult result)
        {
            if (result == null) return null;

            double score = result.Score;
            double x = result.GetPose().TranslationX;
            double y = result.GetPose().TranslationY;
            double rotationDeg = result.GetPose().Rotation * 180.0 / Math.PI;

            return $"Score: {score:F2}, X: {x:F1}, Y: {y:F1}, Rotation: {rotationDeg:F1}";
        }
        public void ShowResult(CogRecordDisplay cogRecordDisplay)
        {
            if (cogRecordDisplay == null || Tool == null) return;

            var record = Tool.CreateLastRunRecord().SubRecords["InputImage"];
            cogRecordDisplay.Record = record;
            cogRecordDisplay.Fit(true);
        }

        // ========== Pattern Vpp
        public void SaveVpp(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("경로가 비었습니다.", nameof(path));
            CogSerializer.SaveObjectToFile(Tool, path, typeof(BinaryFormatter));
        }
        public void LoadVpp(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("경로가 비었습니다.", nameof(path));

            // 파일이 없거나 0바이트면 기본 툴을 먼저 저장
            if (!File.Exists(path) || new FileInfo(path).Length == 0)
            {
                Tool = new CogPMAlignTool();
                SaveVpp(path);
                return;
            }

            var obj = CogSerializer.LoadObjectFromFile(path, typeof(BinaryFormatter));
            var pm = obj as CogPMAlignTool;
            if (pm == null) throw new InvalidOperationException("vpp에 CogPMAlignTool이 없습니다.");
            
            Tool = pm; // 교체(기존 툴 Dispose 후 대입)
        }

        // ========== 자원해제
        public void Dispose()
        {
            Tool = null;
        }
    }
}
