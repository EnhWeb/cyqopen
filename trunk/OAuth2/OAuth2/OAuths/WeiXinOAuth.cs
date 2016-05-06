using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2
{
    public class WeiXinOAuth : OAuth2Base
    {
        internal override OAuthServer server
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
                return "<img align='absmiddle' src=\"/skin/system_tech/images/oauth_weixin.jpg\" /> 微信";
            }
        }
        internal override string OAuthUrl
        {
            get
            {
                return "https://open.weixin.qq.com/connect/qrconnect?response_type=code&client_id={0}&redirect_uri={1}&state={2}&appid={3}";
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
                        Dictionary<string, string> items = CYQ.Data.Tool.JsonHelper.Split(result);
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
                                    nickName = Tool.GetJosnValue(result, "screen_name");
                                    headUrl = Tool.GetJosnValue(result, "profile_image_url");
                                    return true;
                                }
                            }
                            else
                            {
                                CYQ.Data.Log.WriteLogToTxt("SinaWeiBoOAuth.Authorize():" + result);
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        CYQ.Data.Log.WriteLogToTxt(err);
                    }
                }
            }
            return false;
        }
    }
}
