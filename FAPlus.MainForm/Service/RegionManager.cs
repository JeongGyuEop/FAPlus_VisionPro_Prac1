using Cognex.VisionPro;
using Cognex.VisionPro.PMAlign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAPlus.MainForm.Service
{
    public class RegionManager
    {
        public CogRectangle Roi { get; private set; }
        public CogCoordinateAxes Axes { get; private set; }
        public CogRectangle SearchRegion { get; private set; }

        public event Action<CogRectangle, CogCoordinateAxes> RoiChanged; // ROI 영역 변경 이벤트

        // ==========
        // 좌표 및 영역 설정
        public CogRectangle CreateSearchRegion(double x, double y, int w, int h)
        {
            var region = new CogRectangle();
            region.SetCenterWidthHeight(x, y, w, h);
            region.Color = CogColorConstants.Cyan;
            region.GraphicDOFEnable = CogRectangleDOFConstants.All;
            region.Interactive = true;
            SearchRegion = region;
            return region;
        }
        public (CogRectangle, CogCoordinateAxes) CreateRoi(double x, double y, int w, int h)
        {
            Roi = new CogRectangle();
            Roi.SetCenterWidthHeight(x, y, w, h);
            Roi.Color = CogColorConstants.Green;
            Roi.GraphicDOFEnable = CogRectangleDOFConstants.All;
            Roi.Interactive = true;

            Axes = new CogCoordinateAxes();
            Axes.Color = CogColorConstants.Green;
            Axes.OriginX = Roi.CenterX;
            Axes.OriginY = Roi.CenterY;

            Roi.Changed += (s, e) => {
                Axes.OriginX = Roi.CenterX;
                Axes.OriginY = Roi.CenterY;
                RoiChanged?.Invoke(Roi, Axes);
            };

            return (Roi, Axes);
        }

    }
}
