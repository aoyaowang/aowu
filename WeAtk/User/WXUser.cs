using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;
using WeChat.NET.HTTP;
using System.IO;
using System.Text.RegularExpressions;

namespace WeAtk.User
{
    /// <summary>
    /// 微信用户
    /// </summary>
    public class WXUser
    {
        private WXService wxs;

        //用户id
        private string _userName;
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }
        public bool isGroup
        {
            get
            {
                int n = _userName.IndexOf('@');
                if (n >= 0) {
                    int n1 = _userName.IndexOf('@', n + 1);
                    if (n1 >= 0) {
                        int n2 = _userName.IndexOf('@', n1 + 1);
                        if (n2 == -1) {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        //昵称
        private string _onickName;
        public string _nickName;
        public string NickName
        {
            get
            {
                string emoji = Regex.Replace(_nickName, @"<span class=""emoji emoji(.{1,7}?)""></span>", "(表情)");
                return emoji;
            }
            set
            {
                _onickName = value;
                string emoji = Regex.Replace(value, @"<span class=""emoji emoji(.{1,7}?)""></span>", "(表情)");
                _nickName = emoji;
            }
        }
        public string ONickName
        {
            get
            {
                //string emoji = Regex.Replace(_remarkName, @"<span class=""emoji emoji(.{1,7}?)""></span>", "(表情)");
                return NickName;
            }
        }
        //头像url
        private string _headImgUrl;
        public string HeadImgUrl
        {
            get
            {
                return _headImgUrl;
            }
            set
            {
                _headImgUrl = value;
            }
        }
        //备注名
        private string _oremarkName;
        private string _remarkName;
        public string RemarkName
        {
            get
            {
                string emoji = Regex.Replace(_remarkName, @"<span class=""emoji emoji(.{1,7}?)""></span>", "(表情)");
                return emoji;
            }
            set
            {
                _oremarkName = value;
                string emoji = Regex.Replace(value, @"<span class=""emoji emoji(.{1,7}?)""></span>", "(表情)");
                _remarkName = emoji;
            }
        }
        public string ORemarkName
        {
            get
            {
                //string emoji = Regex.Replace(_remarkName, @"<span class=""emoji emoji(.{1,7}?)""></span>", "(表情)");
                return RemarkName;
            }
        }
        //性别 男1 女2 其他0
        private string _sex;
        public string Sex
        {
            get
            {
                return _sex;
            }
            set
            {
                _sex = value;
            }
        }
        //签名
        private string _signature;
        public string Signature
        {
            get
            {
                return _signature;
            }
            set
            {
                _signature = value;
            }
        }
        //城市
        private string _city;
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
            }
        }
        //省份
        private string _province;
        public string Province
        {
            get
            {
                return _province;
            }
            set
            {
                _province = value;
            }
        }
        //昵称全拼
        private string _pyQuanPin;
        public string PYQuanPin
        {
            get
            {
                return _pyQuanPin;
            }
            set
            {
                _pyQuanPin = value;
            }
        }
        //备注名全拼
        private string _remarkPYQuanPin;
        public string RemarkPYQuanPin
        {
            get
            {
                return _remarkPYQuanPin;
            }
            set
            {
                _remarkPYQuanPin = value;
            }
        }
        //头像
        private bool _loading_icon = false;
        private Image _icon;
        public Image Icon
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string ShowName
        {
            get
            {
                string emoji = (_remarkName == null || _remarkName == "") ? _nickName : _remarkName;
                emoji = Regex.Replace(_remarkName, @"<span class=""emoji emoji(.{1,7}?)""></span>", "(表情)");
                return emoji;
            }
        }
        /// <summary>
        /// 显示的拼音全拼
        /// </summary>
        public string ShowPinYin
        {
            get
            {
                return (_remarkPYQuanPin == null || _remarkPYQuanPin == "") ? _pyQuanPin : _remarkPYQuanPin;
            }
        }

        //发送给对方的消息  
        private Dictionary<DateTime, WXMsg> _sentMsg = new Dictionary<DateTime, WXMsg>();
        public Dictionary<DateTime, WXMsg> SentMsg
        {
            get
            {
                return _sentMsg;
            }
        }
        //收到对方的消息
        private Dictionary<DateTime, WXMsg> _recvedMsg = new Dictionary<DateTime, WXMsg>();
        public Dictionary<DateTime, WXMsg> RecvedMsg
        {
            get
            {
                return _recvedMsg;
            }
        }

        public event MsgSentEventHandler MsgSent;
        public event MsgRecvedEventHandler MsgRecved;

        /// <summary>
        /// 接收来自该用户的消息
        /// </summary>
        /// <param name="msg"></param>
        public void ReceiveMsg(WXMsg msg)
        {
            _recvedMsg.Add(msg.Time, msg);
            if (MsgRecved != null)
            {
                MsgRecved(msg);
            }
        }
        /// <summary>
        /// 向该用户发送消息
        /// </summary>
        /// <param name="msg"></param>
        public void SendMsg(WXMsg msg, bool showOnly)
        {
            //发送
            if (!showOnly)
            {
                wxs.SendMsg(msg.Msg, msg.From, msg.To, msg.Type);
            }

            //_sentMsg.Add(msg.Time, msg);
            if (MsgSent != null)
            {
                MsgSent(msg);
            }
        }

        public void SendImg(FileStream bts, string to)
        {
            if (wxs != null) wxs.sendImg(bts, this.UserName, to);
        }
        /// <summary>
        /// 获取该用户发送的未读消息
        /// </summary>
        /// <returns></returns>
        public List<WXMsg> GetUnReadMsg()
        {
            List<WXMsg> list = null;
            foreach (KeyValuePair<DateTime, WXMsg> p in _recvedMsg)
            {
                if (!p.Value.Readed)
                {
                    if (list == null)
                    {
                        list = new List<WXMsg>();
                    }
                    list.Add(p.Value);
                }
            }

            return list;
        }
        /// <summary>
        /// 获取最近的一条消息
        /// </summary>
        /// <returns></returns>
        public WXMsg GetLatestMsg()
        {
            WXMsg msg = null;
            if (_sentMsg.Count > 0 && _recvedMsg.Count>0)
            {
                msg = _sentMsg.Last().Value.Time > _recvedMsg.Last().Value.Time ? _sentMsg.Last().Value : _recvedMsg.Last().Value;
            }
            else if (_sentMsg.Count > 0)
            {
                msg = _sentMsg.Last().Value;
            }
            else if (_recvedMsg.Count > 0)
            {
                msg = _recvedMsg.Last().Value;
            }
            else
            {
                msg = null;
            }
            return msg;
        }

        public void setWxs(object _wxs) {
            wxs = (WXService)_wxs;
        }
    }
    /// <summary>
    /// 表示处理消息发送完成事件的方法
    /// </summary>
    /// <param name="msg"></param>
    public delegate void MsgSentEventHandler(WXMsg msg);
    /// <summary>
    /// 表示处理接收到新消息事件的方法
    /// </summary>
    /// <param name="msg"></param>
    public delegate void MsgRecvedEventHandler(WXMsg msg);
}
