using CYQ.Data.Cache;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
namespace OAuth2
{
    /// <summary>
    /// ��Ȩ������
    /// </summary>
    public static class OAuth2Factory
    {
        /// <summary>
        /// ��ȡ��ǰ����Ȩ���͡�
        /// </summary>
        public static OAuth2Base Create()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                string url = HttpContext.Current.Request.Url.Query;
                if (url.IndexOf("state=") > -1)
                {
                    string code = Tool.QueryString(url, "code");
                    string state = Tool.QueryString(url, "state");
                    if (ServerList.ContainsKey(state))
                    {
                        OAuth2Base ob = ServerList[state].Clone() as OAuth2Base;
                        ob.code = code;
                        Set(ob);
                        return ob;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// ��ȡ�����õ�ǰSession�浵����Ȩ���͡� (ע���û�ʱ���Խ���ֵ��ΪNull)
        /// </summary>
        public static OAuth2Base CurrentOAuth
        {
            get
            {
                return Get();
            }
            set
            {
                Clear();
            }
        }

        #region ˽�ж���

        static void Set(OAuth2Base ob)
        {
            HttpCookie cookie = new HttpCookie("OAuth2Code", ob.code);
            cookie.HttpOnly = true;
            cookie.Expires = DateTime.Now.AddMinutes(30);
            HttpContext.Current.Response.Cookies.Add(cookie);
            CacheManage.Instance.Add("OAuth2Code_" + ob.code, ob, null, 30);
        }
        static OAuth2Base Get()
        {
            object o = null;

            HttpCookie cookie = HttpContext.Current.Request.Cookies["OAuth2Code"];
            if (cookie != null)
            {
                o = CacheManage.Instance.Get(cookie.Value);
            }
            if (o != null)
            {
                return o as OAuth2Base;
            }
            return null;
        }
        static void Clear()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["OAuth2Code"];
            if (cookie != null)
            {
                CacheManage.Instance.Remove("OAuth2Code_" + cookie.Value);
            }
        }

        static Dictionary<string, OAuth2Base> _ServerList;
        /// <summary>
        /// ��ȡ���е����ͣ��¿�����OAuth2��Ҫ������ע�����һ�£�
        /// </summary>
        internal static Dictionary<string, OAuth2Base> ServerList
        {
            get
            {
                if (_ServerList == null)
                {
                    _ServerList = new Dictionary<string, OAuth2Base>(StringComparer.OrdinalIgnoreCase);
                    _ServerList.Add(OAuthServer.SinaWeiBo.ToString(), new SinaWeiBoOAuth());//����΢��
                    _ServerList.Add(OAuthServer.QQ.ToString(), new QQOAuth());//QQ΢��
                    _ServerList.Add(OAuthServer.TaoBao.ToString(), new TaoBaoOAuth());//�Ա�
                    _ServerList.Add(OAuthServer.WeiXin.ToString(), new WeiXinOAuth());//΢��
                }
                return _ServerList;
            }
        }
        #endregion
    }
}
