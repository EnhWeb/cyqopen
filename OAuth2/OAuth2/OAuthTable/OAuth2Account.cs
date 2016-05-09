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
        /// ��Ȩ�ķ�������
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
        /// �����Token
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
        /// �����Ӧ��ID
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
        /// ����ʱ��
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
        /// ���صĵ������ǳ�
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
        /// ���صĵ������˺Ŷ�Ӧ��ͷ���ַ��
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
        /// �󶨵��˺�
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
        #region �������˺�

        /// <summary>
        /// ��ȡ�Ѿ��󶨵��˺�
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
                    oa.Update();//����token�͹���ʱ��
                    account = oa.BindAccount;
                }
            }
            return account;
        }
        /// <summary>
        /// ��Ӱ��˺�
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
