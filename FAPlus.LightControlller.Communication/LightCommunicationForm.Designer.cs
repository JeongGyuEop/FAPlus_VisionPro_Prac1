namespace FAPlus.LightControlller.Communication
{
    partial class LightCommunicationForm
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
            this.Ch1_OffBtn = new System.Windows.Forms.RadioButton();
            this.Ch_1 = new System.Windows.Forms.CheckBox();
            this.Ch_2 = new System.Windows.Forms.CheckBox();
            this.Ch_3 = new System.Windows.Forms.CheckBox();
            this.Ch_4 = new System.Windows.Forms.CheckBox();
            this.Ch2_OnBtn = new System.Windows.Forms.RadioButton();
            this.Ch3_OnBtn = new System.Windows.Forms.RadioButton();
            this.Ch4_OnBtn = new System.Windows.Forms.RadioButton();
            this.Ch2_OffBtn = new System.Windows.Forms.RadioButton();
            this.Ch3_OffBtn = new System.Windows.Forms.RadioButton();
            this.Ch4_OffBtn = new System.Windows.Forms.RadioButton();
            this.Ch1_LSize = new System.Windows.Forms.NumericUpDown();
            this.Ch2_LSize = new System.Windows.Forms.NumericUpDown();
            this.Ch3_LSize = new System.Windows.Forms.NumericUpDown();
            this.Ch4_LSize = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.comboCommType = new System.Windows.Forms.ComboBox();
            this.End = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Ch1_LSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ch2_LSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ch3_LSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ch4_LSize)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // Ch1_OffBtn
            // 
            this.Ch1_OffBtn.AutoSize = true;
            this.Ch1_OffBtn.Location = new System.Drawing.Point(75, 20);
            this.Ch1_OffBtn.Name = "Ch1_OffBtn";
            this.Ch1_OffBtn.Size = new System.Drawing.Size(46, 16);
            this.Ch1_OffBtn.TabIndex = 1;
            this.Ch1_OffBtn.TabStop = true;
            this.Ch1_OffBtn.Tag = "1";
            this.Ch1_OffBtn.Text = "OFF";
            this.Ch1_OffBtn.UseVisualStyleBackColor = true;
            this.Ch1_OffBtn.CheckedChanged += new System.EventHandler(this.OffBtn_CheckedChanged);
            // 
            // Ch_1
            // 
            this.Ch_1.AutoSize = true;
            this.Ch_1.Location = new System.Drawing.Point(20, 23);
            this.Ch_1.Name = "Ch_1";
            this.Ch_1.Size = new System.Drawing.Size(81, 16);
            this.Ch_1.TabIndex = 2;
            this.Ch_1.Tag = "ch_1";
            this.Ch_1.Text = "Channel 1";
            this.Ch_1.UseVisualStyleBackColor = true;
            this.Ch_1.UseWaitCursor = true;
            this.Ch_1.CheckedChanged += new System.EventHandler(this.Ch_CheckedChanged);
            // 
            // Ch_2
            // 
            this.Ch_2.AutoSize = true;
            this.Ch_2.Location = new System.Drawing.Point(26, 23);
            this.Ch_2.Name = "Ch_2";
            this.Ch_2.Size = new System.Drawing.Size(81, 16);
            this.Ch_2.TabIndex = 3;
            this.Ch_2.Tag = "ch_2";
            this.Ch_2.Text = "Channel 2";
            this.Ch_2.UseVisualStyleBackColor = true;
            this.Ch_2.CheckedChanged += new System.EventHandler(this.Ch_CheckedChanged);
            // 
            // Ch_3
            // 
            this.Ch_3.AutoSize = true;
            this.Ch_3.Location = new System.Drawing.Point(29, 23);
            this.Ch_3.Name = "Ch_3";
            this.Ch_3.Size = new System.Drawing.Size(81, 16);
            this.Ch_3.TabIndex = 4;
            this.Ch_3.Tag = "ch_3";
            this.Ch_3.Text = "Channel 3";
            this.Ch_3.UseVisualStyleBackColor = true;
            this.Ch_3.CheckedChanged += new System.EventHandler(this.Ch_CheckedChanged);
            // 
            // Ch_4
            // 
            this.Ch_4.AutoSize = true;
            this.Ch_4.Location = new System.Drawing.Point(30, 19);
            this.Ch_4.Name = "Ch_4";
            this.Ch_4.Size = new System.Drawing.Size(81, 16);
            this.Ch_4.TabIndex = 5;
            this.Ch_4.Tag = "ch_4";
            this.Ch_4.Text = "Channel 4";
            this.Ch_4.UseVisualStyleBackColor = true;
            this.Ch_4.CheckedChanged += new System.EventHandler(this.Ch_CheckedChanged);
            // 
            // Ch2_OnBtn
            // 
            this.Ch2_OnBtn.AutoSize = true;
            this.Ch2_OnBtn.Location = new System.Drawing.Point(22, 21);
            this.Ch2_OnBtn.Name = "Ch2_OnBtn";
            this.Ch2_OnBtn.Size = new System.Drawing.Size(41, 16);
            this.Ch2_OnBtn.TabIndex = 6;
            this.Ch2_OnBtn.TabStop = true;
            this.Ch2_OnBtn.Tag = "2";
            this.Ch2_OnBtn.Text = "ON";
            this.Ch2_OnBtn.UseVisualStyleBackColor = true;
            this.Ch2_OnBtn.CheckedChanged += new System.EventHandler(this.OnBtn_CheckedChanged);
            // 
            // Ch3_OnBtn
            // 
            this.Ch3_OnBtn.AutoSize = true;
            this.Ch3_OnBtn.Location = new System.Drawing.Point(26, 27);
            this.Ch3_OnBtn.Name = "Ch3_OnBtn";
            this.Ch3_OnBtn.Size = new System.Drawing.Size(41, 16);
            this.Ch3_OnBtn.TabIndex = 7;
            this.Ch3_OnBtn.TabStop = true;
            this.Ch3_OnBtn.Tag = "3";
            this.Ch3_OnBtn.Text = "ON";
            this.Ch3_OnBtn.UseVisualStyleBackColor = true;
            this.Ch3_OnBtn.CheckedChanged += new System.EventHandler(this.OnBtn_CheckedChanged);
            // 
            // Ch4_OnBtn
            // 
            this.Ch4_OnBtn.AutoSize = true;
            this.Ch4_OnBtn.Location = new System.Drawing.Point(24, 27);
            this.Ch4_OnBtn.Name = "Ch4_OnBtn";
            this.Ch4_OnBtn.Size = new System.Drawing.Size(41, 16);
            this.Ch4_OnBtn.TabIndex = 8;
            this.Ch4_OnBtn.TabStop = true;
            this.Ch4_OnBtn.Tag = "4";
            this.Ch4_OnBtn.Text = "ON";
            this.Ch4_OnBtn.UseVisualStyleBackColor = true;
            this.Ch4_OnBtn.CheckedChanged += new System.EventHandler(this.OnBtn_CheckedChanged);
            // 
            // Ch2_OffBtn
            // 
            this.Ch2_OffBtn.AutoSize = true;
            this.Ch2_OffBtn.Location = new System.Drawing.Point(80, 21);
            this.Ch2_OffBtn.Name = "Ch2_OffBtn";
            this.Ch2_OffBtn.Size = new System.Drawing.Size(46, 16);
            this.Ch2_OffBtn.TabIndex = 9;
            this.Ch2_OffBtn.TabStop = true;
            this.Ch2_OffBtn.Tag = "2";
            this.Ch2_OffBtn.Text = "OFF";
            this.Ch2_OffBtn.UseVisualStyleBackColor = true;
            this.Ch2_OffBtn.CheckedChanged += new System.EventHandler(this.OffBtn_CheckedChanged);
            // 
            // Ch3_OffBtn
            // 
            this.Ch3_OffBtn.AutoSize = true;
            this.Ch3_OffBtn.Location = new System.Drawing.Point(84, 27);
            this.Ch3_OffBtn.Name = "Ch3_OffBtn";
            this.Ch3_OffBtn.Size = new System.Drawing.Size(46, 16);
            this.Ch3_OffBtn.TabIndex = 10;
            this.Ch3_OffBtn.TabStop = true;
            this.Ch3_OffBtn.Tag = "3";
            this.Ch3_OffBtn.Text = "OFF";
            this.Ch3_OffBtn.UseVisualStyleBackColor = true;
            this.Ch3_OffBtn.CheckedChanged += new System.EventHandler(this.OffBtn_CheckedChanged);
            // 
            // Ch4_OffBtn
            // 
            this.Ch4_OffBtn.AutoSize = true;
            this.Ch4_OffBtn.Location = new System.Drawing.Point(82, 27);
            this.Ch4_OffBtn.Name = "Ch4_OffBtn";
            this.Ch4_OffBtn.Size = new System.Drawing.Size(46, 16);
            this.Ch4_OffBtn.TabIndex = 11;
            this.Ch4_OffBtn.TabStop = true;
            this.Ch4_OffBtn.Tag = "4";
            this.Ch4_OffBtn.Text = "OFF";
            this.Ch4_OffBtn.UseVisualStyleBackColor = true;
            this.Ch4_OffBtn.CheckedChanged += new System.EventHandler(this.OffBtn_CheckedChanged);
            // 
            // Ch1_LSize
            // 
            this.Ch1_LSize.Location = new System.Drawing.Point(17, 54);
            this.Ch1_LSize.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Ch1_LSize.Name = "Ch1_LSize";
            this.Ch1_LSize.Size = new System.Drawing.Size(104, 21);
            this.Ch1_LSize.TabIndex = 12;
            this.Ch1_LSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Ch1_LSize.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Ch1_LSize.ValueChanged += new System.EventHandler(this.Ch_LSize_ValueChanged);
            // 
            // Ch2_LSize
            // 
            this.Ch2_LSize.Location = new System.Drawing.Point(22, 55);
            this.Ch2_LSize.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Ch2_LSize.Name = "Ch2_LSize";
            this.Ch2_LSize.Size = new System.Drawing.Size(104, 21);
            this.Ch2_LSize.TabIndex = 13;
            this.Ch2_LSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Ch2_LSize.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Ch2_LSize.ValueChanged += new System.EventHandler(this.Ch_LSize_ValueChanged);
            // 
            // Ch3_LSize
            // 
            this.Ch3_LSize.Location = new System.Drawing.Point(26, 61);
            this.Ch3_LSize.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Ch3_LSize.Name = "Ch3_LSize";
            this.Ch3_LSize.Size = new System.Drawing.Size(104, 21);
            this.Ch3_LSize.TabIndex = 14;
            this.Ch3_LSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Ch3_LSize.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Ch3_LSize.ValueChanged += new System.EventHandler(this.Ch_LSize_ValueChanged);
            // 
            // Ch4_LSize
            // 
            this.Ch4_LSize.Location = new System.Drawing.Point(24, 61);
            this.Ch4_LSize.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Ch4_LSize.Name = "Ch4_LSize";
            this.Ch4_LSize.Size = new System.Drawing.Size(104, 21);
            this.Ch4_LSize.TabIndex = 15;
            this.Ch4_LSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Ch4_LSize.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Ch4_LSize.ValueChanged += new System.EventHandler(this.Ch_LSize_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.Ch_1);
            this.panel1.Location = new System.Drawing.Point(58, 108);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(153, 288);
            this.panel1.TabIndex = 16;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.radioButton1);
            this.panel5.Controls.Add(this.Ch1_LSize);
            this.panel5.Controls.Add(this.Ch1_OffBtn);
            this.panel5.Location = new System.Drawing.Point(3, 45);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(147, 98);
            this.panel5.TabIndex = 14;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(17, 20);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(41, 16);
            this.radioButton1.TabIndex = 13;
            this.radioButton1.TabStop = true;
            this.radioButton1.Tag = "1";
            this.radioButton1.Text = "ON";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.OnBtn_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.Ch_2);
            this.panel2.Location = new System.Drawing.Point(230, 108);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(155, 288);
            this.panel2.TabIndex = 17;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.Ch2_LSize);
            this.panel6.Controls.Add(this.Ch2_OffBtn);
            this.panel6.Controls.Add(this.Ch2_OnBtn);
            this.panel6.Location = new System.Drawing.Point(3, 45);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(148, 100);
            this.panel6.TabIndex = 14;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel7);
            this.panel3.Controls.Add(this.Ch_3);
            this.panel3.Location = new System.Drawing.Point(402, 108);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(159, 288);
            this.panel3.TabIndex = 18;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.Ch3_LSize);
            this.panel7.Controls.Add(this.Ch3_OffBtn);
            this.panel7.Controls.Add(this.Ch3_OnBtn);
            this.panel7.Location = new System.Drawing.Point(3, 45);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(153, 100);
            this.panel7.TabIndex = 15;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel8);
            this.panel4.Controls.Add(this.Ch_4);
            this.panel4.Location = new System.Drawing.Point(581, 108);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(158, 288);
            this.panel4.TabIndex = 19;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.Ch4_LSize);
            this.panel8.Controls.Add(this.Ch4_OffBtn);
            this.panel8.Controls.Add(this.Ch4_OnBtn);
            this.panel8.Location = new System.Drawing.Point(6, 45);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(149, 100);
            this.panel8.TabIndex = 16;
            // 
            // comboCommType
            // 
            this.comboCommType.FormattingEnabled = true;
            this.comboCommType.Location = new System.Drawing.Point(58, 37);
            this.comboCommType.Name = "comboCommType";
            this.comboCommType.Size = new System.Drawing.Size(150, 20);
            this.comboCommType.TabIndex = 20;
            // 
            // End
            // 
            this.End.Location = new System.Drawing.Point(230, 37);
            this.End.Name = "End";
            this.End.Size = new System.Drawing.Size(75, 23);
            this.End.TabIndex = 21;
            this.End.Text = "End";
            this.End.UseVisualStyleBackColor = true;
            this.End.Click += new System.EventHandler(this.End_Click);
            // 
            // LightCommunicationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.End);
            this.Controls.Add(this.comboCommType);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "LightCommunicationForm";
            this.Text = "LightCommunicationForm";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Ch1_LSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ch2_LSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ch3_LSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ch4_LSize)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RadioButton Ch1_OffBtn;
        private System.Windows.Forms.CheckBox Ch_1;
        private System.Windows.Forms.CheckBox Ch_2;
        private System.Windows.Forms.CheckBox Ch_3;
        private System.Windows.Forms.CheckBox Ch_4;
        private System.Windows.Forms.RadioButton Ch2_OnBtn;
        private System.Windows.Forms.RadioButton Ch3_OnBtn;
        private System.Windows.Forms.RadioButton Ch4_OnBtn;
        private System.Windows.Forms.RadioButton Ch2_OffBtn;
        private System.Windows.Forms.RadioButton Ch3_OffBtn;
        private System.Windows.Forms.RadioButton Ch4_OffBtn;
        private System.Windows.Forms.NumericUpDown Ch1_LSize;
        private System.Windows.Forms.NumericUpDown Ch2_LSize;
        private System.Windows.Forms.NumericUpDown Ch3_LSize;
        private System.Windows.Forms.NumericUpDown Ch4_LSize;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.ComboBox comboCommType;
        private System.Windows.Forms.Button End;
    }
}

