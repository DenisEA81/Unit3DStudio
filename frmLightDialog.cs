using System;
using System.Windows.Forms;

namespace Unit3DStudio
{
    public partial class frmLightDialog : Form
    {
        public DialogResult dialogResult;

        public frmLightDialog()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLightDialog_Shown(object sender, EventArgs e)
        {
            dialogResult = DialogResult.Cancel;            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            dialogResult = DialogResult.OK;
            this.Close();
        }

        private void panelAmbient_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = panelAmbient.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK) panelAmbient.BackColor = colorDialog1.Color;
        }

        private void panelDirect_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = panelDirect.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK) panelDirect.BackColor = colorDialog1.Color;
        }

        public void trackAmbient_ValueChanged(object sender, EventArgs e)
        {
            lblAmbientPower.Text = trackAmbient.Value.ToString() + "%";
        }

        public void trackDirect_ValueChanged(object sender, EventArgs e)
        {
            lblDirectPower.Text = trackDirect.Value.ToString() + "%";
        }

        private void frmLightDialog_Load(object sender, EventArgs e)
        {

        }

        private void trackAmbient_Scroll(object sender, EventArgs e)
        {

        }

        private void panelBackground_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = panelBackground.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK) panelBackground.BackColor = colorDialog1.Color;
        }

        private void panelBackground_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
