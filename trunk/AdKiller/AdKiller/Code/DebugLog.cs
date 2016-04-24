using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdKiller
{
    /// <summary>
    /// »’÷æ ‰≥ˆ
    /// </summary>
    class DebugLog
    {
        static readonly object lockObj = new object();
        static string errorLogFolder = AppDomain.CurrentDomain.BaseDirectory + "log";
        internal static bool allowWriteLog = false;
        public static void WriteError(Exception err)
        {
#if DEBUG
            allowWriteLog = true;
#endif
            if (!allowWriteLog)
            {
                return;
            }
            try
            {
                if (!Directory.Exists(errorLogFolder))
                {
                    Directory.CreateDirectory(errorLogFolder);
                }
                string message = err.Message;
                if (err.InnerException != null)
                {
                    message += ":" + err.InnerException.Message + "\r\n" + err.InnerException.StackTrace;
                }
                else
                {
                    message += "\r\n" + err.StackTrace;
                }
                lock (lockObj)
                {
                    System.IO.File.AppendAllText(errorLogFolder + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt", "\r\n------------------------\r\nlog:Error On : " + DateTime.Now.ToString() + "\r\n" + message);
                }
            }
            catch
            {
            }
        }
    }
}
