using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeAtk.Common;

namespace WeAtk.User
{
    class AtkMsg
    {
        public string id;
        public string msg;
    }
    class Player
    {
        private string _playername;
        public string playername {
            get { return _playername; }
            set { _playername = value; }
        }

        private string _nickname;
        public string nickname
        {
            get { return _nickname; }
            set { _nickname = value; }
        }
        private int _type;
        public int type
        {
            get { return _type; }
            set { _type = value; }
        }
        private float _left;
        public float left
        {
            get { return _left; }
            set { _left = value; }
        }
        private string _num;
        public string num
        {
            get { return _num; }
            set { _num = value; }
        }
        private string _content;
        public string content
        {
            get { return _content; }
            set { _content = value; }
        }
        private float _usedscore;
        public float usedscore
        {
            get { return _usedscore; }
            set { _usedscore = value; }
        }
        private float _perscore;
        public float perscore
        {
            get { return _perscore; }
            set { _perscore = value; }
        }

        private List<AtkMsg> _atkMsg = new List<AtkMsg>();
        public List<AtkMsg> atkMsg
        {
            get { return _atkMsg; }
            set { _atkMsg = value; }
        }
            
        public WXUser me = null;

        public void up()
        {
            Reader.Instance().QueryPlayer("update players set 类型=" + _type.ToString() + ",剩余分数="+_left.ToString()+ ",期号='"+_num.ToString()+ "',玩法内容='"+_content+ "',使用分数="+_usedscore.ToString()+ ",上期得分="+perscore.ToString()+
                " where 玩家='"+_playername+"'");
        }

        public void insert()
        {
            string sql = string.Format("insert into players (玩家,类型,剩余分数,期号,玩法内容,使用分数,上期得分,md5,群昵称) values ('{0}','{1}',{2},'{3}','{4}',{5},{6},'{7}','{8}')",
                _playername,_type,_left,_num,_content,_usedscore,_perscore,"",_nickname);
            Reader.Instance().QueryPlayer(sql);
        }

        public void del()
        {
            Reader.Instance().QueryPlayer("delete from players where 玩家='" + _playername + "'");
        }
    }
}
