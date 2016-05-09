using CYQ.Data;
using CYQ.Data.Tool;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2
{
    public class WeiXinOAuth : OAuth2Base
    {
        internal override OAuthServer Server
        {
            get
            {
                return OAuthServer.WeiXin;
            }
        }
        internal override string ImgUrl
        {
            get
            {
                return "<img align='absmiddle' src=\"/skin/system_tech/images/oauth_weixin.png\" /> 微信";
            }
        }
        internal override string OAuthUrl
        {
            get
            {
                return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state={2}#wechat_redirect", AppKey, CallbackUrl, Server);
            }
        }
        internal override string TokenUrl
        {
            get
            {
                return "https://api.weixin.qq.com/sns/oauth2/access_token";
            }
        }
        internal string UserInfoUrl = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}";
        internal override string Para
        {
            get
            {
                return string.Format("appid={0}&secret={1}&code={2}&grant_type=authorization_code", AppKey, AppSercet, code);
            }
        }
        public override bool Authorize()
        {
            if (!string.IsNullOrEmpty(code))
            {
                string result = GetToken("POST");//一次性返回数据。
                //分解result;
                if (!string.IsNullOrEmpty(result))
                {
                    try
                    {
                        Dictionary<string, string> items = JsonHelper.Split(result);
                        if (items != null && items.Count > 0)
                        {
                            foreach (KeyValuePair<string, string> item in items)
                            {
                                switch (item.Key)
                                {
                                    case "access_token":
                                        token = item.Value;
                                        break;
                                    case "expires_in":
                                        double d = 0;
                                        if (double.TryParse(item.Value, out d) && d > 0)
                                        {
                                            expiresTime = DateTime.Now.AddSeconds(d);
                                        }
                                        break;
                                    case "uid":
                                    case "openid":
                                        openID = item.Value;
                                        break;
                                }
                            }
                            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(openID))
                            {
                                //获取微博昵称和头像
                                result = wc.DownloadString(string.Format(UserInfoUrl, token, openID));
                                if (!string.IsNullOrEmpty(result)) //返回：callback( {"client_id":"YOUR_APPID","openid":"YOUR_OPENID"} ); 
                                {
                                    Dictionary<string, string> dic = JsonHelper.Split(result);
                                    foreach (KeyValuePair<string, string> item in items)
                                    {
                                        switch (item.Key)
                                        {
                                            case "screen_name":
                                            case "nickname":
                                                nickName = item.Value;
                                                break;
                                            case "profile_image_url":
                                            case "headimgurl":
                                                headUrl = item.Value;
                                                break;
                                        }
                                    }
                                    return true;
                                }
                            }
                            else
                            {
                                Log.WriteLogToTxt("SinaWeiBoOAuth.Authorize():" + result);
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        Log.WriteLogToTxt(err);
                    }
                }
            }
            return false;
        }
    }
}
