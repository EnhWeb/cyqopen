using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.Zip;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Net.Sockets;

namespace Update
{
    class Program
    {
        static string softName = string.Empty;
        static string exePath = AppDomain.CurrentDomain.BaseDirectory;
        static string updateExePath = string.Empty;
        static void Main(string[] args)
        {

            if (args.Length > 0 && args[0].StartsWith("http://"))
            {
                if (args.Length > 1)
                {
                    softName = args[1];
                }
                updateExePath = exePath + "update.zip";
                Uri uri = new Uri(args[0]);
                WebClient wc = new WebClient();
                wc.Proxy = GetProxy(uri);
                try
                {
                    wc.DownloadFile(uri, updateExePath);
                    wc_DownloadFileCompleted();
                }
                finally
                {
                    wc.Dispose();
                }
            }

        }
        static void wc_DownloadFileCompleted()
        {
            Thread.Sleep(2000);//间隔5秒
            if (softName.EndsWith(".exe"))
            {
                Process[] ps = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(softName));
                foreach (Process p in ps)
                {
                    p.Kill();
                }
            }
            Thread.Sleep(3000);//间隔5秒

            try
            {
                FastZip fz = new FastZip();
                fz.ExtractZip(updateExePath, exePath);

                Thread.Sleep(3000);//间隔5秒

                if (softName.EndsWith(".exe"))
                {
                    Process.Start(softName, softName);//传递参数，启动后不检测升级。
                }

                File.Delete(updateExePath);
            }
            catch
            {
                //启动新程序

            }
            finally
            {

            }


        }
        static void WriteLine(string msg)
        {
            Console.WriteLine(msg);
        }
        #region 获取可用的WebClient(无法连接时启用代理设置）。

        static IWebProxy GetProxy(Uri host)
        {
            if (TestConnect(host, 80))
            {
                return null;
            }
            Uri uri = new Uri("http://www.cyqdata.com");
            if (TestConnect(uri, 443))//检测秋色园代理。
            {
                return new WebProxy(uri.Host, 443);
            }
            return null;
        }
        internal static bool TestConnect(IPEndPoint point)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(point);
                socket.Disconnect(false);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                socket.Close();
            }
        }
        internal static bool TestConnect(Uri hostUrl, int port)
        {
            if (hostUrl == null)
            {
                return false;
            }
            string host = hostUrl.Host;
            if (hostUrl.HostNameType == UriHostNameType.Dns)
            {
                try
                {
                    host = Dns.GetHostAddresses(host)[0].ToString();
                }
                catch
                {
                    return false;
                }
            }
            return TestConnect(new IPEndPoint(IPAddress.Parse(host), port));
        }
        #endregion
    }
}
