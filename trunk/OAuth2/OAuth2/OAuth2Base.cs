using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using CYQ.Data.Table;
namespace OAuth2
{
    /// <summary>
    /// ��Ȩ����
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
        #region ��������
        /// <summary>
        /// ���صĿ���ID��
        /// </summary>
        public string openID = string.Empty;
        /// <summary>
        /// ���ʵ�Token
        /// </summary>
        public string token = string.Empty;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime expiresTime;

        /// <summary>
        /// �������˺��ǳ�
        /// </summary>
        public string nickName = string.Empty;

        /// <summary>
        /// �������˺�ͷ���ַ
        /// </summary>
        public string headUrl = string.Empty;
        /// <summary>
        /// �״�����ʱ���ص�Code
        /// </summary>
        internal string code = string.Empty;
        internal abstract OAuthServer Server
        {
            get;
        }
        #endregion

        #region �ǹ���������·����LogoͼƬ��ַ��

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

        #region WebConfig��Ӧ�����á�AppKey��AppSercet��CallbackUrl��
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

        #region ��������

        /// <summary>
        /// ���Token
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
        /// ��ȡ�Ƿ�ͨ����Ȩ��
        /// </summary>
        public abstract bool Authorize();
        #endregion

       

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    /// <summary>
    /// �ṩ��Ȩ�ķ�����
    /// </summary>
    public enum OAuthServer
    {
        /// <summary>
        /// ����΢��
        /// </summary>
        SinaWeiBo,
        /// <summary>
        /// ��ѶQQ
        /// </summary>
        QQ,
        /// <summary>
        /// �Ա���
        /// </summary>
        TaoBao,
        /// <summary>
        /// ΢��
        /// </summary>
        WeiXin,
        /// <summary>
        /// 
        /// </summary>
        None,
    }
}
