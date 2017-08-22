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
    public partial class logPanel : Form
    {
        public logPanel()
        {
            InitializeComponent();

            Contant.changeFunc = this.UpdateLog;

            this.UpdateLog();
        }

        public void UpdateLog()
        {
            DataTable data = Reader.Instance().ReadPlayerData("select * from managerlog order by time desc");
            if (data == null) return;
            this.listView1.BeginUpdate();
            this.listView1.Items.Clear();
            //DataTable data = Reader.Instance().ReadPlayerData("select * from players");
            for (int i = 0; i < data.Rows.Count; ++i) {
                DataRow dr = data.Rows[i];
                string time = (string)dr["time"];
                string msg = (string)dr["msg"];

                ListViewItem lvi = new ListViewItem(time);
                lvi.SubItems.Add(msg);

                this.listView1.Items.Add(lvi);

            }
            this.listView1.EndUpdate();
        }
    }
}
