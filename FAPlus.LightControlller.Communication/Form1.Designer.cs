namespace FAPlus.LightControlller.Communication
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
            this.OnButton = new System.Windows.Forms.RadioButton();
            this.OffButton = new System.Windows.Forms.RadioButton();
            this.Ch_1 = new System.Windows.Forms.CheckBox();
            this.Ch_2 = new System.Windows.Forms.CheckBox();
            this.Ch_3 = new System.Windows.Forms.CheckBox();
            this.Ch_4 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OnButton
            // 
            this.OnButton.AutoSize = true;
            this.OnButton.Location = new System.Drawing.Point(29, 218);
            this.OnButton.Name = "OnButton";
            this.OnButton.Size = new System.Drawing.Size(41, 16);
            this.OnButton.TabIndex = 0;
            this.OnButton.TabStop = true;
            this.OnButton.Text = "ON";
            this.OnButton.UseVisualStyleBackColor = true;
            this.OnButton.CheckedChanged += new System.EventHandler(this.OnButton_CheckedChanged);
            // 
            // OffButton
            // 
            this.OffButton.AutoSize = true;
            this.OffButton.Location = new System.Drawing.Point(91, 218);
            this.OffButton.Name = "OffButton";
            this.OffButton.Size = new System.Drawing.Size(46, 16);
            this.OffButton.TabIndex = 1;
            this.OffButton.TabStop = true;
            this.OffButton.Text = "OFF";
            this.OffButton.UseVisualStyleBackColor = true;
            this.OffButton.CheckedChanged += new System.EventHandler(this.OffButton_CheckedChanged);
            // 
            // Ch_1
            // 
            this.Ch_1.AutoSize = true;
            this.Ch_1.Location = new System.Drawing.Point(29, 281);
            this.Ch_1.Name = "Ch_1";
            this.Ch_1.Size = new System.Drawing.Size(81, 16);
            this.Ch_1.TabIndex = 2;
            this.Ch_1.Text = "Channel 1";
            this.Ch_1.UseVisualStyleBackColor = true;
            this.Ch_1.UseWaitCursor = true;
            // 
            // Ch_2
            // 
            this.Ch_2.AutoSize = true;
            this.Ch_2.Location = new System.Drawing.Point(141, 280);
            this.Ch_2.Name = "Ch_2";
            this.Ch_2.Size = new System.Drawing.Size(81, 16);
            this.Ch_2.TabIndex = 3;
            this.Ch_2.Text = "Channel 2";
            this.Ch_2.UseVisualStyleBackColor = true;
            // 
            // Ch_3
            // 
            this.Ch_3.AutoSize = true;
            this.Ch_3.Location = new System.Drawing.Point(29, 303);
            this.Ch_3.Name = "Ch_3";
            this.Ch_3.Size = new System.Drawing.Size(81, 16);
            this.Ch_3.TabIndex = 4;
            this.Ch_3.Text = "Channel 3";
            this.Ch_3.UseVisualStyleBackColor = true;
            // 
            // Ch_4
            // 
            this.Ch_4.AutoSize = true;
            this.Ch_4.Location = new System.Drawing.Point(141, 302);
            this.Ch_4.Name = "Ch_4";
            this.Ch_4.Size = new System.Drawing.Size(81, 16);
            this.Ch_4.TabIndex = 5;
            this.Ch_4.Text = "Channel 4";
            this.Ch_4.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 192);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "※ Light ON/OFF";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 257);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "※ Light Channel";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Ch_4);
            this.Controls.Add(this.Ch_3);
            this.Controls.Add(this.Ch_2);
            this.Controls.Add(this.Ch_1);
            this.Controls.Add(this.OffButton);
            this.Controls.Add(this.OnButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton OnButton;
        private System.Windows.Forms.RadioButton OffButton;
        private System.Windows.Forms.CheckBox Ch_1;
        private System.Windows.Forms.CheckBox Ch_2;
        private System.Windows.Forms.CheckBox Ch_3;
        private System.Windows.Forms.CheckBox Ch_4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

