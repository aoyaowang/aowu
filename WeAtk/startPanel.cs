using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WeAtk.User;
using WeAtk.Common;
using WeAtk.pk10;

namespace WeAtk
{
    public partial class startPanel : Form
    {
        public startPanel()
        {
            InitializeComponent();
        }

        public void ShowList() {
            if (!MainUser.Instance.Logined) return;
            if (GameMgr.Instance().mGroup == null) return;
            
            this.listView4.BeginUpdate();
            this.listView4.Items.Clear();
            for (int i = 0; i < GameMgr.Instance().mGroup._list.Count; ++i) {
                WXUser user = (WXUser)GameMgr.Instance().mGroup._list[i];
                ListViewItem lvi = new ListViewItem(user.NickName);
                lvi.SubItems.Add(user.RemarkName);
                this.listView4.Items.Add(lvi);
            }
            this.listView4.EndUpdate();

            this.listView1.BeginUpdate();
            this.listView1.Items.Clear();
            GameMgr.Instance().Players.Clear();
            DataTable data = Reader.Instance().ReadPlayerData("select * from players");
            for (int i = 0; i < data.Rows.Count; ++i) {
                DataRow dr = data.Rows[i];
                string playername = (string)dr["玩家"];
                string nickname = (string)dr["群昵称"];
                int type = int.Parse((string)dr["类型"]);
                int left = int.Parse((string)dr["剩余分数"]);
                string num = (string)dr["期号"];
                string content = (string)dr["玩法内容"];
                int usescore = int.Parse((string)dr["使用分数"]);
                int perscore = int.Parse((string)dr["上期得分"]);
                string md5 = (string)dr["md5"];

                WXUser cur = null;
                for (int x = 0; x < GameMgr.Instance().mGroup._list.Count; ++x)
                {
                    WXUser user = (WXUser)GameMgr.Instance().mGroup._list[x];
                    if (user.RemarkName == playername) {
                        cur = user;
                        break;
                    }
                }

                if (cur == null) continue;

                ListViewItem lvi = new ListViewItem(playername);
                lvi.SubItems.Add(nickname);
                lvi.SubItems.Add(type == 0 ? "普通玩家" : "虚拟玩家");
                lvi.SubItems.Add(left.ToString());
                lvi.SubItems.Add(num);
                lvi.SubItems.Add(content);
                lvi.SubItems.Add(usescore.ToString());
                lvi.SubItems.Add(perscore.ToString());

                this.listView1.Items.Add(lvi);

                Player player = new Player();
                player.playername = playername;
                player.nickname = nickname;
                player.type = type;
                player.left = left;
                player.num = num;
                player.content = content;
                player.usedscore = usescore;
                player.perscore = perscore;
                player.me = cur;
                GameMgr.Instance().Players.Add(player);
            }
            this.listView1.EndUpdate();
        }

        private void startPanel_Load(object sender, EventArgs e)
        {

            GameMgr.Instance().AddGroupChangeFunc(ShowList);
            GameMgr.Instance().AddAtkFunc(AtkChanged);
            GameMgr.Instance().AddWaitFunc(WaitChanged);
            GameMgr.Instance().DataChangedFunc = DataChanged;
            GameMgr.Instance().TimerChangeFunc = TimerFunc;
            this.BeginInvoke((Action)delegate ()
            {
                checkBox1.Checked = SetMgr.Instance().receivechahui;
                checkBox2.Checked = SetMgr.Instance().receiveorder;
                checkBox3.Checked = SetMgr.Instance().low;
                checkBox4.Checked = SetMgr.Instance().recommend;
            });
        }

        private void TimerFunc(string a, string b)
        {
            this.BeginInvoke((Action)delegate ()
            {
                label15.Text = a;
                label16.Text = b;
            });

        }

        private void DataChanged()
        {
            this.BeginInvoke((Action)delegate ()
            {
                listView2.BeginUpdate();
                listView2.Items.Clear();
                List<pk10data> list = GameMgr.Instance().CurrentGame == 0 ? Cpk10.Instance().list : feiting.Instance().list;
                foreach (pk10data dat in list)
                {
                    ListViewItem lvi = new ListViewItem(dat.time);
                    lvi.SubItems.Add(dat.num);
                    lvi.SubItems.Add(dat.data);
                    listView2.Items.Add(lvi);
                }
                listView2.EndUpdate();
            });
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SetMgr.Instance().receivechahui = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SetMgr.Instance().receiveorder = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            SetMgr.Instance().low = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            SetMgr.Instance().recommend = checkBox4.Checked;
        }

