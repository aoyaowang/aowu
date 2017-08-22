using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WeAtk.Common;

namespace WeAtk
{
    public partial class rootPanel : Form
    {
        public rootPanel()
        {
            InitializeComponent();
        }

        private void rootPanel_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Controls.Count; ++i) {
                if (this.Controls[i].GetType().ToString().Equals("System.Windows.Forms.CheckBox")) {
                    this.Controls[i].Enabled = false;
                }
            }
            this.BeginInvoke((Action)delegate ()
            {
                checkBox1.Checked = SetMgr.Instance().addplayer;
            });
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SetMgr.Instance().addplayer = checkBox1.Checked;
        }
    }
}
