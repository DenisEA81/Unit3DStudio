namespace Unit3DStudio
{
    partial class frmFrameSetting
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
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.chkFrame = new System.Windows.Forms.CheckBox();
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(303, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "и высотой";
            // 
            // nudHeight
            // 
            this.nudHeight.Enabled = false;
            this.nudHeight.Location = new System.Drawing.Point(368, 41);
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
            this.nudHeight.TabIndex = 2;
            this.nudHeight.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            // 
            // chkFrame
            // 
            this.chkFrame.AutoSize = true;
            this.chkFrame.Location = new System.Drawing.Point(30, 41);
            this.chkFrame.Name = "chkFrame";
            this.chkFrame.Size = new System.Drawing.Size(165, 17);
            this.chkFrame.TabIndex = 0;
            this.chkFrame.Text = "Показать рамку с шириной";
            this.chkFrame.UseVisualStyleBackColor = true;
            this.chkFrame.CheckedChanged += new System.EventHandler(this.chkFrame_CheckedChanged);
            // 
            // nudWidth
            // 
            this.nudWidth.Enabled = false;
            this.nudWidth.Location = new System.Drawing.Point(212, 40);
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
            this.nudWidth.TabIndex = 1;
            this.nudWidth.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(156, 98);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Сохранить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(263, 98);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmFrameSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 146);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudHeight);
            this.Controls.Add(this.chkFrame);
            this.Controls.Add(this.nudWidth);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmFrameSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Задайте окно";
            this.Load += new System.EventHandler(this.frmFrameSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.CheckBox chkFrame;
        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}