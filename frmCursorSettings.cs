using System;
using System.Windows.Forms;

namespace Unit3DStudio
{
    public partial class frmCursorSettings : Form
    {
        public frmCursorSettings()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            frmMain.MouseXYDiv = (float)this.trackMouseXY.Value;
            frmMain.MouseWheelDiv = (float)this.trackMouseWheel.Value;
            frmMain.MouseAxisDiv = (float)this.trackMouseAxis.Value;
            frmMain.MouseWheelInverse = chkMouseWheelInverse.Checked;
            frmMain.SaveUserSettings();
            this.Close();
        }

        private void frmCursorSettings_Load(object sender, EventArgs e)
        {
            this.trackMouseXY.Value = (int)Math.Round(frmMain.MouseXYDiv);
            this.trackMouseWheel.Value = (int)Math.Round(frmMain.MouseWheelDiv);
            this.trackMouseAxis.Value = (int)Math.Round(frmMain.MouseAxisDiv);
            chkMouseWheelInverse.Checked = frmMain.MouseWheelInverse;
        }

        private void trackMouseXY_ValueChanged(object sender, EventArgs e)
        {
            this.lblMouseXY.Text = trackMouseXY.Value.ToString()+"%";
        }

        private void trackMouseWheel_ValueChanged(object sender, EventArgs e)
        {
            this.labelMouseWheel.Text = trackMouseWheel.Value.ToString() + "%";
        }

        private void trackMouseAxis_ValueChanged(object sender, EventArgs e)
        {
            this.lblMouseAxis.Text = trackMouseAxis.Value.ToString() + "%";
        }

        private void trackMouseWheel_Scroll(object sender, EventArgs e)
        {

        }
    }
}
