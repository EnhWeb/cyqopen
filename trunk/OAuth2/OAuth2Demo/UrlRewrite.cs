using OAuth2;
using System;
using System.Collections.Generic;
using System.Web;

namespace OAuth2Demo
{
    public class UrlRewrite : IHttpModule
    {
        public void Dispose()
        {
            
        }
        HttpContext context;
        public void Init(HttpApplication context)
        {
            this.context = context.Context;
            context.BeginRequest += context_BeginRequest;
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            context = ((HttpApplication)sender).Context;
            if (context.Request.Url.ToString().Contains("index.xhtml"))//之前的自定义路径测试
            {
                OAuth2.OAuth2Base ob = OAuth2.OAuth2Factory.Create();//获取当前的授权类型，如果成功，则会缓存到Session中。
                if (ob != null) //说明用户点击了授权，并跳回登陆界面来
                {
                    if (ob.Authorize())//检测是否授权成功，并返回绑定的账号（具体是绑定ID还是用户名，你的选择）
                    {
                        string account = OAuth2Account.GetBindAccount(ob);
                    }

                }
                else // 读取授权失败。
                {
                    //提示用户重试，或改用其它社区方法登陆。
                }
            }
        }
    }
}