using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace AdKiller
{
    class WebHost
    {
        #region ����
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

        #region ���������
        static Dictionary<string, HostEntity> _DomainList = null;
        /// <summary>
        /// Ҫ���ε���������������б�(������IP��
        /// </summary>
        public static Dictionary<string, HostEntity> DomainList
        {
            get
            {
                if (_DomainList == null)
                {
                    _DomainList = new Dictionary<string, HostEntity>();
                    //��������
                    _DomainList.Add(domainBaidu, new HostEntity(DomainType.Search));//�ٶ�
                    _DomainList.Add(domainBaiduZhidao, new HostEntity(DomainType.Search));//�ٶ�
                    _DomainList.Add(domainSoso, new HostEntity(DomainType.Search));//����
                    _DomainList.Add(domainSogou, new HostEntity(DomainType.Search));//�ѹ�

                    //��Ƶ��վ����Ҫ���ֶδ���ġ�
                    _DomainList.Add(domainYouku, new HostEntity(DomainType.Video));//"www.y.com"));//�ſ�
                    _DomainList.Add(domainIqiyi, new HostEntity(DomainType.Video));//�����ա�
                    _DomainList.Add(domainTudou, new HostEntity(DomainType.Video));//"www.t.com"));//����
                    //��Ƶ��ַ��ֱ��������ַ���ɡ�
                    
                    _DomainList.Add(domainLetv, new HostEntity());//"www.l.com"));//����
                    _DomainList.Add(domainKankan, new HostEntity());//"www.k.com"));//Ѹ�׿���
                    _DomainList.Add(domainWu6, new HostEntity());//"www.5.com"));//56.com
                    _DomainList.Add(domainPps, new HostEntity());//"www.p.com"));//pps.tv
                    _DomainList.Add(domainKu6, new HostEntity());//"www.6.com"));//��6
                    _DomainList.Add(domainSohu, new HostEntity());//"www.h.com"));//�Ѻ���Ƶ
                    _DomainList.Add(domainVqq, new HostEntity());//"www.h.com"));//��Ѷ��Ƶ


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
        /// ���������
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
                    //����Ƿ񷵻ظ��°汾�š�
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
        /// ���õķ�����IP��
        /// </summary>
        internal static string ServerIP
        {
            get
            {
                if (string.IsNullOrEmpty(_ServerIP))
                {
                    IPAddress[] ips = Tool.GetHostIP("v1.cyqdata.com");//"66.85.180.96"��΢�����������
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
        /// ���õķ�����IP��
        /// </summary>
        internal static string ServerProxyIP
        {
            get
            {
                if (string.IsNullOrEmpty(_ServerProxyIP))
                {
                    IPAddress[] ips = Tool.GetHostIP("www.cyqdata.com");//"216.18.206.210"����ɫ԰������
                    if (ips != null && ips.Length > 0)
                    {
                        _ServerProxyIP = ips[0].ToString();
                    }
                }
                return _ServerProxyIP;
            }
        }


        /// <summary>
        /// ���������
        /// </summary>
        static void StartUpdate(string zipUrl)
        {
            if (zipUrl.StartsWith("http://"))
            {
                string updateExe = AppDomain.CurrentDomain.BaseDirectory + "update.exe";
                if (File.Exists(updateExe))
                {
                    System.Diagnostics.Process.Start(updateExe, zipUrl + " ��ʽ���ɱ��.exe");
                }
            }
        }
    }

}
