using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WeAtk.Common;
using WeAtk.User;

namespace WeAtk
{
    public partial class fightlogPanel : Form
    {
        public fightlogPanel()
        {
            InitializeComponent();

            Contant.changeFunc2 = this.UpdateLog;

            DataTable data = Reader.Instance().ReadPlayerData("select * from msg order by msgtime desc");
            if (data == null) return;
            this.listView1.BeginUpdate();
            this.listView1.Items.Clear();
            //DataTable data = Reader.Instance().ReadPlayerData("select * from players");
            for (int i = 0; i < data.Rows.Count; ++i)
            {
                DataRow dr = data.Rows[i];
                string time = (string)dr["msgtime"];
                string msg = (string)dr["message"];
                string playername = (string)dr["playername"];
                string nickname = (string)dr["nickname"];
                string playertype = (string)dr["playertype"];

                ListViewItem lvi = new ListViewItem(time);
                lvi.SubItems.Add(playername);
                lvi.SubItems.Add(nickname);
                lvi.SubItems.Add(playertype);
                lvi.SubItems.Add(msg);

                this.listView1.Items.Add(lvi);
            }
            this.listView1.EndUpdate();
        }

        public void UpdateLog()
        {
            this.BeginInvoke((Action)delegate ()
            {
                DataTable data = Reader.Instance().ReadPlayerData("select * from msg order by msgtime desc");
                if (data == null) return;
                this.listView1.BeginUpdate();
                this.listView1.Items.Clear();
                //DataTable data = Reader.Instance().ReadPlayerData("select * from players");
                for (int i = 0; i < data.Rows.Count; ++i)
                {
                    DataRow dr = data.Rows[i];
                    string time = (string)dr["msgtime"];
                    string msg = (string)dr["message"];
                    string playername = (string)dr["playername"];
                    string nickname = (string)dr["nickname"];
                    string playertype = (string)dr["playertype"];

                    ListViewItem lvi = new ListViewItem(time);
                    lvi.SubItems.Add(playername);
                    lvi.SubItems.Add(nickname);
                    lvi.SubItems.Add(playertype);
                    lvi.SubItems.Add(msg);

                    this.listView1.Items.Add(lvi);
                }
                this.listView1.EndUpdate();
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sql = "select * from statement";

            DateTime dta;
            dta = dateTimePicker1.Value;
            DateTime dtb;
            dtb = dateTimePicker2.Value;
            DataTable data = Reader.Instance().ReadPlayerData(sql + " order by 时间 desc");
            if (data == null) return;

            Dictionary<string, string> lstA = new Dictionary<string, string>();
            this.listView2.BeginUpdate();
            this.listView2.Items.Clear();
            //DataTable data = Reader.Instance().ReadPlayerData("select * from players");
            for (int i = 0; i < data.Rows.Count; ++i)
            {
                //时间,玩家,群昵称,玩家类型,类型,游戏,剩余分数,分数变化,期号,使用分数,玩法内容,分析内容,开奖
                DataRow dr = data.Rows[i];
                string time = (string)dr["时间"];
                string playername = (string)dr["玩家"];
                string nickname = (string)dr["群昵称"];
                string playertype = (string)dr["玩家类型"];
                string stype = (string)dr["类型"];
                string game = (string)dr["游戏"];
                game = game == "0" ? "赛车" : "游艇";
                string left = (string)dr["剩余分数"];
                string change = (string)dr["分数变化"];
                string num = (string)dr["期号"];
                string usescore = (string)dr["使用分数"];
                string content = (string)dr["玩法内容"];
                string sdata = (string)dr["开奖"];

                float fc = 0;
                float.TryParse(change, out fc);

                DateTime dt1;
                DateTimeFormatInfo dtFormat2 = new DateTimeFormatInfo();
                dtFormat2.ShortDatePattern = "yyyy/MM/dd HH/mm/ss";
                dt1 = Convert.ToDateTime(time, dtFormat2);

                if (dt1 < dta || dt1 > dtb) continue;
                if (!checkBox1.Checked && stype == "上分" && fc > 0) continue;
                if (!checkBox2.Checked && stype == "上分" && fc < 0) continue;
                if (!checkBox3.Checked && stype == "返还") continue;
                if (!checkBox4.Checked && stype == "进攻") continue;
                if (!checkBox5.Checked && stype == "GM修改进攻") continue;
                if (!checkBox7.Checked && stype == "清空内容") continue;
                if (!checkBox8.Checked && stype == "结算") continue;
                if (checkBox14.Checked && playertype == "虚拟玩家") continue;

                lstA[playername] = stype;

                /*ListViewItem lvi = new ListViewItem(time);
                lvi.SubItems.Add(playername);
                lvi.SubItems.Add(nickname);
                lvi.SubItems.Add(playertype);
                lvi.SubItems.Add(stype);
                lvi.SubItems.Add(game);
                lvi.SubItems.Add(left);
                lvi.SubItems.Add(change);
                lvi.SubItems.Add(num);
                lvi.SubItems.Add(usescore);
                lvi.SubItems.Add(content);
                lvi.SubItems.Add(sdata);

                this.listView2.Items.Add(lvi);*/
            }
            this.listView2.EndUpdate();
            this.listView3.BeginUpdate();
            this.listView3.Items.Clear();
            foreach (KeyValuePair<string, string> kvp in lstA)
            {
                
                //DataTable data = Reader.Instance().ReadPlayerData("select * from players");
                ListViewItem lvi = new ListViewItem(kvp.Key);
                    lvi.SubItems.Add(kvp.Value);
                    this.listView3.Items.Add(lvi);
                
            }
            this.listView3.EndUpdate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           /* string sql = "delete from statement where 时间 >={0} and 时间 <={1}";
            sql = string.Format(sql, dateTimePicker1.Text, dateTimePicker2.Text);
            Reader.Instance().QueryPlayer(sql);
            MessageBox.Show("操作成功");*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
             * 损失粮草总值
获得粮草总值
粮草流水总值
回合损失粮草
回合获得粮草
回合流水粮草*/
            string sql = "select * from statement";

            DateTime dta;
            dta = dateTimePicker1.Value;
            DateTime dtb;
            dtb = dateTimePicker2.Value;
            DataTable data = Reader.Instance().ReadPlayerData(sql + " order by 时间 desc");
            if (data == null) return;

            this.listView2.BeginUpdate();
            this.listView2.Items.Clear();
            //DataTable data = Reader.Instance().ReadPlayerData("select * from players");
            for (int i = 0; i < data.Rows.Count; ++i)
            {
                //时间,玩家,群昵称,玩家类型,类型,游戏,剩余分数,分数变化,期号,使用分数,玩法内容,分析内容,开奖
                DataRow dr = data.Rows[i];
                string time = (string)dr["时间"];
                string playername = (string)dr["玩家"];
                string nickname = (string)dr["群昵称"];
                string playertype = (string)dr["玩家类型"];
                string stype = (string)dr["类型"];
                string game = (string)dr["游戏"];
                game = game == "0" ? "赛车" : "游艇";
                string left = (string)dr["剩余分数"];
                string change = (string)dr["分数变化"];
                string num = (string)dr["期号"];
                string usescore = (string)dr["使用分数"];
                string content = (string)dr["玩法内容"];
                string sdata = (string)dr["开奖"];

                float fc = 0;
                float.TryParse(change, out fc);

                DateTime dt1;
                DateTimeFormatInfo dtFormat2 = new DateTimeFormatInfo();
                dtFormat2.ShortDatePattern = "yyyy/MM/dd HH/mm/ss";
                dt1 = Convert.ToDateTime(time, dtFormat2);

                if (dt1 < dta || dt1 > dtb) continue;
                if (!checkBox1.Checked && stype == "上分" && fc > 0) continue;
                if (!checkBox2.Checked && stype == "上分" && fc < 0) continue;
                if (!checkBox3.Checked && stype == "返还") continue;
                if (!checkBox4.Checked && stype == "进攻") continue;
                if (!checkBox5.Checked && stype == "GM修改进攻") continue;
                if (!checkBox7.Checked && stype == "清空内容") continue;
                if (!checkBox8.Checked && stype == "结算") continue;
                if (checkBox14.Checked && playertype == "虚拟玩家") continue;


                ListViewItem lvi = new ListViewItem(time);
                lvi.SubItems.Add(playername);
                lvi.SubItems.Add(nickname);
                lvi.SubItems.Add(playertype);
                lvi.SubItems.Add(stype);
                lvi.SubItems.Add(game);
                lvi.SubItems.Add(left);
                lvi.SubItems.Add(change);
                lvi.SubItems.Add(num);
                lvi.SubItems.Add(usescore);
                lvi.SubItems.Add(content);
                lvi.SubItems.Add(sdata);

                this.listView2.Items.Add(lvi);
            }
            this.listView2.EndUpdate();


            List<tmpPlayer> lst = new List<tmpPlayer>();
            for (int i = 0; i < listView2.Items.Count; ++i)
            {
                //时间,玩家,群昵称,玩家类型,类型,游戏,剩余分数,分数变化,期号,使用分数,玩法内容,分析内容,开奖
                ListViewItem a = listView2.Items[i];
                string palyername = a.SubItems[1].Text;
                tmpPlayer o = null;
                foreach (tmpPlayer xx in lst)
                {
                    if (xx.playername == palyername)
                    {
                        o = xx;
                        break;
                    }
                }
                if (o == null)
                {
                    o = new tmpPlayer();
                    o.playername = palyername;
                    lst.Add(o);
                }
                string c = a.SubItems[7].Text;
                float fc = 0;
                float.TryParse(c, out fc);
                if (comboBox1.Text == "损失粮草总值")
                {
                    if (fc < 0)
                        o.value += fc * -1;
                }
                else if (comboBox1.Text == "获得粮草总值")
                {
                    if (fc > 0)
                        o.value += fc;
                }
                else if (comboBox1.Text == "粮草流水总值")
                {
                    if (fc > 0)
                        o.value += fc;
                    else o.value += fc * -1;
                }
            }

            string ssback = "";
            foreach (tmpPlayer pp in lst)
            {
                Player pl = null;
                foreach (Player ttp in GameMgr.Instance().Players)
                {
                    if (ttp.playername == pp.playername)
                    {
                        pl = ttp;
                        break;
                    }
                }
                if (pl == null) continue;
                string m = "0", mx = "0", v = "0";
                if (checkBox10.Checked)
                {
                    m = textBox1.Text;
                    mx = textBox2.Text;
                    v = textBox3.Text;

                    int nm = int.Parse(m);
                    int nmx = int.Parse(mx);
                    int nv = int.Parse(v);
                    if (pp.value >= nm || pp.value <= nmx)
                    {
                        int back = (int)pp.value * nv / 100;
                        pp.back += back;
                    }
                }
                if (checkBox11.Checked)
                {
                    m = textBox9.Text;
                    mx = textBox8.Text;
                    v = textBox7.Text;


                    int nm = int.Parse(m);
                    int nmx = int.Parse(mx);
                    int nv = int.Parse(v);
                    if (pp.value >= nm || pp.value <= nmx)
                    {
                        int back = (int)pp.value * nv / 100;
                        pp.back += back;
                    }
                }
                if (checkBox12.Checked)
                {
                    m = textBox6.Text;
                    mx = textBox5.Text;
                    v = textBox4.Text;

                    int nm = int.Parse(m);
                    int nmx = int.Parse(mx);
                    int nv = int.Parse(v);
                    if (pp.value >= nm || pp.value <= nmx)
                    {
                        int back = (int)pp.value * nv / 100;
                        pp.back += back;
                    }
                }
                if (checkBox13.Checked)
                {
                    m = textBox12.Text;
                    mx = textBox11.Text;
                    v = textBox10.Text;

                    int nm = int.Parse(m);
                    int nmx = int.Parse(mx);
                    int nv = int.Parse(v);
                    if (pp.value >= nm || pp.value <= nmx)
                    {
                        int back = (int)pp.value * nv / 100;
                        pp.back += back;
                    }
                }


            }


            foreach (tmpPlayer pp in lst)
            {
                Player pl = null;
                foreach (Player ttp in GameMgr.Instance().Players)
                {
                    if (ttp.playername == pp.playername)
                    {
                        pl = ttp;
                        break;
                    }
                }
                if (pl == null) continue;

                ssback += "@" + pl.nickname + " 返还:" + pp.back.ToString() + "\r\n";
                pl.left += pp.back;
                Contant.Log(pl, "返还", pp.back, "", "", "");
            }

            GameMgr.Instance().BroBack(ssback, checkBox9.Checked);
        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count == 0) return;

            string sql = "select * from statement";

            DateTime dta;
            dta = dateTimePicker1.Value;
            DateTime dtb;
            dtb = dateTimePicker2.Value;
            DataTable data = Reader.Instance().ReadPlayerData(sql + " order by 时间 desc");
            if (data == null) return;

            float fcc = 0;
            float fa = 0;

            this.listView2.BeginUpdate();
            this.listView2.Items.Clear();
            //DataTable data = Reader.Instance().ReadPlayerData("select * from players");
            for (int i = 0; i < data.Rows.Count; ++i)
            {
                //时间,玩家,群昵称,玩家类型,类型,游戏,剩余分数,分数变化,期号,使用分数,玩法内容,分析内容,开奖
                DataRow dr = data.Rows[i];
                string time = (string)dr["时间"];
                string playername = (string)dr["玩家"];
                string nickname = (string)dr["群昵称"];
                string playertype = (string)dr["玩家类型"];
                string stype = (string)dr["类型"];
                string game = (string)dr["游戏"];
                game = game == "0" ? "赛车" : "游艇";
                string left = (string)dr["剩余分数"];
                string change = (string)dr["分数变化"];
                string num = (string)dr["期号"];
                string usescore = (string)dr["使用分数"];
                string content = (string)dr["玩法内容"];
                string sdata = (string)dr["开奖"];

                float fc = 0;
                float.TryParse(change, out fc);

                DateTime dt1;
                DateTimeFormatInfo dtFormat2 = new DateTimeFormatInfo();
                dtFormat2.ShortDatePattern = "yyyy/MM/dd HH/mm/ss";
                dt1 = Convert.ToDateTime(time, dtFormat2);

                if (dt1 < dta || dt1 > dtb) continue;
                if (!checkBox1.Checked && stype == "上分" && fc > 0) continue;
                if (!checkBox2.Checked && stype == "上分" && fc < 0) continue;
                if (!checkBox3.Checked && stype == "返还") continue;
                if (!checkBox4.Checked && stype == "进攻") continue;
                if (!checkBox5.Checked && stype == "GM修改进攻") continue;
                if (!checkBox7.Checked && stype == "清空内容") continue;
                if (!checkBox8.Checked && stype == "结算") continue;
                if (checkBox14.Checked && playertype == "虚拟玩家") continue;
                if (playername != listView3.SelectedItems[0].SubItems[0].Text) continue;

                fcc += fc;
                fa += Math.Abs(fc);

                ListViewItem lvi = new ListViewItem(time);
                lvi.SubItems.Add(playername);
                lvi.SubItems.Add(nickname);
                lvi.SubItems.Add(playertype);
                lvi.SubItems.Add(stype);
                lvi.SubItems.Add(game);
                lvi.SubItems.Add(left);
                lvi.SubItems.Add(change);
                lvi.SubItems.Add(num);
                lvi.SubItems.Add(usescore);
                lvi.SubItems.Add(content);
                lvi.SubItems.Add(sdata);

                this.listView2.Items.Add(lvi);
            }
            this.listView2.EndUpdate();

            label21.Text = fcc.ToString();
            label22.Text = fa.ToString();
        }

        private void fightlogPanel_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count == 0) return;

