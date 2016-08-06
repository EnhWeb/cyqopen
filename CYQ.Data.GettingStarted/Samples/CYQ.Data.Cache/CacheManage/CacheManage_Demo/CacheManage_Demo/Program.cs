using CYQ.Data;
using CYQ.Data.Cache;
using CYQ.Data.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManage_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //说明：如果你确定缓存一定是在本机，使用：CacheManage cache= CacheManage.LocalInstance
            //如果只是缓存一般数据，将来有可能启用分布式时，用：CacheManage cache = CacheManage.Instance;

            //比如框架对一些表架构的元数据的缓存，用的是本机（速度快）：CacheManage.LocalInstance
            //而框架对于自动缓存（表的数据），用的是：CacheManage.Instance （将来随便分布式启用分散到各缓存服务器）

            //AppConfig.Cache.MemCacheServers = "127.0.0.1:11211,127.0.0.1:11212";//配置启用MemCache
            CacheManage cache = CacheManage.Instance;
            if (!cache.Contains("a1"))
            {
                cache.Add("a1", "a1", 0.1);
            }
            cache.Set("a2", "a2", 0.5);//存在则更新，不存在则添加。
            cache.Update("a3", "a3");//存在则更新，不存在则跳过
            cache.Add("a4", "a4", 2.2);
            cache.Add("a0", "a0");
            cache.Add("table", cache.CacheTable);

            Console.WriteLine(cache.Get("a0") as String);
            Console.WriteLine(cache.Get<string>("a1"));
            Console.WriteLine(cache.Get<string>("a2"));
            Console.WriteLine(cache.Get<string>("a3"));
            Console.WriteLine(cache.Get<string>("a4"));
            MDataTable table = cache.Get<MDataTable>("table");
            cache.Remove("a0");//单个移除
            cache.Clear();//清除所有缓存
            if (cache.CacheType == CacheType.LocalCache)//只能拿到本机的信息
            {
                Console.WriteLine("缓存数：" + table.Rows.Count);
                Console.WriteLine("总内存(M)：" + GC.GetTotalMemory(false) / 1024); // 感觉拿到的值不太靠谱。
                Console.WriteLine("剩余内存(M)：" + cache.RemainMemoryBytes / 1024);
                Console.WriteLine("剩余百分比%：" + cache.RemainMemoryPercentage);
            }
            Console.Read();
        }
    }
}
