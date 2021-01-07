namespace Unit3DStudio
{
    partial class frmSaveImage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chkAlpha = new System.Windows.Forms.CheckBox();
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.chkFrame = new System.Windows.Forms.CheckBox();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSeriaY = new System.Windows.Forms.CheckBox();
            this.nudRotate = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudCameraAngle = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.nudFileNum = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.nudFileMask = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.nudCameraZ = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRotate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCameraAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFileNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFileMask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCameraZ)).BeginInit();
            this.SuspendLayout();
            // 
            // chkAlpha
            // 
            this.chkAlpha.AutoSize = true;
            this.chkAlpha.Checked = true;
            this.chkAlpha.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAlpha.Location = new System.Drawing.Point(21, 61);
            this.chkAlpha.Name = "chkAlpha";
            this.chkAlpha.Size = new System.Drawing.Size(255, 17);
            this.chkAlpha.TabIndex = 0;
            this.chkAlpha.Text = "Сохранять альфа-канал (прозрачность фона)";
            this.chkAlpha.UseVisualStyleBackColor = true;
            // 
            // nudWidth
            // 
            this.nudWidth.Enabled = false;
            this.nudWidth.Location = new System.Drawing.Point(203, 107);
            this.nudWidth.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.nudWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.Size = new System.Drawing.Size(85, 20);
            this.nudWidth.TabIndex = 3;
            this.nudWidth.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            // 
            // chkFrame
            // 
            this.chkFrame.AutoSize = true;
            this.chkFrame.Location = new System.Drawing.Point(21, 108);
            this.chkFrame.Name = "chkFrame";
            this.chkFrame.Size = new System.Drawing.Size(176, 17);
            this.chkFrame.TabIndex = 2;
            this.chkFrame.Text = "Ограничить окном с шириной";
            this.chkFrame.UseVisualStyleBackColor = true;
            this.chkFrame.CheckedChanged += new System.EventHandler(this.chkFrame_CheckedChanged);
            // 
            // nudHeight
            // 
            this.nudHeight.Enabled = false;
            this.nudHeight.Location = new System.Drawing.Point(359, 108);
            this.nudHeight.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.nudHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.Size = new System.Drawing.Size(85, 20);
            this.nudHeight.TabIndex = 4;
            this.nudHeight.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(294, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "и высотой";
            // 
            // chkSeriaY
            // 
            this.chkSeriaY.AutoSize = true;
            this.chkSeriaY.Location = new System.Drawing.Point(21, 156);
            this.chkSeriaY.Name = "chkSeriaY";
            this.chkSeriaY.Size = new System.Drawing.Size(403, 17);
            this.chkSeriaY.TabIndex = 5;
            this.chkSeriaY.Text = "Выполнить серию снимков с вращением проекта вокруг оси OY с шагом ";
            this.chkSeriaY.UseVisualStyleBackColor = true;
            this.chkSeriaY.CheckedChanged += new System.EventHandler(this.chkSeriaY_CheckedChanged);
            // 
            // nudRotate
            // 
            this.nudRotate.DecimalPlaces = 3;
            this.nudRotate.Enabled = false;
            this.nudRotate.Location = new System.Drawing.Point(430, 155);
            this.nudRotate.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.nudRotate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRotate.Name = "nudRotate";
            this.nudRotate.Size = new System.Drawing.Size(63, 20);
            this.nudRotate.TabIndex = 5;
            this.nudRotate.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(499, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "градусов";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(288, 217);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Выполнить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(369, 217);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(563, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "при наклоне камеры";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(751, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "градусов";
            // 
            // nudCameraAngle
            // 
            this.nudCameraAngle.DecimalPlaces = 3;
            this.nudCameraAngle.Enabled = false;
            this.nudCameraAngle.Location = new System.Drawing.Point(682, 155);
            this.nudCameraAngle.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.nudCameraAngle.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.nudCameraAngle.Name = "nudCameraAngle";
            this.nudCameraAngle.Size = new System.Drawing.Size(63, 20);
            this.nudCameraAngle.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Маска имени файла";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(135, 26);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(309, 20);
            this.txtFileName.TabIndex = 0;
            // 
            // nudFileNum
            // 
            this.nudFileNum.Enabled = false;
            this.nudFileNum.Location = new System.Drawing.Point(538, 27);
            this.nudFileNum.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudFileNum.Name = "nudFileNum";
            this.nudFileNum.Size = new System.Drawing.Size(85, 20);
            this.nudFileNum.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(459, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Нумерация с";
            // 
            // nudFileMask
            // 
            this.nudFileMask.Enabled = false;
            this.nudFileMask.Location = new System.Drawing.Point(702, 27);
            this.nudFileMask.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.nudFileMask.Name = "nudFileMask";
            this.nudFileMask.Size = new System.Drawing.Size(64, 20);
            this.nudFileMask.TabIndex = 2;
            this.nudFileMask.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(629, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "заполнение";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(772, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "цифры";
            // 
            // nudCameraZ
            // 
            this.nudCameraZ.Enabled = false;
            this.nudCameraZ.Location = new System.Drawing.Point(682, 182);
            this.nudCameraZ.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudCameraZ.Name = "nudCameraZ";
            this.nudCameraZ.Size = new System.Drawing.Size(122, 20);
            this.nudCameraZ.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(563, 184);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(118, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "при удалении камеры";
            // 
            // frmSaveImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 273);
            this.Controls.Add(this.nudCameraZ);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nudFileMask);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.nudFileNum);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudCameraAngle);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudRotate);
            this.Controls.Add(this.chkSeriaY);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudHeight);
            this.Controls.Add(this.chkFrame);
            this.Controls.Add(this.nudWidth);
            this.Controls.Add(this.chkAlpha);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSaveImage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сохранение модели в файл рисунка";
            this.Load += new System.EventHandler(this.frmSaveImage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRotate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCameraAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFileNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFileMask)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCameraZ)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkAlpha;
        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.CheckBox chkFrame;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkSeriaY;
        private System.Windows.Forms.NumericUpDown nudRotate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudCameraAngle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.NumericUpDown nudFileNum;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudFileMask;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudCameraZ;
        private System.Windows.Forms.Label label10;
    }
}