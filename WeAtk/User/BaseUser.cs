using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Timers;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeChat.NET.HTTP;
using System.Windows.Forms;

using WeAtk.Common;

namespace WeAtk.User
{
    class WXGroup {
        public WXUser _me = null;
        public List<object> _list = new List<object>();
    }
    class BaseUser
    {
        public delegate void loginSucFunc();

        public BaseService _BaseService = null;
        public LoginService _LoginService = null;
        public WXService _WXService = null;

        public bool Logined = false;
        public bool sendLoginRequest = false;
        public Image headImg = null;


        public WXUser _me = null;
        public List<Object> _contact_all = new List<object>();
        public List<object> _contact_latest = new List<object>();
        public List<object> _group = new List<object>();
        
        private loginSucFunc _func = null;
        private loginSucFunc _leaveFunc = null;

        private System.Timers.Timer _loginTimer = null;
        public BaseUser() {
            _BaseService = new BaseService();
            _LoginService = new LoginService();
            _WXService = new WXService();

            _LoginService.Init(_BaseService);
            _WXService.Init(_BaseService, _LoginService);
        }

        ~BaseUser() {
            _WXService = null;
            _LoginService = null;
            _BaseService = null;
            _me = null;
            headImg = null;
            Logined = false;
            sendLoginRequest = false;

            _contact_all.Clear();
            _contact_latest.Clear();
            _group.Clear();

            if (_loginTimer != null) {
                _loginTimer.Stop();
                _loginTimer = null;
            }
        }

        public void SetFunc(loginSucFunc func, loginSucFunc leave) {
            _func = func;
            _leaveFunc = leave;
        }

        private void Reset() {
            _WXService = null;
            _LoginService = null;
            _BaseService = null;
            _me = null;
            headImg = null;
            Logined = false;
            sendLoginRequest = false;

            _contact_all.Clear();
            _contact_latest.Clear();
            _group.Clear();
            if (_loginTimer != null)
            {
                _loginTimer.Stop();
                _loginTimer = null;
            }

            _BaseService = new BaseService();
            _LoginService = new LoginService();
            _WXService = new WXService();

            _LoginService.Init(_BaseService);
            _WXService.Init(_BaseService, _LoginService);
        }

        public Image GetQRCode() {
            Reset();

            Image ret = _LoginService.GetQRCode();
            if (_loginTimer != null) {
                _loginTimer.Dispose();
                _loginTimer = null;
            }

            _loginTimer = new System.Timers.Timer(2000);
            _loginTimer.Elapsed += loginTimerFunc;
            _loginTimer.Start();

            return ret;
        }

