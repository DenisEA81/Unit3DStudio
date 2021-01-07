using System;
using System.Drawing;
using System.Windows.Forms;
using Graph3DLibrary;

namespace Unit3DStudio
{
    public partial class frmColorPaint : Form
    {
        private VolumetricModel3D PaintModel = null;
        private DialogResult dlgResult = DialogResult.None;
        private PolygonSides ColorSide = PolygonSides.AllSides;

        public frmColorPaint()
        {
            InitializeComponent();
        }

        private void NudRY_ValueChanged(object sender, EventArgs e)
        {

        }

        private void NudRZ_ValueChanged(object sender, EventArgs e)
        {

        }

        public DialogResult Run(ref VolumetricModel3D Model_for_Paint, PolygonSides side)
        {
            try
            {
                PaintModel = Model_for_Paint;
                ColorSide = side;
                dlgResult = DialogResult.Cancel;
                this.ShowDialog();
                return dlgResult;
            }
            catch (Exception er)
            {
                MessageBox.Show("Run\nОшибка: " + er.Message);
                return DialogResult.Abort;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {   
                int side=ColorSide==PolygonSides.FrontSide?0:1;

                switch (tabSelector.SelectedIndex)
                {
                    case 0:
                        #region Сферический градиент
                        {
                            float dist = 0;
                            float sphR = (float)(this.NudR.Value);
                            float sphInR = (float)(this.NudRin.Value);
                            float delta = 0;
                            Point3D sphC = new Point3D((float)this.NudX.Value, (float)this.NudY.Value, (float)this.NudZ.Value);
                            if ((sphR < 1) | (sphInR < 0) | (sphInR >= sphR)) break;
                            for (int i = 0; i < PaintModel.Polygon.Length; i++)
                            {
                                dist = Engine3D.GetDistance(sphC, PaintModel.Polygon[i].Center);

                                if (dist > sphR)
                                {
                                    if (!checkOutColor.Checked) continue;
                                    delta = 1;
                                }
                                else if (dist < sphInR)
                                {
                                    if (!checkInColor.Checked) continue;
                                    delta = 0;
                                }
                                else delta = (dist - sphInR) / (sphR - sphInR);

                                if (PaintModel.SetColor(Color.FromArgb((int)Math.Round(PaintModel.Polygon[i].color[side].A * (1 - (float)this.trackTotalOpacity.Value / 100) + ((float)this.trackTotalOpacity.Value / 100) * 2.55 * (this.trackOpacityIn.Value + (this.trackOpacityOut.Value - this.trackOpacityIn.Value) * delta)),
                                                                   (int)Math.Round(PaintModel.Polygon[i].color[side].R * (1 - (float)this.trackTotalOpacity.Value / 100) + ((float)this.trackTotalOpacity.Value / 100) * (this.panelColorIn.BackColor.R + (this.panelColorOut.BackColor.R - this.panelColorIn.BackColor.R) * delta)),
                                                                   (int)Math.Round(PaintModel.Polygon[i].color[side].G * (1 - (float)this.trackTotalOpacity.Value / 100) + ((float)this.trackTotalOpacity.Value / 100) * (this.panelColorIn.BackColor.G + (this.panelColorOut.BackColor.G - this.panelColorIn.BackColor.G) * delta)),
                                                                   (int)Math.Round(PaintModel.Polygon[i].color[side].B * (1 - (float)this.trackTotalOpacity.Value / 100) + ((float)this.trackTotalOpacity.Value / 100) * (this.panelColorIn.BackColor.B + (this.panelColorOut.BackColor.B - this.panelColorIn.BackColor.B) * delta))), i, i, ColorSide) != 0) throw new Exception(ErrorLog.GetLastError());
                            }
                        }
                        #endregion
                        break;
                    case 1:
                        #region Уровневый градиент
                        {
                            float dist = 0;                            
                            float bottomY = (float)nudLevelBottom.Value;
                            float topY = (float)nudLevelTop.Value;
                            float delta = 0;
                            float dY= topY-bottomY;
                            if (dY<=0) break;
                            for (int i = 0; i < PaintModel.Polygon.Length; i++)
                            {
                                dist = PaintModel.Polygon[i].Center.Y - bottomY;

                                if (dist > dY)
                                {
                                    if (!checkLevelTop.Checked) continue;
                                    delta = 1;
                                }
                                else if (dist < 0)
                                {
                                    if (!checkLevelBottom.Checked) continue;
                                    delta = 0;
                                }
                                else delta = dist / dY;

                                if (PaintModel.SetColor(Color.FromArgb((int)Math.Round(PaintModel.Polygon[i].color[side].A * (1 - (float)this.trackLevelTotal.Value / 100) + ((float)this.trackLevelTotal.Value / 100) * 2.55 * (this.trackLevelBottom.Value + (this.trackLevelTop.Value - this.trackLevelBottom.Value) * delta)),
                                                                   (int)Math.Round(PaintModel.Polygon[i].color[side].R * (1 - (float)this.trackLevelTotal.Value / 100) + ((float)this.trackLevelTotal.Value / 100) * (this.panelLevelBottom.BackColor.R + (this.panelLevelTop.BackColor.R - this.panelLevelBottom.BackColor.R) * delta)),
                                                                   (int)Math.Round(PaintModel.Polygon[i].color[side].G * (1 - (float)this.trackLevelTotal.Value / 100) + ((float)this.trackLevelTotal.Value / 100) * (this.panelLevelBottom.BackColor.G + (this.panelLevelTop.BackColor.G - this.panelLevelBottom.BackColor.G) * delta)),
                                                                   (int)Math.Round(PaintModel.Polygon[i].color[side].B * (1 - (float)this.trackLevelTotal.Value / 100) + ((float)this.trackLevelTotal.Value / 100) * (this.panelLevelBottom.BackColor.B + (this.panelLevelTop.BackColor.B - this.panelLevelBottom.BackColor.B) * delta))), i, i, ColorSide) != 0) throw new Exception(ErrorLog.GetLastError());
                            }
                        }
                        #endregion
                        break;
                    case 2:
                        #region Смещение цвета
                        {
                            Color tmpColor = new Color();
                            for (int i = 0; i < PaintModel.Polygon.Length; i++)
                            {
                                tmpColor = Color.FromArgb(PaintModel.Polygon[i].color[side].ToArgb());
                                PaintModel.Polygon[i].color[side] = Color.FromArgb(tmpColor.A,
                                                                                Math.Min(255, (int)Math.Round((tmpColor.R * nudRR.Value + tmpColor.G * nudRG.Value + tmpColor.B * nudRB.Value) / 100)),
                                                                                Math.Min(255, (int)Math.Round((tmpColor.R * nudGR.Value + tmpColor.G * nudGG.Value + tmpColor.B * nudGB.Value) / 100)),
                                                                                Math.Min(255, (int)Math.Round((tmpColor.R * nudBR.Value + tmpColor.G * nudBG.Value + tmpColor.B * nudBB.Value) / 100)));

                            }
                        }
                        #endregion
                        break;
                }
                dlgResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show("btnOK_Click\nОшибка: "+er.Message);
                dlgResult = DialogResult.Abort;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panelColorIn_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = panelColorIn.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK) panelColorIn.BackColor = colorDialog1.Color;
        }

        private void panelColorOut_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = panelColorOut.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK) panelColorOut.BackColor = colorDialog1.Color;
        }

