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
        private CogPMAlignTool CogPMAlignTool = new CogPMAlignTool();

        // 프로퍼티 생성

        // ========== 트레인 설정, 트레인, 트레인 결과
        public CogRectangle GetSearchRegion() => CogPMAlignTool.SearchRegion as CogRectangle; // vpp 검색 영역 값 반환
        public CogRectangle GetTrainRegion() => CogPMAlignTool.Pattern.TrainRegion as CogRectangle; // vpp 트레인 ROI 영역 반환
        public CogPMAlignTool GetTool() => CogPMAlignTool; // vpp CogPMAlignTool 객체 반환
        public bool Train(ICogImage image, CogRectangle roiRegion, CogCoordinateAxes coordinateAxes)
        {
            CogPMAlignTool.Pattern.TrainImage = image; // PMAlign 패턴 학습 이미지 설정
            CogPMAlignTool.Pattern.TrainRegion = roiRegion; // 학습 ROI 영역 설정
            CogPMAlignTool.Pattern.Origin.TranslationX = coordinateAxes.OriginX; // 학습 패턴의 기준 좌표 원점 설정
            CogPMAlignTool.Pattern.Origin.TranslationY = coordinateAxes.OriginY; // 학습 패턴의 기준 좌표 원점 설정

            CogPMAlignTool.RunParams.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh; // 회전 각도의 최대,최소 수동설정
            CogPMAlignTool.RunParams.ZoneAngle.Low = CogMisc.RadToDeg(-180); // -180도
            CogPMAlignTool.RunParams.ZoneAngle.High = CogMisc.RadToDeg(180); // 180도

            CogPMAlignTool.Pattern.TrainAlgorithm = CogPMAlignTrainAlgorithmConstants.PatMax; // 패턴 학습 알고리즘 선택
            CogPMAlignTool.Pattern.Train(); // 패턴 학습

            return CogPMAlignTool.Pattern.Trained;
        } // 패턴 트레인
        public bool IsTrained => CogPMAlignTool.Pattern?.Trained == true; // 트레인 여부
        public ICogImage GetTrainedPatternImage() => CogPMAlignTool.Pattern?.GetTrainedPatternImage(); // 트레인 이미지 획득
        public CogGraphicCollection CreateTrainedPatternCoarseGraphics(CogColorConstants color) => CogPMAlignTool.Pattern?.CreateGraphicsCoarse(color); // 트레인 영역 표시

        // ========== 패턴 초기화
        public void ResetPattern()
        {
            CogPMAlignTool.Pattern.TrainImage = null;
        }

        // ========== 패턴 매칭
        public CogPMAlignResult Match(ICogImage image)
        {
            CogPMAlignTool.InputImage = image; // 검사 대상 이미지를 PMAlignTool에 설정

            CogPMAlignTool.CurrentRecordEnable = CogPMAlignCurrentRecordConstants.All; // 모든 실행 결과 기록 (입력 이미지, 매칭 결과 등)

            CogPMAlignTool.RunParams.SaveMatchInfo = true; // 매칭된 특징점 정보 저장 (ResultMathchFeatures 사용 시 필수)

            // 진단 기록에 입력 이미지 참조 + 매치 특징점 포함
            CogPMAlignTool.LastRunRecordDiagEnable = CogPMAlignLastRunRecordDiagConstants.InputImageByReference |
                                                     CogPMAlignLastRunRecordDiagConstants.ResultsMatchFeatures;

            // 마지막 실행 기록에 원점 좌표 + 매칭 영역 포함
            CogPMAlignTool.LastRunRecordEnable = CogPMAlignLastRunRecordConstants.ResultsOrigin |
                                                 CogPMAlignLastRunRecordConstants.ResultsMatchRegion;

            CogPMAlignTool.Run(); // 툴 실행 -> 내부적으로 학습된 패턴을 이미지에서 찾음.

            if (CogPMAlignTool.Results == null || CogPMAlignTool.Results.Count == 0) return null; // 매치 결과가 없으면 null 반환
       
            return CogPMAlignTool.Results[0]; // 패턴과 일치하는 결과의 가장 첫번째 값을 반환
        }  // 패턴 매칭 검사 실행

        // ========== Pattern Vpp
        public void SaveVpp(string path)
        {
            // PMAlignTool 자체(또는 ToolBlock에 담아서) 직렬화
            var tb = new CogToolBlock();
            tb.Tools.Add(CogPMAlignTool); // 검색영역, TrainRegion, RunParams, 패턴 데이터 전부 포함
            CogSerializer.SaveObjectToFile(tb, path, typeof(BinaryFormatter));
        }
        public void LoadVpp(string path)
        {
            if (!File.Exists(path) || new FileInfo(path).Length == 0)
            {
                // 파일이 없거나 빈 경우 → 기본 툴 저장
                Console.WriteLine($"[VPP 로드] 파일 없음, 기본 패턴 저장: {path}");
                SaveVpp(path); // 초기 상태 저장
            }

            var obj = CogSerializer.LoadObjectFromFile(path, typeof(BinaryFormatter));
            var toolBlock = obj as CogToolBlock;
            if (toolBlock == null)
                throw new InvalidOperationException("vpp에 ToolBlock이 없습니다.");

            // 첫 번째 PMAlignTool을 꺼내서 교체
            foreach (ICogTool t in toolBlock.Tools)
            {
                if (t is CogPMAlignTool pm)
                {
                    // 기존 객체 정리 후 교체
                    CogPMAlignTool?.Dispose();
                    CogPMAlignTool = pm;
                    
                    return;
                }
            }
            throw new InvalidOperationException("vpp에 CogPMAlignTool이 없습니다.");
        }

        // ========== 결과 출력
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
            var record = CogPMAlignTool.CreateLastRunRecord().SubRecords["InputImage"];
            cogRecordDisplay.Record = record;
            cogRecordDisplay.Fit(true);
        }

        // ========== 자원해제
        public void Dispose()
        {
            CogPMAlignTool?.Dispose();
            CogPMAlignTool = null;
        }
    }
}