        private void loginTimerFunc(object o, EventArgs e) {
            if (!sendLoginRequest) {
                sendLoginRequest = true;
                object obj = _LoginService.LoginCheck();
                
                if (obj == null)
                {
                    sendLoginRequest = false;
                    return;
                }
                if (obj.GetType() == typeof(Bitmap))
                {
                    sendLoginRequest = false;
                    headImg = (Image)obj;
                }
                else {
                    if (_loginTimer != null)
                    {
                        _loginTimer.Stop();
                        _loginTimer = null;
                    }
                    _LoginService.GetSidUid(obj as string);

                    JObject init_result = _WXService.WxInit((obj as string).IndexOf("wx2")>=0);
                    sendLoginRequest = false;
                    if (init_result == null) //登录失败
                    {
                        MessageBox.Show("登录微信失败！");
                    }
                    else { //登录成功
                        
                        Logined = true;
                        
                        List<object> contact_all = new List<object>();
                        if (init_result != null)
                        {
                            _me = new WXUser();
                            _me.setWxs(_WXService);
                            _me.UserName = init_result["User"]["UserName"].ToString();
                            _me.City = "";
                            _me.HeadImgUrl = init_result["User"]["HeadImgUrl"].ToString();
                            _me.NickName = init_result["User"]["NickName"].ToString();
                            _me.Province = "";
                            _me.PYQuanPin = init_result["User"]["PYQuanPin"].ToString();
                            _me.RemarkName = init_result["User"]["RemarkName"].ToString();
                            _me.RemarkPYQuanPin = init_result["User"]["RemarkPYQuanPin"].ToString();
                            _me.Sex = init_result["User"]["Sex"].ToString();
                            _me.Signature = init_result["User"]["Signature"].ToString();

                            foreach (JObject contact in init_result["ContactList"])  //部分好友名单
                            {
                                WXUser user = new WXUser();
                                user.UserName = contact["UserName"].ToString();
                                user.City = contact["City"].ToString();
                                user.HeadImgUrl = contact["HeadImgUrl"].ToString();
                                user.NickName = contact["NickName"].ToString();
                                user.Province = contact["Province"].ToString();
                                user.PYQuanPin = contact["PYQuanPin"].ToString();
                                user.RemarkName = contact["RemarkName"].ToString();
                                user.RemarkPYQuanPin = contact["RemarkPYQuanPin"].ToString();
                                user.Sex = contact["Sex"].ToString();
                                user.Signature = contact["Signature"].ToString();

                                _contact_latest.Add(user);
                            }
                        }
                        
                        JObject contact_result = _WXService.GetContact(); //通讯录
                        if (contact_result != null)
                        {
                            foreach (JObject contact in contact_result["MemberList"])  //完整好友名单
                            {
                                WXUser user = new WXUser();
                                user.UserName = contact["UserName"].ToString();
                                user.City = contact["City"].ToString();
                                user.HeadImgUrl = contact["HeadImgUrl"].ToString();
                                user.NickName = contact["NickName"].ToString();
                                user.Province = contact["Province"].ToString();
                                user.PYQuanPin = contact["PYQuanPin"].ToString();
                                user.RemarkName = contact["RemarkName"].ToString();
                                user.RemarkPYQuanPin = contact["RemarkPYQuanPin"].ToString();
                                user.Sex = contact["Sex"].ToString();
                                user.Signature = contact["Signature"].ToString();
                                if (user.isGroup)
                                {
                                    WXGroup gp = new WXGroup();
                                    gp._me = user;
                                    _group.Add(gp);
                                }
                                _contact_all.Add(user);
                            }
                        }
                        string strJ = "{\"BaseRequest\":{\"Uin\":{0},\"Sid\":\"{1}\",\"Skey\":\"{2}\",\"DeviceID\":\"e1615250492\"},\"Count\":" + _group.Count+",\"List\":[";
                        bool bf = true;
                        foreach(WXGroup gpp in _group)
                        {
                            if (!bf) strJ += ",";
                            bf = false;
                            strJ += "{\"UserName\":\"" + gpp._me.UserName + "\",\"ChatRoomId\":\"\"}"; 
                        }
                        strJ += "]}";
                        
                        JObject contact_g_result = _WXService.GetBatchContact(strJ); //通讯录
                        if (contact_g_result != null && contact_g_result["ContactList"] != null)
                        {
                            foreach (JObject contact in contact_g_result["ContactList"])  //完整好友名单
                            {
                                foreach (WXGroup gpp in _group)
                                {
                                    if (gpp._me.UserName == contact["UserName"].ToString())
                                    {
                                        foreach (JObject mem in contact["MemberList"])  //部分好友名单
                                        {
                                            WXUser m = new WXUser();
                                            m.UserName = mem["UserName"].ToString();
                                            m.NickName = mem["NickName"].ToString();
                                            m.PYQuanPin = mem["PYQuanPin"].ToString();
                                            m.RemarkName = mem["NickName"].ToString();
                                            m.RemarkPYQuanPin = mem["RemarkPYQuanPin"].ToString();
                                            gpp._list.Add(m);
                                        }
                                        break;
                                    }
                                }
                            }
                        }

                        Thread td = new Thread(new ThreadStart(Run));
                        td.Start();
                        _func();
                    }
                }
            }
        }

