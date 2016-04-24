using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace AdKiller
{
    class UrlFilter
    {
        Process pro = null;
        string urlFilterTxt = AppDomain.CurrentDomain.BaseDirectory + "urlfilter.txt";
        [DllImport("user32.dll")]
        public static extern void SetForegroundWindow(IntPtr hwnd);
        public UrlFilter()
        {
            LoadUrl();
        }
        void LoadUrl()
        {
            if (CheckFilterTxt())
            {
                CheckFilterTxt();
            }
            filters = File.ReadAllLines(urlFilterTxt, Encoding.Default);
        }
        public void OpenForEdit()
        {
            if (CheckFilterTxt())
            {
                if (pro == null || pro.HasExited)
                {
                    pro = Process.Start("notepad.exe", urlFilterTxt);
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
            if (!File.Exists(urlFilterTxt))
            {
                try
                {
                    File.WriteAllText(urlFilterTxt, "#请在下面添加要过滤的网址，一行一条网址，支持正则，（.*) 代表0或多个任意字符。\r\nhttp://b.scorecardresearch.com(.*)\r\n", Encoding.Default);
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
        /// 是否需要过滤
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
