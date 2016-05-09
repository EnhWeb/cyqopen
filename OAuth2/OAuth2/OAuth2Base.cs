using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using CYQ.Data.Table;
namespace OAuth2
{
    /// <summary>
    /// 授权基类
    /// </summary>
    public abstract class OAuth2Base : ICloneable
    {
        protected WebClient wc = new WebClient();
        public OAuth2Base()
        {
            wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            wc.Headers.Add("Pragma", "no-cache");
        }
        #region 基础属性
        /// <summary>
        /// 返回的开放ID。
        /// </summary>
        public string openID = string.Empty;
        /// <summary>
        /// 访问的Token
        /// </summary>
        public string token = string.Empty;
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime expiresTime;

        /// <summary>
        /// 第三方账号昵称
        /// </summary>
        public string nickName = string.Empty;

        /// <summary>
        /// 第三方账号头像地址
        /// </summary>
        public string headUrl = string.Empty;
        /// <summary>
        /// 首次请求时返回的Code
        /// </summary>
        internal string code = string.Empty;
        internal abstract OAuthServer Server
        {
            get;
        }
        #endregion

        #region 非公开的请求路径和Logo图片地址。

        internal abstract string OAuthUrl
        {
            get;
        }
        internal abstract string TokenUrl
        {
            get;
        }
        internal abstract string ImgUrl
        {
            get;
        }
        internal virtual string Para
        {
            get
            {

                string para = "grant_type=authorization_code&client_id=" + AppKey + "&client_secret=" + AppSercet + "&code=" + code + "&state=" + Server;
                para += "&redirect_uri=" + System.Web.HttpUtility.UrlEncode(CallbackUrl) + "&rnd=" + DateTime.Now.Second;
                return para;
            }
        }
        #endregion

        #region WebConfig对应的配置【AppKey、AppSercet、CallbackUrl】
        internal string AppKey
        {
            get
            {
                return Tool.GetConfig(Server.ToString() + ".AppKey");
            }
        }
        internal string AppSercet
        {
            get
            {
                return Tool.GetConfig(Server.ToString() + ".AppSercet");
            }
        }
        internal string CallbackUrl
        {
            get
            {
                return Tool.GetConfig(Server.ToString() + ".CallbackUrl");
            }
        }
        #endregion

        #region 基础方法

        /// <summary>
        /// 获得Token
        /// </summary>
        /// <returns></returns>
        protected string GetToken(string method)
        {
            string result = string.Empty;
            try
            {
                if (method == "POST")
                {
                    if (string.IsNullOrEmpty(wc.Headers["Content-Type"]))
                    {
                        wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    }
                    result = wc.UploadString(TokenUrl, method, Para);
                }
                else
                {
                    result = wc.DownloadString(TokenUrl + "?" + Para);
                }
            }
            catch (Exception err)
            {
                CYQ.Data.Log.WriteLogToTxt(err);
            }
            return result;
        }
        /// <summary>
        /// 获取是否通过授权。
        /// </summary>
        public abstract bool Authorize();
        #endregion

       

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    /// <summary>
    /// 提供授权的服务商
    /// </summary>
    public enum OAuthServer
    {
        /// <summary>
        /// 新浪微博
        /// </summary>
        SinaWeiBo,
        /// <summary>
        /// 腾讯QQ
        /// </summary>
        QQ,
        /// <summary>
        /// 淘宝网
        /// </summary>
        TaoBao,
        /// <summary>
        /// 微信
        /// </summary>
        WeiXin,
        /// <summary>
        /// 
        /// </summary>
        None,
    }
}
