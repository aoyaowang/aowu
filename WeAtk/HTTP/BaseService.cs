using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeChat.NET.HTTP
{
    public class WebClientWx : WebClient
    {
        /// <summary>  
        /// 过期时间  
        /// </summary>  
        public int Timeout { get; set; }
        public CookieContainer _cookie {get; set;}
        public WebClientWx(int timeout, CookieContainer cookie)
        {
            Timeout = timeout;
            _cookie = cookie;
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

            List<Cookie> cookies = GetAllCookies(_cookie);
            for (int i = 0; i < cookies.Count; ++i) {
                _cookie.SetCookies(address, cookies[i].Name + "=" + cookies[i].Value);
            }
            request.CookieContainer = _cookie;  //启用cookie

            return request;
        }

        private List<Cookie> GetAllCookies(CookieContainer cc)
        {
            List<Cookie> lstCookies = new List<Cookie>();

            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cc, new object[] { });

            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies) lstCookies.Add(c);
            }
            return lstCookies;
        }

    }
    /// <summary>
    /// 访问http服务器类
    /// </summary>
    class BaseService
    {
        /// <summary>
        /// 访问服务器时的cookies
        /// </summary>
        public CookieContainer _CookiesContainer = null;
        /// <summary>
        /// 向服务器发送get请求  返回服务器回复数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public byte[] SendGetRequest(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "get";

                if (_CookiesContainer == null)
                {
                    request.CookieContainer = new CookieContainer();
                    _CookiesContainer = request.CookieContainer;
                }

                

                if (url.IndexOf("weixin.qq.com") >= 0) {
                    Uri uri = new Uri(url);
                    List<Cookie> cookies = GetAllCookies(_CookiesContainer);
                    for (int i = 0; i < cookies.Count; ++i) {
                        _CookiesContainer.SetCookies(uri, cookies[i].Name + "=" + cookies[i].Value);
                    }
                }
                request.CookieContainer = _CookiesContainer;  //启用cookie
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream response_stream = response.GetResponseStream();
                _CookiesContainer = request.CookieContainer;
                int count = (int)response.ContentLength;
                int offset = 0;
                byte[] buf = new byte[count];
                while (count > 0)  //读取返回数据
                {
                    int n = response_stream.Read(buf, offset, count);
                    if (n == 0) break;
                    count -= n;
                    offset += n;
                }
                return buf;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 向服务器发送post请求 返回服务器回复数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public byte[] SendPostRequest(string url, string body)
        {
            try
            {
                byte[] request_body = Encoding.UTF8.GetBytes(body);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "post";
                request.ContentLength = request_body.Length;

                Stream request_stream = request.GetRequestStream();

                request_stream.Write(request_body, 0, request_body.Length);

                if (_CookiesContainer == null)
                {
                    request.CookieContainer = new CookieContainer();
                    _CookiesContainer = request.CookieContainer;
                }

                request.CookieContainer = _CookiesContainer;  //启用cookie

                if (url.IndexOf("weixin.qq.com") >= 0)
                {
                    Uri uri = new Uri(url);
                    List<Cookie> cookies = GetAllCookies(_CookiesContainer);
                    for (int i = 0; i < cookies.Count; ++i)
                    {
                        _CookiesContainer.SetCookies(uri, cookies[i].Name + "=" + cookies[i].Value);
                    }
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream response_stream = response.GetResponseStream();
                _CookiesContainer = request.CookieContainer;
                int count = (int)response.ContentLength;
                int offset = 0;
                byte[] buf = new byte[count];
                while (count > 0)  //读取返回数据
                {
                    int n = response_stream.Read(buf, offset, count);
                    if (n == 0) break;
                    count -= n;
                    offset += n;
                }
                return buf;
            }
            catch
            {
                return null;
            }
        }

        public byte[] SendPostImgRequest(string url, string body, string begin, FileStream file, string end)
        {
            /*if (_CookiesContainer == null)
            {
                _CookiesContainer = new CookieContainer();
            }
            WebClientWx wxClient = new WebClientWx(5000, _CookiesContainer);

            wxClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            
            int nAll = 0;
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(begin);
            nAll = byteArray.Length;
            nAll += file.Length;
            byteArray = System.Text.Encoding.Default.GetBytes(end);
            nAll += byteArray.Length;
            
            byte[] allBy = new byte[nAll];
            byteArray = System.Text.Encoding.Default.GetBytes(begin);
            for (int i = 0; i < byteArray.Length; ++i)
            {
                allBy[i] = byteArray[i];
            }
            for (int i = byteArray.Length;i < byteArray.Length + file.Length;++i) {
                allBy[i] = file[i - byteArray.Length];
            }
            byte[] byteArray2 = System.Text.Encoding.Default.GetBytes(end);
            for (int i = byteArray.Length + file.Length;i < byteArray.Length + file.Length + byteArray2.Length;++i) {
                allBy[i] = byteArray2[i - byteArray.Length - file.Length];
            }

            byte[] buf = wxClient.UploadData(url, allBy);
            return buf;*/
            try
            {
                //byte[] request_body = Encoding.UTF8.GetBytes(body);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "post";
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(begin);
                request.ContentLength = byteArray.Length;
                request.ContentLength += file.Length;
                byteArray = System.Text.Encoding.UTF8.GetBytes(end);
                request.ContentLength += byteArray.Length;
                request.ContentType = "multipart/form-data; boundary=------WebKitFormBoundaryPXiOtJhXoRRhLn4y";

                Uri uri = new Uri(url);
                List<Cookie> cookies = GetAllCookies(_CookiesContainer);
                for (int i = 0; i < cookies.Count; ++i)
                {
                    _CookiesContainer.SetCookies(uri, cookies[i].Name + "=" + cookies[i].Value);
                }
                request.CookieContainer = _CookiesContainer;  //启用cookie

                Stream postStream =request.GetRequestStream();
                byteArray = System.Text.Encoding.UTF8.GetBytes(begin);
                postStream.Write(byteArray, 0, byteArray.Length);
                byte[] buffer = new byte[1024];
                int nAll = 0;
                int bytesRead = 0;
                file.Position = 0;
                while ((bytesRead = file.Read(buffer, 0, buffer.Length)) != 0)
                {
                    postStream.Write(buffer, 0, bytesRead);
                    nAll += bytesRead;
                }
                byteArray = System.Text.Encoding.UTF8.GetBytes(end);
                postStream.Write(byteArray, 0, byteArray.Length);
                file.Close();


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream response_stream = response.GetResponseStream();
                _CookiesContainer = request.CookieContainer;
                int count = (int)response.ContentLength;
                int offset = 0;
                byte[] buf = new byte[count];
                while (count > 0)  //读取返回数据
                {
                    int n = response_stream.Read(buf, offset, count);
                    if (n == 0) break;
                    count -= n;
                    offset += n;
                }
                return buf;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取指定cookie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Cookie GetCookie(string name)
        {
            List<Cookie> cookies = GetAllCookies(_CookiesContainer);
            foreach (Cookie c in cookies)
            {
                if (c.Name == name)
                {
                    return c;
                }
            }
            return null;
        }

        private List<Cookie> GetAllCookies(CookieContainer cc)
        {
            List<Cookie> lstCookies = new List<Cookie>();

            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cc, new object[] { });

            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies) lstCookies.Add(c);
            }
            return lstCookies;
        }

        public static string GetValue(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }

    }
}
