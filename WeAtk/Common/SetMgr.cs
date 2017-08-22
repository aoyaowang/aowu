using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeAtk.Common
{
    class StrValue {
        public delegate void valueChanged(string value);

        public string value;
        public List<valueChanged> list = new List<valueChanged>();

        private string _path;

        public void add(valueChanged func) {
            list.Add(func);
        }

        public void remove(valueChanged func) {
            list.Remove(func);
        }

        public StrValue(string path) {
            _path = path;
            value = Reader.readTxt(path);
        }

        public void up() {
            Reader.writeTxt(_path, value);
            for (int i = 0; i < list.Count; ++i) list[i](value);
        }
    }
    class iniStr {
        public delegate void valueChanged(string value);

        public string value;
        public List<valueChanged> list = new List<valueChanged>();

        private string _path;
        private string _sec;
        private string _key;

        public void add(valueChanged func)
        {
            list.Add(func);
        }

        public void remove(valueChanged func)
        {
            list.Remove(func);
        }

        public iniStr(string sec, string key, string path)
        {
            path = System.IO.Directory.GetCurrentDirectory() + "/" + path;
            _path = path;
            _sec = sec;
            _key = key;
            value = Reader.IniReadValue(sec, key, path);
        }

        public void up()
        {
            Reader.IniWrite(_sec, _key, value, _path);
            for (int i = 0; i < list.Count; ++i) list[i](value);
        }
    }

    class iniInt
    {
        public delegate void valueNChanged(int value);

        public int value;
        public List<valueNChanged> list = new List<valueNChanged>();

        private string _path;
        private string _sec;
        private string _key;

        public void add(valueNChanged func)
        {
            list.Add(func);
        }

        public void remove(valueNChanged func)
        {
            list.Remove(func);
        }

        public iniInt(string sec, string key, string path)
        {
            path = System.IO.Directory.GetCurrentDirectory() + "/" + path;
            _path = path;
            _sec = sec;
            _key = key;
            value = int.Parse(Reader.IniReadValue(sec, key, path));
        }

        public void up()
        {
            Reader.IniWrite(_sec, _key, value.ToString(), _path);
            for (int i = 0; i < list.Count; ++i) list[i](value);
        }
    }

    class SetMgr
    {
        private static SetMgr _self = null;
        public static SetMgr Instance() {
            if (_self == null) {
                _self = new SetMgr();
            }
            return _self;
        }
        #region STRSETTING
        public StrValue _查分回复 = new StrValue("./data/msg/查分回复.txt");
        public string 查分回复 {
            get {
                return _查分回复.value;
            }

            set {
                _查分回复.value = value;
                _查分回复.up();
            }
        }

        public StrValue _封盘消息 = new StrValue("./data/msg/封盘消息.txt");
        public string 封盘消息
        {
            get
            {
                return _封盘消息.value;
            }

            set
            {
                _封盘消息.value = value;
                _封盘消息.up();
            }
        }

        public StrValue _核对格式 = new StrValue("./data/msg/核对格式.txt");
        public string 核对格式
        {
            get
            {
                return _核对格式.value;
            }

            set
            {
                _核对格式.value = value;
                _核对格式.up();
            }
        }

        public StrValue _计划格式 = new StrValue("./data/msg/计划格式.txt");
        public string 计划格式
        {
            get
            {
                return _计划格式.value;
            }

            set
            {
                _计划格式.value = value;
                _计划格式.up();
            }
        }

        public StrValue _计划消息 = new StrValue("./data/msg/计划消息.txt");
        public string 计划消息
        {
            get
            {
                return _计划消息.value;
            }

            set
            {
                _计划消息.value = value;
                _计划消息.up();
            }
        }

        public StrValue _接近封盘消息 = new StrValue("./data/msg/接近封盘消息.txt");
        public string 接近封盘消息
        {
            get
            {
                return _接近封盘消息.value;
            }

            set
            {
                _接近封盘消息.value = value;
                _接近封盘消息.up();
            }
        }

        public StrValue _结算格式 = new StrValue("./data/msg/结算格式.txt");
        public string 结算格式
        {
            get
            {
                return _结算格式.value;
            }

            set
            {
                _结算格式.value = value;
                _结算格式.up();
            }
        }

        public StrValue _结算消息 = new StrValue("./data/msg/结算消息.txt");
        public string 结算消息
        {
            get
            {
                return _结算消息.value;
            }

            set
            {
                _结算消息.value = value;
                _结算消息.up();
            }
        }

        public StrValue _开奖消息 = new StrValue("./data/msg/开奖消息.txt");
        public string 开奖消息
        {
            get
            {
                return _开奖消息.value;
            }

            set
            {
                _开奖消息.value = value;
                _开奖消息.up();
            }
        }

        public StrValue _开盘消息 = new StrValue("./data/msg/开盘消息.txt");
        public string 开盘消息
        {
            get
            {
                return _开盘消息.value;
            }

            set
            {
                _开盘消息.value = value;
                _开盘消息.up();
            }
        }

        public StrValue _开战替换 = new StrValue("./data/msg/开战替换.txt");
        public string 开战替换
        {
            get
            {
                return _开战替换.value;
            }

            set
            {
                _开战替换.value = value;
                _开战替换.up();
            }
        }

        public StrValue _休战替换 = new StrValue("./data/msg/休战替换.txt");
        public string 休战替换
        {
            get
            {
                return _休战替换.value;
            }

            set
            {
                _休战替换.value = value;
                _休战替换.up();
            }
        }

        public StrValue _游戏成功回复 = new StrValue("./data/msg/游戏成功回复.txt");
        public string 游戏成功回复
        {
            get
            {
                return _游戏成功回复.value;
            }

            set
            {
                _游戏成功回复.value = value;
                _游戏成功回复.up();
            }
        }

        public StrValue _账单格式 = new StrValue("./data/msg/账单格式.txt");
        public string 账单格式
        {
            get
            {
                return _账单格式.value;
            }

            set
            {
                _账单格式.value = value;
                _账单格式.up();
            }
        }

        public StrValue _自定义1 = new StrValue("./data/msg/自定义1.txt");
        public string 自定义1
        {
            get
            {
                return _自定义1.value;
            }

            set
            {
                _自定义1.value = value;
                _自定义1.up();
            }
        }

        public StrValue _自定义2 = new StrValue("./data/msg/自定义2.txt");
        public string 自定义2
        {
            get
            {
                return _自定义2.value;
            }

            set
            {
                _自定义2.value = value;
                _自定义2.up();
            }
        }

        public StrValue _自定义3 = new StrValue("./data/msg/自定义3.txt");
        public string 自定义3
        {
            get
            {
                return _自定义3.value;
            }

            set
            {
                _自定义3.value = value;
                _自定义3.up();
            }
        }

        public StrValue _自定义4 = new StrValue("./data/msg/自定义4.txt");
        public string 自定义4
        {
            get
            {
                return _自定义4.value;
            }

            set
            {
                _自定义4.value = value;
                _自定义4.up();
            }
        }

        public StrValue _1_1 = new StrValue("./data/spilt/1_1.txt");
        public string S1_1
        {
            get
            {
                return _1_1.value;
            }

            set
            {
                _1_1.value = value;
                _1_1.up();
            }
        }

        public StrValue _1_2 = new StrValue("./data/spilt/1_2.txt");
        public string S1_2
        {
            get
            {
                return _1_2.value;
            }

            set
            {
                _1_2.value = value;
                _1_2.up();
            }
        }

        public StrValue _2_1 = new StrValue("./data/spilt/2_1.txt");
        public string S2_1
        {
            get
            {
                return _2_1.value;
            }

            set
            {
                _2_1.value = value;
                _2_1.up();
            }
        }

        public StrValue _2_2 = new StrValue("./data/spilt/2_2.txt");
        public string S2_2
        {
            get
            {
                return _2_2.value;
            }

            set
            {
                _2_2.value = value;
                _2_2.up();
            }
        }

        public StrValue _3_1 = new StrValue("./data/spilt/3_1.txt");
        public string S3_1
        {
            get
            {
                return _3_1.value;
            }

            set
            {
                _3_1.value = value;
                _3_1.up();
            }
        }

        public StrValue _3_2 = new StrValue("./data/spilt/3_2.txt");
        public string S3_2
        {
            get
            {
                return _3_2.value;
            }

            set
            {
                _3_2.value = value;
                _3_2.up();
            }
        }

        #endregion

        #region CONFIGSETTING
        public iniStr _addplayer = new iniStr("微信互动", "addplayer", Contant.config);
        public bool addplayer
        {
            get
            {
                //return true;
                return _addplayer.value == "True";
            }

            set
            {
                _addplayer.value = value ? "True" : "False";
                _addplayer.up();
            }
        }
        public iniStr _editmoney = new iniStr("微信互动", "editmoney", Contant.config);
        public bool editmoney
        {
            get
            {
                return _editmoney.value == "True";
            }

            set
            {
                _editmoney.value = value ? "True" : "False";
                _editmoney.up();
            }
        }
        public iniStr _editneirong = new iniStr("微信互动", "editneirong", Contant.config);
        public bool editneirong
        {
            get
            {
                return _editneirong.value == "True";
            }

            set
            {
                _editneirong.value = value ? "True" : "False";
                _editneirong.up();
            }
        }
        public iniStr _delplayer = new iniStr("微信互动", "delplayer", Contant.config);
        public bool delplayer
        {
            get
            {
                return _delplayer.value == "True";
            }

            set
            {
                _delplayer.value = value ? "True" : "False";
                _delplayer.up();
            }
        }
        public iniStr _clear = new iniStr("微信互动", "clear", Contant.config);
        public bool clear
        {
            get
            {
                return _clear.value == "True";
            }

            set
            {
                _clear.value = value ? "True" : "False";
                _clear.up();
            }
        }
        public iniStr _receiveorder = new iniStr("微信互动", "receiveorder", Contant.config);
        public bool receiveorder
        {
            get
            {
                return _receiveorder.value == "True";
            }

            set
            {
                _receiveorder.value = value ? "True" : "False";
                _receiveorder.up();
            }
        }
        public iniStr _receivechahui = new iniStr("微信互动", "receivechahui", Contant.config);
        public bool receivechahui
        {
            get
            {
                return _receivechahui.value == "True";
            }

            set
            {
                _receivechahui.value = value ? "True" : "False";
                _receivechahui.up();
            }
        }
        public iniStr _backmoney = new iniStr("微信互动", "backmoney", Contant.config);
        public bool backmoney
        {
            get
            {
                return _backmoney.value == "True";
            }

            set
            {
                _backmoney.value = value ? "True" : "False";
                _backmoney.up();
            }
        }
        public iniStr _wrongorder = new iniStr("微信互动", "wrongorder", Contant.config);
        public bool wrongorder
        {
            get
            {
                return _wrongorder.value == "True";
            }

            set
            {
                _wrongorder.value = value ? "True" : "False";
                _wrongorder.up();
            }
        }
        public iniStr _changename = new iniStr("微信互动", "changename", Contant.config);
        public bool changename
        {
            get
            {
                return _changename.value == "True";
            }

            set
            {
                _changename.value = value ? "True" : "False";
                _changename.up();
            }
        }
        public iniStr _key_cha = new iniStr("微信互动", "key_cha", Contant.config);
        public string key_cha
        {
            get
            {
                return _key_cha.value;
            }

            set
            {
                _key_cha.value = value;
                _key_cha.up();
            }
        }
        public iniStr _key_hui = new iniStr("微信互动", "key_hui", Contant.config);
        public string key_hui
        {
            get
            {
                return _key_hui.value;
            }

            set
            {
                _key_hui.value = value;
                _key_hui.up();
            }
        }
        public iniStr _key_edit = new iniStr("微信互动", "key_edit", Contant.config);
        public string key_edit
        {
            get
            {
                return _key_edit.value;
            }

            set
            {
                _key_edit.value = value;
                _key_edit.up();
            }
        }
        public iniStr _key_cancel = new iniStr("微信互动", "key_cancel", Contant.config);
        public string key_cancel
        {
            get
            {
                return _key_cancel.value;
            }

            set
            {
                _key_cancel.value = value;
                _key_cancel.up();
            }
        }
        public iniStr _key_check = new iniStr("微信互动", "key_check", Contant.config);
        public string key_check
        {
            get
            {
                return _key_check.value;
            }

            set
            {
                _key_check.value = value;
                _key_check.up();
            }
        }
        public iniStr _protect = new iniStr("微信互动", "protect", Contant.config);
        public bool protect
        {
            get
            {
                return _protect.value == "True";
            }

            set
            {
                _protect.value = value ? "True" : "False";
                _protect.up();
            }
        }
        public iniStr _recommend = new iniStr("微信互动", "recommend", Contant.config);
        public bool recommend
        {
            get
            {
                return _recommend.value == "True";
            }

            set
            {
                _recommend.value = value ? "True" : "False";
                _recommend.up();
            }
        }
        public iniStr _notfind = new iniStr("微信互动", "notfind", Contant.config);
        public bool notfind
        {
            get
            {
                return _notfind.value == "True";
            }

            set
            {
                _notfind.value = value ? "True" : "False";
                _notfind.up();
            }
        }
        public iniStr _shuaping = new iniStr("微信互动", "shuaping", Contant.config);
        public bool shuaping
        {
            get
            {
                return _shuaping.value == "True";
            }

            set
            {
                _shuaping.value = value ? "True" : "False";
                _shuaping.up();
            }
        }
        public iniStr _picboom = new iniStr("微信互动", "picboom", Contant.config);
        public bool picboom
        {
            get
            {
                return _picboom.value == "True";
            }

            set
            {
                _picboom.value = value ? "True" : "False";
                _picboom.up();
            }
        }
        public iniStr _low = new iniStr("微信互动", "low", Contant.config);
        public bool low
        {
            get
            {
                return _low.value == "True";
            }

            set
            {
                _low.value = value ? "True" : "False";
                _low.up();
            }
        }
        public iniStr _limited = new iniStr("微信互动", "limited", Contant.config);
        public bool limited
        {
            get
            {
                return _limited.value == "True";
            }

            set
            {
                _limited.value = value ? "True" : "False";
                _limited.up();
            }
        }
        public iniStr _closed = new iniStr("微信互动", "closed", Contant.config);
        public bool closed
        {
            get
            {
                return _closed.value == "True";
            }

            set
            {
                _closed.value = value ? "True" : "False";
                _closed.up();
            }
        }
        public iniStr _sentpic = new iniStr("微信互动", "sentpic", Contant.config);
        public bool sentpic
        {
            get
            {
                return _sentpic.value == "True";
            }

            set
            {
                _sentpic.value = value ? "True" : "False";
                _sentpic.up();
            }
        }
        public iniStr _backmessage = new iniStr("微信互动", "backmessage", Contant.config);
        public bool backmessage
        {
            get
            {
                return _backmessage.value == "True";
            }

            set
            {
                _backmessage.value = value ? "True" : "False";
                _backmessage.up();
            }
        }
        public iniStr _backmessage2 = new iniStr("微信互动", "backmessage2", Contant.config);
        public bool backmessage2
        {
            get
            {
                return _backmessage2.value == "True";
            }

            set
            {
                _backmessage2.value = value ? "True" : "False";
                _backmessage2.up();
            }
        }

        public iniStr _kaiguan_ziding1 = new iniStr("微信消息", "kaiguan_ziding1", Contant.config);
        public bool kaiguan_ziding1
        {
            get
            {
                return _kaiguan_ziding1.value == "True";
            }

            set
            {
                _kaiguan_ziding1.value = value ? "True" : "False";
                _kaiguan_ziding1.up();
            }
        }
        public iniStr _kaiguan_ziding2 = new iniStr("微信消息", "kaiguan_ziding2", Contant.config);
        public bool kaiguan_ziding2
        {
            get
            {
                return _kaiguan_ziding2.value == "True";
            }

            set
            {
                _kaiguan_ziding2.value = value ? "True" : "False";
                _kaiguan_ziding2.up();
            }
        }
        public iniStr _kaiguan_ziding3 = new iniStr("微信消息", "kaiguan_ziding3", Contant.config);
        public bool kaiguan_ziding3
        {
            get
            {
                return _kaiguan_ziding3.value == "True";
            }

            set
            {
                _kaiguan_ziding3.value = value ? "True" : "False";
                _kaiguan_ziding3.up();
            }
        }
        public iniStr _kaiguan_ziding4 = new iniStr("微信消息", "kaiguan_ziding4", Contant.config);
        public bool kaiguan_ziding4
        {
            get
            {
                return _kaiguan_ziding4.value == "True";
            }

            set
            {
                _kaiguan_ziding4.value = value ? "True" : "False";
                _kaiguan_ziding4.up();
            }
        }

        public iniStr _check_jinzhi_0 = new iniStr("微信消息", "check_jinzhi_0", Contant.config);
        public bool check_jinzhi_0
        {
            get
            {
                return _check_jinzhi_0.value == "True";
            }

            set
            {
                _check_jinzhi_0.value = value ? "True" : "False";
                _check_jinzhi_0.up();
            }
        }
        public iniStr _check_jinzhi_1 = new iniStr("微信消息", "check_jinzhi_1", Contant.config);
        public bool check_jinzhi_1
        {
            get
            {
                return _check_jinzhi_1.value == "True";
            }

            set
            {
                _check_jinzhi_1.value = value ? "True" : "False";
                _check_jinzhi_1.up();
            }
        }
        public iniStr _check_jinzhi_2 = new iniStr("微信消息", "check_jinzhi_2", Contant.config);
        public bool check_jinzhi_2
        {
            get
            {
                return _check_jinzhi_2.value == "True";
            }

            set
            {
                _check_jinzhi_2.value = value ? "True" : "False";
                _check_jinzhi_2.up();
            }
        }
        public iniStr _check_jinzhi_3 = new iniStr("微信消息", "check_jinzhi_3", Contant.config);
        public bool check_jinzhi_3
        {
            get
            {
                return _check_jinzhi_3.value == "True";
            }

            set
            {
                _check_jinzhi_3.value = value ? "True" : "False";
                _check_jinzhi_3.up();
            }
        }
        public iniStr _check_jinzhi_4 = new iniStr("微信消息", "check_jinzhi_4", Contant.config);
        public bool check_jinzhi_4
        {
            get
            {
                return _check_jinzhi_4.value == "True";
            }

            set
            {
                _check_jinzhi_4.value = value ? "True" : "False";
                _check_jinzhi_4.up();
            }
        }

        public iniStr _check_pic_1 = new iniStr("微信消息", "check_pic_1", Contant.config);
        public bool check_pic_1
        {
            get
            {
                return _check_pic_1.value == "True";
            }

            set
            {
                _check_pic_1.value = value ? "True" : "False";
                _check_pic_1.up();
            }
        }
        public iniStr _check_pic_2 = new iniStr("微信消息", "check_pic_2", Contant.config);
        public bool check_pic_2
        {
            get
            {
                return _check_pic_2.value == "True";
            }

            set
            {
                _check_pic_2.value = value ? "True" : "False";
                _check_pic_2.up();
            }
        }
        public iniStr _check_pic_3 = new iniStr("微信消息", "check_pic_3", Contant.config);
        public bool check_pic_3
        {
            get
            {
                return _check_pic_3.value == "True";
            }

            set
            {
                _check_pic_3.value = value ? "True" : "False";
                _check_pic_3.up();
            }
        }
        public iniStr _check_pic_4 = new iniStr("微信消息", "check_pic_4", Contant.config);
        public bool check_pic_4
        {
            get
            {
                return _check_pic_4.value == "True";
            }

            set
            {
                _check_pic_4.value = value ? "True" : "False";
                _check_pic_4.up();
            }
        }
        public iniStr _check_xiaoshu_1 = new iniStr("微信消息", "check_xiaoshu_1", Contant.config);
        public bool check_xiaoshu_1
        {
            get
            {
                return _check_xiaoshu_1.value == "True";
            }

            set
            {
                _check_xiaoshu_1.value = value ? "True" : "False";
                _check_xiaoshu_1.up();
            }
        }
        public iniStr _check_xiaoshu_2 = new iniStr("微信消息", "check_xiaoshu_2", Contant.config);
        public bool check_xiaoshu_2
        {
            get
            {
                return _check_xiaoshu_2.value == "True";
            }

            set
            {
                _check_xiaoshu_2.value = value ? "True" : "False";
                _check_xiaoshu_2.up();
            }
        }
        public iniStr _check_shuangpai = new iniStr("微信消息", "check_shuangpai", Contant.config);
        public bool check_shuangpai
        {
            get
            {
                return _check_shuangpai.value == "True";
            }

            set
            {
                _check_shuangpai.value = value ? "True" : "False";
                _check_shuangpai.up();
            }
        }
        public iniStr _check_shuangpai_1 = new iniStr("微信消息", "check_shuangpai_1", Contant.config);
        public bool check_shuangpai_1
        {
            get
            {
                return _check_shuangpai_1.value == "True";
            }

            set
            {
                _check_shuangpai_1.value = value ? "True" : "False";
                _check_shuangpai_1.up();
            }
        }
        public iniStr _check_shuangpai_2 = new iniStr("微信消息", "check_shuangpai_2", Contant.config);
        public bool check_shuangpai_2
        {
            get
            {
                return _check_shuangpai_2.value == "True";
            }

            set
            {
                _check_shuangpai_2.value = value ? "True" : "False";
                _check_shuangpai_2.up();
            }
        }

        public iniInt _msg_kaijiang_num = new iniInt("微信消息", "msg_kaijiang_num", Contant.config);
        public int msg_kaijiang_num
        {
            get
            {
                return _msg_kaijiang_num.value;
            }

            set
            {
                _msg_kaijiang_num.value = value;
                _msg_kaijiang_num.up();
            }
        }
        public iniInt _time_ziding1 = new iniInt("微信消息", "time_ziding1", Contant.config);
        public int time_ziding1
        {
            get
            {
                return _time_ziding1.value;
            }

            set
            {
                _time_ziding1.value = value;
                _time_ziding1.up();
            }
        }
        public iniInt _time_ziding2 = new iniInt("微信消息", "time_ziding2", Contant.config);
        public int time_ziding2
        {
            get
            {
                return _time_ziding2.value;
            }

            set
            {
                _time_ziding2.value = value;
                _time_ziding2.up();
            }
        }
        public iniInt _time_ziding3 = new iniInt("微信消息", "time_ziding3", Contant.config);
        public int time_ziding3
        {
            get
            {
                return _time_ziding3.value;
            }

            set
            {
                _time_ziding3.value = value;
                _time_ziding3.up();
            }
        }
        public iniInt _time_ziding4 = new iniInt("微信消息", "time_ziding4", Contant.config);
        public int time_ziding4
        {
            get
            {
                return _time_ziding4.value;
            }

            set
            {
                _time_ziding4.value = value;
                _time_ziding1.up();
            }
        }
        public iniInt _msg_nicheng_num = new iniInt("微信消息", "msg_nicheng_num", Contant.config);
        public int msg_nicheng_num
        {
            get
            {
                return _msg_nicheng_num.value;
            }

            set
            {
                _msg_nicheng_num.value = value;
                _msg_nicheng_num.up();
            }
        }
        public iniInt _txt_shunxu = new iniInt("微信消息", "txt_shunxu", Contant.config);
        public int txt_shunxu
        {
            get
            {
                return _txt_shunxu.value;
            }

            set
            {
                _txt_shunxu.value = value;
                _txt_shunxu.up();
            }
        }
        public iniInt _msg_kaijiang_num2 = new iniInt("微信消息", "msg_kaijiang_num2", Contant.config);
        public int msg_kaijiang_num2
        {
            get
            {
                return _msg_kaijiang_num2.value;
            }

            set
            {
                _msg_kaijiang_num2.value = value;
                _msg_kaijiang_num2.up();
            }
        }
        public iniInt _txt_zhangdanmin = new iniInt("微信消息", "txt_zhangdanmin", Contant.config);
        public int txt_zhangdanmin
        {
            get
            {
                return _txt_zhangdanmin.value;
            }

            set
            {
                _txt_zhangdanmin.value = value;
                _txt_zhangdanmin.up();
            }
        }

        public iniInt _封盘时间 = new iniInt("设置", "封盘时间", Contant.config);
        public int 封盘时间
        {
            get
            {
                return _封盘时间.value;
            }

            set
            {
                _封盘时间.value = value;
                _封盘时间.up();
            }
        }

        public iniInt _线路 = new iniInt("设置", "线路", Contant.config);
        public int 线路
        {
            get
            {
                return _线路.value;
            }

            set
            {
                _线路.value = value;
                _线路.up();
            }
        }

        public iniInt _接近封盘时间 = new iniInt("设置", "接近封盘时间", Contant.config);
        public int 接近封盘时间
        {
            get
            {
                return _接近封盘时间.value;
            }

            set
            {
                _接近封盘时间.value = value;
                _接近封盘时间.up();
            }
        }
        public iniInt _每期总限额 = new iniInt("设置", "每期总限额", Contant.config);
        public int 每期总限额
        {
            get
            {
                return _每期总限额.value;
            }

            set
            {
                _每期总限额.value = value;
                _每期总限额.up();
            }
        }
        public iniInt _超时 = new iniInt("设置", "超时", Contant.config);
        public int 超时
        {
            get
            {
                return _超时.value;
            }

            set
            {
                _超时.value = value;
                _超时.up();
            }
        }
        #endregion
    }
}
