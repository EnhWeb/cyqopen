using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace AdKiller
{
    class WebHost
    {
        #region 常量
        internal const string domainBaidu = "www.baidu.com";
        internal const string domainBaiduZhidao = "zhidao.baidu.com";

        internal const string domainSoso = "www.soso.com";
        //  internal const string domainSosoMap = "www.s.com";

        internal const string domainSogou = "www.sogou.com";
        // internal const string domainSogouMap = "www.g.com";

        internal const string domainIqiyi = "data.video.qiyi.com";
        // internal const string domainIqiyiMap = "data2.video.qiyi.com";

        internal const string domainYouku = "valf.atm.youku.com";
        internal const string domainTudou = "td.atm.youku.com";

        internal const string domainLetv = "pro.hoye.letv.com";//"g3.letv.cn";// 
        internal const string domainKankan = "float.sandai.net";
        internal const string domainWu6 = "acs.56.com";
        internal const string domainPps = "ugcfile.ppstream.com";
        internal const string domainKu6 = "g.aa.sdo.com";
        internal const string domainSohu = "images.sohu.com";
        internal const string domainVqq = "adslvfile.qq.com";
        #endregion

        #region 处理的域名
        static Dictionary<string, HostEntity> _DomainList = null;
        /// <summary>
        /// 要屏蔽的搜索引擎的域名列表(域名，IP）
        /// </summary>
        public static Dictionary<string, HostEntity> DomainList
        {
            get
            {
                if (_DomainList == null)
                {
                    _DomainList = new Dictionary<string, HostEntity>();
                    //搜索引擎
                    _DomainList.Add(domainBaidu, new HostEntity(DomainType.Search));//百度
                    _DomainList.Add(domainBaiduZhidao, new HostEntity(DomainType.Search));//百度
                    _DomainList.Add(domainSoso, new HostEntity(DomainType.Search));//搜搜
                    _DomainList.Add(domainSogou, new HostEntity(DomainType.Search));//搜狗

                    //视频网站，需要点手段处理的。
                    _DomainList.Add(domainYouku, new HostEntity(DomainType.Video));//"www.y.com"));//优酷
                    _DomainList.Add(domainIqiyi, new HostEntity(DomainType.Video));//爱奇艺。
                    _DomainList.Add(domainTudou, new HostEntity(DomainType.Video));//"www.t.com"));//土豆
                    //视频网址，直接屏蔽网址即可。
                    
                    _DomainList.Add(domainLetv, new HostEntity());//"www.l.com"));//乐视
                    _DomainList.Add(domainKankan, new HostEntity());//"www.k.com"));//迅雷看看
                    _DomainList.Add(domainWu6, new HostEntity());//"www.5.com"));//56.com
                    _DomainList.Add(domainPps, new HostEntity());//"www.p.com"));//pps.tv
                    _DomainList.Add(domainKu6, new HostEntity());//"www.6.com"));//酷6
                    _DomainList.Add(domainSohu, new HostEntity());//"www.h.com"));//搜狐视频
                    _DomainList.Add(domainVqq, new HostEntity());//"www.h.com"));//腾讯视频


                }
                return _DomainList;
            }
            set
            {
                _DomainList = value;
            }
        }

        #endregion

        /// <summary>
        /// 检测升级。
        /// </summary>
        public static void CheckUpdate()
        {
            using (WebClient wc = new WebClient())
            {
                if (!Update(wc, false))
                {
                    Update(wc, true);
                }
            }
        }
        static bool Update(WebClient wc, bool useProxy)
        {
            try
            {
                if (useProxy)
                {
                    wc.Proxy = new WebProxy(ServerProxyIP, 443);
                }
                string result = wc.DownloadString("http://" + ServerIP + "/ping?v=" + Program.version);
                if (result.StartsWith("ok"))
                {
                    if (string.IsNullOrEmpty(Program.argPara) && result.IndexOf(',') > -1)
                    {
                        StartUpdate(result.Split(',')[1]);
                    }
                    //检测是否返回更新版本号。
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        static string _ServerIP = null;
        /// <summary>
        /// 可用的服务器IP。
        /// </summary>
        internal static string ServerIP
        {
            get
            {
                if (string.IsNullOrEmpty(_ServerIP))
                {
                    IPAddress[] ips = Tool.GetHostIP("v1.cyqdata.com");//"66.85.180.96"，微博精灵服务器
                    if (ips != null && ips.Length > 0)
                    {
                        _ServerIP = ips[0].ToString();
                    }
                }
                return _ServerIP;
            }
        }

        static string _ServerProxyIP = null;
        /// <summary>
        /// 可用的服务器IP。
        /// </summary>
        internal static string ServerProxyIP
        {
            get
            {
                if (string.IsNullOrEmpty(_ServerProxyIP))
                {
                    IPAddress[] ips = Tool.GetHostIP("www.cyqdata.com");//"216.18.206.210"，秋色园服务器
                    if (ips != null && ips.Length > 0)
                    {
                        _ServerProxyIP = ips[0].ToString();
                    }
                }
                return _ServerProxyIP;
            }
        }


        /// <summary>
        /// 起动软件升级
        /// </summary>
        static void StartUpdate(string zipUrl)
        {
            if (zipUrl.StartsWith("http://"))
            {
                string updateExe = AppDomain.CurrentDomain.BaseDirectory + "update.exe";
                if (File.Exists(updateExe))
                {
                    System.Diagnostics.Process.Start(updateExe, zipUrl + " 秋式广告杀手.exe");
                }
            }
        }
    }

}
