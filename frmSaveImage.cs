using System;
using System.Drawing;
using System.Windows.Forms;

namespace Unit3DStudio
{
    public partial class frmSaveImage : Form
    {
        public frmSaveImage()
        {
            InitializeComponent();            
        }

        private void chkFrame_CheckedChanged(object sender, EventArgs e)
        {
            nudHeight.Enabled = nudWidth.Enabled = chkFrame.Checked;
        }

        private void chkSeriaY_CheckedChanged(object sender, EventArgs e)
        {
            nudCameraZ.Enabled = nudFileMask.Enabled = nudFileNum.Enabled = nudCameraAngle.Enabled = nudRotate.Enabled = chkSeriaY.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {                
                frmMain.ScreenshotAlpha = chkAlpha.Checked;

                frmMain.ScreenshotFrame = chkFrame.Checked;
                if (frmMain.ScreenshotFrame) frmMain.ScreenshotSize = new Point((int)nudWidth.Value, (int)nudHeight.Value);

                frmMain.ScreenshotFileName = txtFileName.Text.Trim();                

                frmMain.ScreenshotSeries = chkSeriaY.Checked;
                if (frmMain.ScreenshotSeries)
                {
                    frmMain.ScreenshotSeriesStep = (float)nudRotate.Value * Graph3DLibrary.Engine3D.RadianDegrees;
                    frmMain.ScreenshotSeriesPos = 0;
                    frmMain.ScreenshotSeriesAngle = (float)nudCameraAngle.Value* Graph3DLibrary.Engine3D.RadianDegrees;
                    frmMain.ScreenshotSeriesFileNum = (int)nudFileNum.Value;
                    frmMain.ScreenshotSeriesFileMask = (int)nudFileMask.Value;
                    frmMain.ScreenshotSeriesCameraZ = (int)nudCameraZ.Value;
                }
                frmMain.ScreenshotMode = true;
                this.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void frmSaveImage_Load(object sender, EventArgs e)
        {
            try
            {
                txtFileName.Text = DateTime.Now.ToString().Replace('.', '_').Replace(':', '_').Replace(' ', '_');
                if (frmMain.WindowInFrame)
                {
                    nudWidth.Value = frmMain.WindowFrame.X;
                    nudHeight.Value = frmMain.WindowFrame.Y;
                }
                else
                {
                    nudWidth.Value = frmMain.ScreenshotSize.X;
                    nudHeight.Value = frmMain.ScreenshotSize.Y;
                }
                nudCameraZ.Value = (int)frmMain.ScreenshotSeriesCameraZ;
                nudFileMask.Value = frmMain.ScreenshotSeriesFileMask;
                nudCameraAngle.Value = (decimal)(frmMain.ScreenshotSeriesAngle / Graph3DLibrary.Engine3D.RadianDegrees);
                nudRotate.Value = (decimal)(frmMain.ScreenshotSeriesStep / Graph3DLibrary.Engine3D.RadianDegrees);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
    }
}