        private void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView4.SelectedItems.Count > 1 || listView4.SelectedItems.Count == 0) return;

            string name = listView4.SelectedItems[0].Text;
            string nick = listView4.SelectedItems[0].SubItems[1].Text;

            textBox3.Text = name;
            textBox2.Text = nick;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0) return;

            string playername = textBox2.Text;
            string nickname = textBox3.Text;
            int type = comboBox1.SelectedIndex;
            int left = 0;
            string num = "";
            string content = "";
            int usescore = 0;
            int perscore = 0;

            WXUser cur = null;
            for (int i = 0; i < GameMgr.Instance().mGroup._list.Count; ++i)
            {
                WXUser user = (WXUser)GameMgr.Instance().mGroup._list[i];
                if (user.RemarkName == playername)
                {
                    cur = user;
                    break;
                }
            }

            if (cur == null)
            {
                MessageBox.Show("没有找到该玩家");
                return;
            }

            ListViewItem lvi = new ListViewItem(playername);
            lvi.SubItems.Add(nickname);
            lvi.SubItems.Add(type == 0 ? "普通玩家" : "虚拟玩家");
            lvi.SubItems.Add(left.ToString());
            lvi.SubItems.Add(num);
            lvi.SubItems.Add(content);
            lvi.SubItems.Add(usescore.ToString());
            lvi.SubItems.Add(perscore.ToString());

            this.listView1.Items.Add(lvi);

            Player player = new Player();
            player.playername = playername;
            player.nickname = nickname;
            player.type = type;
            player.left = left;
            player.num = num;
            player.content = content;
            player.usedscore = usescore;
            player.perscore = perscore;
            player.me = cur;
            GameMgr.Instance().Players.Add(player);
            player.insert();

            string s = GameMgr.Instance().buildAt(player) + "加入战斗,剩余粮草【" + left +"】";
            GameMgr.Instance().sendMsg(s, player);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 1 || listView1.SelectedItems.Count == 0) return;

            string name = listView1.SelectedItems[0].Text;
            string nick = listView1.SelectedItems[0].SubItems[1].Text;
            string type = listView1.SelectedItems[0].SubItems[2].Text;

            textBox4.Text = name;
            textBox6.Text = nick;
            textBox5.Text = "0";
            comboBox3.SelectedIndex = type == "普通玩家" ? 0 : 1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 1 || listView1.SelectedItems.Count == 0) return;

            listView1.SelectedItems[0].SubItems[2].Text = comboBox3.SelectedIndex == 0 ? "普通玩家" : "虚拟玩家";
            listView1.SelectedItems[0].SubItems[3].Text = (int.Parse(listView1.SelectedItems[0].SubItems[3].Text) + int.Parse(textBox5.Text)).ToString();

            Player cur = null;
            for (int i = 0; i < GameMgr.Instance().Players.Count; ++i)
            {
                Player user = (Player)GameMgr.Instance().Players[i];
                if (user.playername == listView1.SelectedItems[0].Text)
                {
                    cur = user;
                    break;
                }
            }

            if (cur == null)
            {
                MessageBox.Show("没有找到该玩家");
                return;
            }

            cur.left = int.Parse(listView1.SelectedItems[0].SubItems[3].Text);
            cur.type = comboBox3.SelectedIndex;
            cur.up();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameMgr.Instance().CurrentGame = comboBox2.SelectedIndex;
        }

        private void AtkChanged(){
            this.BeginInvoke((Action)delegate ()
            {
                float nAll = 0;
                this.listView1.BeginUpdate();
                this.listView1.Items.Clear();
                //GameMgr.Instance().Players.Clear();
                for (int i = 0; i < GameMgr.Instance().Players.Count; ++i)
                {
                    Player dr = GameMgr.Instance().Players[i];

                    ListViewItem lvi = new ListViewItem(dr.playername);
                    lvi.SubItems.Add(dr.nickname);
                    lvi.SubItems.Add(dr.type == 0 ? "普通玩家" : "虚拟玩家");
                    lvi.SubItems.Add(dr.left.ToString());
                    nAll += dr.left;
                    lvi.SubItems.Add(dr.num);
                    lvi.SubItems.Add(dr.content);
                    lvi.SubItems.Add(dr.usedscore.ToString());
                    lvi.SubItems.Add(dr.perscore.ToString());

                    this.listView1.Items.Add(lvi);
                }
                this.listView1.EndUpdate();

                playercount.Text = GameMgr.Instance().Players.Count.ToString();
                totalcoin.Text = nAll.ToString();
                allget.Text = GameMgr.Instance().totalget.ToString();
                lastturn.Text = GameMgr.Instance().lastturn.ToString();
            });
        }

        private void WaitChanged(){
            this.BeginInvoke((Action)delegate ()
            {
                this.listView3.BeginUpdate();
                this.listView3.Items.Clear();
                //GameMgr.Instance().Players.Clear();
                for (int i = 0; i < GameMgr.Instance().waitList.Count; ++i)
                {
                    waitLiang dr = GameMgr.Instance().waitList[i];

                    ListViewItem lvi = new ListViewItem(dr.player.playername);
                    lvi.SubItems.Add(dr.nValue > 0 ? "上分" : "下分");
                    lvi.SubItems.Add(dr.nValue.ToString());
                    lvi.SubItems.Add(dr.player.type == 0 ? "普通玩家" : "虚拟玩家");

                    this.listView3.Items.Add(lvi);
                }
                this.listView3.EndUpdate();
            });
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedIndices.Count != 1) return;
            int nIndex = listView3.SelectedIndices[0];
            if (nIndex < 0 || nIndex > GameMgr.Instance().waitList.Count) {
                WaitChanged();
                return;
            }
            waitLiang dr = GameMgr.Instance().waitList[nIndex];

            GameMgr.Instance().AgreeAddLiang(dr);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedIndices.Count != 1) return;
            int nIndex = listView3.SelectedIndices[0];
            if (nIndex < 0 || nIndex > GameMgr.Instance().waitList.Count)
            {
                WaitChanged();
                return;
            }
            waitLiang dr = GameMgr.Instance().waitList[nIndex];

            GameMgr.Instance().DeagreeAddLiang(dr);
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (GameMgr.Instance().InStarted)
            {
                startBtn.Text = "开始游戏";
                GameMgr.Instance().InStarted = false;
            } else
            {
                startBtn.Text = "停止游戏";
                GameMgr.Instance().InStarted = true;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 1 || listView1.SelectedItems.Count == 0) return;

            Player cur = null;
            for (int i = 0; i < GameMgr.Instance().Players.Count; ++i)
            {
                Player user = (Player)GameMgr.Instance().Players[i];
                if (user.playername == listView1.SelectedItems[0].Text)
                {
                    cur = user;
                    GameMgr.Instance().Players.Remove(user);
                    break;
                }
            }

            if (cur == null)
            {
                MessageBox.Show("没有找到该玩家");
                return;
            }

            cur.del();
            this.AtkChanged();

            string s = GameMgr.Instance().buildAt(cur) + "已经远离了这座城堡的争斗";
            GameMgr.Instance().sendMsg(s, cur);
        }

        private void groupBox9_Enter(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (MainUser.Instance.Logined)
            {
                MainUser.Instance.reload();
            }
            if (SubUser.Instance.Logined)
            {
                SubUser.Instance.reload();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            GameMgr.Instance().MsgLiang();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem a in listView1.Items)
            {
                if (a.SubItems[0].Text.IndexOf(textBox1.Text) >= 0)
                {
                    a.Selected = true;
                    break;
                }
                    
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GameMgr.Instance().MsgXiu();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 1 || listView1.SelectedItems.Count == 0) return;

            Player cur = null;
            for (int i = 0; i < GameMgr.Instance().Players.Count; ++i)
            {
                Player user = (Player)GameMgr.Instance().Players[i];
                if (user.playername == listView1.SelectedItems[0].Text)
                {
                    cur = user;
                    break;
                }
            }

            if (cur == null)
            {
                MessageBox.Show("请选择玩家");
                return;
            }

            if (!GameMgr.Instance().ChangeAtk(cur, textBox7.Text))
            {
                MessageBox.Show("修改失败，请检查攻击格式");
                return;
            }

            AtkChanged();
        }
    }
}
