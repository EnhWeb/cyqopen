using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CYQ.Data.Table;

namespace CYQ.Data.ProjectTool
{
    static class Program
    {
        internal static string path = string.Empty;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] para)
        {
            //MProc p = new MProc("select * from sharedb.v_share_xsxx where xm='严雯嘉'", "Provider=MSDAORA;Data Source=202.115.159.180/xhudb;User ID=mobileportal;Password=mobileportal");
            //MDataTable dt = p.ExeMDataTable();
            // MAction action2= new MAction("v_share_xsxx", "Provider=MSDAORA;Data Source=202.115.159.180/xhudb;User ID=mobileportal;Password=mobileportal");
            //MAction action = new MAction("GuidDemo","server=.;database=demo;uid=sa;pwd=123456");
            //action.AllowInsertID = true;
            //action.Set("id", Guid.NewGuid());
            //action.Set("name", "dfdf");
            //action.Insert();
            //return;

            if (para.Length > 0)
            {
                path = para[0].TrimEnd('"');
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.Run(new OpForm());
        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Application_ThreadException(sender, null);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception error = e.Exception as Exception;
            MessageBox.Show("发生未处理的异常错误，错误信息如下：" + error.Message, "系统错误提示!");
        }
    }
}