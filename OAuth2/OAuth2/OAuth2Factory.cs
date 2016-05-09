using CYQ.Data.Cache;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
namespace OAuth2
{
    /// <summary>
    /// 授权工厂类
    /// </summary>
    public static class OAuth2Factory
    {
        /// <summary>
        /// 获取当前的授权类型。
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
        /// 读取或设置当前Session存档的授权类型。 (注销用户时可以将此值置为Null)
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

        #region 私有对象

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
        /// 获取所有的类型（新开发的OAuth2需要到这里注册添加一下）
        /// </summary>
        internal static Dictionary<string, OAuth2Base> ServerList
        {
            get
            {
                if (_ServerList == null)
                {
                    _ServerList = new Dictionary<string, OAuth2Base>(StringComparer.OrdinalIgnoreCase);
                    _ServerList.Add(OAuthServer.SinaWeiBo.ToString(), new SinaWeiBoOAuth());//新浪微博
                    _ServerList.Add(OAuthServer.QQ.ToString(), new QQOAuth());//QQ微博
                    _ServerList.Add(OAuthServer.TaoBao.ToString(), new TaoBaoOAuth());//淘宝
                    _ServerList.Add(OAuthServer.WeiXin.ToString(), new WeiXinOAuth());//微信
                }
                return _ServerList;
            }
        }
        #endregion
    }
}
