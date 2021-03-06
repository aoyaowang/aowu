﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace WeChat.NET.HTTP
{
    /// <summary>
    /// 微信登录服务类
    /// </summary>
    class LoginService
    {
        public string Pass_Ticket = "";
        public string SKey = "";
        private string _session_id = null;

        //获取会话ID的URL
        private static string _session_id_url = "https://login.weixin.qq.com/jslogin?appid=wx782c26e4c19acffb";
        //获取二维码的URL
        private static string _qrcode_url = "https://login.weixin.qq.com/qrcode/"; //后面增加会话id
        //判断二维码扫描情况   200表示扫描登录  201表示已扫描未登录  其它表示未扫描
        private static string _login_check_url = "https://login.weixin.qq.com/cgi-bin/mmwebwx-bin/login?loginicon=true&uuid="; //后面增加会话id

		private BaseService _BaseService = null;
		
		public void Init(BaseService service)
		{
			_BaseService = service;
		}
        /// <summary>
        /// 获取登录二维码
        /// </summary>
        /// <returns></returns>
        public Image GetQRCode()
        {

            try
            {
                byte[] bytes = _BaseService.SendGetRequest(_session_id_url);
                _session_id = Encoding.UTF8.GetString(bytes).Split(new string[] { "\"" }, StringSplitOptions.None)[1];
                bytes = _BaseService.SendGetRequest(_qrcode_url + _session_id);

                return Image.FromStream(new MemoryStream(bytes));
            }
            catch (Exception e) {
                return null;
            }

        }
        /// <summary>
        /// 登录扫描检测
        /// </summary>
        /// <returns></returns>
        public object LoginCheck()
        {
            try
            {
                byte[] bytes = _BaseService.SendGetRequest(_login_check_url + _session_id);
                string login_result = Encoding.UTF8.GetString(bytes);
                if (login_result.Contains("=201")) //已扫描 未登录
                {
                    if (login_result.Split(new string[] { "\'" }, StringSplitOptions.None).Length > 1)
                    {
                        string base64_image = login_result.Split(new string[] { "\'" }, StringSplitOptions.None)[1].Split(',')[1];
                        byte[] base64_image_bytes = Convert.FromBase64String(base64_image);
                        MemoryStream memoryStream = new MemoryStream(base64_image_bytes, 0, base64_image_bytes.Length);
                        memoryStream.Write(base64_image_bytes, 0, base64_image_bytes.Length);
                        //转成图片
                        return Image.FromStream(memoryStream);
                    }
                    else
                    {
                        return null;
                    }

                }
                else if (login_result.Contains("=200"))  //已扫描 已登录
                {
                    string login_redirect_url = login_result.Split(new string[] { "\"" }, StringSplitOptions.None)[1];
                    return login_redirect_url;
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
            if (_session_id == null)
            {
                return null;
            }

        }
        /// <summary>
        /// 获取sid uid   结果存放在cookies中
        /// </summary>
        public void GetSidUid(string login_redirect)
        {
            try{
                byte[] bytes = _BaseService.SendGetRequest(login_redirect + "&fun=new");
                string pass_ticket = Encoding.UTF8.GetString(bytes);
                Pass_Ticket = pass_ticket.Split(new string[] { "pass_ticket" }, StringSplitOptions.None)[1].TrimStart('>').TrimEnd('<', '/');
                SKey = pass_ticket.Split(new string[] { "skey" }, StringSplitOptions.None)[1].TrimStart('>').TrimEnd('<', '/');
            }
            catch(Exception e)
            {
                
            }

        }
    }
}
