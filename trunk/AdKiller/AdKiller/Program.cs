using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace AdKiller
{
    static class Program
    {
        /// <summary>
        /// 软件启动的数量
        /// </summary>
        internal static int processCount = 0;
        /// <summary>
        /// 软件版本号（日期）
        /// </summary>
        internal static long version = 13041504;
        /// <summary>
        /// 外置参数（如果从升级程序传进来，则不再检测升级）。
        /// </summary>
        internal static string argPara = string.Empty;

        ///// <summary>
        ///// 扩展DLL
        ///// </summary>
        //internal static ExtendMethod eMethod = null;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (AppDomain.CurrentDomain.BaseDirectory.IndexOf('$') > -1)
            {
                MessageBox.Show("软件不能在解压文件里运行，请解压后再运行！", ST.MsgTitle);
                return;
            }
            Process cp = Process.GetCurrentProcess();
            Process[] allPros = Process.GetProcessesByName(cp.ProcessName);
            processCount = allPros.Length;
            if (processCount > 1)
            {
                foreach (Process pro in allPros)
                {
                    if (cp.Id != pro.Id && cp.MainModule.FileName == pro.MainModule.FileName)
                    {
                        MessageBox.Show("软件已经启动过了!", "运行提示");
                        return;
                    }
                }
            }
            if (!Config.Exists(false) && !IsRunningAsAdmin())//非管理员身份运行时，提醒用户！
            {
                string tip = "权限不足！请关闭软件，按以下提示设置后重新运行：\r\n\r\n";
                tip += "对着 软件右键-》属性-》兼容性-》勾选以管理员身份运行此程序-》确定\r\n\r\n";
                tip += "点击“确定”关闭软件，点击“取消”继续运行软件！";
                if (MessageBox.Show(tip, ST.MsgTitle, MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    return;
                }
            }
            if (args.Length > 0)
            {
                argPara = args[0];
            }

            //eMethod = new ExtendMethod();
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StartForm());
        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Application_ThreadException(sender, null);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception error = e.Exception as Exception;
            DebugLog.WriteError(error);
            MessageBox.Show("发生未处理的异常错误，错误信息如下：" + error.Message, "系统错误提示!");
            DebugLog.WriteError(error);
        }
        /// <summary>  
        /// 判断程序是否以管理员身份运行  
        /// </summary>  
        /// <returns></returns>  
        public static bool IsRunningAsAdmin()
        {
            bool result = false;
            try
            {
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
                result = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
            }
            catch
            {
                result = true;
            }

            return result;
        }

    }
}