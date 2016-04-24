using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace AdKiller
{
    /// <summary>
    /// 全局配置文件
    /// </summary>
    class Config
    {
        /// <summary>
        /// 是否允许保存配置文件（服务器无法连通时，拒绝保存）
        /// </summary>
        public static bool AllowSaveConfig = true;//
        static string configFileName = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        internal static bool Exists(bool saveConfig)
        {
            bool exists = File.Exists(configFileName);
            if (!exists && saveConfig)
            {
                SaveConfig();
            }
            return exists;
        }
        internal static void SaveConfig()
        {
            if (!AllowSaveConfig)
            {
                return;
            }
            if (File.Exists(configFileName))
            {
                File.WriteAllText(configFileName, string.Empty); //清空当前配置文件
            }
            //重新写入配置文件
            try
            {
                using (StreamWriter sw = new StreamWriter(configFileName, false))
                {
                    //搜索引擎
                    sw.WriteLine("baidu=" + (Baidu ? "1" : "0"));
                    sw.WriteLine("soso=" + (Soso ? "1" : "0"));
                    sw.WriteLine("sogou=" + (Sogou ? "1" : "0"));

                    //视频网站
                    sw.WriteLine("youku=" + (Youku ? "1" : "0"));
                    sw.WriteLine("tudou=" + (Tudou ? "1" : "0"));
                    sw.WriteLine("letv=" + (Letv ? "1" : "0"));
                    sw.WriteLine("vqq=" + (Vqq ? "1" : "0"));
                    sw.WriteLine("kankan=" + (Kankan ? "1" : "0"));
                    sw.WriteLine("wu6=" + (Wu6 ? "1" : "0"));
                    sw.WriteLine("pps=" + (Pps ? "1" : "0"));
                    sw.WriteLine("ku6=" + (Ku6 ? "1" : "0"));
                    sw.WriteLine("sohu=" + (Sohu ? "1" : "0"));
                    sw.WriteLine("iqiyi=" + (Iqiyi ? "1" : "0"));

                    //其它
                    sw.WriteLine("writelog=" + (DebugLog.allowWriteLog ? "1" : "0"));

                    //高级设置
                    sw.WriteLine("smode=" + Smode);
                    sw.WriteLine("sauto=" + (Sauto ? "1" : "0"));
                    sw.WriteLine("altkey=" + Altkey);

                    //代理设置
                    sw.WriteLine("outwall=" + (OutWall ? "1" : "0"));
                    sw.WriteLine("useproxy=" + (UseProxy ? "1" : "0"));
                    sw.WriteLine("useport=" + UsePort);
                    sw.WriteLine("nextproxyip=" + NextProxyIP);
                    sw.WriteLine("nextproxyport=" + NextProxyPort);
                    sw.WriteLine("runattype=" + RunAtType);
                }
            }
            catch (Exception err)
            {
                DebugLog.WriteError(err);
            }
        }
        /// <summary>
        /// 初始化变量
        /// </summary>
        internal static void LoadConfig()
        {
            if (File.Exists(configFileName))
            {
                string text = File.ReadAllText(configFileName);
                if (!string.IsNullOrEmpty(text))
                {
                    //搜索引擎
                    Baidu = !text.Contains("baidu=0");
                    Soso = !text.Contains("soso=0");
                    Sogou = !text.Contains("sogou=0");

                    //视频网站
                    Youku = !text.Contains("youku=0");
                    Tudou = !text.Contains("tudou=0");
                    Letv = !text.Contains("letv=0");
                    Vqq = !text.Contains("vqq=0");
                    Kankan = !text.Contains("kankan=0");
                    Wu6 = !text.Contains("wu6=0");
                    Pps = !text.Contains("pps=0");
                    Ku6 = !text.Contains("ku6=0");
                    Sohu = !text.Contains("sohu=0");
                    Iqiyi = text.Contains("iqiyi=1");//这个默认是false。

                    //其它
                    WriteLog = text.Contains("writelog=1");

                    //高级设置
                    Sauto = !text.Contains("sauto=0");
                    int.TryParse(GetValue(text, "smode"), out Smode);
                    int.TryParse(GetValue(text, "altkey"), out Altkey);


                    //代理设置
                    OutWall = text.Contains("outwall=1");
                    UseProxy = Program.processCount == 1 && !text.Contains("useproxy=0");
                    int.TryParse(GetValue(text, "useport"), out UsePort);
                    NextProxyIP = GetValue(text, "nextproxyip");
                    int.TryParse(GetValue(text, "nextproxyport"), out NextProxyPort);
                    int.TryParse(GetValue(text, "runattype"), out RunAtType);
                }
            }
        }
        static string GetValue(string text, string key)
        {
            key = key + "=";
            int index = text.IndexOf(key) + key.Length;
            if (index > key.Length)
            {
                int end = text.IndexOf("\r\n", index);
                if (end > 0)
                {
                    return text.Substring(index, end - index);
                }
            }
            return string.Empty;
        }
        #region 表态属性
        //屏蔽的视频及网页
        internal static bool Baidu = true;
        internal static bool Soso = true;
        internal static bool Sogou = true;
        internal static bool Youku = true;
        internal static bool Tudou = true;
        internal static bool Letv = true;
        internal static bool Vqq = true;
        internal static bool Kankan = true;
        internal static bool Wu6 = true;
        internal static bool Pps = true;
        internal static bool Ku6 = true;
        internal static bool Sohu = true;
        internal static bool Iqiyi = false;
        internal static bool WriteLog = false;

        //高级选项

        internal static bool Sauto = true;
        internal static int Smode = 0;
        internal static int Altkey = 0;



        //代理选项

        internal static bool UseProxy = true;
        internal static int UsePort = 81;
        internal static string NextProxyIP = ST.LocalIP;
        internal static int NextProxyPort = 808;
        /// <summary>
        /// 运行环境（0无，1客户端；2服务端）；
        /// </summary>
        internal static int RunAtType = 0;

        /// <summary>
        /// 下一级代理设置
        /// </summary>
        public static IPEndPoint NextProxy = null;
        public static void SetNextProxy()
        {
            IPAddress ipAddress;
            if (IPAddress.TryParse(NextProxyIP, out ipAddress) && NextProxyPort > 0)
            {
                NextProxy = new IPEndPoint(ipAddress, NextProxyPort);
            }
        }
        public static void ClearNextProxy()
        {
            NextProxy = null;
        }
        internal static bool OutWall = false;
        internal static int DefaultProxyPort = 1080;

        private static IPEndPoint _DefaultProxy;
        /// <summary>
        /// 默认代理
        /// </summary>
        public static IPEndPoint DefaultProxy
        {
            get
            {
                if (_DefaultProxy == null)
                {
                    try
                    {
                        _DefaultProxy = new IPEndPoint(IPAddress.Parse(WebHost.ServerIP), DefaultProxyPort);//换成66.85.180.96了官网IP了。
                    }
                    catch (Exception err)
                    {
                        _DefaultProxy = new IPEndPoint(IPAddress.Parse(ST.LocalIP), DefaultProxyPort);
                        DebugLog.WriteError(err);
                    }
                }
                return _DefaultProxy;
            }
        }
            /// <summary>
            /// 检测系统提供的默认代理是否可用。
            /// </summary>
            /// <returns></returns>
            public static bool CheckSysProxy()
            {
                if (ProxyList.Count > 0)
                {
                    int index = MyRandom.Rnd.Next(proxyList.Count);
                    //随机取一个服务器。
                    IPAddress ip = proxyList[index];

                    //初始化一个可用的服务器链接。
                    string msg;
                    if (!Tool.TestConnect(new IPEndPoint(ip, DefaultProxyPort), out msg))
                    {
                        for (int i = 0; i < proxyList.Count; i++)
                        {
                            if (i == index)
                            {
                                continue;
                            }
                            ip = proxyList[i];
                            if (Tool.TestConnect(new IPEndPoint(ip, DefaultProxyPort), out msg))
                            {
                                _DefaultProxy = new IPEndPoint(ip, DefaultProxyPort);
                                return true;
                            }
                        }
                    }
                    else
                    {
                        _DefaultProxy = new IPEndPoint(ip, DefaultProxyPort);
                        return true;
                    }
                }
                return false;
            }

            internal static List<IPAddress> proxyList = new List<IPAddress>();
            internal static List<IPAddress> ProxyList
            {
                get
                {
                    if (proxyList.Count == 0)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            string key = (i == 0 ? "www" : "v" + i);
                            IPAddress[] ips = Tool.GetHostIP(key + ".cyqdata.com");
                            if (ips != null && !proxyList.Contains(ips[0]) && ips[0].ToString() != ST.LocalIP)
                            {
                                proxyList.Add(ips[0]);
                            }
                        }
                    }
                    return proxyList;
                }
            }
        #endregion
        }
    }
