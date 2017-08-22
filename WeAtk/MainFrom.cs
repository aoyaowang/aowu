using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeAtk
{
    public partial class MainForm : Form
    {
        public loginPanel _loginPanel = null;
        public rootPanel _rootPanel = null;
        public settingPanel _settingPanel = null;
        public startPanel _startPanel = null;
        public logPanel _logPanel = null;
        public fightlogPanel _fightlogPanel = null;

        private int _selecttab;
        public int NowTab
        {
            get
            {
                return _selecttab;
            }
            set
            {
                _selecttab = value;
                ShowForm();
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _loginPanel = new loginPanel();
            _rootPanel = new rootPanel();
            _settingPanel = new settingPanel();
            _startPanel = new startPanel();
            _logPanel = new logPanel();
            _fightlogPanel = new fightlogPanel();


            _loginPanel.TopLevel = false;
            _rootPanel.TopLevel = false;
            _settingPanel.TopLevel = false;
            _startPanel.TopLevel = false;
            _logPanel.TopLevel = false;
            _fightlogPanel.TopLevel = false;

            _loginPanel.Dock = DockStyle.Fill;
            _rootPanel.Dock = DockStyle.Fill;
            _settingPanel.Dock = DockStyle.Fill;
            _startPanel.Dock = DockStyle.Fill;
            _logPanel.Dock = DockStyle.Fill;
            _fightlogPanel.Dock = DockStyle.Fill;


            panel1.Controls.Add(_loginPanel);
            panel1.Controls.Add(_rootPanel);
            panel1.Controls.Add(_settingPanel);
            panel1.Controls.Add(_startPanel);
            panel1.Controls.Add(_logPanel);
            panel1.Controls.Add(_fightlogPanel);

            _loginPanel.Show();//这个必须有，不然显示不出来
            _rootPanel.Show();
            _settingPanel.Show();
            _startPanel.Show();
            _logPanel.Show();
            _fightlogPanel.Show();
            NowTab = 0;
        }

        public void ShowForm()
        {
            _loginPanel.Visible = NowTab == 0;
            _startPanel.Visible = NowTab == 1;
            _settingPanel.Visible = NowTab == 2;
            _rootPanel.Visible = NowTab == 3;
            _logPanel.Visible = NowTab == 4;
            _fightlogPanel.Visible = NowTab == 5;
        }

        private void loginbtn_Click(object sender, EventArgs e)
        {
            NowTab = 0;
        }

        private void startbtn_Click(object sender, EventArgs e)
        {
            NowTab = 1;
        }

        private void settingbtn_Click(object sender, EventArgs e)
        {
            NowTab = 2;
        }

        private void rootbtn_Click(object sender, EventArgs e)
        {
            NowTab = 3;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            System.Environment.Exit(0);        }

        private void logBtn_Click(object sender, EventArgs e)
        {
            NowTab = 4;
        }

        private void fightlogBtn_Click(object sender, EventArgs e)
        {
            NowTab = 5;
        }
    }
}
