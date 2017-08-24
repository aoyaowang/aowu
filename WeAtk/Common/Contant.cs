using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeAtk.pk10;
using WeAtk.User;

namespace WeAtk.Common
{
    class caijicfg {
        public int type = 0; //0 - 9 value -1 num -2 time -3 -4 value -5 findnext
        public string strbegin;
        public string strend;
    }
    public class tmpPlayer
    {
        public string playername;
        public float value;
    }
    class Contant
    {
        //Kingooo123123
        public static string gameDB = "Provider=Microsoft.Jet.OLEDB.4.0;Jet OLEDB:DataBase PassWord=Kingooo123123;User Id=Admin;Data source=./data/games.mdb";
        public static string playerDB = "Provider=Microsoft.Jet.OLEDB.4.0;Jet OLEDB:DataBase PassWord=Kingooo123123;User Id=Admin;Data source=./data/players.mdb";

        public static string config = "config.ini";

        public static List<caijicfg> parseCfg(string[] lst) {
            List<caijicfg> a = new List<caijicfg>();
            for (int i = 0;i < lst.Length;i+=3) {
                caijicfg x = new caijicfg();
                if (lst[i] == "") break;
                x.type = int.Parse(lst[i]);
                x.strbegin = lst[i+1];
                x.strend = lst[i+2];
                a.Add(x);
            }
            return a;
        }
        public static List<pk10data> caiji(string content, string[] lst, int type)
        {
            string begin = lst[0],end = lst[1],onebg = lst[2], oneend = lst[3], timebegin = lst[4],timeend = lst[5],numbegin = lst[6], numend = lst[7];
            int nStart = content.IndexOf(begin);
            if (nStart == -1) return null;
            int nEnd = content.IndexOf(end, nStart);
            if (nEnd == -1) return null;
            
            while (content.IndexOf(onebg, nStart) > 0 && content.IndexOf(onebg, nStart) < nEnd)
            {
                int nA = content.IndexOf(timebegin, nStart);
                if (nA == -1)
                {
                    Console.Write("ERR nA");
                    return null;
                }
                int nB = content.IndexOf(timeend, nA);
                if (nB == -1)
                {
                    Console.Write("ERR nB");
                    return null;
                }
                nA += timebegin.Length;
                string strtime = content.Substring(nA, nB - nA);

                int nA1 = content.IndexOf(numbegin, nB);
                if (nA1 == -1)
                {
                    Console.Write("ERR nA1");
                    return null;
                }
                int nB1 = content.IndexOf(numend, nA1);
                if (nB1 == -1)
                {
                    Console.Write("ERR nB1");
                    return null;
                }
                nA1 += numbegin.Length;
                string strnum = content.Substring(nA1, nB1 - nA1);
            }

            return null;
        }

        public static List<pk10data> caijiall(string content, string begin, string end, List<caijicfg> lst) {
            int nStart = content.IndexOf(begin);
            if (nStart == -1) return null;
            int nEnd = content.IndexOf(end, nStart);
            if (nEnd == -1) return null;
            List<pk10data> x = new List<pk10data>();
            while (content.IndexOf(lst[0].strbegin, nStart) > 0 && content.IndexOf(lst[0].strbegin, nStart) < nEnd)
            {
                pk10data pk = new pk10data();
                for (int i = 0;i < lst.Count;++i) {
                    nStart = content.IndexOf(lst[i].strbegin, nStart);
                    if (nStart == -1)
                    {
                        Console.Write("ERR nStart");
                        break;
                    }
                    nStart += lst[i].strbegin.Length;
                    int nB = content.IndexOf(lst[i].strend, nStart);
                    if (nB == -1)
                    {
                        Console.Write("ERR nB");
                        break;
                    }
                    string value = content.Substring(nStart, nB - nStart);
                    if (lst[i].type == -1) {
                        pk.num = value;
                    }
                    else if (lst[i].type == -2) {
                        pk.time = value;
                    }
                    else if (lst[i].type == -3) {
                        pk.data = value;
                    }
                    else if (lst[i].type == -4) {
                        pk.time = value + ":00";
                    }
                    else if (lst[i].type >= 0) {
                        if (lst[i].type == 9)
                            pk.data += value;
                        else
                            pk.data += value + ",";
                    }
                }
                if (nStart == -1) break;
                x.Add(pk);
            }
            if (x.Count == 0) return null;
            x.Sort((a, b) => {
                DateTime dta,dtb;
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy/MM/dd HH/mm";
                dta = Convert.ToDateTime(a.time, dtFormat);
                dtb = Convert.ToDateTime(b.time, dtFormat);
                if (dta.Ticks > dtb.Ticks)
                {
                    return -1;
                }
                else {
                    return 1;
                }
            });
            return x;
        }

        public delegate void CHANGEFUNC();
        public static CHANGEFUNC changeFunc = null;
        public static void InsertLog(string log)
        {
            string sql = "insert into managerlog (`time`, `msg`) values ('{0}','{1}')";
            sql = string.Format(sql, DateTime.Now.ToString(), log.Replace("\'", "\\\'"));
            Reader.Instance().QueryPlayer(sql);

            if (changeFunc != null) changeFunc();
        }

        public delegate void CHANGEFUNC2();
        public static CHANGEFUNC2 changeFunc2 = null;
        public static void InsertLog2(Player player,string log)
        {
            string sql = "insert into msg (`msgtime`, `message`, `playername`, `nickname`, `playertype`) values ('{0}','{1}','{2}','{3}','{4}')";
            sql = string.Format(sql, DateTime.Now.ToString(), log.Replace("\'", "\\\'"), 
                player.playername.Replace("\'", "\\\'"), 
                player.nickname.Replace("\'", "\\\'"),
                player.type == 0 ? "普通玩家" : "虚拟玩家");
            Reader.Instance().QueryPlayer(sql);

            if (changeFunc2 != null) changeFunc2();
        }
        
        public static void Log(Player player,string stype,float usescore,string game,string num,string data) {
            string sql = "insert into statement (时间,玩家,群昵称,玩家类型,类型,游戏,剩余分数,分数变化,期号,使用分数,玩法内容,分析内容,开奖) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')";
            sql = string.Format(sql, DateTime.Now.ToString(), 
                player.playername.Replace("\'", "\\\'"), 
                player.nickname.Replace("\'", "\\\'"),
                player.type == 0 ? "普通玩家" : "虚拟玩家",
                stype.Replace("\'","\\\'"),
                game.Replace("\'","\\\'"),
                player.left.ToString(),
                usescore,
                num,
                player.usedscore.ToString(),
                player.content.Replace("\'","\\\'"),
                "",
                data);
            Reader.Instance().QueryPlayer(sql);
        
        }
    }
}
