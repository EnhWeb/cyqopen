using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace AdKiller
{
    /// <summary>
    /// 清除广告标签
    /// </summary>
    class AdFormat
    {
        public static SearchMode searchMode = SearchMode.NoAd;

        public static string GetCss(string host)
        {
            string css = string.Empty;
            switch (host)
            {
                case WebHost.domainBaidu:
                    css = "<style>.EC_mr15,#ec_im_container,.fsblock,.ec_pp_f,#content_right{display: none !important;}</style>";
                    break;
                case WebHost.domainBaiduZhidao:
                    css = "<style>.wgt-ads,.widget-ads,.page-main-slider{display: none !important;}</style>";
                    break;
                case WebHost.domainSoso:
                    css = "<style>.ad_zdq,#side{display: none !important;}</style>";
                    break;
                case WebHost.domainSogou:
                    css = "<style>.sponsored,#right{display: none !important;}</style>";
                    break;
                default:
                    return string.Empty;
            }
            if (searchMode == SearchMode.ShowAd)
            {
                css = css.Replace("display: none", "background-color:Silver");
            }
            return css;
        }
    }
}
