using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace AdKiller
{
    /// <summary>
    /// 下一级代理
    /// </summary>
    class NextProxy
    {
        static string proxyConfig = AppDomain.CurrentDomain.BaseDirectory + "proxy.txt";
        /// <summary>
        /// 下一级代理设置
        /// </summary>
        public static IPEndPoint Proxy = null;
        /// <summary>
        /// 运行环境（0无；1客户端；2服务端）
        /// </summary>
        public static int RunAtType = 0;
        public static bool SetNextProxy(string ip, int port)
        {
            IPAddress ipAddress;
            if (IPAddress.TryParse(ip, out ipAddress) && port > 0)
            {
                Proxy = new IPEndPoint(ipAddress, port);
                SaveConfig();
                return true;
            }
            return false;
        }
        public static void SaveConfig()
        {
            try
            {
                string ip = ST.LocalIP;
                int port = 808;
                if (Proxy != null)
                {
                    ip = Proxy.Address.ToString();
                    port = Proxy.Port;
                }
                using (StreamWriter sw = new StreamWriter(proxyConfig, false, Encoding.Default))
                {
                    sw.WriteLine(ip);
                    sw.WriteLine(port);
                    sw.WriteLine(RunAtType);
                }
            }
            catch
            {

            }
        }
        public static string[] LoadConfig()
        {
            string[] proxy = null;
            if (File.Exists(proxyConfig))
            {
                proxy = File.ReadAllLines(proxyConfig, Encoding.Default);
            }
            if (proxy != null && proxy.Length > 2)
            {
                return proxy;
            }
            return null;
        }
        public static void ClearNextProxy()
        {
            Proxy = null;
        }

        //代理加密设置

    }
}
