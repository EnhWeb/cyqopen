using CYQ.Data;
using CYQ.Data.Orm;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2
{
    public partial class OAuth2Account:OrmBase
    {
        public OAuth2Account()
        {
            base.SetInit(this, "OAuth2Account", "Txt Path={0}App_Data");
        }
        private int _ID;

        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        private string _OAuthServer;
        /// <summary>
        /// 授权的服务类型
        /// </summary>
        public string OAuthServer
        {
            get
            {
                return _OAuthServer;
            }
            set
            {
                _OAuthServer = value;
            }
        }
        private string _Token;
        /// <summary>
        /// 保存的Token
        /// </summary>
        public string Token
        {
            get
            {
                return _Token;
            }
            set
            {
                _Token = value;
            }
        }
        private string _OpenID;
        /// <summary>
        /// 保存对应的ID
        /// </summary>
        public string OpenID
        {
            get
            {
                return _OpenID;
            }
            set
            {
                _OpenID = value;
            }
        }
        private string _BindAccount;
        
        private DateTime _ExpireTime;
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime
        {
            get
            {
                return _ExpireTime;
            }
            set
            {
                _ExpireTime = value;
            }
        }

        private string _NickName;
        /// <summary>
        /// 返回的第三方昵称
        /// </summary>
        public string NickName
        {
            get
            {
                return _NickName;
            }
            set
            {
                _NickName = value;
            }
        }
        private string _HeadUrl;
        /// <summary>
        /// 返回的第三方账号对应的头像地址。
        /// </summary>
        public string HeadUrl
        {
            get
            {
                return _HeadUrl;
            }
            set
            {
                _HeadUrl = value;
            }
        }


        /// <summary>
        /// 绑定的账号
        /// </summary>
        public string BindAccount
        {
            get
            {
                return _BindAccount;
            }
            set
            {
                _BindAccount = value;
            }
        }
    }

    public partial class OAuth2Account
    {
        #region 关联绑定账号

        /// <summary>
        /// 读取已经绑定的账号
        /// </summary>
        /// <returns></returns>
        public static string GetBindAccount(OAuth2Base ob)
        {
            string account = string.Empty;
            using (OAuth2Account oa = new OAuth2Account())
            {
                if (oa.Fill(string.Format("OAuthServer='{0}' and OpenID='{1}'", ob.Server, ob.openID)))
                {
                    oa.Token = ob.token;
                    oa.ExpireTime = ob.expiresTime;
                    oa.NickName = ob.nickName;
                    oa.HeadUrl = ob.headUrl;
                    oa.Update();//更新token和过期时间
                    account = oa.BindAccount;
                }
            }
            return account;
        }
        /// <summary>
        /// 添加绑定账号
        /// </summary>
        /// <param name="bindAccount"></param>
        /// <returns></returns>
        public static bool SetBindAccount(OAuth2Base ob, string bindAccount)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(ob.openID) && !string.IsNullOrEmpty(ob.token) && !string.IsNullOrEmpty(bindAccount))
            {
                using (OAuth2Account oa = new OAuth2Account())
                {
                    if (!oa.Exists(string.Format("OAuthServer='{0}' and OpenID='{1}'", ob.Server, ob.openID)))
                    {
                        oa.OAuthServer = ob.Server.ToString();
                        oa.Token = ob.token;
                        oa.OpenID = ob.openID;
                        oa.ExpireTime = ob.expiresTime;
                        oa.BindAccount = bindAccount;
                        oa.NickName = ob.nickName;
                        oa.HeadUrl = ob.headUrl;
                        result = oa.Insert(InsertOp.None);
                    }
                }
            }
            return result;
        }
        #endregion
    }
}
