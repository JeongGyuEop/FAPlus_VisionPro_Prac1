namespace FAPlus.AquisitionCamera
{
    partial class AquisitionCameraForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AquisitionCameraForm));
            this.cogDisplay1 = new Cognex.VisionPro.Display.CogDisplay();
            this.BoardTypeLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.VideoFormatCombo = new System.Windows.Forms.ComboBox();
            this.brightnessUpDown = new System.Windows.Forms.NumericUpDown();
            this.contrastUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblNoBrightness = new System.Windows.Forms.Label();
            this.lblNoContrast = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblNoExposure = new System.Windows.Forms.Label();
            this.exposureUpDown = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contrastUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exposureUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // cogDisplay1
            // 
            this.cogDisplay1.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay1.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay1.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay1.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay1.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay1.DoubleTapZoomCycleLength = 2;
            this.cogDisplay1.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay1.Location = new System.Drawing.Point(394, 12);
            this.cogDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay1.MouseWheelSensitivity = 1D;
            this.cogDisplay1.Name = "cogDisplay1";
            this.cogDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay1.OcxState")));
            this.cogDisplay1.Size = new System.Drawing.Size(394, 366);
            this.cogDisplay1.TabIndex = 0;
            // 
            // BoardTypeLabel
            // 
            this.BoardTypeLabel.AutoSize = true;
            this.BoardTypeLabel.Location = new System.Drawing.Point(14, 59);
            this.BoardTypeLabel.Name = "BoardTypeLabel";
            this.BoardTypeLabel.Size = new System.Drawing.Size(17, 12);
            this.BoardTypeLabel.TabIndex = 4;
            this.BoardTypeLabel.Text = " : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "FrameGrabber";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "Video Format :";
            // 
            // VideoFormatCombo
            // 
            this.VideoFormatCombo.FormattingEnabled = true;
            this.VideoFormatCombo.Location = new System.Drawing.Point(14, 101);
            this.VideoFormatCombo.Name = "VideoFormatCombo";
            this.VideoFormatCombo.Size = new System.Drawing.Size(357, 20);
            this.VideoFormatCombo.TabIndex = 7;
            this.VideoFormatCombo.SelectedIndexChanged += new System.EventHandler(this.VideoFormatCombo_SelectedIndexChanged);
            // 
            // brightnessUpDown
            // 
            this.brightnessUpDown.DecimalPlaces = 3;
            this.brightnessUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.brightnessUpDown.Location = new System.Drawing.Point(131, 183);
            this.brightnessUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.brightnessUpDown.Name = "brightnessUpDown";
            this.brightnessUpDown.Size = new System.Drawing.Size(156, 21);
            this.brightnessUpDown.TabIndex = 8;
            this.brightnessUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.brightnessUpDown.ValueChanged += new System.EventHandler(this.brightnessUpDown_ValueChanged);
            // 
            // contrastUpDown
            // 
            this.contrastUpDown.DecimalPlaces = 3;
            this.contrastUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.contrastUpDown.Location = new System.Drawing.Point(131, 219);
            this.contrastUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.contrastUpDown.Name = "contrastUpDown";
            this.contrastUpDown.Size = new System.Drawing.Size(156, 21);
            this.contrastUpDown.TabIndex = 9;
            this.contrastUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.contrastUpDown.ValueChanged += new System.EventHandler(this.contrastUpDown_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 188);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "Brightness : ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(67, 224);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "Contrast : ";
            // 
            // lblNoBrightness
            // 
            this.lblNoBrightness.AutoSize = true;
            this.lblNoBrightness.Location = new System.Drawing.Point(59, 188);
            this.lblNoBrightness.Name = "lblNoBrightness";
            this.lblNoBrightness.Size = new System.Drawing.Size(228, 12);
            this.lblNoBrightness.TabIndex = 22;
            this.lblNoBrightness.Text = "CogAcqFifo Doesn\'t Support Brightness";
            this.lblNoBrightness.Visible = false;
            // 
            // lblNoContrast
            // 
            this.lblNoContrast.AutoSize = true;
            this.lblNoContrast.Location = new System.Drawing.Point(67, 224);
            this.lblNoContrast.Name = "lblNoContrast";
            this.lblNoContrast.Size = new System.Drawing.Size(215, 12);
            this.lblNoContrast.TabIndex = 23;
            this.lblNoContrast.Text = "CogAcqFifo Doesn\'t Support Contrast";
            this.lblNoContrast.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(293, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 24;
            this.label5.Text = "ms";
            // 
            // lblNoExposure
            // 
            this.lblNoExposure.AutoSize = true;
            this.lblNoExposure.Location = new System.Drawing.Point(60, 150);
            this.lblNoExposure.Name = "lblNoExposure";
            this.lblNoExposure.Size = new System.Drawing.Size(71, 12);
            this.lblNoExposure.TabIndex = 25;
            this.lblNoExposure.Text = "Exposure : ";
            // 
            // exposureUpDown
            // 
            this.exposureUpDown.DecimalPlaces = 2;
            this.exposureUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.exposureUpDown.Location = new System.Drawing.Point(131, 146);
            this.exposureUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.exposureUpDown.Name = "exposureUpDown";
            this.exposureUpDown.Size = new System.Drawing.Size(156, 21);
            this.exposureUpDown.TabIndex = 26;
            this.exposureUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.exposureUpDown.ValueChanged += new System.EventHandler(this.exposureUpDown_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(67, 150);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(222, 12);
            this.label7.TabIndex = 27;
            this.label7.Text = "CogAcqFifo Doesn\'t Support Exposure";
            this.label7.Visible = false;
            // 
            // AquisitionCameraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 400);
            this.Controls.Add(this.exposureUpDown);
            this.Controls.Add(this.lblNoExposure);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.contrastUpDown);
            this.Controls.Add(this.brightnessUpDown);
            this.Controls.Add(this.VideoFormatCombo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BoardTypeLabel);
            this.Controls.Add(this.cogDisplay1);
            this.Controls.Add(this.lblNoBrightness);
            this.Controls.Add(this.lblNoContrast);
            this.Controls.Add(this.label7);
            this.Name = "AquisitionCameraForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contrastUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exposureUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Cognex.VisionPro.Display.CogDisplay cogDisplay1;
        private System.Windows.Forms.Label BoardTypeLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox VideoFormatCombo;
        private System.Windows.Forms.NumericUpDown brightnessUpDown;
        private System.Windows.Forms.NumericUpDown contrastUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        internal System.Windows.Forms.Label lblNoBrightness;
        internal System.Windows.Forms.Label lblNoContrast;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblNoExposure;
        private System.Windows.Forms.NumericUpDown exposureUpDown;
        internal System.Windows.Forms.Label label7;
    }
}

