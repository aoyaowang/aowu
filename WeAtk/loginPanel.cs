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
    public partial class loginPanel : Form
    {
        public loginPanel()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ((Action)(delegate ()
            {
                Image img = MainUser.Instance.GetQRCode();

                pictureBox1.Image = img;

                MainUser.Instance.SetFunc(ShowImg1, leaveMsg1);
            })).BeginInvoke(null, null);
        }

        private void ShowImg1() {
            this.BeginInvoke((Action)delegate ()
            {
                pictureBox1.Image = MainUser.Instance.headImg;
                comboBox1.BeginUpdate();
                comboBox1.Items.Clear();
                comboBox1.EndUpdate();
                int nCount = 0;
                for (int i = 0; i < MainUser.Instance._group.Count; ++i)
                {
                    WXGroup gp = (WXGroup)(MainUser.Instance._group[i]);
                    WXUser user = gp._me;
                    comboBox1.Items.Insert(nCount++, user.NickName);
                }
            });
        }

        private void leaveMsg1()
        {
            this.BeginInvoke((Action)delegate ()
            {
                pictureBox1.Image = null;
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Image img = SubUser.Instance.GetQRCode();

            pictureBox2.Image = img;

            SubUser.Instance.SetFunc(ShowImg2, leaveMsg2);
        }

        private void ShowImg2()
        {
            this.BeginInvoke((Action)delegate ()
            {
                pictureBox2.Image = SubUser.Instance.headImg;
                comboBox2.BeginUpdate();
                comboBox2.Items.Clear();
                comboBox2.EndUpdate();
                int nCount = 0;
                for (int i = 0; i < SubUser.Instance._group.Count; ++i)
                {
                    WXGroup gp = (WXGroup)(SubUser.Instance._group[i]);
                    WXUser user = gp._me;
                    comboBox2.Items.Insert(nCount++, user.NickName);
                }
            });
        }

        private void leaveMsg2()
        {
            this.BeginInvoke((Action)delegate ()
            {
                pictureBox2.Image = null;
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String to = "";
            int nCount = 0;
            for (int i = 0; i < MainUser.Instance._group.Count; ++i)
            {
                WXGroup gp = (WXGroup)(MainUser.Instance._group[i]);
                WXUser user = gp._me;
                if (user.isGroup) {
                    if (nCount == comboBox1.SelectedIndex) {
                        to = user.UserName;
                        break;
                    }
                    nCount++;
                }
            }
            if (to == "") return;
            String smsg = richTextBox1.Text;
            ((Action)(delegate ()
            {
                if (MainUser.Instance.Logined) {
                    WXMsg msg = new WXMsg();
                    msg.From = MainUser.Instance._me.UserName;
                    msg.Msg = smsg;  //只接受文本消息
                    msg.Readed = false;
                    msg.Time = DateTime.Now;
                    msg.To = to;
                    msg.Type = 1;
                    MainUser.Instance.SendMsg(msg);
                }
            })).BeginInvoke(null, null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String to = "";
            int nCount = 0;
            for (int i = 0; i < SubUser.Instance._group.Count; ++i)
            {
                WXGroup gp = (WXGroup)(SubUser.Instance._group[i]);
                WXUser user = gp._me;
                if (user.isGroup)
                {
                    if (nCount == comboBox1.SelectedIndex)
                    {
                        to = user.UserName;
                        break;
                    }
                    nCount++;
                }
            }
            if (to == "") return;
            String smsg = richTextBox1.Text;
            ((Action)(delegate ()
            {
                if (SubUser.Instance.Logined)
                {
                    WXMsg msg = new WXMsg();
                    msg.From = SubUser.Instance._me.UserName;
                    msg.Msg = smsg;  //只接受文本消息
                    msg.Readed = false;
                    msg.Time = DateTime.Now;
                    msg.To = to;
                    msg.Type = 1;
                    SubUser.Instance.SendMsg(msg);
                }
            })).BeginInvoke(null, null);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nCount = 0;
            for (int i = 0; i < MainUser.Instance._group.Count; ++i)
            {
                WXGroup gp = (WXGroup)(MainUser.Instance._group[i]);
                WXUser user = gp._me;
                if (nCount == comboBox1.SelectedIndex)
                {
                    GameMgr.Instance().mGroup = gp;
                    break;
                }
                nCount++;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nCount = 0;
            for (int i = 0; i < SubUser.Instance._group.Count; ++i)
            {
                WXGroup gp = (WXGroup)(SubUser.Instance._group[i]);
                WXUser user = gp._me;
                if (nCount == comboBox1.SelectedIndex)
                {
                    GameMgr.Instance().sGroup = gp;
                    break;
                }
                nCount++;
            }
        }
    }
}
