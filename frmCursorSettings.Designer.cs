namespace Unit3DStudio
{
    partial class frmCursorSettings
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
            this.label1 = new System.Windows.Forms.Label();
            this.trackMouseXY = new System.Windows.Forms.TrackBar();
            this.lblMouseXY = new System.Windows.Forms.Label();
            this.labelMouseWheel = new System.Windows.Forms.Label();
            this.trackMouseWheel = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblMouseAxis = new System.Windows.Forms.Label();
            this.trackMouseAxis = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.chkMouseWheelInverse = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackMouseXY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackMouseWheel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackMouseAxis)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Скорость перемещения указателем";
            // 
            // trackMouseXY
            // 
            this.trackMouseXY.Location = new System.Drawing.Point(226, 12);
            this.trackMouseXY.Maximum = 1000;
            this.trackMouseXY.Minimum = 1;
            this.trackMouseXY.Name = "trackMouseXY";
            this.trackMouseXY.Size = new System.Drawing.Size(241, 45);
            this.trackMouseXY.TabIndex = 20;
            this.trackMouseXY.Value = 50;
            this.trackMouseXY.ValueChanged += new System.EventHandler(this.trackMouseXY_ValueChanged);
            // 
            // lblMouseXY
            // 
            this.lblMouseXY.AutoSize = true;
            this.lblMouseXY.Location = new System.Drawing.Point(473, 24);
            this.lblMouseXY.Name = "lblMouseXY";
            this.lblMouseXY.Size = new System.Drawing.Size(27, 13);
            this.lblMouseXY.TabIndex = 21;
            this.lblMouseXY.Text = "50%";
            // 
            // labelMouseWheel
            // 
            this.labelMouseWheel.AutoSize = true;
            this.labelMouseWheel.Location = new System.Drawing.Point(473, 75);
            this.labelMouseWheel.Name = "labelMouseWheel";
            this.labelMouseWheel.Size = new System.Drawing.Size(27, 13);
            this.labelMouseWheel.TabIndex = 24;
            this.labelMouseWheel.Text = "50%";
            // 
            // trackMouseWheel
            // 
            this.trackMouseWheel.Location = new System.Drawing.Point(226, 63);
            this.trackMouseWheel.Maximum = 1000;
            this.trackMouseWheel.Minimum = 1;
            this.trackMouseWheel.Name = "trackMouseWheel";
            this.trackMouseWheel.Size = new System.Drawing.Size(241, 45);
            this.trackMouseWheel.TabIndex = 23;
            this.trackMouseWheel.Value = 50;
            this.trackMouseWheel.Scroll += new System.EventHandler(this.trackMouseWheel_Scroll);
            this.trackMouseWheel.ValueChanged += new System.EventHandler(this.trackMouseWheel_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(198, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Скорость масштабирования колесом";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(344, 197);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 25;
            this.btnOK.Text = "Сохранить";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(425, 197);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblMouseAxis
            // 
            this.lblMouseAxis.AutoSize = true;
            this.lblMouseAxis.Location = new System.Drawing.Point(473, 158);
            this.lblMouseAxis.Name = "lblMouseAxis";
            this.lblMouseAxis.Size = new System.Drawing.Size(27, 13);
            this.lblMouseAxis.TabIndex = 29;
            this.lblMouseAxis.Text = "50%";
            // 
            // trackMouseAxis
            // 
            this.trackMouseAxis.Location = new System.Drawing.Point(226, 146);
            this.trackMouseAxis.Maximum = 1000;
            this.trackMouseAxis.Minimum = 1;
            this.trackMouseAxis.Name = "trackMouseAxis";
            this.trackMouseAxis.Size = new System.Drawing.Size(241, 45);
            this.trackMouseAxis.TabIndex = 28;
            this.trackMouseAxis.Value = 50;
            this.trackMouseAxis.ValueChanged += new System.EventHandler(this.trackMouseAxis_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Скорость поворота осей";
            // 
            // chkMouseWheelInverse
            // 
            this.chkMouseWheelInverse.AutoSize = true;
            this.chkMouseWheelInverse.Location = new System.Drawing.Point(31, 113);
            this.chkMouseWheelInverse.Name = "chkMouseWheelInverse";
            this.chkMouseWheelInverse.Size = new System.Drawing.Size(176, 17);
            this.chkMouseWheelInverse.TabIndex = 30;
            this.chkMouseWheelInverse.Text = "Инвертировать колесо мыши";
            this.chkMouseWheelInverse.UseVisualStyleBackColor = true;
            // 
            // frmCursorSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 242);
            this.Controls.Add(this.chkMouseWheelInverse);
            this.Controls.Add(this.lblMouseAxis);
            this.Controls.Add(this.trackMouseAxis);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.labelMouseWheel);
            this.Controls.Add(this.trackMouseWheel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblMouseXY);
            this.Controls.Add(this.trackMouseXY);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCursorSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки курсора мыши";
            this.Load += new System.EventHandler(this.frmCursorSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackMouseXY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackMouseWheel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackMouseAxis)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackMouseXY;
        private System.Windows.Forms.Label lblMouseXY;
        private System.Windows.Forms.Label labelMouseWheel;
        private System.Windows.Forms.TrackBar trackMouseWheel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblMouseAxis;
        private System.Windows.Forms.TrackBar trackMouseAxis;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkMouseWheelInverse;
    }
}