using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace WeChat.NET.HTTP
{
    /// <summary>
    /// 微信主要业务逻辑服务类
    /// </summary>
    class WXService
    {
        private Dictionary<string, string> _syncKey = new Dictionary<string, string>();

        //微信初始化url
        private static string _init_url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxinit?r={0}";
        //获取好友头像
        private static string _geticon_url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxgeticon?username=";
        //获取群聊（组）头像
        private static string _getheadimg_url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxgetheadimg?username=";
        //获取好友列表
        private static string _getcontact_url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxgetcontact";

        private static string _getbatchcontact_url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxbatchgetcontact?type=ex&r={0}";
        //同步检查url
        private static string _synccheck_url = "https://webpush.weixin.qq.com/cgi-bin/mmwebwx-bin/synccheck?sid={0}&uin={1}&synckey={2}&r={3}&skey={4}&deviceid={5}";
        //同步url
        private static string _sync_url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxsync?sid=";
        //发送消息url
        private static string _sendmsg_url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxsendmsg?sid=";
        //微信初始化url
        private static string _init_url2 = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxinit?r={0}";
        //获取好友头像
        private static string _geticon_url2 = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxgeticon?username=";
        //获取群聊（组）头像
        private static string _getheadimg_url2 = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxgetheadimg?username=";
        //获取好友列表
        private static string _getcontact_url2 = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxgetcontact";

        private static string _getbatchcontact_url2 = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxbatchgetcontact?type=ex&r={0}";
        //同步检查url
        private static string _synccheck_url2 = "https://webpush2.weixin.qq.com/cgi-bin/mmwebwx-bin/synccheck?sid={0}&uin={1}&synckey={2}&r={3}&skey={4}&deviceid={5}";
        //同步url
        private static string _sync_url2 = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxsync?sid=";
        //发送消息url
        private static string _sendmsg_url2 = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxsendmsg?sid=";

        private static string _upload_url = "https://file.wx.qq.com/cgi-bin/mmwebwx-bin/webwxuploadmedia?f=json";

        private static string _upload_url2 = "https://file.wx2.qq.com/cgi-bin/mmwebwx-bin/webwxuploadmedia?f=json";

        private static string _sendimg_url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxsendmsgimg?fun=async&f=json&sid=";

        private static string _sendimg_url2 = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxsendmsgimg?fun=async&f=json&sid=";

        private BaseService _BaseService = null;

        private LoginService _LoginService = null;

        private bool _bOther = false;
		
		public void Init(BaseService service, LoginService lservice)
		{
			_BaseService = service;
            _LoginService = lservice;
		}
        /// <summary>
        /// 微信初始化
        /// </summary>
        /// <returns></returns>
        public JObject WxInit(bool bother)
        {
            try
            {
                _bOther = bother;
                string init_json = "{{\"BaseRequest\":{{\"Uin\":\"{0}\",\"Sid\":\"{1}\",\"Skey\":\"\",\"DeviceID\":\"e1615250492\"}}}}";
                Cookie sid = _BaseService.GetCookie("wxsid");
                Cookie uin = _BaseService.GetCookie("wxuin");

                if (sid != null && uin != null)
                {
                    init_json = string.Format(init_json, uin.Value, sid.Value);
                    string burl = string.Format((_bOther ? _init_url2 : _init_url), (long)(DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalMilliseconds);
                    byte[] bytes = _BaseService.SendPostRequest((_bOther?_init_url2: _init_url) + "&pass_ticket=" + _LoginService.Pass_Ticket, init_json);
                    string init_str = Encoding.UTF8.GetString(bytes);

                    JObject init_result = JsonConvert.DeserializeObject(init_str) as JObject;

                    foreach (JObject synckey in init_result["SyncKey"]["List"])  //同步键值
                    {
                        _syncKey.Add(synckey["Key"].ToString(), synckey["Val"].ToString());
                    }
                    return init_result;
                }
                else
                {
                    return null;
                }
            } catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取好友头像
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Image GetIcon(string username)
        {
            try
            {
                byte[] bytes = _BaseService.SendGetRequest((_bOther?_geticon_url2: _geticon_url) + username);

                return Image.FromStream(new MemoryStream(bytes));
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取微信讨论组头像
        /// </summary>
        /// <param name="usename"></param>
        /// <returns></returns>
        public Image GetHeadImg(string usename)
        {
            try
            {
                byte[] bytes = _BaseService.SendGetRequest((_bOther?_getheadimg_url2: _getheadimg_url) + usename);

                return Image.FromStream(new MemoryStream(bytes));
            } catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <returns></returns>
        public JObject GetContact()
        {
            try
            {
                byte[] bytes = _BaseService.SendGetRequest((_bOther ? _getcontact_url2 : _getcontact_url));
                string contact_str = Encoding.UTF8.GetString(bytes);

                return JsonConvert.DeserializeObject(contact_str) as JObject;
            } catch
            {
                return null;
            }
        }
        public JObject GetBatchContact(string str)
        {
            try
            {
                Cookie sid = _BaseService.GetCookie("wxsid");
                Cookie uin = _BaseService.GetCookie("wxuin");
                string str2 = str;
                str2 = str2.Replace("{0}", uin.Value).Replace("{1}", sid.Value).Replace("{2}", _LoginService.SKey);
                string uel = string.Format((_bOther ? _getbatchcontact_url2 : _getbatchcontact_url), (long)(DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalMilliseconds);
                byte[] bytes = _BaseService.SendPostRequest(uel + "&pass_ticket=" + _LoginService.Pass_Ticket, str2);
                string contact_str = Encoding.UTF8.GetString(bytes);

                return JsonConvert.DeserializeObject(contact_str) as JObject;
            } catch
            {
                return null;
            }
        }
        /// <summary>
        /// 微信同步检测
        /// </summary>
        /// <returns></returns>
        public string WxSyncCheck()
        {
            try
            {
                string sync_key = "";
                foreach (KeyValuePair<string, string> p in _syncKey)
                {
                    sync_key += p.Key + "_" + p.Value + "%7C";
                }
                sync_key = sync_key.TrimEnd('%', '7', 'C');

                Cookie sid = _BaseService.GetCookie("wxsid");
                Cookie uin = _BaseService.GetCookie("wxuin");

                if (sid != null && uin != null)
                {
                    string synccheck_url = string.Format((_bOther ? _synccheck_url2 : _synccheck_url), sid.Value, uin.Value, sync_key, (long)(DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalMilliseconds, _LoginService.SKey.Replace("@", "%40"), "e1615250492");

                    byte[] bytes = _BaseService.SendGetRequest(synccheck_url + "&_=" + DateTime.Now.Ticks);
                    if (bytes != null)
                    {
                        return Encoding.UTF8.GetString(bytes);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
        /// <summary>
        /// 微信同步
        /// </summary>
        /// <returns></returns>
        public JObject WxSync()
        {
            try
            {
                string sync_json = "{{\"BaseRequest\" : {{\"DeviceID\":\"e1615250492\",\"Sid\":\"{1}\", \"Skey\":\"{5}\", \"Uin\":\"{0}\"}},\"SyncKey\" : {{\"Count\":{2},\"List\":[{3}]}},\"rr\" :{4}}}";
                Cookie sid = _BaseService.GetCookie("wxsid");
                Cookie uin = _BaseService.GetCookie("wxuin");

                string sync_keys = "";
                foreach (KeyValuePair<string, string> p in _syncKey)
                {
                    sync_keys += "{\"Key\":" + p.Key + ",\"Val\":" + p.Value + "},";
                }
                sync_keys = sync_keys.TrimEnd(',');
                sync_json = string.Format(sync_json, uin.Value, sid.Value, _syncKey.Count, sync_keys, (long)(DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalMilliseconds, _LoginService.SKey);

                if (sid != null && uin != null)
                {
                    byte[] bytes = _BaseService.SendPostRequest((_bOther ? _sync_url2 : _sync_url) + sid.Value + "&lang=zh_CN&skey=" + _LoginService.SKey + "&pass_ticket=" + _LoginService.Pass_Ticket, sync_json);
                    if (bytes == null) return null;

                    string sync_str = Encoding.UTF8.GetString(bytes);

                    JObject sync_resul = JsonConvert.DeserializeObject(sync_str) as JObject;

                    if (sync_resul["SyncKey"]["Count"].ToString() != "0")
                    {
                        _syncKey.Clear();
                        foreach (JObject key in sync_resul["SyncKey"]["List"])
                        {
                            _syncKey.Add(key["Key"].ToString(), key["Val"].ToString());
                        }
                    }
                    return sync_resul;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="type"></param>
        public void SendMsg(string msg, string from, string to, int type)
        {
            try
            {
                string msg_json = "{{" +
                "\"BaseRequest\":{{" +
                    "\"DeviceID\" : \"e441551176\"," +
                    "\"Sid\" : \"{0}\"," +
                    "\"Skey\" : \"{6}\"," +
                    "\"Uin\" : \"{1}\"" +
                "}}," +
                "\"Msg\" : {{" +
                    "\"ClientMsgId\" : {8}," +
                    "\"Content\" : \"{2}\"," +
                    "\"FromUserName\" : \"{3}\"," +
                    "\"LocalID\" : {9}," +
                    "\"ToUserName\" : \"{4}\"," +
                    "\"Type\" : {5}" +
                "}}," +
                "\"rr\" : {7}" +
                "}}";

                Cookie sid = _BaseService.GetCookie("wxsid");
                Cookie uin = _BaseService.GetCookie("wxuin");

                if (sid != null && uin != null)
                {
                    msg_json = string.Format(msg_json, sid.Value, uin.Value, msg, from, to, type, _LoginService.SKey, DateTime.Now.Millisecond, DateTime.Now.Millisecond, DateTime.Now.Millisecond);

                    byte[] bytes = _BaseService.SendPostRequest((_bOther ? _sendmsg_url2 : _sendmsg_url) + sid.Value + "&lang=zh_CN&pass_ticket=" + _LoginService.Pass_Ticket, msg_json);

                    string send_result = Encoding.UTF8.GetString(bytes);
                }
            }
            catch (Exception e)
            {

            }

        }

         public string sendImg(FileStream bts, string from, string touser)
         {
             String boundary = "------WebKitFormBoundaryPXiOtJhXoRRhLn4y"; //区分每个参数之间
             String freFix = "--";
             String newLine = "\r\n";

             string json = "";

             json = freFix + boundary;
             json = json + newLine;
             json = json + "Content-Disposition: form-data; name=\"id\"";
             json = json + newLine;
             json = json + newLine;

             json = json + "WU_FILE_0";
             json = json + newLine;
             json = json + freFix;
             json = json + boundary;
             json = json + newLine;
             json = json + "Content-Disposition: form-data; name=\"name\"";
             json = json + newLine;
             json = json + newLine;

             json = json + "untitle.png";
            json = json + newLine;
            json = json + freFix;
            json = json + boundary;
            json = json + newLine;
            json = json + "Content-Disposition: form-data; name=\"filename\"";
            json = json + newLine;
            json = json + newLine;

            json = json + "untitle.png";
            json = json + newLine;
             json = json + freFix;
             json = json + boundary;
             json = json + newLine;
             json = json + "Content-Disposition: form-data; name=\"type\"";
             json = json + newLine;
             json = json + newLine;

             json = json + "image/png";
             json = json + newLine;
             json = json + freFix;
             json = json + boundary;
             json = json + newLine;
             json = json + "Content-Disposition: form-data; name=\"lastModifiedDate\"";
             json = json + newLine;
             json = json + newLine;

             json = json + "Tue Feb 14 2017 22:07:03 GMT+0800";
             json = json + newLine;
             json = json + freFix;
             json = json + boundary;
             json = json + newLine;
             json = json + "Content-Disposition: form-data; name=\"size\"";
             json = json + newLine;
             json = json + newLine;

             json = json + bts.Length.ToString();
             json = json + newLine;
             json = json + freFix;
             json = json + boundary;
             json = json + newLine;
             json = json + "Content-Disposition: form-data; name=\"mediatype\"";
             json = json + newLine;
             json = json + newLine;

             json = json + "pic";
             json = json + newLine;
             json = json + freFix;
             json = json + boundary;
             json = json + newLine;
             json = json + "Content-Disposition: form-data; name=\"uploadmediarequest\"";
             json = json + newLine;
             json = json + newLine;

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash_byte = md5.ComputeHash(bts);
            string strMd5 = System.BitConverter.ToString(hash_byte);
            strMd5 = strMd5.Replace("-", "");
            Cookie sid = _BaseService.GetCookie("wxsid");
             Cookie uin = _BaseService.GetCookie("wxuin");
             json = json + "{\"UploadType\":2,\"BaseRequest\":{\"Uin\":" + uin.Value + ",\"Sid\":\"" + sid.Value + "\",\"Skey\":\"" + _LoginService.SKey + "\",\"DeviceID\":\"e441551176\"},\"ClientMediaId\":" + (long)(DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalMilliseconds + ",\"TotalLen\":" + bts.Length.ToString() + ",\"StartPos\":0,\"DataLen\":" + bts.Length.ToString() + ",\"MediaType\":4,\"FromUserName\":\"" + from + "\",\"ToUserName\":\"filehelper\",\"FileMd5\":\""+ strMd5 + "\"}";
             json = json + newLine;
             json = json + freFix;
             json = json + boundary;
             json = json + newLine;
             json = json + "Content-Disposition: form-data; name=\"webwx_data_ticket\"";
             json = json + newLine;
             json = json + newLine;

             Cookie wxdata = _BaseService.GetCookie("webwx_data_ticket");
             json = json + wxdata.Value;
             json = json + newLine;
             json = json + freFix;
             json = json + boundary;
             json = json + newLine;
             json = json + "Content-Disposition: form-data; name=\"pass_ticket\"";
             json = json + newLine;
             json = json + newLine;

             json = json + "undefined";
             json = json + newLine;
             json = json + freFix;
             json = json + boundary;
             json = json + newLine;
             json = json + "Content-Disposition: form-data; name=\"filename\"; filename=\"untitle.png\"";
             json = json + newLine;
             json = json + "Content-Type: application/octet-stream";
             json = json + newLine;
             json = json + newLine;

            

             string jsonend = "";
             jsonend = jsonend + newLine;
             jsonend = jsonend + freFix;
             jsonend = jsonend + boundary;
            jsonend = jsonend + freFix;
            jsonend = jsonend + newLine;

            try
            {
                if (sid != null && uin != null)
                {
                    //string burl = string.Format((_bOther ? _init_url2 : _init_url), (long)(DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalMilliseconds);
                    byte[] bytes = _BaseService.SendPostImgRequest((_bOther ? _upload_url2 : _upload_url),"", json, bts, jsonend);
                    string init_str = Encoding.UTF8.GetString(bytes);

                    JObject init_result = JsonConvert.DeserializeObject(init_str) as JObject;
                    string mid = init_result["MediaId"].ToString();

                    if (mid != "") {

                            string msg_json = "{{" +
                            "\"BaseRequest\":{{" +
                                "\"DeviceID\" : \"e441551176\"," +
                                "\"Sid\" : \"{0}\"," +
                                "\"Skey\" : \"{6}\"," +
                                "\"Uin\" : \"{1}\"" +
                            "}}," +
                            "\"Msg\" : {{" +
                                "\"ClientMsgId\" : {8}," +
                                "\"Content\" : \"{2}\"," +
                                "\"FromUserName\" : \"{3}\"," +
                                "\"LocalID\" : {9}," +
                                "\"MediaId\" : \"{10}\"," +
                                "\"ToUserName\" : \"{4}\"," +
                                "\"Type\" : {5}" +
                            "}}," +
                            "\"Scene\" : {7}" +
                            "}}";


                            if (sid != null && uin != null)
                            {
                                msg_json = string.Format(msg_json, sid.Value, uin.Value, "", from, touser, 3, _LoginService.SKey, 0, (long)(DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalMilliseconds, (long)(DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalMilliseconds, mid);

                                byte[] bytes2 = _BaseService.SendPostRequest((_bOther ? _sendimg_url2 : _sendimg_url) + sid.Value + "&lang=zh_CN&pass_ticket=" + _LoginService.Pass_Ticket, msg_json);

                                string send_result = Encoding.UTF8.GetString(bytes2);

                                return send_result;
                            }

                    }

                    return null;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
