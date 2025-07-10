namespace FAPlus.MainForm
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.showImage = new Cognex.VisionPro.Display.CogDisplay();
            this.trainDisplay = new Cognex.VisionPro.Display.CogDisplay();
            this.roi_Btn = new System.Windows.Forms.Button();
            this.Train = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.loadImage = new System.Windows.Forms.Button();
            this.nextImage = new System.Windows.Forms.Button();
            this.beforeImage = new System.Windows.Forms.Button();
            this.toolRun = new System.Windows.Forms.Button();
            this.resultLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.resultDisplay = new Cognex.VisionPro.CogRecordDisplay();
            this.imageListView = new System.Windows.Forms.ListView();
            this.label4 = new System.Windows.Forms.Label();
            this.autoPlay = new System.Windows.Forms.Button();
            this.playTimer = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.setTime = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.SearchRegion = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.showImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trainDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playTimer)).BeginInit();
            this.SuspendLayout();
            // 
            // showImage
            // 
            this.showImage.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.showImage.ColorMapLowerRoiLimit = 0D;
            this.showImage.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.showImage.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.showImage.ColorMapUpperRoiLimit = 1D;
            this.showImage.DoubleTapZoomCycleLength = 2;
            this.showImage.DoubleTapZoomSensitivity = 2.5D;
            this.showImage.Location = new System.Drawing.Point(10, 39);
            this.showImage.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.showImage.MouseWheelSensitivity = 1D;
            this.showImage.Name = "showImage";
            this.showImage.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("showImage.OcxState")));
            this.showImage.Size = new System.Drawing.Size(361, 329);
            this.showImage.TabIndex = 0;
            // 
            // trainDisplay
            // 
            this.trainDisplay.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.trainDisplay.ColorMapLowerRoiLimit = 0D;
            this.trainDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.trainDisplay.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.trainDisplay.ColorMapUpperRoiLimit = 1D;
            this.trainDisplay.DoubleTapZoomCycleLength = 2;
            this.trainDisplay.DoubleTapZoomSensitivity = 2.5D;
            this.trainDisplay.Location = new System.Drawing.Point(392, 39);
            this.trainDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.trainDisplay.MouseWheelSensitivity = 1D;
            this.trainDisplay.Name = "trainDisplay";
            this.trainDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("trainDisplay.OcxState")));
            this.trainDisplay.Size = new System.Drawing.Size(278, 329);
            this.trainDisplay.TabIndex = 1;
            // 
            // roi_Btn
            // 
            this.roi_Btn.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.roi_Btn.Location = new System.Drawing.Point(199, 419);
            this.roi_Btn.Name = "roi_Btn";
            this.roi_Btn.Size = new System.Drawing.Size(175, 41);
            this.roi_Btn.TabIndex = 2;
            this.roi_Btn.Text = "ROI 설정";
            this.roi_Btn.UseVisualStyleBackColor = true;
            this.roi_Btn.Click += new System.EventHandler(this.RoiBtn_Click);
            // 
            // Train
            // 
            this.Train.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.Train.Location = new System.Drawing.Point(392, 374);
            this.Train.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Train.Name = "Train";
            this.Train.Size = new System.Drawing.Size(278, 39);
            this.Train.TabIndex = 3;
            this.Train.Text = "트레인";
            this.Train.UseVisualStyleBackColor = true;
            this.Train.Click += new System.EventHandler(this.Train_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 19);
            this.label1.TabIndex = 4;
            this.label1.Text = "이미지 로드";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(395, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "학습 이미지";
            // 
            // loadImage
            // 
            this.loadImage.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.loadImage.Location = new System.Drawing.Point(8, 374);
            this.loadImage.Name = "loadImage";
            this.loadImage.Size = new System.Drawing.Size(366, 39);
            this.loadImage.TabIndex = 6;
            this.loadImage.Text = "이미지 로드";
            this.loadImage.UseVisualStyleBackColor = true;
            this.loadImage.Click += new System.EventHandler(this.LoadImage_Click);
            // 
            // nextImage
            // 
            this.nextImage.BackColor = System.Drawing.Color.Snow;
            this.nextImage.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.nextImage.ForeColor = System.Drawing.Color.Black;
            this.nextImage.Location = new System.Drawing.Point(81, 585);
            this.nextImage.Name = "nextImage";
            this.nextImage.Size = new System.Drawing.Size(74, 35);
            this.nextImage.TabIndex = 7;
            this.nextImage.Text = "다음";
            this.nextImage.UseVisualStyleBackColor = false;
            this.nextImage.Click += new System.EventHandler(this.NextImage_Click);
            // 
            // beforeImage
            // 
            this.beforeImage.BackColor = System.Drawing.Color.Black;
            this.beforeImage.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.beforeImage.ForeColor = System.Drawing.Color.White;
            this.beforeImage.Location = new System.Drawing.Point(8, 585);
            this.beforeImage.Name = "beforeImage";
            this.beforeImage.Size = new System.Drawing.Size(74, 35);
            this.beforeImage.TabIndex = 8;
            this.beforeImage.Text = "이전";
            this.beforeImage.UseVisualStyleBackColor = false;
            this.beforeImage.Click += new System.EventHandler(this.BeforeImage_Click);
            // 
            // toolRun
            // 
            this.toolRun.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.toolRun.Location = new System.Drawing.Point(392, 419);
            this.toolRun.Name = "toolRun";
            this.toolRun.Size = new System.Drawing.Size(278, 39);
            this.toolRun.TabIndex = 12;
            this.toolRun.Text = "검사";
            this.toolRun.UseVisualStyleBackColor = true;
            this.toolRun.Click += new System.EventHandler(this.CheckRun_Click);
            // 
            // resultLabel
            // 
            this.resultLabel.AutoSize = true;
            this.resultLabel.Location = new System.Drawing.Point(695, 401);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(5, 12);
            this.resultLabel.TabIndex = 13;
            this.resultLabel.Text = "\r\n";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(694, 374);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 17);
            this.label3.TabIndex = 14;
            this.label3.Text = "검사 결과";
            // 
            // resultDisplay
            // 
            this.resultDisplay.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.resultDisplay.ColorMapLowerRoiLimit = 0D;
            this.resultDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.resultDisplay.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.resultDisplay.ColorMapUpperRoiLimit = 1D;
            this.resultDisplay.DoubleTapZoomCycleLength = 2;
            this.resultDisplay.DoubleTapZoomSensitivity = 2.5D;
            this.resultDisplay.Location = new System.Drawing.Point(691, 38);
            this.resultDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.resultDisplay.MouseWheelSensitivity = 1D;
            this.resultDisplay.Name = "resultDisplay";
            this.resultDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("resultDisplay.OcxState")));
            this.resultDisplay.Size = new System.Drawing.Size(378, 330);
            this.resultDisplay.TabIndex = 21;
            // 
            // imageListView
            // 
            this.imageListView.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.imageListView.HideSelection = false;
            this.imageListView.Location = new System.Drawing.Point(160, 476);
            this.imageListView.MultiSelect = false;
            this.imageListView.Name = "imageListView";
            this.imageListView.Size = new System.Drawing.Size(909, 150);
            this.imageListView.TabIndex = 22;
            this.imageListView.UseCompatibleStateImageBehavior = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(693, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 17);
            this.label4.TabIndex = 23;
            this.label4.Text = "검사 결과 이미지";
            // 
            // autoPlay
            // 
            this.autoPlay.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.autoPlay.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoPlay.Location = new System.Drawing.Point(8, 506);
            this.autoPlay.Name = "autoPlay";
            this.autoPlay.Size = new System.Drawing.Size(150, 35);
            this.autoPlay.TabIndex = 24;
            this.autoPlay.Text = "Auto Play";
            this.autoPlay.UseVisualStyleBackColor = false;
            this.autoPlay.Click += new System.EventHandler(this.AutoPlay_Click);
            // 
            // playTimer
            // 
            this.playTimer.DecimalPlaces = 1;
            this.playTimer.Increment = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.playTimer.Location = new System.Drawing.Point(59, 480);
            this.playTimer.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.playTimer.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.playTimer.Name = "playTimer";
            this.playTimer.Size = new System.Drawing.Size(95, 21);
            this.playTimer.TabIndex = 25;
            this.playTimer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.playTimer.ThousandsSeparator = true;
            this.playTimer.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(13, 483);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 15);
            this.label5.TabIndex = 26;
            this.label5.Text = "Timer";
            // 
            // setTime
            // 
            this.setTime.Tick += new System.EventHandler(this.SetTime_Tick);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Tomato;
            this.button1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.Location = new System.Drawing.Point(8, 545);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 35);
            this.button1.TabIndex = 27;
            this.button1.Text = "Auto Play Stop";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.StopAutoPlay_Click);
            // 
            // SearchRegion
            // 
            this.SearchRegion.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.SearchRegion.Location = new System.Drawing.Point(8, 419);
            this.SearchRegion.Name = "SearchRegion";
            this.SearchRegion.Size = new System.Drawing.Size(174, 41);
            this.SearchRegion.TabIndex = 28;
            this.SearchRegion.Text = "검색 영역";
            this.SearchRegion.UseVisualStyleBackColor = true;
            this.SearchRegion.Click += new System.EventHandler(this.SearchRegion_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 638);
            this.Controls.Add(this.SearchRegion);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.playTimer);
            this.Controls.Add(this.autoPlay);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.imageListView);
            this.Controls.Add(this.resultDisplay);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.resultLabel);
            this.Controls.Add(this.toolRun);
            this.Controls.Add(this.beforeImage);
            this.Controls.Add(this.nextImage);
            this.Controls.Add(this.loadImage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Train);
            this.Controls.Add(this.roi_Btn);
            this.Controls.Add(this.trainDisplay);
            this.Controls.Add(this.showImage);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.showImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trainDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playTimer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Cognex.VisionPro.Display.CogDisplay showImage;
        private Cognex.VisionPro.Display.CogDisplay trainDisplay;
        private System.Windows.Forms.Button roi_Btn;
        private System.Windows.Forms.Button Train;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button loadImage;
        private System.Windows.Forms.Button nextImage;
        private System.Windows.Forms.Button beforeImage;
        private System.Windows.Forms.Button toolRun;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.Label label3;
        private Cognex.VisionPro.CogRecordDisplay resultDisplay;
        private System.Windows.Forms.ListView imageListView;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button autoPlay;
        private System.Windows.Forms.NumericUpDown playTimer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Timer setTime;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button SearchRegion;
    }
}

