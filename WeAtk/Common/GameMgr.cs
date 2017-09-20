using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeAtk.User;
using WeAtk.pk10;
using System.Globalization;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace WeAtk.Common
{
    class waitLiang
    {
        public Player player = null;
        public int nValue = 0;
        public string id = "";
    }
    class atkMsg
    {
        public Player player = null;
        public string content = "";
        public string id = "";
        public List<string> coin = new List<string>();
        public List<string> main = new List<string>();
        public int used;
        public int allused;
    }
    class result
    {
        public Player player = null;
        public float nScore = 0;
        public string sContent = "";
    }
    class GameMgr
    {
        public static string GAMEINEND = "休战中,无法进行操作";
        public delegate void SELECTCHANGE();
        public delegate void WAITCHANGE();
        public delegate void ATKCHANGE();

        public delegate void TIMERCHANGE(string a, string b);
        private static GameMgr _Instance = null;
        public static GameMgr Instance()
        {
            if (_Instance == null) _Instance = new GameMgr();
            return _Instance;
        }

        public List<SELECTCHANGE> _list = new List<SELECTCHANGE>();
        public List<WAITCHANGE> _waitFunc = new List<WAITCHANGE>();
        public List<ATKCHANGE> _atkFunc = new List<ATKCHANGE>();
        public void AddGroupChangeFunc(SELECTCHANGE func)
        {
            _list.Add(func);
        }
        public void DelGroupChangeFunc(SELECTCHANGE func)
        {
            _list.Remove(func);
        }

        public void AddWaitFunc(WAITCHANGE func)
        {
            _waitFunc.Add(func);
        }
        public void DelWaitFunc(WAITCHANGE func)
        {
            _waitFunc.Remove(func);
        }
        public void WaitChange()
        {
            foreach (WAITCHANGE w in _waitFunc)
            {
                w();
            }
        }

        public void AddAtkFunc(ATKCHANGE func)
        {
            _atkFunc.Add(func);
        }
        public void DelAtkFunc(ATKCHANGE func)
        {
            _atkFunc.Remove(func);
        }
        public void AtkChange()
        {
            foreach (ATKCHANGE w in _atkFunc)
            {
                w();
            }
        }
        public SELECTCHANGE DataChangedFunc = null;
        public TIMERCHANGE TimerChangeFunc = null;

        private WXGroup _group1 = null;
        private WXGroup _group2 = null;
        public WXGroup mGroup
        {
            get
            {
                return _group1;
            }
            set
            {
                _group1 = value;
                for (int i = 0; i < _list.Count; ++i) _list[i]();
            }
        }
        public WXGroup sGroup
        {
            get
            {
                return _group2;
            }
            set
            {
                _group2 = value;
                for (int i = 0; i < _list.Count; ++i) _list[i]();
            }
        }
        public bool _inStarted = false;
        public bool InStarted
        {
            get { return _inStarted; }
            set
            {
                if (_inStarted == value) return;
                _inStarted = value;
                if (_inStarted == false)
                {
                    MsgAll("主机已关机");
                }
                Started = value;
            }
        }
        public bool _Started = false; //游戏是否开始
        public bool Started
        {
            get { return _Started; }
            set
            {
                if (!_Started && value)
                {
                    //_waitbegin = true;
                    GameStart();
                }
                else
                {
                    GamePause();
                }
                _Started = value;
            }
        }

        public string num = "";
        public DateTime begintime;
        public bool _bSendA = false;
        public bool _bSendB = false;
        public bool _bSendC = false;
        public bool _bSendD = false;
        public bool _bSendX = false;
        public bool _bSendXX = false;
        public bool _InEnd = false;
        public bool _lock = false;
        public bool _loop = false;
        public bool _waitbegin = false;
        private System.Timers.Timer _Timer = null;
        public float totalget = 0;
        public float lastturn = 0;
        public List<Player> lstCurPlayer = new List<Player>();

        private void GameStart()
        {
            if (CurrentGame == 2)
            {
                _loop = true;
                CurrentGame = 0;
            } else
            {
                _loop = false;
            }
												lstCurPlayer.Clear();
            if (CurrentGame == 0)
            {
                if (!Cpk10.Instance().Init())
                {
                    Started = false;
                    return;
                }
                DataChangedFunc();
                num = Cpk10.Instance().curnum;

                DateTime dt;
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy/MM/dd HH/mm";
                dt = Convert.ToDateTime(Cpk10.Instance().list[0].time, dtFormat);
                begintime = dt;
                if (begintime.AddMinutes(5).Hour >= 0 && begintime.AddMinutes(5).Hour < 9)
                {
                    Started = false;
                }
                else
                {
                    SendStart();
                }
            }
            else if (CurrentGame == 1)
            {
                if (!feiting.Instance().Init())
                {
                    Started = false;
                    return;
                }
                DataChangedFunc();
                num = feiting.Instance().curnum;

                DateTime dt;
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy/MM/dd HH/mm";
                dt = Convert.ToDateTime(feiting.Instance().list[0].time, dtFormat);

                DateTime ndt = DateTime.Now;
                if (CurrentGame == 0)
                {
                    if (ndt.Minute % 10 >= 2 && ndt.Minute % 10 < 7)
                        ndt.AddMinutes((ndt.Minute % 10 - 2) * -1);
                    else if (ndt.Minute % 10 >= 7)
                        ndt.AddMinutes((ndt.Minute % 10 - 7) * -1);
                    else ndt.AddMinutes((ndt.Minute % 10 - 7));
                }
                else
                {
                    if (ndt.Minute % 10 >= 0 && ndt.Minute % 10 < 5)
                        ndt.AddMinutes((ndt.Minute % 10 - 0) * -1);
                    else if (ndt.Minute % 10 >= 5)
                        ndt.AddMinutes((ndt.Minute % 10 - 5) * -1);
                    else ndt.AddMinutes((ndt.Minute % 10 - 5));
                }
                begintime = ndt;
                if (CurrentGame == 0)
                {
                    if (begintime.AddMinutes(5).Hour >= 0 && begintime.AddMinutes(5).Hour < 9)
                    {
                        if (!_loop)
                        {
                            Started = false;
                            MsgEnd();
                        }
                        else
                        {
                            if (begintime.AddMinutes(-5).Hour >= 4 && begintime.AddMinutes(-5).Hour < 13)
                            {
                                Started = false;
                                MsgEnd();
                            }
                            else
                            {
                                CurrentGame = 1;
                                GameStart();
                            }
                                
                        }
                    }
                    else
                    {
                        SendStart();
                    }
                }
                else if (CurrentGame == 1)
                {
                    if (begintime.AddMinutes(-5).Hour >= 4 && begintime.AddMinutes(-5).Hour < 13)
                    {
                        if (!_loop)
                        {
                            Started = false;
                            MsgEnd();
                        }
                        else
                        {
                            if (begintime.AddMinutes(5).Hour >= 0 && begintime.AddMinutes(5).Hour < 9)
                            {
                                Started = false;
                                MsgEnd();
                            }
                            else
                            {
                                CurrentGame = 0;
                                GameStart();
                            }

                        }
                    }
                    else
                    {
                        SendStart();
                    }
                }
            }
        }

        private void GamePause()
        {
            _bSendA = false;
            _bSendB = false;
            _bSendC = false;
            _bSendD = false;
            _bSendX = false;
            _bSendXX = false;
            _lock = false;
            _InEnd = false;
            _waitbegin = false;
            if (_Timer != null)
            {
                _Timer.Stop();
                _Timer = null;
            }
        }

        private void TimerFunc(object o, EventArgs e)
        {
            try
            {
                if (_InEnd || !_Started) return;
                if (_waitbegin)
                {
                    if (CurrentGame == 0)
                    {
                        pk10data data = Cpk10.Instance().GetLast();
                        if (!_Started) return;
                        if (data.num == num || int.Parse(data.num) > int.Parse(num))
                        {
                            _waitbegin = false;
                            _InEnd = true;
                            Cpk10.Instance().list.Insert(0, data);
                            num = (int.Parse(num) + 1).ToString();
                            Cpk10.Instance().curnum = num;
                            DataChangedFunc();
                            if (begintime.AddMinutes(5).Hour >= 0 && begintime.AddMinutes(5).Hour < 9)
                            {
                                if (!_loop)
                                {
                                    Started = false;
                                    MsgEnd();
                                }
                                else
                                {
                                    if (begintime.AddMinutes(-5).Hour >= 4 && begintime.AddMinutes(-5).Hour < 13)
                                    {
                                        Started = false;
                                        MsgEnd();
                                    }
                                    else
                                    {
                                        MsgAll("今日北京赛车结束，夜场幸运飞艇马上开始！");
                                        //_waitbegin = true;
                                        CurrentGame = 1;
                                        GameStart();
                                    }

                                }
                            }
                            else
                            {
                                NormalRun();
                            }

                            _InEnd = false;
                        }
                    }
                    else if (CurrentGame == 1)
                    {
                        pk10data data = feiting.Instance().GetLast();
                        if (!_Started) return;
                        if (data.num == num || long.Parse(data.num) > long.Parse(num))
                        {
                            _waitbegin = false;
                            _InEnd = true;
                            feiting.Instance().list.Insert(0, data);
                            num = (long.Parse(num) + 1).ToString();
                            feiting.Instance().curnum = num;
                            DataChangedFunc();
                            if (begintime.AddMinutes(-5).Hour >= 4 && begintime.AddMinutes(-5).Hour < 13)
                            {
                                if (!_loop)
                                {
                                    Started = false;
                                    MsgEnd();
                                }
                                else
                                {
                                    if (begintime.AddMinutes(5).Hour >= 0 && begintime.AddMinutes(5).Hour < 9)
                                    {
                                        Started = false;
                                        MsgEnd();
                                    }
                                    else
                                    {
                                        MsgEnd();
                                        //_waitbegin = true;
                                        CurrentGame = 0;
                                        GameStart();
                                    }

                                }
                            }
                            else
                            {
                                NormalRun();
                            }
                            _InEnd = false;
                        }
                    }

                    return;
                }

                long n = DateTime.Now.Ticks / 10000000;
                long ee = begintime.AddMinutes(5).Ticks / 10000000;
                if (SetMgr.Instance().kaiguan_ziding1 && ee - n < SetMgr.Instance().time_ziding1 && !_bSendA)
                {
                    _bSendA = true;
                    MsgAll(SetMgr.Instance().自定义1);
                }
                if (SetMgr.Instance().kaiguan_ziding1 && ee - n < SetMgr.Instance().time_ziding1 && !_bSendA)
                {
                    _bSendB = true;
                    MsgAll(SetMgr.Instance().自定义2);
                }
                if (SetMgr.Instance().kaiguan_ziding1 && ee - n < SetMgr.Instance().time_ziding1 && !_bSendA)
                {
                    _bSendC = true;
                    MsgAll(SetMgr.Instance().自定义3);
                }
                if (SetMgr.Instance().kaiguan_ziding1 && ee - n < SetMgr.Instance().time_ziding1 && !_bSendA)
                {
                    _bSendD = true;
                    MsgAll(SetMgr.Instance().自定义4);
                }
                if (TimerChangeFunc != null) TimerChangeFunc(_bSendXX ? "已经封盘" : (SetMgr.Instance().封盘时间 - ee + n).ToString() + "秒", (ee - n) > 0 ? (ee - n).ToString() + "秒" : "0" + "秒");
                if (ee - n < SetMgr.Instance().接近封盘时间 + SetMgr.Instance().封盘时间 && !_bSendXX)
                {
                    _bSendXX = true;
                    MsgAll(SetMgr.Instance().接近封盘消息);
                }

                
                if (ee - n < SetMgr.Instance().封盘时间 && !_bSendX)
                {
                    _bSendX = true;
                    string m = "";
                    List<atkMsg> ls = new List<atkMsg>();
                    foreach (atkMsg am in atkList)
                    {
                        atkMsg tmp = null;
                        foreach (atkMsg tt in ls)
                        {
                            if (tt.player == am.player)
                            {
                                tmp = tt;
                                break;
                            }
                        }
                        if (tmp == null)
                        {
                            tmp = new atkMsg();
                            tmp.player = am.player;
                            tmp.content = am.content;
                            ls.Add(tmp);
                        }
                        else
                        {
                            tmp.content += "#" + am.content;
                        }
                    }
                    foreach (atkMsg tt in ls)
                    {
                        Player player = tt.player;
                        int nalluse = 0;
                        string all = "";
                        foreach (atkMsg a in atkList)
                        {
                            if (a.player == player)
                            {
                                all += a.content;
                                all += "#";
                                nalluse += a.allused;
                            }
                        }
                        char[] carr = all.ToCharArray();
                        all = new string(carr);
                        string sz = buildAt(player) + SetMgr.Instance().游戏成功回复.Replace("{期号}", num).
                                                                            Replace("{多行玩法明细}", all).
                                                                            Replace("{使用分数}", nalluse.ToString()).
                                                                            Replace("{剩余分数}", player.left.ToString());
                        m += sz + "\n";
                    }


                    string s = SetMgr.Instance().封盘消息.Replace("{核对}", m);
                    _lock = true;
                    MsgAll(s);
                }
                if (CurrentGame == 0)
                {
                    pk10data data = Cpk10.Instance().GetLast();
                    if (!_Started) return;
                    if (data.num == num || int.Parse(data.num) > int.Parse(num))
                    {
                        _InEnd = true;
                        Cpk10.Instance().list.Insert(0, data);
                        num = (int.Parse(num) + 1).ToString();
                        Cpk10.Instance().curnum = num;
                        SendEnd();
                        _InEnd = false;
                    }
                }
                else if (CurrentGame == 1)
                {
                    pk10data data = feiting.Instance().GetLast();
                    if (!_Started) return;
                    if (data.num == num || long.Parse(data.num) > long.Parse(num))
                    {
                        _InEnd = true;
                        feiting.Instance().list.Insert(0, data);
                        num = (long.Parse(num) + 1).ToString();
                        feiting.Instance().curnum = num;
                        SendEnd();
                        _InEnd = false;
                    }
                }
            }
            catch(Exception _e)
            {
                MessageBox.Show(_e.ToString());
            }

        }

        private void SendStart()
        {
        					lstCurPlayer.Clear();
            atkList.Clear();
            if (_Timer != null)
            {
                _Timer.Stop();
                _Timer = null;
            }
            _Timer = new System.Timers.Timer(1000);
            _Timer.Elapsed += TimerFunc;
            _Timer.Start();
            _bSendA = false;
            _bSendB = false;
            _bSendC = false;
            _bSendD = false;
            _bSendX = false;
            _bSendXX = false;
            _lock = false;
            int nTotal = 0;
            string strZhang = "";
            foreach (Player p in Players)
            {
                nTotal += (int)p.left;
                strZhang += "★" + p.nickname + ":" + p.left.ToString() + "\n";
            }
            string s = SetMgr.Instance().开盘消息.Replace("{玩家数量}", Players.Count.ToString()).
             Replace("{总分}", nTotal.ToString()).Replace("{期号}", num).Replace("{游戏}",
             CurrentGame == 0 ? "北京赛车" : CurrentGame == 1 ? "幸运飞艇" : CurrentGame.ToString()
             ).
             Replace("{账单}", strZhang);
            if (!_waitbegin)
                MsgAll(s);
        }

        private string lastover = "";
        private void SendEnd()
        {
            if (_Timer != null)
            {
                _Timer.Stop();
                _Timer = null;
            }
            string sover = SetMgr.Instance().结算消息;
            string zhong = "";
            lastturn = 0;
            foreach(Player p in Players)
            {
                p.perscore = 0;
            }
            foreach (atkMsg aa in atkList) {
                totalget += aa.allused;
                lastturn += aa.allused;
                aa.player.perscore -= aa.allused;
            }

            List<result> rs = CurrentGame == 0 ? Cpk10.Instance().Over(this.atkList) : feiting.Instance().Over(this.atkList);
            
            string ssnum= CurrentGame == 0 ? Cpk10.Instance().list[0].num : feiting.Instance().list[0].num;
            string ssdata= CurrentGame == 0 ? Cpk10.Instance().list[0].data : feiting.Instance().list[0].data;
            string sstime = CurrentGame == 0 ? Cpk10.Instance().list[0].time : feiting.Instance().list[0].time;
            foreach (result rrs in rs)
            {
                zhong += SetMgr.Instance().结算格式.Replace("{玩家}", (rrs.player.me == null ? rrs.player.nickname : rrs.player.me.ONickName)).Replace("{中奖内容}", rrs.sContent).Replace("{得分}", ((int)rrs.nScore).ToString()) + "\n";
                rrs.player.left += (int)rrs.nScore;
                rrs.player.perscore += (int)rrs.nScore;
                totalget -= (int)rrs.nScore;
                lastturn -= (int)rrs.nScore;
                rrs.player.up();
                //Contant.InsertLog2(rrs.player, "玩家中奖:" + rrs.sContent + " 分数:" + rrs.nScore.ToString() + " 期数:" + sstime + " " + ssnum);
                Contant.Log(rrs.player,"结算",rrs.nScore,CurrentGame.ToString(),ssnum,ssdata);
            }
            sover = sover.Replace("{中奖名单}", zhong);
            atkList.Clear();
            AtkChange();
            int nTotal = 0;
            string strZhang = "";
            foreach (Player p in Players)
            {
                if (p.me == null) continue;
                nTotal += (int)p.left;
                strZhang += "★" + p.me._nickName + ":" + p.left.ToString() + "\n";
                p.usedscore = 0;
            }
            sover = sover.Replace("{玩家数量}", Players.Count.ToString()).Replace("{账单}", strZhang);
            MsgAll(sover);
            lastover = sover;
            if (CurrentGame == 0)
            {
                MsgImg(Cpk10.Instance().getImg(Cpk10.Instance().list[0]));
                MsgImg(Cpk10.Instance().getImg2());
            }
            else if (CurrentGame == 1)
            {
                MsgImg(feiting.Instance().getImg(feiting.Instance().list[0]));
                MsgImg(feiting.Instance().getImg2());
            }
            DataChangedFunc();
            DateTime dt;
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd HH/mm/ss";
            dt = Convert.ToDateTime(CurrentGame == 0 ? Cpk10.Instance().list[0].time : feiting.Instance().list[0].time, dtFormat);

            DateTime ndt = DateTime.Now;
            if (CurrentGame == 0)
            {
                if (ndt.Minute % 10 >= 2 && ndt.Minute % 10 < 7)
                    ndt.AddMinutes((ndt.Minute % 10 - 2)*-1);
                else if (ndt.Minute % 10 >= 7)
                    ndt.AddMinutes((ndt.Minute % 10 - 7)*-1);
                else ndt.AddMinutes((ndt.Minute % 10 - 7));
            } else
            {
                if (ndt.Minute % 10 >= 0 && ndt.Minute % 10 < 5)
                    ndt.AddMinutes((ndt.Minute % 10 - 0) * -1);
                else if (ndt.Minute % 10 >= 5)
                    ndt.AddMinutes((ndt.Minute % 10 - 5) * -1);
                else ndt.AddMinutes((ndt.Minute % 10 - 5));
            }
            begintime = ndt;
            _bSendA = false;
            _bSendB = false;
            _bSendC = false;
            _bSendD = false;
            _bSendX = false;
            _bSendXX = false;
            _waitbegin = false;
            if (CurrentGame == 0)
            {
                if (begintime.AddMinutes(5).Hour >= 0 && begintime.AddMinutes(5).Hour < 9)
                {
                    if (!_loop)
                    {
                        Started = false;
                        MsgEnd();
                    }
                    else
                    {
                        if (begintime.AddMinutes(-5).Hour >= 4 && begintime.AddMinutes(-5).Hour < 13)
                        {
                            Started = false;
                            MsgEnd();
                        }
                        else
                        {
                            MsgAll("今日北京赛车结束，夜场幸运飞艇马上开始！");
                            //_waitbegin = true;
                            CurrentGame = 1;
                            GameStart();
                        }

                    }
                }
                else
                {
                    SendStart();
                }
            }
            else if (CurrentGame == 1)
            {
                if (begintime.AddMinutes(-2).Hour >= 4 && begintime.AddMinutes(-2).Hour < 13)
                {
                    if (!_loop)
                    {
                        Started = false;
                        MsgEnd();
                    }
                    else
                    {
                        if (begintime.AddMinutes(5).Hour >= 0 && begintime.AddMinutes(5).Hour < 9)
                        {
                            Started = false;
                            MsgEnd();
                        }
                        else
                        {
                            MsgEnd();
                            //_waitbegin = true;
                            CurrentGame = 0;
                            GameStart();
                        }

                    }
                }
                else
                {
                    SendStart();
                }
            }

        }

        public void NormalRun() {
            if (_Timer != null)
            {
                _Timer.Stop();
                _Timer = null;
            }
            atkList.Clear();
            DateTime dt;
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd HH/mm/ss";
            dt = Convert.ToDateTime(CurrentGame == 0 ? Cpk10.Instance().list[0].time : feiting.Instance().list[0].time, dtFormat);

            DateTime ndt = DateTime.Now;
            if (CurrentGame == 0)
            {
                if (ndt.Minute % 10 >= 2 && ndt.Minute % 10 < 7)
                    ndt.AddMinutes((ndt.Minute % 10 - 2) * -1);
                else if (ndt.Minute % 10 >= 7)
                    ndt.AddMinutes((ndt.Minute % 10 - 7) * -1);
                else ndt.AddMinutes((ndt.Minute % 10 - 7));
            }
            else
            {
                if (ndt.Minute % 10 >= 0 && ndt.Minute % 10 < 5)
                    ndt.AddMinutes((ndt.Minute % 10 - 0) * -1);
                else if (ndt.Minute % 10 >= 5)
                    ndt.AddMinutes((ndt.Minute % 10 - 5) * -1);
                else ndt.AddMinutes((ndt.Minute % 10 - 5));
            }
            begintime = ndt;
            _bSendA = false;
            _bSendB = false;
            _bSendC = false;
            _bSendD = false;
            _bSendX = false;
            _bSendXX = false;
            _waitbegin = false;
            if (CurrentGame == 0)
            {
                if (begintime.AddMinutes(5).Hour >= 0 && begintime.AddMinutes(5).Hour < 9)
                {
                    if (!_loop)
                    {
                        Started = false;
                        MsgEnd();
                    }
                        
                    else
                    {
                        if (begintime.AddMinutes(-5).Hour >= 4 && begintime.AddMinutes(-5).Hour < 13)
                        {
                            Started = false;
                            MsgEnd();
                        }
                        else
                        {
                            CurrentGame = 1;
                            GameStart();
                        }

                    }
                }
                else
                {
                    SendStart();
                }
            }
            else if (CurrentGame == 1)
            {
                if (begintime.AddMinutes(-5).Hour >= 4 && begintime.AddMinutes(-5).Hour < 13)
                {
                    if (!_loop)
                    {
                        Started = false;
                        MsgEnd();
                    }
                        
                    else
                    {
                        if (begintime.AddMinutes(5).Hour >= 0 && begintime.AddMinutes(5).Hour < 9)
                        {
                            Started = false;
                            MsgEnd();
                        }
                        else
                        {
                            CurrentGame = 0;
                            GameStart();
                        }

                    }
                }
                else
                {
                    SendStart();
                }
            }
        }

        private List<Player> _player = new List<Player>();
        public List<Player> Players
        {
            get
            {
                return _player;
            }
            set
            {
                _player = value;
            }
        }

        public List<waitLiang> waitList = new List<waitLiang>();
        public List<atkMsg> atkList = new List<atkMsg>();

        public int CurrentGame = 0; // 0 pk10

        public void OnMsg(WXMsg msg)
        {
            if (mGroup == null) return;
            if (msg.From == mGroup._me.UserName)
            {
                int nFirst = msg.Msg.IndexOf("br/>");
                if (nFirst >= 0)
                {
                    msg.To = msg.Msg.Substring(0, nFirst - 2);
                    msg.Msg = msg.Msg.Substring(msg.Msg.IndexOf("br/>") + 4, msg.Msg.Length - msg.Msg.IndexOf("br/>") - 4);
                }

                Player user = null;
                foreach (Player tmp in _player)
                {
                    if (tmp.me!=null&&tmp.me.UserName == msg.To)
                    {
                        user = tmp;
                        break;
                    }
                }

                

                string[] lsSLKey = SetMgr.Instance().key_cha.Split(' ');
                string[] lsXLKey = SetMgr.Instance().key_hui.Split(' ');
                string[] lsXGKey = SetMgr.Instance().key_edit.Split(' ');
                string[] lsSCKey = SetMgr.Instance().key_cancel.Split(' ');
                string[] lsCKKey = SetMgr.Instance().key_check.Split(' ');

                string[] lsAKey = SetMgr.Instance().S1_1.Split(' ');


                
                foreach (string key in lsCKKey)
                {
                    if (msg.Msg.IndexOf(key) >= 0)
                    {
                        if (user == null) return;
                        ChaKan(msg.Msg, key, user);
                        return;
                    }
                }
                foreach (string key in lsSLKey)
                {
                    if (msg.Msg.IndexOf(key) >= 0)
                    {
                        if (user == null)
                        {
                            WXUser cur = null;
                            for (int x = 0; x < GameMgr.Instance().mGroup._list.Count; ++x)
                            {
                                WXUser usera = (WXUser)GameMgr.Instance().mGroup._list[x];
                                if (usera.UserName == msg.To)
                                {
                                    cur = usera;
                                    break;
                                }
                            }

                            if (cur == null) return;

                            Player player = new Player();
                            player.playername = cur.NickName;
                            player.nickname = cur.NickName;
                            player.type = 0;
                            player.left = 0;
                            player.num = "";
                            player.content = "";
                            player.usedscore = 0;
                            player.perscore = 0;
                            player.me = cur;

                            user = player;
                        }


                        ShangLiang(msg.ID, msg.Msg, key, user);
                        return;
                    }
                }
                if (user == null) return;
                foreach (string key in lsXLKey)
                {
                    if (msg.Msg.IndexOf(key) >= 0)
                    {
                        XiaLiang(msg.ID, msg.Msg, key, user);
                        return;
                    }
                }
                foreach (string key in lsXGKey)
                {
                    if (msg.Msg.IndexOf(key) >= 0)
                    {
                        XiuGai(msg.Msg, key, user);
                        return;
                    }
                }
                foreach (string key in lsSCKey)
                {
                    if (msg.Msg.IndexOf(key) >= 0)
                    {
                        ShanChu(msg.Msg, key, user);
                        return;
                    }
                }
        

                if (!Started || _lock || _waitbegin)
                {
                    ErrorDiy(GAMEINEND, user);
                    return;
                }
                foreach (string key in lsAKey)
                {
                    if (msg.Msg.Split(key == "空格" ? ' ' : key[0]).Length >= 1)
                    {
                        Atk(msg.ID, msg.Msg, key, user);
                        return;
                    }
                }

                if (msg.Type == 10002)
                {
                    foreach (atkMsg a in atkList)
                    {
                        if (a.id == msg.ID)
                        {
                            atkList.Remove(a);
                            AtkChange();
                            return;
                        }
                    }
                    return;
                } else
                {
                    ErrorMsg(user);
                }

                
                return;
            }
        }

        void ShangLiang(string id, string msg, string key, Player player)
        {
            string strMsg = msg.Substring(msg.IndexOf(key) + key.Length);
            int nResult = 0;

            if (!int.TryParse(strMsg, out nResult))
            {
                ErrorMsg(player);
                return;
            }

            if (SetMgr.Instance().receivechahui || Started)
            {
                if (player.type == 1 && SetMgr.Instance().low)
                {
                    player.left += nResult;
                    player.up();
                    AtkChange();
                    string s = buildAt(player) + SetMgr.Instance().查分回复.Replace("{当期使用分数}", player.usedscore.ToString()).Replace("{剩余分数}", player.left.ToString());
                    sendMsg(s, player);
                }
                else
                {
                    waitLiang wl = new waitLiang();
                    wl.player = player;
                    wl.nValue = nResult;
                    wl.id = id;
                    waitList.Add(wl);
                    WaitChange();
                    string s = buildAt(player) + "收到上粮请求";
                    sendMsg(s, player);

                    Contant.InsertLog2(player, "上粮请求" + nResult);
                }
            }
            else
            {
                ErrorDiy(GAMEINEND, player);
            }
        }
        void XiaLiang(string id, string msg, string key, Player player)
        {
            string strMsg = msg.Substring(msg.IndexOf(key) + key.Length);
            int nResult = 0;

            if (!int.TryParse(strMsg, out nResult))
            {
                ErrorMsg(player);
                return;
            }

            if (SetMgr.Instance().receivechahui || Started)
            {
                if (player.left < nResult)
                {
                    ErrorDiy("你压根没有那么多粮草好不好。", player);
                    return;
                }
                if (player.type == 1 && SetMgr.Instance().low)
                {
                    player.left -= nResult;
                    player.up();
                    AtkChange();
                    string s = buildAt(player) + SetMgr.Instance().查分回复.Replace("{当期使用分数}", player.usedscore.ToString()).Replace("{剩余分数}", player.left.ToString());
                    sendMsg(s, player);
                }
                else
                {
                    waitLiang wl = new waitLiang();
                    wl.player = player;
                    wl.nValue = nResult * -1;
                    wl.id = id;
                    waitList.Add(wl);
                    WaitChange();
                    string s = buildAt(player) + "收到下粮请求";
                    sendMsg(s, player);

                    Contant.InsertLog2(player, "下粮请求" + nResult);
                }
            }
            else
            {
                ErrorDiy(GAMEINEND, player);
            }
        }
        void XiuGai(string msg, string key, Player player)
        {
            sendMsg("暂不支持修改命令", player);
        }
        void ShanChu(string msg, string key, Player player)
        {
            atkMsg lastatk = null;
            foreach (atkMsg a in atkList)
            {
                if (a.player == player)
                {
                    lastatk = a;
                }
            }

            if (lastatk != null) {
                atkList.Remove(lastatk);
                sendMsg("最后发送的攻击消息已取消。", player);

                //Contant.InsertLog2(player, "删除最后攻击");
            }
        }
        void ChaKan(string msg, string key, Player player)
        {
            string s = buildAt(player) + SetMgr.Instance().查分回复.Replace("{当期使用分数}", player.usedscore.ToString()).Replace("{剩余分数}", player.left.ToString());
            sendMsg(s, player);
            return;
        }
        void Atk(string id, string msg, string key, Player player)
        {
            string[] atks = msg.Split(key[0]);
            if (Started)
            {
                msg.Replace(key, SetMgr.Instance().S1_2);
                atkMsg wl = new atkMsg();
                wl.player = player;

                wl.id = id;
                string[] lsAKey = SetMgr.Instance().S2_1.Split(' ');
                string[] lsBKey = SetMgr.Instance().S3_1.Split(' ');
                for (int i = 0; i < atks.Length; ++i)
                {
                    bool get = false;
                    bool nonon = true;
                    foreach (string s1 in lsAKey)
                    {
                        if (atks[i].IndexOf(s1 == "空格" ? ' ' : s1[0]) >= 0)
                        {
                            nonon = false;
                            break;
                        }
                    }
                    foreach (string s1 in lsAKey)
                    {
                        string[] sub = atks[i].Split(s1 == "空格" ? ' ' : s1[0]);
                        if (sub.Length == 3)
                        {
                            msg.Replace(s1, SetMgr.Instance().S2_2);
                            string[] subA = null;
                            foreach (string s2 in lsBKey)
                            {
                                subA = sub[0].Split(s2 == "空格" ? ' ' : s2[0]);
                                if (subA.Length > 1)
                                {
                                    msg.Replace(s2, SetMgr.Instance().S3_2);
                                    break;
                                }
                            }
                            if (subA == null)
                            {
                                subA = new string[1];
                                subA[0] = sub[0];
                            }
                            foreach (string m in subA)
                            {
                                for (int x = 0; x < m.Length; ++x)
                                {
                                    int xxxxx = 0;
                                    if (int.TryParse(m[x].ToString(), out xxxxx))
                                    {
                                        if (xxxxx < 0 || xxxxx > 10) continue;
                                        wl.main.Add(xxxxx == 0 ? "10" : m[x].ToString());
                                    }
                                    else if (m[x].ToString() == "和" || m[x].ToString() == "合" || m[x].ToString() == "特")
                                    {
                                        wl.main.Add("冠亚");
                                    } else
                                    {
                                        continue;
                                    }

                                }
                            }
                            if (wl.main.Count == 0) {
                                ErrorMsg(player);
                                return;                                
                            }
                            for (int x = 0; x < sub[1].Length; ++x)
                            {
                                char[] ayC = { '大', '小', '单', '双', '龙', '虎', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
                                bool bggg = false;
                                for (int v = 0; v < ayC.Length; ++v) if (ayC[v] == sub[1][x]) bggg = true;
                                if (!bggg) continue;
                                wl.coin.Add(sub[1][x].ToString());
                            }
                            if (wl.coin.Count == 0) {
                                ErrorMsg(player);
                                return;                                
                            }
                            int nAdd = 0;
                            if (!int.TryParse(sub[2], out nAdd))
                            {
                                ErrorMsg(player);
                                return;
                            }
                            wl.used = nAdd;
                            wl.allused = nAdd * wl.main.Count * wl.coin.Count;
                            get = true;
                            break;
                        }
                        else if (sub.Length == 2)
                        {
                            msg.Replace(s1, SetMgr.Instance().S2_2);
                            bool bGuanYa = false;
                            if (sub[0].Length > 2 && sub[0].Substring(0, 2) == "冠亚")
                            {
                                bGuanYa = true;
                                wl.main.Add("冠亚");
                                sub[0] = sub[0].Substring(2, sub[0].Length - 2);
                            }
                            else if (sub[0].Substring(0, 1) == "和" || sub[0].Substring(0, 1) == "合" || sub[0].Substring(0, 1) == "特")
                            {
                                bGuanYa = true;
                                wl.main.Add("冠亚");
                                sub[0] = sub[0].Substring(1, sub[0].Length - 1);
                            }
                            else wl.main.Add("1");
                            if (wl.main.Count == 0) {
                                ErrorMsg(player);
                                return;                                
                            }
                            for (int x = 0; x < sub[0].Length; ++x)
                            {
                                if (sub[0][x] == ',') continue;
                                if (bGuanYa && x < sub[0].Length - 1 && (sub[0][x].ToString() == "1" || sub[0][x].ToString() == "2"))
                                {
                                    string ii = sub[0][x].ToString() + sub[0][x + 1].ToString();
                                    int _sdio = 0;
                                    if (!int.TryParse(ii, out _sdio))
                                    {
                                        ErrorMsg(player);
                                        return;
                                    }
                                    wl.coin.Add(ii);
                                    x += 1;
                                }
                                else
                                    wl.coin.Add(sub[0][x].ToString());
                            }
                            if (wl.coin.Count == 0) {
                                ErrorMsg(player);
                                return;                                
                            }
                            int nAdd = 0;
                            if (!int.TryParse(sub[1], out nAdd))
                            {
                                continue;
                            }
                            wl.used = nAdd;
                            wl.allused += nAdd * wl.main.Count * wl.coin.Count;
                            get = true;
                            break;
                        }
                        else if (sub.Length> 3)
                        {
                            ErrorMsg(player);
                            return;


                        }
                        else if (sub.Length == 1 && nonon)
                        {
                            char[] ayC = { '大', '小', '单', '双', '龙', '虎' };
                            if (sub[0].IndexOfAny(ayC) >= 0)
                            {
                                int nA = sub[0].IndexOfAny(ayC);
                                int nE = sub[0].LastIndexOfAny(ayC);
                                for (int x = 0; x < nA; ++x)
                                {
                                    int xxxxx = 0;
                                    if (int.TryParse(sub[0][x].ToString(), out xxxxx))
                                    {
                                        if (xxxxx < 0 || xxxxx > 10) continue;
                                        wl.main.Add(xxxxx == 0? "10":sub[0][x].ToString());
                                    } else
                                    {
                                        continue;
                                    }
                                    
                                }
                                if (wl.main.Count == 0) wl.main.Add(1.ToString());
                                if (nE == sub[0].Length - 1)
                                {
                                    continue;
                                }
                                for (int x = nA; x <= nE; ++x)
                                {
                                    bool bggg = false;
                                    for (int v = 0; v < ayC.Length; ++v) if (ayC[v] == sub[0][x]) bggg = true;
                                    if (!bggg) continue;
                                    wl.coin.Add(sub[0][x].ToString());
                                }
                                string scoint = sub[0].Substring(nE + 1, sub[0].Length - nE - 1);
                                int nAdd = 0;
                                if (!int.TryParse(scoint, out nAdd))
                                {
                                    continue;
                                }
                                wl.used = nAdd;
                                wl.allused += nAdd * wl.main.Count * wl.coin.Count;
                                get = true;
                            }
                            break;
                        }
                    }
                    if (!get)
                    {
                        ErrorMsg(player);
                        return;
                    }
                }
                foreach (string sm in wl.main)
                {
                    if (sm != "冠亚")
                    {
                        if (int.Parse(sm) < 1 || int.Parse(sm) > 10) continue;
                    }


                    int nSum = 0;
                    float peilv = 0.0f;
                    float zuidi = 0;
                    float zuigao = 0;
                    float li = 0;
                    foreach (string ss in wl.coin)
                    {
                        bool bWin = false;
                        int num = 0;
                        if (ss == "大")
                        {
                            if (sm == "冠亚")
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚大'");
                                if (dt.Rows.Count > 0)
                                {
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                }
                                bWin = nSum > 11;
                                if (wl.used < zuidi)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[最低携带积分限量{1}]", "冠亚大", zuidi), player);
                                    return;
                                }
                                else if (wl.used > zuigao)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]高于:[最高携带积分限量{1}]", "冠亚大", zuigao), player);
                                    return;
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名大'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                bWin = nSum > 5;
                                if (wl.used < zuidi)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[最低携带积分限量{1}]", "第" + int.Parse(sm) + "名大", zuidi), player);
                                    return;
                                }
                                else if (wl.used > zuigao)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]高于:[最高携带积分限量{1}]", "第" + int.Parse(sm) + "名大", zuigao), player);
                                    return;
                                }
                            }
                        }
                        else if (ss == "小")
                        {
                            if (sm == "冠亚")
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚小'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                bWin = nSum <= 11;
                                if (wl.used < zuidi)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[最低携带积分限量{1}]", "冠亚小", zuidi), player);
                                    return;
                                }
                                else if (wl.used > zuigao)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]高于:[最高携带积分限量{1}]", "冠亚小", zuigao), player);
                                    return;
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名小'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                bWin = nSum <= 5;
                                if (wl.used < zuidi)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[最低携带积分限量{1}]", "第" + int.Parse(sm) + "名小", zuidi), player);
                                    return;
                                }
                                else if (wl.used > zuigao)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]高于:[最高携带积分限量{1}]", "第" + int.Parse(sm) + "名小", zuigao), player);
                                    return;
                                }
                            }
                        }
                        else if (ss == "单")
                        {
                            bWin = (nSum % 2 == 1);
                            if (sm == "冠亚")
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }

                                if (wl.used < zuidi)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[最低携带积分限量{1}]", "冠亚" + ss, zuidi), player);
                                    return;
                                }
                                else if (wl.used > zuigao)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]高于:[最高携带积分限量{1}]", "冠亚" + ss, zuigao), player);
                                    return;
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }

                                if (wl.used < zuidi)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[最低携带积分限量{1}]", "第" + int.Parse(sm) + "名" + ss, zuidi), player);
                                    return;
                                }
                                else if (wl.used > zuigao)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]高于:[最高携带积分限量{1}]", "第" + int.Parse(sm) + "名" + ss, zuigao), player);
                                    return;
                                }
                            }
                        }
                        else if (ss == "双")
                        {
                            bWin = (nSum % 2 == 0);
                            if (sm == "冠亚")
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                if (wl.used < zuidi)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[最低携带积分限量{1}]", "冠亚" + ss, zuidi), player);
                                    return;
                                }
                                else if (wl.used > zuigao)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]高于:[最高携带积分限量{1}]", "冠亚" + ss, zuigao), player);
                                    return;
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                if (wl.used < zuidi)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[最低携带积分限量{1}]", "第" + int.Parse(sm) + "名" + ss, zuidi), player);
                                    return;
                                }
                                else if (wl.used > zuigao)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]高于:[最高携带积分限量{1}]", "第" + int.Parse(sm) + "名" + ss, zuigao), player);
                                    return;
                                }
                            }
                        }
                        else if (ss == "龙")
                        {
                            int iindex = 10 - int.Parse(sm);
                            if (sm == "冠亚")
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                if (wl.used < zuidi)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[最低携带积分限量{1}]", "冠亚" + ss, zuidi), player);
                                    return;
                                }
                                else if (wl.used > zuigao)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]高于:[最高携带积分限量{1}]", "冠亚" + ss, zuigao), player);
                                    return;
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                if (wl.used < zuidi)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[最低携带积分限量{1}]", "第" + int.Parse(sm) + "名" + ss, zuidi), player);
                                    return;
                                }
                                else if (wl.used > zuigao)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]高于:[最高携带积分限量{1}]", "第" + int.Parse(sm) + "名" + ss, zuigao), player);
                                    return;
                                }
                            }
                        }
                        else if (ss == "虎")
                        {
                            int iindex = 10 - int.Parse(sm);
                            if (sm == "冠亚")
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                if (wl.used < zuidi)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[最低携带积分限量{1}]", "冠亚" + ss, zuidi), player);
                                    return;
                                }
                                else if (wl.used > zuigao)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]高于:[最高携带积分限量{1}]", "冠亚" + ss, zuigao), player);
                                    return;
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                if (wl.used < zuidi)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[最低携带积分限量{1}]", "第" + int.Parse(sm) + "名" + ss, zuidi), player);
                                    return;
                                }
                                else if (wl.used > zuigao)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]高于:[最高携带积分限量{1}]", "第" + int.Parse(sm) + "名" + ss, zuigao), player);
                                    return;
                                }
                            }
                        }
                        else if (int.TryParse(ss, out num))
                        {
                            if (num == 0) num = 10;
                            if (num == nSum) bWin = true;
                            else bWin = false;

                            if (sm == "冠亚")
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + num + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                if (wl.used < zuidi)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[最低携带积分限量{1}]", "冠亚" + num, zuidi), player);
                                    return;
                                }
                                else if (wl.used > zuigao)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]高于:[最高携带积分限量{1}]", "冠亚" + num, zuigao), player);
                                    return;
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + num + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                if (wl.used < zuidi)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[最低携带积分限量{1}]", "第" + int.Parse(sm) + "名" + num, zuidi), player);
                                    return;
                                }
                                else if (wl.used > zuigao)
                                {
                                    ErrorDiy(String.Format("[失败]玩法内容:[{0}]高于:[最高携带积分限量{1}]", "第" + int.Parse(sm) + "名" + num, zuigao), player);
                                    return;
                                }
                            }
                        }
                    }
                }

                Dictionary<string, int> sumAll = new Dictionary<string, int>();
                foreach (atkMsg atk in atkList)
                {
                    foreach (string sm in atk.main)
                    {
                        if (sm != "冠亚")
                        {
                            if (int.Parse(sm) < 1 || int.Parse(sm) > 10) continue;
                        }


                        int nSum = 0;
                        float peilv = 0.0f;
                        float zuidi = 0;
                        float zuigao = 0;
                        float li = 0;
                        foreach (string ss in atk.coin)
                        {
                            if (sumAll.ContainsKey(sm + ss))
                            {
                                sumAll[sm + ss] += atk.used;
                            }
                            else
                            {
                                sumAll.Add(sm + ss, atk.used);
                            }
                            bool bWin = false;
                            int num = 0;
                            if (ss == "大")
                            {
                                if (sm == "冠亚")
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚大'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    }
                                    bWin = nSum > 11;
                                    if (sumAll[sm + ss] > li)
                                    {
                                        ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[本期最高积分限量{1}]", "冠亚大", zuidi), player);
                                        return;
                                    }
                                }
                                else
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名大'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    bWin = nSum > 5;
                                    if (sumAll[sm + ss] > li)
                                    {
                                        ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[本期最高积分限量{1}]", "第" + int.Parse(sm) + "名大", zuidi), player);
                                        return;
                                    }
                                }
                            }
                            else if (ss == "小")
                            {
                                if (sm == "冠亚")
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚小'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    bWin = nSum <= 11;
                                    if (sumAll[sm + ss] > li)
                                    {
                                        ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[本期最高积分限量{1}]", "冠亚小", zuidi), player);
                                        return;
                                    }
                                }
                                else
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名小'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    bWin = nSum <= 5;
                                    if (sumAll[sm + ss] > li)
                                    {
                                        ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[本期最高积分限量{1}]", "第" + int.Parse(sm) + "名小", zuidi), player);
                                        return;
                                    }
                                }
                            }
                            else if (ss == "单")
                            {
                                bWin = (nSum % 2 == 1);
                                if (sm == "冠亚")
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + ss + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }

                                    if (sumAll[sm + ss] > li)
                                    {
                                        ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[本期最高积分限量{1}]", "冠亚" + ss, zuidi), player);
                                        return;
                                    }
                                }
                                else
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }

                                    if (sumAll[sm + ss] > li)
                                    {
                                        ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[本期最高积分限量{1}]", "第" + int.Parse(sm) + "名" + ss, zuidi), player);
                                        return;
                                    }
                                }
                            }
                            else if (ss == "双")
                            {
                                bWin = (nSum % 2 == 0);
                                if (sm == "冠亚")
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + ss + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    if (sumAll[sm + ss] > li)
                                    {
                                        ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[本期最高积分限量{1}]", "冠亚" + ss, zuidi), player);
                                        return;
                                    }
                                }
                                else
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    if (sumAll[sm + ss] > li)
                                    {
                                        ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[本期最高积分限量{1}]", "第" + int.Parse(sm) + "名" + ss, zuidi), player);
                                        return;
                                    }
                                }
                            }
                            else if (ss == "龙")
                            {
                                int iindex = 10 - int.Parse(sm);
                                if (sm == "冠亚")
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + ss + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    if (sumAll[sm + ss] > li)
                                    {
                                        ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[本期最高积分限量{1}]", "冠亚" + ss, zuidi), player);
                                        return;
                                    }
                                }
                                else
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    if (sumAll[sm + ss] > li)
                                    {
                                        ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[本期最高积分限量{1}]", "第" + int.Parse(sm) + "名" + ss, zuidi), player);
                                        return;
                                    }
                                }
                            }
                            else if (ss == "虎")
                            {
                                int iindex = 10 - int.Parse(sm);
                                if (sm == "冠亚")
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + ss + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    if (sumAll[sm + ss] > li)
                                    {
                                        ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[本期最高积分限量{1}]", "冠亚" + ss, zuidi), player);
                                        return;
                                    }
                                }
                                else
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    if (sumAll[sm + ss] > li)
                                    {
                                        ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[本期最高积分限量{1}]", "第" + int.Parse(sm) + "名" + ss, zuidi), player);
                                        return;
                                    }
                                }
                            }
                            else if (int.TryParse(ss, out num))
                            {
                                if (num == 0) num = 10;
                                if (num == nSum) bWin = true;
                                else bWin = false;

                                if (sm == "冠亚")
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + num + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    if (sumAll[sm + ss] > li)
                                    {
                                        ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[本期最高积分限量{1}]", "冠亚" + num, zuidi), player);
                                        return;
                                    }
                                }
                                else
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + num + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    if (sumAll[sm + ss] > li)
                                    {
                                        ErrorDiy(String.Format("[失败]玩法内容:[{0}]低于:[本期最高积分限量{1}]", "第" + int.Parse(sm) + "名" + num, zuidi), player);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                if (wl.allused > player.left)
                {
                    ErrorDiy(String.Format("[失败]携带粮草:{0},剩余粮草:{1}", wl.allused, player.left), player);
                    return;
                }
                player.usedscore += wl.allused;
                player.left -= wl.allused;
                wl.content = msg;
                Player isIn = null;
                foreach(Player pp in lstCurPlayer){
                		if(pp == player) {
                			isIn = pp;
                			break;
                			}
                }
                if(isIn == null) {
                			lstCurPlayer.Add(player);
                }
                atkList.Add(wl);
                Contant.Log(player,"进攻",wl.allused,CurrentGame.ToString(),num,"");
                //Contant.InsertLog2(player, "攻击：" + msg);
                string s = buildAt(player) + SetMgr.Instance().核对格式.Replace("{玩家}", (player.me == null ? player.nickname : player.me.ONickName)).Replace("{玩法}", msg);
                //sendMsg(s, player);
                int nalluse = 0;
                string all = "";
                foreach (atkMsg a in atkList)
                {
                    if (a.player == player)
                    {
                        all += a.content;
                        all += "#";
                        nalluse += wl.allused;
                    }
                }
                char[] carr = all.ToCharArray();
                all = new string(carr);
                s = buildAt(player) + SetMgr.Instance().游戏成功回复.Replace("{期号}", num).
                                                                    Replace("{多行玩法明细}", all).
                                                                    Replace("{使用分数}", wl.allused.ToString()).
                                                                    Replace("{剩余分数}", player.left.ToString());
                //sendMsg(s, player);
                player.num = num;
                player.content = all;
                player.up();
                AtkChange();
            }
            else
            {
                ErrorDiy(GAMEINEND, player);
            }
        }
        public void sendMsg(string s, Player to)
        {
            WXMsg msg = new WXMsg();
            msg.From = MainUser.Instance._me.UserName;
            msg.Msg = s;  //只接受文本消息
            msg.Readed = false;
            msg.Time = DateTime.Now;
            msg.To = mGroup._me.UserName;
            msg.Type = 1;
            if (MainUser.Instance.Logined)
                MainUser.Instance.SendMsg(msg);
            else if (SubUser.Instance.Logined)
            {
                msg.From = SubUser.Instance._me.UserName;
                msg.To = sGroup._me.UserName;
                SubUser.Instance.SendMsg(msg);
            }
        }
        public string buildAt(Player player)
        {
            return "@" + (player.me == null ? player.nickname : player.me.ONickName) + " ";
        }
        void ErrorMsg(Player player)
        {
            WXMsg msg = new WXMsg();
            msg.From = MainUser.Instance._me.UserName;
            msg.Msg = buildAt(player) + "命令无效，请检查后再试";  //只接受文本消息
            msg.Readed = false;
            msg.Time = DateTime.Now;
            msg.To = mGroup._me.UserName;
            msg.Type = 1;
            if (MainUser.Instance.Logined)
                MainUser.Instance.SendMsg(msg);
            else if (SubUser.Instance.Logined)
            {
                msg.From = SubUser.Instance._me.UserName;
                msg.To = sGroup._me.UserName;
                SubUser.Instance.SendMsg(msg);
            }
        }
        void ErrorDiy(string s, Player player)
        {
            WXMsg msg = new WXMsg();
            msg.From = MainUser.Instance._me.UserName;
            msg.Msg = buildAt(player) + s;  //只接受文本消息
            msg.Readed = false;
            msg.Time = DateTime.Now;
            msg.To = mGroup._me.UserName;
            msg.Type = 1;
            if (MainUser.Instance.Logined)
                MainUser.Instance.SendMsg(msg);
            else if (SubUser.Instance.Logined)
            {
                msg.From = SubUser.Instance._me.UserName;
                msg.To = sGroup._me.UserName;
                SubUser.Instance.SendMsg(msg);
            }
                
        }
        void MsgAll(string s)
        {
            WXMsg msg = new WXMsg();
            msg.From = MainUser.Instance._me.UserName;
            msg.Msg = s;  //只接受文本消息
            msg.Readed = false;
            msg.Time = DateTime.Now;
            msg.To = mGroup._me.UserName;
            msg.Type = 1;
            if (MainUser.Instance.Logined)
                MainUser.Instance.SendMsg(msg);
            else if (SubUser.Instance.Logined)
            {
                msg.From = SubUser.Instance._me.UserName;
                msg.To = sGroup._me.UserName;
                SubUser.Instance.SendMsg(msg);
            }
        }

        public void AgreeAddLiang(waitLiang m)
        {

            Player player = m.player;
            bool bFind = false;
            foreach (Player other in this.Players)
            {
                if (other == player)
                {
                    bFind = true;
                    break;
                }
            }
            if (!bFind)
            {
                this.Players.Add(player);
                AtkChange();
                player.insert();
            }
            waitList.Remove(m);
            player.left += m.nValue;
            player.up();
            WaitChange();
            AtkChange();
            string s = buildAt(player) + SetMgr.Instance().查分回复.Replace("{当期使用分数}", player.usedscore.ToString()).Replace("{剩余分数}", player.left.ToString());
            sendMsg(s, player);

            Contant.InsertLog("同意了" + player.nickname + "的改分请求。" + (m.nValue > 0 ? "+" : "") + m.nValue);
            Contant.InsertLog2(player, "同意了上粮" + m.nValue.ToString());
            Contant.Log(player,"上粮",m.nValue,"",num,"");
        }

        public void DeagreeAddLiang(waitLiang m)
        {
            Player player = m.player;
            waitList.Remove(m);
            WaitChange();

            Contant.InsertLog("拒绝了" + player.nickname + "的上分请求。" + "+" + m.nValue);
            Contant.InsertLog2(player, "拒绝了上粮" + m.nValue.ToString());
        }

        public void MsgEnd()
        {
            WXMsg msg = new WXMsg();
            msg.From = MainUser.Instance._me.UserName;
            msg.Msg = "当前回合已结束，清空当前回合攻防记录";  //只接受文本消息
            msg.Readed = false;
            msg.Time = DateTime.Now;
            msg.To = mGroup._me.UserName;
            msg.Type = 1;
            if (MainUser.Instance.Logined)
                MainUser.Instance.SendMsg(msg);
            else if (SubUser.Instance.Logined)
            {
                msg.From = SubUser.Instance._me.UserName;
                msg.To = sGroup._me.UserName;
                SubUser.Instance.SendMsg(msg);
            }

            int nTotal = 0;
            string strZhang = "";
            foreach (Player p in Players)
            {
                nTotal += (int)p.left;
                strZhang += SetMgr.Instance().账单格式.Replace("{玩家}", p.me == null ? p.nickname : p.me.ONickName).
                     Replace("{剩余}", p.left.ToString());
                strZhang += "\n";
            }
            if (!_waitbegin)
                MsgAll(strZhang);
        }

        public void MsgLiang()
        {

            int nTotal = 0;
            string strZhang = "";
            foreach (Player p in Players)
            {
                nTotal += (int)p.left;
                strZhang += SetMgr.Instance().账单格式.Replace("{玩家}", p.me == null ? p.nickname : p.me.ONickName).
                     Replace("{剩余}", p.left.ToString());
                strZhang += "\n";
            }
            if (!_waitbegin)
                MsgAll(strZhang);
        }

        public void BroBack(string back, bool b)
        {
            AtkChange();
            if (b)
                MsgAll(back);
        }

        public void MsgXiu()
        {
            /*string m = "";
            List<atkMsg> ls = new List<atkMsg>();
            foreach (atkMsg am in atkList)
            {
                atkMsg tmp = null;
                foreach (atkMsg tt in ls)
                {
                    if (tt.player == am.player)
                    {
                        tmp = tt;
                        break;
                    }
                }
                if (tmp == null)
                {
                    tmp = new atkMsg();
                    tmp.player = am.player;
                    tmp.content = am.content;
                    ls.Add(tmp);
                }
                else
                {
                    tmp.content += "#" + am.content;
                }
            }
            foreach (atkMsg tt in ls)
            {
                Player player = tt.player;
                int nalluse = 0;
                string all = "";
                foreach (atkMsg a in atkList)
                {
                    if (a.player == player)
                    {
                        all += a.content;
                        all += "#";
                        nalluse += a.allused;
                    }
                }
                char[] carr = all.ToCharArray();
                all = new string(carr);
                string sz = buildAt(player) + SetMgr.Instance().游戏成功回复.Replace("{期号}", num).
                                                                    Replace("{多行玩法明细}", all).
                                                                    Replace("{使用分数}", nalluse.ToString()).
                                                                    Replace("{剩余分数}", player.left.ToString());
                m += sz + "\n";
            }


            string s = SetMgr.Instance().封盘消息.Replace("{核对}", m);
            _lock = true;
            MsgAll(s);*/
            MsgAll(lastover);
        }

        public void MsgImg(FileStream bts)
        {
            if (MainUser.Instance.Logined)
                MainUser.Instance.SendImg(bts, mGroup._me.UserName);
            else if (SubUser.Instance.Logined)
            {
                SubUser.Instance.SendImg(bts, sGroup._me.UserName);
            }
        }

        public bool ChangeAtk(Player cur, string atk)
        {
            if (atk.Length == 0)
            {
                for(int i = 0;i < atkList.Count;++i)
                {
                    atkMsg a = atkList[i];
                    if (a.player == cur)
                    {
                        a.player.left += a.allused;
                        atkList.Remove(a);
                        i--;
                    }
                }
                Contant.Log(cur, "清空内容", 0, CurrentGame.ToString(), num, "");
                return true;
            } else
            {
                return Atk2("000", atk, "#", cur);

            }

            return true;
        }

        bool Atk2(string id, string msg, string key, Player player)
        {
            string[] atks = msg.Split(key[0]);
            if (Started)
            {
                msg.Replace(key, SetMgr.Instance().S1_2);
                atkMsg wl = new atkMsg();
                wl.player = player;

                wl.id = id;
                string[] lsAKey = SetMgr.Instance().S2_1.Split(' ');
                string[] lsBKey = SetMgr.Instance().S3_1.Split(' ');
                for (int i = 0; i < atks.Length; ++i)
                {
                    bool get = false;
                    bool nonon = true;
                    foreach (string s1 in lsAKey)
                    {
                        if (atks[i].IndexOf(s1 == "空格" ? ' ' : s1[0]) >= 0)
                        {
                            nonon = false;
                            break;
                        }
                    }
                    foreach (string s1 in lsAKey)
                    {
                        string[] sub = atks[i].Split(s1 == "空格" ? ' ' : s1[0]);
                        if (sub.Length == 3)
                        {
                            msg.Replace(s1, SetMgr.Instance().S2_2);
                            string[] subA = null;
                            foreach (string s2 in lsBKey)
                            {
                                subA = sub[0].Split(s2 == "空格" ? ' ' : s2[0]);
                                if (subA.Length > 1)
                                {
                                    msg.Replace(s2, SetMgr.Instance().S3_2);
                                    break;
                                }
                            }
                            if (subA == null)
                            {
                                subA = new string[1];
                                subA[0] = sub[0];
                            }
                            foreach (string m in subA)
                            {
                                for (int x = 0; x < m.Length; ++x)
                                {
                                    int xxxxx = 0;
                                    if (int.TryParse(m[x].ToString(), out xxxxx))
                                    {
                                        if (xxxxx < 0 || xxxxx > 10) continue;
                                        wl.main.Add(xxxxx == 0 ? "10" : m[x].ToString());
                                    }
                                    else if (m[x].ToString() == "和" || m[x].ToString() == "合" || m[x].ToString() == "特")
                                    {
                                        wl.main.Add("冠亚");
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }
                            }
                            if (wl.main.Count == 0)
                            {
                                //ErrorMsg(player);
                                return false;
                            }
                            for (int x = 0; x < sub[1].Length; ++x)
                            {
                                char[] ayC = { '大', '小', '单', '双', '龙', '虎', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
                                bool bggg = false;
                                for (int v = 0; v < ayC.Length; ++v) if (ayC[v] == sub[1][x]) bggg = true;
                                if (!bggg) continue;
                                wl.coin.Add(sub[1][x].ToString());
                            }
                            if (wl.coin.Count == 0)
                            {
                                return false;
                            }
                            int nAdd = 0;
                            if (!int.TryParse(sub[2], out nAdd))
                            {
                                return false;
                            }
                            wl.used = nAdd;
                            wl.allused = nAdd * wl.main.Count * wl.coin.Count;
                            get = true;
                            break;
                        }
                        else if (sub.Length == 2)
                        {
                            msg.Replace(s1, SetMgr.Instance().S2_2);
                            bool bGuanYa = false;
                            if (sub[0].Length > 2 && sub[0].Substring(0, 2) == "冠亚")
                            {
                                bGuanYa = true;
                                wl.main.Add("冠亚");
                                sub[0] = sub[0].Substring(2, sub[0].Length - 2);
                            }
                            else if (sub[0].Substring(0, 1) == "和" || sub[0].Substring(0, 1) == "合" || sub[0].Substring(0, 1) == "特")
                            {
                                bGuanYa = true;
                                wl.main.Add("冠亚");
                                sub[0] = sub[0].Substring(1, sub[0].Length - 1);
                            }
                            else wl.main.Add("1");
                            if (wl.main.Count == 0)
                            {
                                return false;
                            }
                            for (int x = 0; x < sub[0].Length; ++x)
                            {
                                if (sub[0][x] == ',') continue;
                                if (bGuanYa && x < sub[0].Length - 1 && (sub[0][x].ToString() == "1" || sub[0][x].ToString() == "2"))
                                {
                                    string ii = sub[0][x].ToString() + sub[0][x + 1].ToString();
                                    int _sdio = 0;
                                    if (!int.TryParse(ii, out _sdio))
                                    {
                                        return false;
                                    }
                                    wl.coin.Add(ii);
                                    x += 1;
                                }
                                else
                                    wl.coin.Add(sub[0][x].ToString());
                            }
                            if (wl.coin.Count == 0)
                            {
                                return false;
                            }
                            int nAdd = 0;
                            if (!int.TryParse(sub[1], out nAdd))
                            {
                                continue;
                            }
                            wl.used = nAdd;
                            wl.allused += nAdd * wl.main.Count * wl.coin.Count;
                            get = true;
                            break;
                        }
                        else if (sub.Length > 3)
                        {
                            return false;


                        }
                        else if (sub.Length == 1 && nonon)
                        {
                            char[] ayC = { '大', '小', '单', '双', '龙', '虎' };
                            if (sub[0].IndexOfAny(ayC) >= 0)
                            {
                                int nA = sub[0].IndexOfAny(ayC);
                                int nE = sub[0].LastIndexOfAny(ayC);
                                for (int x = 0; x < nA; ++x)
                                {
                                    int xxxxx = 0;
                                    if (int.TryParse(sub[0][x].ToString(), out xxxxx))
                                    {
                                        if (xxxxx < 0 || xxxxx > 10) continue;
                                        wl.main.Add(xxxxx == 0 ? "10" : sub[0][x].ToString());
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }
                                if (wl.main.Count == 0) wl.main.Add(1.ToString());
                                if (nE == sub[0].Length - 1)
                                {
                                    continue;
                                }
                                for (int x = nA; x <= nE; ++x)
                                {
                                    bool bggg = false;
                                    for (int v = 0; v < ayC.Length; ++v) if (ayC[v] == sub[0][x]) bggg = true;
                                    if (!bggg) continue;
                                    wl.coin.Add(sub[0][x].ToString());
                                }
                                string scoint = sub[0].Substring(nE + 1, sub[0].Length - nE - 1);
                                int nAdd = 0;
                                if (!int.TryParse(scoint, out nAdd))
                                {
                                    continue;
                                }
                                wl.used = nAdd;
                                wl.allused += nAdd * wl.main.Count * wl.coin.Count;
                                get = true;
                            }
                            break;
                        }
                    }
                    if (!get)
                    {
                        return false;
                    }
                }
                foreach (string sm in wl.main)
                {
                    if (sm != "冠亚")
                    {
                        if (int.Parse(sm) < 1 || int.Parse(sm) > 10) continue;
                    }


                    int nSum = 0;
                    float peilv = 0.0f;
                    float zuidi = 0;
                    float zuigao = 0;
                    float li = 0;
                    foreach (string ss in wl.coin)
                    {
                        bool bWin = false;
                        int num = 0;
                        if (ss == "大")
                        {
                            if (sm == "冠亚")
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚大'");
                                if (dt.Rows.Count > 0)
                                {
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                }
                                bWin = nSum > 11;
                                if (wl.used < zuidi)
                                {
                                    return false;
                                }
                                else if (wl.used > zuigao)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名大'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                bWin = nSum > 5;
                                if (wl.used < zuidi)
                                {
                                    return false;
                                }
                                else if (wl.used > zuigao)
                                {
                                    return false;
                                }
                            }
                        }
                        else if (ss == "小")
                        {
                            if (sm == "冠亚")
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚小'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                bWin = nSum <= 11;
                                if (wl.used < zuidi)
                                {
                                    return false;
                                }
                                else if (wl.used > zuigao)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名小'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                bWin = nSum <= 5;
                                if (wl.used < zuidi)
                                {
                                    return false;
                                }
                                else if (wl.used > zuigao)
                                {
                                    return false;
                                }
                            }
                        }
                        else if (ss == "单")
                        {
                            bWin = (nSum % 2 == 1);
                            if (sm == "冠亚")
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }

                                if (wl.used < zuidi)
                                {
                                    return false;
                                }
                                else if (wl.used > zuigao)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }

                                if (wl.used < zuidi)
                                {
                                    return false;
                                }
                                else if (wl.used > zuigao)
                                {
                                    return false;
                                }
                            }
                        }
                        else if (ss == "双")
                        {
                            bWin = (nSum % 2 == 0);
                            if (sm == "冠亚")
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                if (wl.used < zuidi)
                                {
                                    return false;
                                }
                                else if (wl.used > zuigao)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                if (wl.used < zuidi)
                                {
                                     return false;
                                }
                                else if (wl.used > zuigao)
                                {
                                    return false;
                                }
                            }
                        }
                        else if (ss == "龙")
                        {
                            int iindex = 10 - int.Parse(sm);
                            if (sm == "冠亚")
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                if (wl.used < zuidi)
                                {
                                    return false;
                                }
                                else if (wl.used > zuigao)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                if (wl.used < zuidi)
                                {
                                    return false;
                                }
                                else if (wl.used > zuigao)
                                {
                                    return false;
                                }
                            }
                        }
                        else if (ss == "虎")
                        {
                            int iindex = 10 - int.Parse(sm);
                            if (sm == "冠亚")
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                if (wl.used < zuidi)
                                {
                                    return false;
                                }
                                else if (wl.used > zuigao)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                if (wl.used < zuidi)
                                {
                                    return false;
                                }
                                else if (wl.used > zuigao)
                                {
                                    return false;
                                }
                            }
                        }
                        else if (int.TryParse(ss, out num))
                        {
                            if (num == 0) num = 10;
                            if (num == nSum) bWin = true;
                            else bWin = false;

                            if (sm == "冠亚")
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + num + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                if (wl.used < zuidi)
                                {
                                    return false;
                                }
                                else if (wl.used > zuigao)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + num + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                    zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                    li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                if (wl.used < zuidi)
                                {
                                    return false;
                                }
                                else if (wl.used > zuigao)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }

                Dictionary<string, int> sumAll = new Dictionary<string, int>();
                foreach (atkMsg atk in atkList)
                {
                    foreach (string sm in atk.main)
                    {
                        if (sm != "冠亚")
                        {
                            if (int.Parse(sm) < 1 || int.Parse(sm) > 10) continue;
                        }


                        int nSum = 0;
                        float peilv = 0.0f;
                        float zuidi = 0;
                        float zuigao = 0;
                        float li = 0;
                        foreach (string ss in atk.coin)
                        {
                            if (sumAll.ContainsKey(sm + ss))
                            {
                                sumAll[sm + ss] += atk.used;
                            }
                            else
                            {
                                sumAll.Add(sm + ss, atk.used);
                            }
                            bool bWin = false;
                            int num = 0;
                            if (ss == "大")
                            {
                                if (sm == "冠亚")
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚大'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                    }
                                    bWin = nSum > 11;
                                    if (sumAll[sm + ss] > li)
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名大'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    bWin = nSum > 5;
                                    if (sumAll[sm + ss] > li)
                                    {
                                        return false;
                                    }
                                }
                            }
                            else if (ss == "小")
                            {
                                if (sm == "冠亚")
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚小'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    bWin = nSum <= 11;
                                    if (sumAll[sm + ss] > li)
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名小'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    bWin = nSum <= 5;
                                    if (sumAll[sm + ss] > li)
                                    {
                                        return false;
                                    }
                                }
                            }
                            else if (ss == "单")
                            {
                                bWin = (nSum % 2 == 1);
                                if (sm == "冠亚")
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + ss + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }

                                    if (sumAll[sm + ss] > li)
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }

                                    if (sumAll[sm + ss] > li)
                                    {
                                        return false;
                                    }
                                }
                            }
                            else if (ss == "双")
                            {
                                bWin = (nSum % 2 == 0);
                                if (sm == "冠亚")
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + ss + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    if (sumAll[sm + ss] > li)
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    if (sumAll[sm + ss] > li)
                                    {
                                        return false;
                                    }
                                }
                            }
                            else if (ss == "龙")
                            {
                                int iindex = 10 - int.Parse(sm);
                                if (sm == "冠亚")
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + ss + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    if (sumAll[sm + ss] > li)
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    if (sumAll[sm + ss] > li)
                                    {
                                        return false;
                                    }
                                }
                            }
                            else if (ss == "虎")
                            {
                                int iindex = 10 - int.Parse(sm);
                                if (sm == "冠亚")
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + ss + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    if (sumAll[sm + ss] > li)
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    if (sumAll[sm + ss] > li)
                                    {
                                        return false;
                                    }
                                }
                            }
                            else if (int.TryParse(ss, out num))
                            {
                                if (num == 0) num = 10;
                                if (num == nSum) bWin = true;
                                else bWin = false;

                                if (sm == "冠亚")
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + num + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    if (sumAll[sm + ss] > li)
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + num + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        zuidi = float.Parse((string)dt.Rows[0]["最低"]);
                                        zuigao = float.Parse((string)dt.Rows[0]["最高"]);
                                        li = float.Parse((string)dt.Rows[0]["每期限额"]);
                                        peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                    }
                                    if (sumAll[sm + ss] > li)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

                float nAll = 0;
                foreach(atkMsg a in atkList)
                {
                    if (a.player == player)
                    {
                        nAll += a.allused;
                    }
                }
                nAll += player.left;
                if (wl.allused > nAll)
                {
                    return false;
                }
                for (int xz = 0; xz < atkList.Count;++xz)
                {
                    atkMsg a = atkList[xz];
                    if (a.player == player)
                    {
                        player.left += a.allused;
                        atkList.Remove(a);
                        xz--;
                    }
                }
                player.usedscore = wl.allused;
                player.left -= wl.allused;
                wl.content = msg;
                player.content = msg;
                player.num = num;
                player.up();

                atkList.Add(wl);
																//Contant.InsertLog2(player, "管理修改进攻" + msg);
																Contant.Log(player,"GM修改进攻",wl.allused,CurrentGame.ToString(),num,"");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
