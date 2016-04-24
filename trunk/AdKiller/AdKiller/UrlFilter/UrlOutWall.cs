using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


namespace AdKiller
{
    class UrlOutWall
    {
        Process pro = null;
        string urlOutWallTxt = AppDomain.CurrentDomain.BaseDirectory + "urloutwall.txt";
        [DllImport("user32.dll")]
        public static extern void SetForegroundWindow(IntPtr hwnd);
        public UrlOutWall()
        {
            LoadUrl();
        }
        void LoadUrl()
        {
            if (CheckFilterTxt())
            {
                CheckFilterTxt();
            }
            filters = File.ReadAllLines(urlOutWallTxt, Encoding.Default);
        }
        public void OpenForEdit()
        {
            if (CheckFilterTxt())
            {
                if (pro == null || pro.HasExited)
                {
                    pro = Process.Start("notepad.exe", urlOutWallTxt);
                    pro.EnableRaisingEvents = true;
                    pro.Exited += new EventHandler(pro_Exited);
                }
                else
                {
                    SetForegroundWindow(pro.MainWindowHandle);
                }
            }
        }
        void pro_Exited(object sender, EventArgs e)
        {
            LoadUrl();
            pro.Dispose();
            pro = null;
        }


        static string[] filters = null;
        bool CheckFilterTxt()
        {
            if (!File.Exists(urlOutWallTxt))
            {
                try
                {
                    File.WriteAllText(urlOutWallTxt, "#请在下面添加要出墙的网址，一行一条网址，支持正则， (.*) 代表0或多个任意字符。\r\nhttp://(.*).google.com(.*)\r\nhttp://(.*).cyqdata.com(.*)\r\n", Encoding.Default);
                }
                catch (Exception err)
                {
                    DebugLog.WriteError(err);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 包含指定的网址。
        /// </summary>
        public static bool Contains(Uri uri)
        {
            if (filters != null && filters.Length > 0)
            {
                string url = uri.OriginalString;
                string filter = string.Empty;
                for (int i = 0; i < filters.Length; i++)
                {
                    filter = filters[i];
                    try
                    {
                        if (filter.Length > 0 && filter[0] != '#' && Regex.IsMatch(url, "^" + filter + "$", RegexOptions.Singleline))
                        {
                            return true;
                        }
                    }
                    catch (Exception err)
                    {
                        DebugLog.WriteError(err);
                        filters[i] = "#" + filter;
                    }
                }
            }
            return false;
        }
    }
}