            string sql = "select * from statement";

            DateTime dta;
            dta = dateTimePicker1.Value;
            DateTime dtb;
            dtb = dateTimePicker2.Value;
            DataTable data = Reader.Instance().ReadPlayerData(sql + " order by 时间 desc");
            if (data == null) return;

            float fcc = 0;
            float fa = 0;

            this.listView2.BeginUpdate();
            this.listView2.Items.Clear();
            //DataTable data = Reader.Instance().ReadPlayerData("select * from players");
            for (int i = 0; i < data.Rows.Count; ++i)
            {
                //时间,玩家,群昵称,玩家类型,类型,游戏,剩余分数,分数变化,期号,使用分数,玩法内容,分析内容,开奖
                DataRow dr = data.Rows[i];
                string time = (string)dr["时间"];
                string playername = (string)dr["玩家"];
                string nickname = (string)dr["群昵称"];
                string playertype = (string)dr["玩家类型"];
                string stype = (string)dr["类型"];
                string game = (string)dr["游戏"];
                game = game == "0" ? "赛车" : "游艇";
                string left = (string)dr["剩余分数"];
                string change = (string)dr["分数变化"];
                string num = (string)dr["期号"];
                string usescore = (string)dr["使用分数"];
                string content = (string)dr["玩法内容"];
                string sdata = (string)dr["开奖"];

                float fc = 0;
                float.TryParse(change, out fc);

                DateTime dt1;
                DateTimeFormatInfo dtFormat2 = new DateTimeFormatInfo();
                dtFormat2.ShortDatePattern = "yyyy/MM/dd HH/mm/ss";
                dt1 = Convert.ToDateTime(time, dtFormat2);

                if (dt1 < dta || dt1 > dtb) continue;
                if (!checkBox1.Checked && stype == "上分" && fc > 0) continue;
                if (!checkBox2.Checked && stype == "上分" && fc < 0) continue;
                if (!checkBox3.Checked && stype == "返还") continue;
                if (!checkBox4.Checked && stype == "进攻") continue;
                if (!checkBox5.Checked && stype == "GM修改进攻") continue;
                if (!checkBox7.Checked && stype == "清空内容") continue;
                if (!checkBox8.Checked && stype == "结算") continue;
                if (checkBox14.Checked && playertype == "虚拟玩家") continue;
                if (playername != listView3.SelectedItems[0].SubItems[0].Text) continue;

                fcc += fc;
                fa += Math.Abs(fc);

                ListViewItem lvi = new ListViewItem(time);
                lvi.SubItems.Add(playername);
                lvi.SubItems.Add(nickname);
                lvi.SubItems.Add(playertype);
                lvi.SubItems.Add(stype);
                lvi.SubItems.Add(game);
                lvi.SubItems.Add(left);
                lvi.SubItems.Add(change);
                lvi.SubItems.Add(num);
                lvi.SubItems.Add(usescore);
                lvi.SubItems.Add(content);
                lvi.SubItems.Add(sdata);

                this.listView2.Items.Add(lvi);
            }

