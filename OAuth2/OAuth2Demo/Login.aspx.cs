using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using OAuth2;

namespace OAuth2Demo
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            OAuth2.UI.GoTo(OAuthServer.WeiXin);
            litOtherLoginInfo.Text = OAuth2.UI.GetHtml();
            if (IsPostBack)
            {
                return;
            }
            //OAuth2.OAuth2Base ob = OAuth2.OAuth2Factory.Create();//��ȡ��ǰ����Ȩ���ͣ�����ɹ�����Ỻ�浽Session�С�
            //if (ob != null) //˵���û��������Ȩ�������ص�½������
            //{
            //    if (ob.Authorize())//����Ƿ���Ȩ�ɹ��������ذ󶨵��˺ţ������ǰ�ID�����û��������ѡ��
            //    {
            //        string account = OAuth2Account.GetBindAccount(ob);
            //        if (!string.IsNullOrEmpty(account))//�Ѱ��˺ţ�ֱ���ø��˺����õ�½��
            //        {
            //            UserLogin ul = new UserLogin();
            //            if (ul.Login(account))
            //            {
            //               // Response.Redirect("/");
            //            }
            //        }
            //        else // δ���˺ţ�������ʾ�û����˺š�
            //        {
            //            //Response.Write(ob.nickName + " �״�ʹ����Ҫ����վ�˺ţ����½��ע�����˺�");
            //        }
            //    }

            //}
            //else // ��ȡ��Ȩʧ�ܡ�
            //{
            //    //��ʾ�û����ԣ��������������������½��
            //}

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            UserLogin ul = new UserLogin();
            if (ul.Login(txtUserName.Text, txtPassword.Text))
            {
                OAuth2Account.SetBindAccount(OAuth2Factory.CurrentOAuth, txtUserName.Text);
                Response.Redirect("/");//��½����ת
            }
        }

        protected void btnReg_Click(object sender, EventArgs e)
        {
            UserLogin ul = new UserLogin();
            if (ul.Reg(txtUserName.Text, txtPassword.Text))
            {
                //ע��ɹ������Ƿ�����˺� -- ����͵�½��һ����
                OAuth2Account.SetBindAccount(OAuth2Factory.CurrentOAuth, txtUserName.Text);
                Response.Redirect("/");//��½����ת
            }
        }
    }
    public class UserLogin
    {
        /// <summary>
        /// ��Ȩʱֱ�����û�����½��
        /// </summary>
        public bool Login(string userName)
        {
            return true;
        }
        public bool Login(string userName, string password)
        {
            return true;
        }
        public bool Reg(string userName, string password)
        {
            return true;
        }
    }
}