        public void reload() {
            List<object> contact_all = new List<object>();
            List<object> group = new List<object>();

            JObject contact_result = _WXService.GetContact(); //通讯录
            if (contact_result != null)
            {
                foreach (JObject contact in contact_result["MemberList"])  //完整好友名单
                {
                    WXUser user = new WXUser();
                    user.UserName = contact["UserName"].ToString();
                    user.City = contact["City"].ToString();
                    user.HeadImgUrl = contact["HeadImgUrl"].ToString();
                    user.NickName = contact["NickName"].ToString();
                    user.Province = contact["Province"].ToString();
                    user.PYQuanPin = contact["PYQuanPin"].ToString();
                    user.RemarkName = contact["RemarkName"].ToString();
                    user.RemarkPYQuanPin = contact["RemarkPYQuanPin"].ToString();
                    user.Sex = contact["Sex"].ToString();
                    user.Signature = contact["Signature"].ToString();
                    if (user.isGroup)
                    {
                        WXGroup gp = new WXGroup();
                        gp._me = user;
                        group.Add(gp);
                    }
                    contact_all.Add(user);
                }
            }
            string strJ = "{\"BaseRequest\":{\"Uin\":{0},\"Sid\":\"{1}\",\"Skey\":\"{2}\",\"DeviceID\":\"e1615250492\"},\"Count\":" + _group.Count + ",\"List\":[";
            bool bf = true;
            foreach (WXGroup gpp in group)
            {
                if (!bf) strJ += ",";
                bf = false;
                strJ += "{\"UserName\":\"" + gpp._me.UserName + "\",\"ChatRoomId\":\"\"}";
            }
            strJ += "]}";

            JObject contact_g_result = _WXService.GetBatchContact(strJ); //通讯录
            if (contact_g_result != null && contact_g_result["ContactList"] != null)
            {
                foreach (JObject contact in contact_g_result["ContactList"])  //完整好友名单
                {
                    foreach (WXGroup gpp in group)
                    {
                        if (gpp._me.UserName == contact["UserName"].ToString())
                        {
                            foreach (JObject mem in contact["MemberList"])  //部分好友名单
                            {
                                WXUser m = new WXUser();
                                m.UserName = mem["UserName"].ToString();
                                m.NickName = mem["NickName"].ToString();
                                m.PYQuanPin = mem["PYQuanPin"].ToString();
                                m.RemarkName = mem["NickName"].ToString();
                                m.RemarkPYQuanPin = mem["RemarkPYQuanPin"].ToString();
                                gpp._list.Add(m);
                            }
                            break;
                        }
                    }
                }
            }

            _contact_all = contact_all;
            _group = group;

            foreach (WXGroup gpp in _group)
            {
                if (GameMgr.Instance().mGroup != null && gpp._me.UserName == GameMgr.Instance().mGroup._me.UserName)
                {
                    GameMgr.Instance().mGroup = gpp;
                }

                if (GameMgr.Instance().sGroup != null && gpp._me.UserName == GameMgr.Instance().sGroup._me.UserName)
                {
                    GameMgr.Instance().sGroup = gpp;
                }
            } 

            GameMgr.Instance().mGroup = GameMgr.Instance().mGroup;
        }

        public void Run()
        {
            string sync_flag = "";
            JObject sync_result;
            while (true)
            {
                sync_flag = _WXService.WxSyncCheck();  //同步检查
                if (sync_flag == null || sync_flag.IndexOf("1100") >= 0 || sync_flag.IndexOf("1101") >= 0)
                {
                    MessageBox.Show("玩家已掉线！" + _me.NickName + sync_flag);
                    Logined = false;
                    _leaveFunc();
                    return;
                }
                //这里应该判断 sync_flag中selector的值
                else if (sync_flag.IndexOf("selector:0") == -1)//有消息
                {
                    
                    sync_result = _WXService.WxSync();  //进行同步
                    if (sync_result != null)
                    {
                        if (sync_result["AddMsgCount"] != null && sync_result["AddMsgCount"].ToString() != "0")
                        {
                            foreach (JObject m in sync_result["AddMsgList"])
                            {
                                string from = m["FromUserName"].ToString();
                                string to = m["ToUserName"].ToString();
                                string content = m["Content"].ToString();
                                string type = m["MsgType"].ToString();
                                string id = m["MsgId"].ToString();

                                WXMsg msg = new WXMsg();
                                msg.From = from;
                                msg.Msg = type == "1" ? content : "请在其他设备上查看消息";  //只接受文本消息
                                msg.Readed = false;
                                msg.Time = DateTime.Now;
                                msg.To = to;
                                msg.Type = int.Parse(type);
                                msg.ID = id;

                                if (msg.Type == 51)  //屏蔽一些系统数据
                                {
                                    continue;
                                }

                                Console.Write(msg.Msg);
                                this.OnMsg(msg);
                            }
                        }
                    }
                }
                System.Threading.Thread.Sleep(10);
            }
        }

        public void SendMsg(WXMsg msg)
        {
            _me.SendMsg(msg, false);
        }

        public void SendImg(FileStream img, string toUser)
        {
            _me.SendImg(img, toUser);
        }

        public virtual void OnMsg(WXMsg wxMsg) {
            GameMgr.Instance().OnMsg(wxMsg); ;
        }

    }
}
