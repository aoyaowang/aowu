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
    public partial class settingPanel : Form
    {
        public DataTable pk10 = null;
        public DataTable ship = null;
        public settingPanel()
        {
            InitializeComponent();
        }

        private void settingPanel_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Controls.Count; ++i)
            {
                if (!this.Controls[i].GetType().ToString().Equals("System.Windows.Forms.Button") && !this.Controls[i].GetType().ToString().Equals("System.Windows.Forms.TabControl"))
                {
                    this.Controls[i].Enabled = false;
                }
            }
            this.BeginInvoke((Action)delegate ()
            {
                textBox1.Text = SetMgr.Instance().S1_1;
                textBox2.Text = SetMgr.Instance().S1_2;
                textBox4.Text = SetMgr.Instance().S2_1;
                textBox3.Text = SetMgr.Instance().S2_2;
                textBox6.Text = SetMgr.Instance().S3_1;
                textBox5.Text = SetMgr.Instance().S3_2;
                textBox7.Text = SetMgr.Instance().key_cha;
                textBox8.Text = SetMgr.Instance().key_hui;
                textBox9.Text = SetMgr.Instance().key_edit;
                textBox10.Text = SetMgr.Instance().key_cancel;
                textBox11.Text = SetMgr.Instance().key_check;

                checkBox1.Checked = SetMgr.Instance().addplayer;
                checkBox2.Checked = SetMgr.Instance().delplayer;
                checkBox3.Checked = SetMgr.Instance().editmoney;
                checkBox4.Checked = SetMgr.Instance().editneirong;
                checkBox5.Checked = SetMgr.Instance().clear;
                checkBox6.Checked = SetMgr.Instance().changename;
                checkBox7.Checked = SetMgr.Instance().backmessage;
                checkBox8.Checked = SetMgr.Instance().backmessage2;
                checkBox9.Checked = SetMgr.Instance().receivechahui;
                checkBox10.Checked = SetMgr.Instance().receiveorder;
                checkBox11.Checked = SetMgr.Instance().wrongorder;
                checkBox11.Checked = SetMgr.Instance().low;
                checkBox13.Checked = SetMgr.Instance().limited;
                checkBox14.Checked = SetMgr.Instance().closed;
                checkBox15.Checked = SetMgr.Instance().recommend;

                checkBox16.Checked = SetMgr.Instance().check_xiaoshu_1;
                checkBox17.Checked = SetMgr.Instance().check_xiaoshu_2;
                checkBox18.Checked = SetMgr.Instance().check_shuangpai;
                checkBox19.Checked = SetMgr.Instance().protect;
                domainUpDown1.Text = SetMgr.Instance().封盘时间.ToString();
                domainUpDown2.Text = SetMgr.Instance().接近封盘时间.ToString();
                richTextBox1.Text = SetMgr.Instance().封盘消息;
                richTextBox2.Text = SetMgr.Instance().接近封盘消息;
                richTextBox3.Text = SetMgr.Instance().核对格式;
                numericUpDown1.Value = SetMgr.Instance().msg_kaijiang_num;
                numericUpDown2.Value = SetMgr.Instance().msg_kaijiang_num2;
                richTextBox4.Text = SetMgr.Instance().开奖消息;
                richTextBox5.Text = SetMgr.Instance().开盘消息;
                richTextBox6.Text = SetMgr.Instance().结算消息;
                richTextBox7.Text = SetMgr.Instance().结算格式;
                richTextBox8.Text = SetMgr.Instance().账单格式;
                textBox13.Text = "0";
                checkBox20.Checked = SetMgr.Instance().kaiguan_ziding1;
                checkBox21.Checked = SetMgr.Instance().kaiguan_ziding2;
                checkBox22.Checked = SetMgr.Instance().kaiguan_ziding3;
                checkBox23.Checked = SetMgr.Instance().kaiguan_ziding4;
                numericUpDown3.Value = SetMgr.Instance().time_ziding1;
                numericUpDown4.Value = SetMgr.Instance().time_ziding2;
                numericUpDown5.Value = SetMgr.Instance().time_ziding3;
                numericUpDown6.Value = SetMgr.Instance().time_ziding4;
                richTextBox9.Text = SetMgr.Instance().自定义1;
                richTextBox10.Text = SetMgr.Instance().自定义2;
                richTextBox11.Text = SetMgr.Instance().自定义3;
                richTextBox12.Text = SetMgr.Instance().自定义4;
                richTextBox13.Text = SetMgr.Instance().游戏成功回复;
                richTextBox14.Text = SetMgr.Instance().查分回复;

                textBox15.Text = SetMgr.Instance().每期总限额.ToString();

                pk10 = Reader.Instance().ReadGameData("select * from peilv_pk10 order by 序号");
                ship = Reader.Instance().ReadGameData("select * from peilv_ship order by 序号");
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Controls.Count; ++i)
            {
                if (!this.Controls[i].GetType().ToString().Equals("System.Windows.Forms.Button") && !this.Controls[i].GetType().ToString().Equals("System.Windows.Forms.TabControl"))
                {
                    this.Controls[i].Enabled = false;
                }
            }

            textBox1.Text = SetMgr.Instance().S1_1;
            textBox2.Text = SetMgr.Instance().S1_2;
            textBox4.Text = SetMgr.Instance().S2_1;
            textBox3.Text = SetMgr.Instance().S2_2;
            textBox6.Text = SetMgr.Instance().S3_1;
            textBox5.Text = SetMgr.Instance().S3_2;
            textBox7.Text = SetMgr.Instance().key_cha;
            textBox8.Text = SetMgr.Instance().key_hui;
            textBox9.Text = SetMgr.Instance().key_edit;
            textBox10.Text = SetMgr.Instance().key_cancel;
            textBox11.Text = SetMgr.Instance().key_check;

            checkBox1.Checked = SetMgr.Instance().addplayer;
            checkBox2.Checked = SetMgr.Instance().delplayer;
            checkBox3.Checked = SetMgr.Instance().editmoney;
            checkBox4.Checked = SetMgr.Instance().editneirong;
            checkBox5.Checked = SetMgr.Instance().clear;
            checkBox6.Checked = SetMgr.Instance().changename;
            checkBox7.Checked = SetMgr.Instance().backmessage;
            checkBox8.Checked = SetMgr.Instance().backmessage2;
            checkBox9.Checked = SetMgr.Instance().receivechahui;
            checkBox10.Checked = SetMgr.Instance().receiveorder;
            checkBox11.Checked = SetMgr.Instance().wrongorder;
            checkBox11.Checked = SetMgr.Instance().low;
            checkBox13.Checked = SetMgr.Instance().limited;
            checkBox14.Checked = SetMgr.Instance().closed;
            checkBox15.Checked = SetMgr.Instance().recommend;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Controls.Count; ++i)
            {
                if (!this.Controls[i].GetType().ToString().Equals("System.Windows.Forms.Button") && !this.Controls[i].GetType().ToString().Equals("System.Windows.Forms.TabControl"))
                {
                    this.Controls[i].Enabled = true;
                }
            }

            SetMgr.Instance().S1_1 = textBox1.Text;
            SetMgr.Instance().S1_2 = textBox2.Text;
            SetMgr.Instance().S2_1 = textBox4.Text;
            SetMgr.Instance().S2_2 = textBox3.Text;
            SetMgr.Instance().S3_1 = textBox6.Text;
            SetMgr.Instance().S3_2 = textBox5.Text;
            SetMgr.Instance().key_cha = textBox7.Text;
            SetMgr.Instance().key_hui = textBox8.Text;
            SetMgr.Instance().key_edit = textBox9.Text;
            SetMgr.Instance().key_cancel = textBox10.Text;
            SetMgr.Instance().key_check = textBox11.Text;

            SetMgr.Instance().addplayer = checkBox1.Checked;
            SetMgr.Instance().delplayer = checkBox2.Checked;
            SetMgr.Instance().editmoney = checkBox3.Checked;
            SetMgr.Instance().editneirong = checkBox4.Checked;
            SetMgr.Instance().clear = checkBox5.Checked;
            SetMgr.Instance().changename = checkBox6.Checked;
            SetMgr.Instance().backmessage = checkBox7.Checked;
            SetMgr.Instance().backmessage2 = checkBox8.Checked;
            SetMgr.Instance().receivechahui = checkBox9.Checked;
            SetMgr.Instance().receiveorder = checkBox10.Checked;
            SetMgr.Instance().wrongorder = checkBox11.Checked;
            SetMgr.Instance().low = checkBox11.Checked;
            SetMgr.Instance().limited = checkBox13.Checked;
            SetMgr.Instance().closed = checkBox14.Checked;
            SetMgr.Instance().recommend = checkBox15.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Controls.Count; ++i)
            {
                if (!this.Controls[i].GetType().ToString().Equals("System.Windows.Forms.Button") && !this.Controls[i].GetType().ToString().Equals("System.Windows.Forms.TabControl"))
                {
                    this.Controls[i].Enabled = true;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tabControl1.Enabled = false;

            checkBox16.Checked = SetMgr.Instance().check_xiaoshu_1;
            checkBox17.Checked = SetMgr.Instance().check_xiaoshu_2;
            checkBox18.Checked = SetMgr.Instance().check_shuangpai;
            checkBox19.Checked = SetMgr.Instance().protect;
            domainUpDown1.Text = SetMgr.Instance().封盘时间.ToString();
            domainUpDown2.Text = SetMgr.Instance().接近封盘时间.ToString();
            richTextBox1.Text = SetMgr.Instance().封盘消息;
            richTextBox2.Text = SetMgr.Instance().接近封盘消息;
            richTextBox3.Text = SetMgr.Instance().核对格式;
            numericUpDown1.Value = SetMgr.Instance().msg_kaijiang_num;
            numericUpDown2.Value = SetMgr.Instance().msg_kaijiang_num2;
            richTextBox4.Text = SetMgr.Instance().开奖消息;
            richTextBox5.Text = SetMgr.Instance().开盘消息;
            richTextBox6.Text = SetMgr.Instance().结算消息;
            richTextBox7.Text = SetMgr.Instance().结算格式;
            richTextBox8.Text = SetMgr.Instance().账单格式;
            textBox13.Text = "0";
            checkBox20.Checked = SetMgr.Instance().kaiguan_ziding1;
            checkBox21.Checked = SetMgr.Instance().kaiguan_ziding2;
            checkBox22.Checked = SetMgr.Instance().kaiguan_ziding3;
            checkBox23.Checked = SetMgr.Instance().kaiguan_ziding4;
            numericUpDown3.Value = SetMgr.Instance().time_ziding1;
            numericUpDown4.Value = SetMgr.Instance().time_ziding2;
            numericUpDown5.Value = SetMgr.Instance().time_ziding3;
            numericUpDown6.Value = SetMgr.Instance().time_ziding4;
            richTextBox9.Text = SetMgr.Instance().自定义1;
            richTextBox10.Text = SetMgr.Instance().自定义2;
            richTextBox11.Text = SetMgr.Instance().自定义3;
            richTextBox12.Text = SetMgr.Instance().自定义4;
            richTextBox13.Text = SetMgr.Instance().游戏成功回复;
            richTextBox14.Text = SetMgr.Instance().查分回复;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl1.Enabled = false;

            SetMgr.Instance().check_xiaoshu_1 = checkBox16.Checked;
            SetMgr.Instance().check_xiaoshu_2 = checkBox17.Checked;
            SetMgr.Instance().check_shuangpai = checkBox18.Checked;
            SetMgr.Instance().protect = checkBox19.Checked;
            SetMgr.Instance().封盘时间 = int.Parse(domainUpDown1.Text);
            SetMgr.Instance().接近封盘时间 = int.Parse(domainUpDown2.Text);
            SetMgr.Instance().封盘消息 = richTextBox1.Text;
            SetMgr.Instance().接近封盘消息 = richTextBox2.Text;
            SetMgr.Instance().核对格式 = richTextBox3.Text;
            SetMgr.Instance().msg_kaijiang_num = (int)numericUpDown1.Value;
            SetMgr.Instance().msg_kaijiang_num2 = (int)numericUpDown2.Value;
            SetMgr.Instance().开奖消息 = richTextBox4.Text;
            SetMgr.Instance().开盘消息 = richTextBox5.Text;
            SetMgr.Instance().结算消息 = richTextBox6.Text;
            SetMgr.Instance().结算格式 = richTextBox7.Text;
            SetMgr.Instance().账单格式 = richTextBox8.Text;
            SetMgr.Instance().kaiguan_ziding1 = checkBox20.Checked;
            SetMgr.Instance().kaiguan_ziding2 = checkBox21.Checked;
            SetMgr.Instance().kaiguan_ziding3 = checkBox22.Checked;
            SetMgr.Instance().kaiguan_ziding4 = checkBox23.Checked;
            SetMgr.Instance().time_ziding1 = (int)numericUpDown3.Value;
            SetMgr.Instance().time_ziding2 = (int)numericUpDown4.Value;
            SetMgr.Instance().time_ziding3 = (int)numericUpDown5.Value;
            SetMgr.Instance().time_ziding4 = (int)numericUpDown6.Value;
            SetMgr.Instance().自定义1 = richTextBox9.Text;
            SetMgr.Instance().自定义2 = richTextBox10.Text;
            SetMgr.Instance().自定义3 = richTextBox11.Text;
            SetMgr.Instance().自定义4 = richTextBox12.Text;
            SetMgr.Instance().游戏成功回复 = richTextBox13.Text;
            SetMgr.Instance().查分回复 = richTextBox14.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox15.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SetMgr.Instance().每期总限额 = int.Parse(textBox15.Text);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox15.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateData();
        }

        public void updateData() {
            DataTable dat = null;
            if (comboBox1.Text == "北京赛车")
            {
                dat = pk10;
            } else if (comboBox1.Text == "飞艇")
            {
                dat = ship;
            }

            listView1.Items.Clear();
            listView1.BeginUpdate();
            listView2.Items.Clear();
            listView2.BeginUpdate();
            listView3.Items.Clear();
            listView3.BeginUpdate();
            for (int i = 0; i < dat.Rows.Count; ++i)
            {
                DataRow dr = dat.Rows[i];
                ListViewItem lvi = new ListViewItem((string)dr["内容"]);
                lvi.SubItems.Add((string)dr["赔率"]);
                lvi.SubItems.Add((string)dr["最低"]);
                lvi.SubItems.Add((string)dr["最高"]);
                lvi.SubItems.Add((string)dr["每期限额"]);
                if ((string)dr["类型"] == "数字")
                {
                    listView1.Items.Add(lvi);
                }
                else if ((string)dr["类型"] == "双面")
                {
                    listView2.Items.Add(lvi);
                }
                else
                {
                    listView3.Items.Add(lvi);
                }
            }
            listView1.EndUpdate();
            listView2.EndUpdate();
            listView3.EndUpdate();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                int zuidi = int.Parse(textBox16.Text);
                int zuigao = int.Parse(textBox17.Text);
                float gongji = float.Parse(textBox18.Text);
                int huihe = int.Parse(textBox19.Text);
                string kind = "";
                if (comboBox1.Text == "北京赛车")
                {
                    kind = "peilv_pk10";
                }
                else if (comboBox1.Text == "飞艇")
                {
                    kind = "peilv_ship";
                }
                foreach (ListViewItem a in listView1.Items)
                {
                    string sql = string.Format("update {0} set 最低='{1}',最高='{2}',赔率='{3}',每期限额='{4}' where 内容='{5}'",
                        kind, zuidi.ToString(), zuigao.ToString(), gongji.ToString(), huihe.ToString(), a.SubItems[0].Text);
                    this.BeginInvoke((Action)delegate ()
                    {
                        Reader.Instance().QueryGame(sql);
                        pk10 = Reader.Instance().ReadGameData("select * from peilv_pk10 order by 序号");
                        ship = Reader.Instance().ReadGameData("select * from peilv_ship order by 序号");
                        updateData();
                    });
                }
            }
            catch
            {
                MessageBox.Show("请检查数据后再试!");
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            try { 
                int zuidi = int.Parse(textBox20.Text);
                int zuigao = int.Parse(textBox21.Text);
                int gongji = int.Parse(textBox22.Text);
                int huihe = int.Parse(textBox23.Text);
                string kind = "";
                if (comboBox1.Text == "北京赛车")
                {
                    kind = "peilv_pk10";
                }
                else if (comboBox1.Text == "飞艇")
                {
                    kind = "peilv_ship";
                }
                foreach (ListViewItem a in listView2.Items)
                {
                    string sql = string.Format("update {0} set 最低='{1}',最高='{2}',赔率='{3}',每期限额='{4}' where 内容='{5}'",
                        kind, zuidi.ToString(), zuigao.ToString(), gongji.ToString(), huihe.ToString(), a.SubItems[0].Text);

                    Reader.Instance().QueryGame(sql);
                    pk10 = Reader.Instance().ReadGameData("select * from peilv_pk10 order by 序号");
                    ship = Reader.Instance().ReadGameData("select * from peilv_ship order by 序号");
                    updateData();
                }
            }
            catch
            {
                MessageBox.Show("请检查数据后再试!");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try { 

                int zuidi = int.Parse(textBox24.Text);
                int zuigao = int.Parse(textBox25.Text);
                int gongji = int.Parse(textBox26.Text);
                int huihe = int.Parse(textBox27.Text);
                string kind = "";
                if (comboBox1.Text == "北京赛车")
                {
                    kind = "peilv_pk10";
                }
                else if (comboBox1.Text == "飞艇")
                {
                    kind = "peilv_ship";
                }
                foreach (ListViewItem a in listView3.Items)
                {
                    string sql = string.Format("update {0} set 最低='{1}',最高='{2}',赔率='{3}',每期限额='{4}' where 内容='{5}'",
                        kind, zuidi.ToString(), zuigao.ToString(), gongji.ToString(), huihe.ToString(), a.SubItems[0].Text);

                    Reader.Instance().QueryGame(sql);
                    pk10 = Reader.Instance().ReadGameData("select * from peilv_pk10 order by 序号");
                    ship = Reader.Instance().ReadGameData("select * from peilv_ship order by 序号");
                    updateData();
                }
            }
            catch
            {
                MessageBox.Show("请检查数据后再试!");
            }
        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count == 0) return;
            textBox24.Text = listView3.SelectedItems[0].SubItems[2].Text;
            textBox25.Text = listView3.SelectedItems[0].SubItems[3].Text;
            textBox26.Text = listView3.SelectedItems[0].SubItems[1].Text;
            textBox27.Text = listView3.SelectedItems[0].SubItems[4].Text;
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0) return;
            textBox20.Text = listView2.SelectedItems[0].SubItems[2].Text;
            textBox21.Text = listView2.SelectedItems[0].SubItems[3].Text;
            textBox22.Text = listView2.SelectedItems[0].SubItems[1].Text;
            textBox23.Text = listView2.SelectedItems[0].SubItems[4].Text;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            textBox16.Text = listView1.SelectedItems[0].SubItems[2].Text;
            textBox17.Text = listView1.SelectedItems[0].SubItems[3].Text;
            textBox18.Text = listView1.SelectedItems[0].SubItems[1].Text;
            textBox19.Text = listView1.SelectedItems[0].SubItems[4].Text;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                int zuidi = int.Parse(textBox16.Text);
                int zuigao = int.Parse(textBox17.Text);
                float gongji = float.Parse(textBox18.Text);
                int huihe = int.Parse(textBox19.Text);
                string kind = "";
                if (comboBox1.Text == "北京赛车")
                {
                    kind = "peilv_pk10";
                }
                else if (comboBox1.Text == "飞艇")
                {
                    kind = "peilv_ship";
                }
                foreach (ListViewItem a in listView1.SelectedItems)
                {
                    string sql = string.Format("update {0} set 最低='{1}',最高='{2}',赔率='{3}',每期限额='{4}' where 内容='{5}'",
                        kind, zuidi.ToString(), zuigao.ToString(), gongji.ToString(), huihe.ToString(), a.SubItems[0].Text);
                    this.BeginInvoke((Action)delegate ()
                    {
                        Reader.Instance().QueryGame(sql);
                        pk10 = Reader.Instance().ReadGameData("select * from peilv_pk10 order by 序号");
                        ship = Reader.Instance().ReadGameData("select * from peilv_ship order by 序号");
                        updateData();
                    });
                }
            }
            catch
            {
                MessageBox.Show("请检查数据后再试!");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                int zuidi = int.Parse(textBox20.Text);
                int zuigao = int.Parse(textBox21.Text);
                int gongji = int.Parse(textBox22.Text);
                int huihe = int.Parse(textBox23.Text);
                string kind = "";
                if (comboBox1.Text == "北京赛车")
                {
                    kind = "peilv_pk10";
                }
                else if (comboBox1.Text == "飞艇")
                {
                    kind = "peilv_ship";
                }
                foreach (ListViewItem a in listView2.SelectedItems)
                {
                    string sql = string.Format("update {0} set 最低='{1}',最高='{2}',赔率='{3}',每期限额='{4}' where 内容='{5}'",
                        kind, zuidi.ToString(), zuigao.ToString(), gongji.ToString(), huihe.ToString(), a.SubItems[0].Text);

                    Reader.Instance().QueryGame(sql);
                    pk10 = Reader.Instance().ReadGameData("select * from peilv_pk10 order by 序号");
                    ship = Reader.Instance().ReadGameData("select * from peilv_ship order by 序号");
                    updateData();
                }
            }
            catch
            {
                MessageBox.Show("请检查数据后再试!");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {

                int zuidi = int.Parse(textBox24.Text);
                int zuigao = int.Parse(textBox25.Text);
                int gongji = int.Parse(textBox26.Text);
                int huihe = int.Parse(textBox27.Text);
                string kind = "";
                if (comboBox1.Text == "北京赛车")
                {
                    kind = "peilv_pk10";
                }
                else if (comboBox1.Text == "飞艇")
                {
                    kind = "peilv_ship";
                }
                foreach (ListViewItem a in listView3.SelectedItems)
                {
                    string sql = string.Format("update {0} set 最低='{1}',最高='{2}',赔率='{3}',每期限额='{4}' where 内容='{5}'",
                        kind, zuidi.ToString(), zuigao.ToString(), gongji.ToString(), huihe.ToString(), a.SubItems[0].Text);

                    Reader.Instance().QueryGame(sql);
                    pk10 = Reader.Instance().ReadGameData("select * from peilv_pk10 order by 序号");
                    ship = Reader.Instance().ReadGameData("select * from peilv_ship order by 序号");
                    updateData();
                }
            }
            catch
            {
                MessageBox.Show("请检查数据后再试!");
            }
        }
    }
}
