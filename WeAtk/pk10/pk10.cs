﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeAtk.Common;

namespace WeAtk.pk10
{
    public class WebClientto : WebClient
    {
        /// <summary>  
        /// 过期时间  
        /// </summary>  
        public int Timeout { get; set; }

        public WebClientto(int timeout)
        {
            Timeout = timeout;
        }

        /// <summary>  
        /// 重写GetWebRequest,添加WebRequest对象超时时间  
        /// </summary>  
        /// <param name="address"></param>  
        /// <returns></returns>  
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = Timeout;
            request.ReadWriteTimeout = Timeout;
            return request;
        }
    }

    class pk10data {
        public string num;
        public string data;
        public string time;
    }
    class Cpk10
    {
        private static Cpk10 _instance = null;
        public static Cpk10 Instance() {
            if (_instance == null) _instance = new Cpk10();
            return _instance;
        }
        public static string[] urls = { "http://www.zbjel.com/pk10/getHistoryData.do?count=20",
                                        "http://www.bwlc.gov.cn/bulletin/trax.html", 
                                        "http://www.bwlc.net/bulletin/trax.html"
                                        };
        
        public static string[] sbegin = {"{\"success\":true,\"code\":null,\"msg\":null,\"rows\":[{\"",
                                           "<th width=\"30%\">开奖时间</th>",
                                        "<th width=\"30%\">开奖时间</th>"
                                        };
        public static string[] send = {"}]}",
                                        "<div class=\"fc_fanye\">",
                                        "<div class=\"fc_fanye\">"
                                        };
        public static string[,] scfg = {{"-1", "\"termNum\":\"", "\"", "-2", "\"lotteryTime\":\"", "\"", "0", "n1\":", ",", "1", "n2\":", ",", "2", "n3\":", ",", "3", "n4\":", ",", "4", "n5\":", ",", "5", "n6\":", ",", "6", "n7\":", ",", "7", "n8\":", ",", "8", "n9\":", ",", "9", "n10\":", ","},
                                        {"-1", "<td>", "</td>", "-3", "<td>", "</td>", "-4", "<td>", "</td>", "-5", "odd", " ", "", "", "", "", "</td>", " ", "-5", "</td>", " ", "-5", "</td>", " ", "-5", "</td>", " ", "-5", "</td>", " ", "-5", "</td>", " ", "-5", "</td>", " "},
                                        {"-1", "<td>", "</td>", "-3", "<td>", "</td>", "-4", "<td>", "</td>", "-5", "odd", " ", "", "</td>", " ", "-5", "</td>", " ", "-5", "</td>", " ", "-5", "</td>", " ", "-5", "</td>", " ", "-5", "</td>", " ", "-5", "</td>", " ", "-5", "</td>", " "}};
        public bool _inget = false;
        public string curnum = "";
        public string cururl = "";
        public List<pk10data> list = new List<pk10data>();

        public bool Init() {
            string data = null;
            int vv = 0;
            for (vv = 0;vv < urls.Length;++vv) {
                string url = urls[vv];
                data = GetData(String.Format(url, (long)(DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalMilliseconds));
                cururl = url;
                if (data != null) break;
            }

            if (data == null) {
                MessageBox.Show("PK10获取数据失败");
                return false;
            }
            List<string> ll = new List<string>();
            for (int i = 0; i < scfg.GetLength(1); i++) {
                ll.Add(scfg[vv,i]);
            }
            List<pk10data> lst = Contant.caijiall(data, sbegin[vv], send[vv], Contant.parseCfg(ll.ToArray()));
            if (lst == null) return false;
            list = lst;
            curnum = (long.Parse(list[0].num) + 1).ToString();
            /*int nsec = 0;
            int nStart = data.IndexOf("<th width=\"30%\">开奖时间</th>");
            if (nStart == -1)
            {
                nStart = data.IndexOf("success\",\"data\":");
                nsec = 1;
            }
            int nEnd = data.IndexOf("<div class=\"fc_fanye\">", nStart);
            if (nEnd == -1) nEnd = data.IndexOf("}]}", nStart);
            while (list.Count < 20) {
                if (nsec == 0)
                {
                    int nA = data.IndexOf("<td>", nStart) + "<td>".Length;
                    if (nA == -1 || nA > nEnd) break;
                    int nB = data.IndexOf("</td>", nA);
                    if (list.Count == 0)
                        curnum = (int.Parse(data.Substring(nA, nB - nA)) + 1).ToString();
                    string num = data.Substring(nA, nB - nA);
                    nA = data.IndexOf("<td>", nB) + "<td>".Length;
                    nB = data.IndexOf("</td>", nA);
                    string vl = data.Substring(nA, nB - nA);
                    nA = data.IndexOf("<td>", nB) + "<td>".Length;
                    nB = data.IndexOf("</td>", nA);
                    string time = data.Substring(nA, nB - nA);
                    time = time + ":00";
                    nStart = nB + 10;

                    pk10data d = new pk10data();
                    d.num = num;
                    d.data = vl;
                    d.time = time;
                    list.Add(d);
                }
                else if (nsec == 1)
                {
                    nStart = data.LastIndexOf("\"issue\":");
                    int nA = data.IndexOf("\"issue\":", nStart) + "\"issue\":".Length;
                    if (nA == -1 || nA > nEnd) break;
                    int nX = nA;
                    int nB = data.IndexOf(",", nA);
                    if (list.Count == 0)
                        curnum = (int.Parse(data.Substring(nA, nB - nA)) + 1).ToString();
                    string num = data.Substring(nA, nB - nA);
                    nA = data.IndexOf("\"code\":\"", nB) + "\"code\":\"".Length;
                    nB = data.IndexOf("\"", nA);
                    string xxx = data.Substring(nA, nB - nA);
                    nA = data.IndexOf("\"time\":\"", nB) + "\"time\":\"".Length;
                    nB = data.IndexOf("\"", nA);

                    string time = data.Substring(nA, nB - nA);
                    data = data.Substring(0, nX - "\"issue\":".Length);
                    nStart = data.LastIndexOf("\"issue\":");

                    pk10data d = new pk10data();
                    d.num = num;
                    d.data = xxx;
                    d.time = time;
                    list.Add(d);

                    if (nStart == -1) break;
                }
            }*/

            return true;
        }

        public pk10data GetLast() {
            if (_inget) return list[0];
            _inget = true;

            string data = null;
            int vv = 0;
            for (vv = 0;vv < urls.Length;++vv) {
                string url = urls[vv];
                data = GetData(String.Format(url, (long)(DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalMilliseconds));
                cururl = url;
                if (data != null) break;
            }
            //_inget = false;
            if (data == null) {
                _inget = false;
                return list[0];
            }
            List<string> ll = new List<string>();
            for (int i = 0; i < scfg.GetLength(1); i++) {
                ll.Add(scfg[vv,i]);
            }
            List<pk10data> lst = Contant.caijiall(data, sbegin[vv], send[vv], Contant.parseCfg(ll.ToArray()));
            if (lst == null)
            {
                _inget = false;
                return list[0];
            }
            _inget = false;
            return lst[0];
        }

        public FileStream getImg(pk10data data)
        {
            string[] pkdata = data.data.Split(',');
            System.Drawing.Image imgSrc2 = System.Drawing.Image.FromFile("data/pic/back_1.jpg");
            System.Drawing.Image imgba = System.Drawing.Image.FromFile("data/pic/back_2.jpg");
            System.Drawing.Image imgSrc = new System.Drawing.Bitmap(imgSrc2.Width, imgSrc2.Height + imgba.Height);
            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                g.DrawImage(imgSrc2, new Rectangle(0, 0,
                            imgSrc2.Width,
                            imgSrc2.Height), 0, 0, imgSrc2.Width, imgSrc2.Height, GraphicsUnit.Pixel);
            }
            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                g.DrawImage(imgba, new Rectangle(0,
                                                 0,
                                                 imgba.Width,
                                                 imgba.Height),
                        0, 0, imgba.Width, imgba.Height, GraphicsUnit.Pixel);
            }
            System.Drawing.Image img1 = System.Drawing.Image.FromFile((@"data/pic/m_2.png"));
            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                g.DrawImage(img1, new Rectangle(0 + (imgSrc.Width / 3 - img1.Width / 2) - 160,
                                                 imgSrc.Height / 5 - 30 + 80,
                                                 img1.Width,
                                                 img1.Height),
                        0, 0, img1.Width, img1.Height, GraphicsUnit.Pixel);
            }
            System.Drawing.Image img2 = System.Drawing.Image.FromFile((@"data/pic/m_1.png"));
            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                g.DrawImage(img2, new Rectangle(imgSrc.Width * 2/ 3 - (img1.Width / 2) - 160,
                                                 imgSrc.Height / 5 - 50 + 80,
                                                 img1.Width,
                                                 img1.Height),
                        0, 0, img2.Width, img2.Height, GraphicsUnit.Pixel);
            }
            System.Drawing.Image img3 = System.Drawing.Image.FromFile((@"data/pic/m_3.png"));
            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                g.DrawImage(img3, new Rectangle(imgSrc.Width * 3 / 3 - (img1.Width / 2) - 160,
                                                 imgSrc.Height / 5 - 30 + 80,
                                                 img1.Width,
                                                 img1.Height),
                        0, 0, img3.Width, img3.Height, GraphicsUnit.Pixel);
            }


            System.Drawing.Image img4 = System.Drawing.Image.FromFile((@"data/pic/car_" + pkdata[0]+".png"));
            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                g.DrawImage(img4, new Rectangle(0 + (imgSrc.Width * 2 / 3 - img4.Width / 2) - 160,
                                                 imgSrc.Height / 3 + 30 + 80,
                                                 img4.Width,
                                                 img4.Height),
                        0, 0, img4.Width, img4.Height, GraphicsUnit.Pixel);
            }

            System.Drawing.Image img5 = System.Drawing.Image.FromFile((@"data/pic/car_" + pkdata[1]+".png"));
            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                g.DrawImage(img5, new Rectangle(0 + (imgSrc.Width / 3 - img5.Width / 2) - 160,
                                                 imgSrc.Height / 3 + 80,
                                                 img5.Width,
                                                 img5.Height),
                        0, 0, img5.Width, img5.Height, GraphicsUnit.Pixel);
            }

            System.Drawing.Image img6 = System.Drawing.Image.FromFile((@"data/pic/car_" + pkdata[2]+".png"));
            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                g.DrawImage(img6, new Rectangle(0 + (imgSrc.Width * 3/ 3 - img6.Width / 2) - 160,
                                                 imgSrc.Height / 3 + 80,
                                                 img6.Width,
                                                 img6.Height),
                        0, 0, img6.Width, img6.Height, GraphicsUnit.Pixel);
            }
            for (int i = 0;i < pkdata.Length;++i)
            {
                System.Drawing.Image imgxx = System.Drawing.Image.FromFile((@"data/pic/num_" + pkdata[i]+".png"));
                using (Graphics g = Graphics.FromImage(imgSrc))
                {
                    g.DrawImage(imgxx, new Rectangle(i*(imgxx.Width) + 200,
                                                     40,
                                                     imgxx.Width,
                                                     imgxx.Height),
                            0, 0, imgxx.Width, imgxx.Height, GraphicsUnit.Pixel);
                }
            }
            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                g.DrawImage(imgba, new Rectangle(0, imgSrc2.Height,
                                                 imgba.Width,
                                                 imgba.Height),
                        0, 0, imgba.Width, imgba.Height, GraphicsUnit.Pixel);
            }
            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                Font font = new Font("Arial", 23);
                Brush brush = System.Drawing.Brushes.Yellow;
                g.DrawString(data.num + "期", font, brush, 20, 40);
                
                font = new Font("Arial", 18);
                brush = System.Drawing.Brushes.Yellow;
                g.DrawString(data.time + " " + data.num + "期", font, brush, 30, imgSrc2.Height + 50);
            }



            //MemoryStream ms = new MemoryStream();
            //imgSrc.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            imgSrc.Save("data/tmp.png");
            FileStream f = File.OpenRead("data/tmp.png");

            return f;
        }

        public FileStream getImg2()
        {
            int count = this.list.Count > 20 ? 20 : this.list.Count;
            System.Drawing.Image imgSrc = new System.Drawing.Bitmap(850 + 480, (count+1) * 66);

            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                Rectangle rect = new Rectangle(0, 0, imgSrc.Width, imgSrc.Height);
                g.FillRectangle(Brushes.White, rect);
            }
            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                Rectangle rect = new Rectangle(0, 0, imgSrc.Width, 66);
                g.FillRectangle(Brushes.IndianRed, rect);
            }
            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                Font font = new Font("Arial", 22);
                Brush brush = System.Drawing.Brushes.Black;

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.DrawString("期号", font, brush, new Rectangle(0, 0, 200, 66), sf);

                g.DrawString("开奖号码", font, brush, new Rectangle(200, 0, 600, 66), sf);

                g.DrawString("冠亚", font, brush, new Rectangle(850, 0, 180, 66), sf);

                g.DrawString("龙虎", font, brush, new Rectangle(1030, 0, 300, 66), sf);
                //g.DrawString(d.num, font, brush, 0, (i + 1) * 66 + 3);

                //g.DrawString(d.time, font, brush, 100, i * 66 + 3);
            }
            /*for (int i = 0;i < count;++i)
            {
                using (Graphics g = Graphics.FromImage(imgSrc))
                {
                    g.DrawLine(Pens.Gray, new Point(0, i*66), new Point(imgSrc.Width, i*66));
                }
            }

            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                g.DrawLine(Pens.Gray, new Point(300, 0), new Point(300, imgSrc.Height));
            }*/

            for (int i = 0; i < count; ++i) {
                pk10data d = this.list[i];
                using (Graphics g = Graphics.FromImage(imgSrc))
                {
                    Font font = new Font("Arial", 18);
                    Brush brush = System.Drawing.Brushes.Black;

                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString(d.num, font, brush, new Rectangle(0,(i+1)*66,200,66), sf);

                    //g.DrawString(d.num, font, brush, 0, (i + 1) * 66 + 3);

                    //g.DrawString(d.time, font, brush, 100, i * 66 + 3);
                }
                string[] pkdata = d.data.Split(',');
                int nGuanYa = 0;
                string[] longhu = new string[5];
                for (int x = 0; x < pkdata.Length; ++x)
                {
                    if (x == 0 || x == 1)
                    {
                        nGuanYa += int.Parse(pkdata[x]);
                    }
                    if (x < 5)
                    {
                        int nA = int.Parse(pkdata[x]);
                        int nB = int.Parse(pkdata[9 - x]);
                        longhu[x] = nA > nB ? "龙" : (nA == nB ? "和" : "虎");
                    }
                    System.Drawing.Image imgxx = System.Drawing.Image.FromFile((@"data/pic/num_" + pkdata[x] + ".png"));
                    using (Graphics g = Graphics.FromImage(imgSrc))
                    {
                        g.DrawImage(imgxx, new Rectangle(x * (imgxx.Width + 3) + 201,
                                                         (i+1)*66 +3,
                                                         imgxx.Width,
                                                         imgxx.Height),
                                0, 0, imgxx.Width, imgxx.Height, GraphicsUnit.Pixel);
                    }
                }
                using (Graphics g = Graphics.FromImage(imgSrc))
                {
                    Font font = new Font("Arial", 18, FontStyle.Bold);
                    Brush brush = System.Drawing.Brushes.Black;
                    Brush brushblue = System.Drawing.Brushes.Blue;
                    Brush brushpink = System.Drawing.Brushes.DeepPink;
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString(nGuanYa.ToString(), font, brush, new Rectangle(850, (i + 1) * 66, 60, 66), sf);

                    if (nGuanYa % 2 == 0)
                    {
                        g.DrawString("双", font, brushpink, new Rectangle(850 + 60, (i + 1) * 66, 60, 66), sf);
                    }
                    else
                    {
                        g.DrawString("单", font, brushblue, new Rectangle(850 + 60, (i + 1) * 66, 60, 66), sf);
                    }

                    if (nGuanYa >= 12)
                    {
                        g.DrawString("大", font, brushpink, new Rectangle(850 + 120, (i + 1) * 66, 60, 66), sf);
                    }
                    else
                    {
                        g.DrawString("小", font, brushblue, new Rectangle(850 + 120, (i + 1) * 66, 60, 66), sf);
                    }

                    for (int xx = 0; xx < 5; ++xx)
                    {
                        if (longhu[xx] == "龙")
                            g.DrawString(longhu[xx], font, brushpink, new Rectangle(850 + 180 + (xx*60), (i + 1) * 66, 60, 66), sf);
                        else
                            g.DrawString(longhu[xx], font, brushblue, new Rectangle(850 + 180 + (xx * 60), (i + 1) * 66, 60, 66), sf);
                    }
                }
            }

            //MemoryStream ms = new MemoryStream();
            //imgSrc.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            imgSrc.Save("data/tmp.png");
            FileStream f = File.OpenRead("data/tmp.png");
            
            return f;
        }

        public string GetData(string url) {
            try
            {

                WebClientto MyWebClient = new WebClientto(15000);

                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据

                Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据

                string pageHtml = Encoding.UTF8.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句            

                return pageHtml;

            }

            catch (WebException webEx)
            {

                return null;

            }
        }

        public List<result> Over(List<atkMsg> atks)
        {
            string data = list[0].data;
            if (data == null || data == "") return null;
            string[] pkdata = data.Split(',');
            List<result> lsrs = new List<result>();

            foreach (atkMsg atk in atks)
            {
                foreach(string sm in atk.main)
                {
                    if (sm != "冠亚") {
                        if (int.Parse(sm) < 1 || int.Parse(sm) > 10) continue;
                    }
                        
                    
                    int nSum = sm == "冠亚" ? int.Parse(pkdata[0]) + int.Parse(pkdata[1]) : int.Parse(pkdata[int.Parse(sm) - 1]);
                    float peilv = 0.0f;
                    foreach (string ss in atk.coin)
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
                                }
                                bWin = nSum > 11;
                            } else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第"+ int.Parse(sm) + "名大'");
                                if (dt.Rows.Count > 0)
                                {
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                bWin = nSum > 5;
                            }
                        }
                        else if (ss == "小")
                        {
                            if (sm == "冠亚")
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚小'");
                                if (dt.Rows.Count > 0)
                                {
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                bWin = nSum <= 11;
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名小'");
                                if (dt.Rows.Count > 0)
                                {
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                                bWin = nSum <= 5;
                            }
                        }
                        else if (ss == "单")
                        {
                            bWin = (nSum % 2 == 1);
                            if (sm == "冠亚")
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚"+ss+"'");
                                if (dt.Rows.Count > 0)
                                {
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名"+ss+"'");
                                if (dt.Rows.Count > 0)
                                {
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
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
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                            }
                        }
                        else if (ss == "龙")
                        {
                            int iindex = 10 - int.Parse(sm);
                            int nSum2 = int.Parse(pkdata[iindex]);
                            if (nSum > nSum2)
                            {
                                bWin = true;
                            } else
                            {
                                bWin = false;
                            }
                            if (sm == "冠亚")
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                            }
                        }
                        else if (ss == "虎")
                        {
                            int iindex = 10 - int.Parse(sm);
                            int nSum2 = int.Parse(pkdata[iindex]);
                            if (nSum > nSum2)
                            {
                                bWin = true;
                            } else
                            {
                                bWin = false;
                            }
                            if (sm == "冠亚")
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='冠亚" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + ss + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
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
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                            }
                            else
                            {
                                DataTable dt = Reader.Instance().ReadGameData("select * from peilv_pk10 where 内容='第" + int.Parse(sm) + "名" + num + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    peilv = float.Parse((string)dt.Rows[0]["赔率"]);
                                }
                            }
                        }

                        if (bWin)
                        {
                            result rrs = null;
                            foreach(result rs in lsrs)
                            {
                                if (rs.player == atk.player)
                                {
                                    rrs = rs;
                                    break;
                                }
                            }
                            if (rrs == null)
                            {
                                rrs = new result();
                                lsrs.Add(rrs);
                            }
                            rrs.player = atk.player;
                            rrs.nScore += atk.used*peilv;
                            rrs.sContent += "#" +atk.content;


                        }
                    }
                }
            }
            return lsrs;
        }
    }
}
