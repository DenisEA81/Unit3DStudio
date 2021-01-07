using System;
using System.Windows.Forms;

namespace Unit3DStudio
{
    public partial class frmFrameSetting : Form
    {
        public frmFrameSetting()
        {
            InitializeComponent();
            nudWidth.Value = frmMain.WindowFrame.X;
            nudHeight.Value = frmMain.WindowFrame.Y;
            chkFrame.Checked = frmMain.WindowInFrame;
        }

        private void chkFrame_CheckedChanged(object sender, EventArgs e)
        {
            nudHeight.Enabled = nudWidth.Enabled = chkFrame.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmMain.WindowFrame.X = (int)nudWidth.Value;
            frmMain.WindowFrame.Y = (int)nudHeight.Value;
            frmMain.WindowInFrame = chkFrame.Checked;
            this.Close();
        }

        private void frmFrameSetting_Load(object sender, EventArgs e)
        {

        }
    }
}
