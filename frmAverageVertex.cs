using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Unit3DStudio
{
    public partial class frmMergeNearVertex : Form
    {
        private int result = -1;

        public frmMergeNearVertex()
        {
            InitializeComponent();
        }

        private void frmAverageVertex_Load(object sender, EventArgs e)
        {

        }

        public int Run()
        {
            result = -1;
            ShowDialog();
            return result;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            result = (int)numericUpDown1.Value;
            Close();
        }
    }
}