            label21.Text = fcc.ToString();
            label22.Text = fa.ToString();

            /*
 * 损失粮草总值
获得粮草总值
粮草流水总值
回合损失粮草
回合获得粮草
回合流水粮草*/
            List<tmpPlayer> lst = new List<tmpPlayer>();
            for (int i = 0; i < listView2.Items.Count; ++i)
            {
                //时间,玩家,群昵称,玩家类型,类型,游戏,剩余分数,分数变化,期号,使用分数,玩法内容,分析内容,开奖
                ListViewItem a = listView2.Items[i];
                string palyername = a.SubItems[1].Text;
                tmpPlayer o = null;
                foreach (tmpPlayer xx in lst)
                {
                    if (xx.playername == palyername)
                    {
                        o = xx;
                        break;
                    }
                }
                if (o == null)
                {
                    o = new tmpPlayer();
                    o.playername = palyername;
                    lst.Add(o);
                }
                string c = a.SubItems[7].Text;
                float fc = 0;
                float.TryParse(c, out fc);
                if (comboBox1.Text == "损失粮草总值")
                {
                    if (fc < 0)
                        o.value += fc * -1;
                }
                else if (comboBox1.Text == "获得粮草总值")
                {
                    if (fc > 0)
                        o.value += fc;
                }
                else if (comboBox1.Text == "粮草流水总值")
                {
                    if (fc > 0)
                        o.value += fc;
                    else o.value += fc * -1;
                }
            }

