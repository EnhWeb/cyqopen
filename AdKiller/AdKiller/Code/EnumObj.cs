using System;
using System.Collections.Generic;
using System.Text;

namespace AdKiller
{
    /// <summary>
    /// 输出模式
    /// </summary>
    public enum SearchMode
    {  
        /// <summary>
        /// 屏蔽广告显示
        /// </summary>
        NoAd,
        /// <summary>
        /// 高亮显示广告
        /// </summary>
        ShowAd,
        ///// <summary>
        ///// 智能处理（通常显示其它搜索结果）
        ///// </summary>
        //AutoAd,
    }
    /// <summary>
    /// 搜索引擎
    /// </summary>
    public enum SearchEngine
    {
        /// <summary>
        /// 百度 baidu.com
        /// </summary>
        Baidu,
        /// <summary>
        /// 谷歌 google.com
        /// </summary>
       // Google,
        /// <summary>
        /// 腾讯 soso.com
        /// </summary>
        Soso,
        /// <summary>
        /// 搜狗 sogou.com
        /// </summary>
        Sogou,
        /// <summary>
        /// 360 so.com
        /// </summary>
      //  So,

    }
    /// <summary>
    /// 域名的类型
    /// </summary>
    public enum DomainType
    {
        /// <summary>
        /// 搜索引擎
        /// </summary>
        Search,
        /// <summary>
        /// 视频类
        /// </summary>
        Video,
    }

}