        private void trackOpacityIn_ValueChanged(object sender, EventArgs e)
        {
            labelOpacity1.Text = "Непрозрачность: "+trackOpacityIn.Value.ToString("0")+"%";
        }

        private void trackOpacityOut_ValueChanged(object sender, EventArgs e)
        {
            labelOpacity2.Text = "Непрозрачность: " + trackOpacityOut.Value.ToString("0") + "%";
        }

        private void trackTotalOpacity_ValueChanged(object sender, EventArgs e)
        {
            labelTotalOpacity.Text = "Непрозрачность наложения: " + trackTotalOpacity.Value.ToString("0") + "%";
        }

        private void panelLevelTop_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = ((Panel)sender).BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK) ((Panel)sender).BackColor = colorDialog1.Color;
        }

        private void trackLevelTotal_ValueChanged(object sender, EventArgs e)
        {
            labelLevelTotal.Text = "Непрозрачность наложения: " + trackLevelTotal.Value.ToString("0") + "%";
        }

        private void trackLevelTop_ValueChanged(object sender, EventArgs e)
        {
            labelLevelTop.Text = "непрозрачность: " + trackLevelTop.Value.ToString("0") + "%";
        }

        private void trackLevelBottom_ValueChanged(object sender, EventArgs e)
        {
            labelLevelBottom.Text = "непрозрачность: " + trackLevelBottom.Value.ToString("0") + "%";
        }

        private void btnSH50Color_Click(object sender, EventArgs e)
        {
            nudRR.Value = 0;
            nudRG.Value = 50;
            nudRB.Value = 50;

            nudGR.Value = 50;
            nudGG.Value = 0;
            nudGB.Value = 50;

            nudBR.Value = 50;
            nudBG.Value = 50;
            nudBB.Value = 0;
        }

        private void btnSHLColor_Click(object sender, EventArgs e)
        {
            nudRR.Value = 0;
            nudRG.Value = 0;
            nudRB.Value = 100;

            nudGR.Value = 100;
            nudGG.Value = 0;
            nudGB.Value = 0;

            nudBR.Value = 0;
            nudBG.Value = 100;
            nudBB.Value = 0;
        }

        private void btnSHRColor_Click(object sender, EventArgs e)
        {
            nudRR.Value = 0;
            nudRG.Value = 100;
            nudRB.Value = 0;

            nudGR.Value = 0;
            nudGG.Value = 0;
            nudGB.Value = 100;

            nudBR.Value = 100;
            nudBG.Value = 0;
            nudBB.Value = 0;
        }
    }
}