            string ssback = "";
            foreach (tmpPlayer pp in lst)
            {
                Player pl = null;
                foreach (Player ttp in GameMgr.Instance().Players)
                {
                    if (ttp.playername == pp.playername)
                    {
                        pl = ttp;
                        break;
                    }
                }
                if (pl == null) continue;
                string m = "0", mx = "0", v = "0";
                if (checkBox10.Checked)
                {
                    m = textBox1.Text;
                    mx = textBox2.Text;
                    v = textBox3.Text;

                    int nm = int.Parse(m);
                    int nmx = int.Parse(mx);
                    int nv = int.Parse(v);
                    if (pp.value >= nm || pp.value <= nmx)
                    {
                        int back = (int)pp.value * nv / 100;
                        pp.back += back;
                    }
                }
                if (checkBox11.Checked)
                {
                    m = textBox9.Text;
                    mx = textBox8.Text;
                    v = textBox7.Text;


                    int nm = int.Parse(m);
                    int nmx = int.Parse(mx);
                    int nv = int.Parse(v);
                    if (pp.value >= nm || pp.value <= nmx)
                    {
                        int back = (int)pp.value * nv / 100;
                        pp.back += back;
                    }
                }
                if (checkBox12.Checked)
                {
                    m = textBox6.Text;
                    mx = textBox5.Text;
                    v = textBox4.Text;

                    int nm = int.Parse(m);
                    int nmx = int.Parse(mx);
                    int nv = int.Parse(v);
                    if (pp.value >= nm || pp.value <= nmx)
                    {
                        int back = (int)pp.value * nv / 100;
                        pp.back += back;
                    }
                }
                if (checkBox13.Checked)
                {
                    m = textBox12.Text;
                    mx = textBox11.Text;
                    v = textBox10.Text;

                    int nm = int.Parse(m);
                    int nmx = int.Parse(mx);
                    int nv = int.Parse(v);
                    if (pp.value >= nm || pp.value <= nmx)
                    {
                        int back = (int)pp.value * nv / 100;
                        pp.back += back;
                    }
                }


            }


            foreach (tmpPlayer pp in lst)
            {
                Player pl = null;
                foreach (Player ttp in GameMgr.Instance().Players)
                {
                    if (ttp.playername == pp.playername)
                    {
                        pl = ttp;
                        break;
                    }
                }
                if (pl == null) continue;

                ssback += "@" + pl.nickname + " 返还:" + pp.back.ToString() + "\r\n";
                pl.left += pp.back;
                Contant.Log(pl, "返还", pp.back, "", "", "");
            }

            GameMgr.Instance().BroBack(ssback, checkBox9.Checked);
        }
    }
}
