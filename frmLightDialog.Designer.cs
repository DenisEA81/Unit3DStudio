namespace Unit3DStudio
{
    partial class frmLightDialog
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
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.panelAmbient = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.trackAmbient = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.trackDirect = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.panelDirect = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblAmbientPower = new System.Windows.Forms.Label();
            this.lblDirectPower = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panelBackground = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.trackAmbient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackDirect)).BeginInit();
            this.SuspendLayout();
            // 
            // panelAmbient
            // 
            this.panelAmbient.BackColor = System.Drawing.Color.White;
            this.panelAmbient.Location = new System.Drawing.Point(136, 21);
            this.panelAmbient.Name = "panelAmbient";
            this.panelAmbient.Size = new System.Drawing.Size(99, 33);
            this.panelAmbient.TabIndex = 2;
            this.panelAmbient.Click += new System.EventHandler(this.panelAmbient_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Фоновый свет";
            // 
            // trackAmbient
            // 
            this.trackAmbient.Location = new System.Drawing.Point(250, 21);
            this.trackAmbient.Maximum = 1000;
            this.trackAmbient.Name = "trackAmbient";
            this.trackAmbient.Size = new System.Drawing.Size(104, 45);
            this.trackAmbient.TabIndex = 4;
            this.trackAmbient.Scroll += new System.EventHandler(this.trackAmbient_Scroll);
            this.trackAmbient.ValueChanged += new System.EventHandler(this.trackAmbient_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(265, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Сила света, %";
            // 
            // trackDirect
            // 
            this.trackDirect.Location = new System.Drawing.Point(250, 75);
            this.trackDirect.Maximum = 1000;
            this.trackDirect.Name = "trackDirect";
            this.trackDirect.Size = new System.Drawing.Size(104, 45);
            this.trackDirect.TabIndex = 8;
            this.trackDirect.ValueChanged += new System.EventHandler(this.trackDirect_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Направленный свет";
            // 
            // panelDirect
            // 
            this.panelDirect.BackColor = System.Drawing.Color.White;
            this.panelDirect.Location = new System.Drawing.Point(136, 75);
            this.panelDirect.Name = "panelDirect";
            this.panelDirect.Size = new System.Drawing.Size(99, 33);
            this.panelDirect.TabIndex = 6;
            this.panelDirect.Click += new System.EventHandler(this.panelDirect_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(231, 195);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "Сохранить";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(312, 195);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblAmbientPower
            // 
            this.lblAmbientPower.AutoSize = true;
            this.lblAmbientPower.Location = new System.Drawing.Point(360, 31);
            this.lblAmbientPower.Name = "lblAmbientPower";
            this.lblAmbientPower.Size = new System.Drawing.Size(27, 13);
            this.lblAmbientPower.TabIndex = 11;
            this.lblAmbientPower.Text = "90%";
            // 
            // lblDirectPower
            // 
            this.lblDirectPower.AutoSize = true;
            this.lblDirectPower.Location = new System.Drawing.Point(360, 85);
            this.lblDirectPower.Name = "lblDirectPower";
            this.lblDirectPower.Size = new System.Drawing.Size(27, 13);
            this.lblDirectPower.TabIndex = 12;
            this.lblDirectPower.Text = "90%";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Цвет фона";
            // 
            // panelBackground
            // 
            this.panelBackground.BackColor = System.Drawing.Color.White;
            this.panelBackground.Location = new System.Drawing.Point(136, 126);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(99, 33);
            this.panelBackground.TabIndex = 13;
            this.panelBackground.Click += new System.EventHandler(this.panelBackground_Click);
            this.panelBackground.Paint += new System.Windows.Forms.PaintEventHandler(this.panelBackground_Paint);
            // 
            // frmLightDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 242);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panelBackground);
            this.Controls.Add(this.lblDirectPower);
            this.Controls.Add(this.lblAmbientPower);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.trackDirect);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panelDirect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trackAmbient);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelAmbient);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmLightDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройка освещения сцены";
            this.Load += new System.EventHandler(this.frmLightDialog_Load);
            this.Shown += new System.EventHandler(this.frmLightDialog_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.trackAmbient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackDirect)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.Panel panelAmbient;
        public System.Windows.Forms.TrackBar trackAmbient;
        public System.Windows.Forms.TrackBar trackDirect;
        public System.Windows.Forms.Panel panelDirect;
        private System.Windows.Forms.Label lblAmbientPower;
        private System.Windows.Forms.Label lblDirectPower;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Panel panelBackground;
    }
}