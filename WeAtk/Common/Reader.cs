using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;

namespace WeAtk.Common
{
    class Reader
    {

        private static Reader _Instance = null;
        public static Reader Instance() {
            if (_Instance == null)
            {
                _Instance = new Reader();
                _Instance.Init();
            }
            return _Instance;
        }

        public static void writeTxt(string file, string value)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入
                sw.Write(value);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static string readTxt(string file)
        {
            try
            {
                StreamReader sr = new StreamReader(file, Encoding.UTF8);
                return sr.ReadToEnd();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
                return "";
            }
        }

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        /// <summary>    
        /// 读取INI文件    
        /// </summary>    
        /// <param name="section">项目名称(如 [section] )</param>    
        /// <param name="skey">键</param>   
        /// <param name="path">路径</param> 
        public static string IniReadValue(string section, string skey, string path)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(section, skey, "", temp, 500, path);
            return temp.ToString();
        }


        /// <summary>
        /// 写入ini文件
        /// </summary>
        /// <param name="section">项目名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="path">路径</param>
        public static void IniWrite(string section, string key, string value, string path)
        {
            WritePrivateProfileString(section, key, value, path);
        }


        private OleDbConnection _gameDB = null;
        private OleDbConnection _playerDB = null;

        public void Init() {
            _gameDB = new OleDbConnection(Contant.gameDB);
            _playerDB = new OleDbConnection(Contant.playerDB);
        }

        public DataTable ReadGameData(string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow dr;
                //2、打开连接 C#操作Access之读取mdb  
                _gameDB.Open();

                //建立SQL查询   
                OleDbCommand odCommand = _gameDB.CreateCommand();

                //3、输入查询语句 C#操作Access之读取mdb  

                odCommand.CommandText = sql;

                //建立读取   
                OleDbDataReader odrReader = odCommand.ExecuteReader();

                //查询并显示数据   
                int size = odrReader.FieldCount;
                for (int i = 0; i < size; i++)
                {
                    DataColumn dc;
                    dc = new DataColumn(odrReader.GetName(i));
                    dt.Columns.Add(dc);
                }
                while (odrReader.Read())
                {
                    dr = dt.NewRow();
                    for (int i = 0; i < size; i++)
                    {
                        dr[odrReader.GetName(i)] =
                        odrReader[odrReader.GetName(i)].ToString();
                    }
                    dt.Rows.Add(dr);
                }
                //关闭连接 C#操作Access之读取mdb  
                odrReader.Close();
                _gameDB.Close();
                return dt;
            }
            catch
            {
                return null;
            }
        }

        public DataTable ReadPlayerData(string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow dr;
                //2、打开连接 C#操作Access之读取mdb  
                _playerDB.Open();

                //建立SQL查询   
                OleDbCommand odCommand = _playerDB.CreateCommand();

                //3、输入查询语句 C#操作Access之读取mdb  

                odCommand.CommandText = sql;

                //建立读取   
                OleDbDataReader odrReader = odCommand.ExecuteReader();

                //查询并显示数据   
                int size = odrReader.FieldCount;
                for (int i = 0; i < size; i++)
                {
                    DataColumn dc;
                    dc = new DataColumn(odrReader.GetName(i));
                    dt.Columns.Add(dc);
                }
                while (odrReader.Read())
                {
                    dr = dt.NewRow();
                    for (int i = 0; i < size; i++)
                    {
                        dr[odrReader.GetName(i)] =
                        odrReader[odrReader.GetName(i)].ToString();
                    }
                    dt.Rows.Add(dr);
                }
                //关闭连接 C#操作Access之读取mdb  
                odrReader.Close();
                _playerDB.Close();
                return dt;
            }
            catch
            {
                return null;
            }
        }

        public bool QueryGame(string sql) {
            try
            {
                DataTable dt = new DataTable();
                DataRow dr;
                //2、打开连接 C#操作Access之读取mdb  
                _gameDB.Open();

                //建立SQL查询   
                OleDbCommand odCommand = _gameDB.CreateCommand();

                //3、输入查询语句 C#操作Access之读取mdb  

                odCommand.CommandText = sql;

                //建立读取   
                odCommand.ExecuteNonQuery();
                
                _gameDB.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool QueryPlayer(string sql) {
            try
            {
                DataTable dt = new DataTable();
                DataRow dr;
                //2、打开连接 C#操作Access之读取mdb  
                _playerDB.Open();

                //建立SQL查询   
                OleDbCommand odCommand = _playerDB.CreateCommand();

                //3、输入查询语句 C#操作Access之读取mdb  

                odCommand.CommandText = sql;

                //建立读取   
                odCommand.ExecuteNonQuery();

                _playerDB.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
