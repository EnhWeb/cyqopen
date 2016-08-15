using CYQ.Data.Orm;
using CYQ.Data.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taurus.Core;
using Taurus.Logic;

namespace Taurus.Controllers.Logic
{
    /// <summary>
    /// 不想建一个Taurus.Logic项目来存逻辑，只好把业务逻辑写在这里。
    /// </summary>
    public class DefaultLogic : LogicBase
    {
        public DefaultLogic(IController controller)
            : base(controller)
        {

        }
        public void BindMenu()
        {
            using (Menu m = new Menu())
            {
                m.Select("order by ordernum").Bind(View);
            }
        }
        public void BindArticleClass()
        {
            using (ArticleClass a = new ArticleClass())
            {
                a.Select("order by orderNum asc").Bind(View);
            }
        }
        public void BindPhotoClass()
        {
            using (PhotoClass a = new PhotoClass())
            {
                a.Select().Bind(View);
            }
        }
        public void BindHomePhoto()
        {
            using (HomePhoto hp = new HomePhoto())
            {
                hp.Select("TypeName='首页左侧图' order by orderNum").Bind(View,"homephoto1");
                hp.Select("TypeName='首页右侧图' order by orderNum").Bind(View,"homephoto2");
            }
        }
        public void BindTopArticle()
        {
            ArticleClass ac = DBFast.Find<ArticleClass>("Name='行业资讯' or Name='最新资讯'");
            if (ac != null)
            {
                View.Set("lnkMore", SetType.Href, "/articlelist?id=" + ac.ID);
                using (Article a = new Article())
                {
                    View.OnForeach += View_OnForeach;
                    a.Select(3, "CateID=" + ac.ID + " order by id desc").Bind(View);
                }
            }
        }

        string View_OnForeach(string text, Dictionary<string, string> values, int rowIndex)
        {
            values["CreateTime"] = FormatDate(values["CreateTime"]);
            return text;
        }

        /// <summary>
        /// 格式化日期格式为[yyyy-MM-dd]的形式
        /// </summary>
        /// <param name="objDate"></param>
        /// <returns></returns>
        protected string FormatDate(object objDate)
        {
            if (objDate == null)
            {
                return "";
            }
            DateTime CurrentDate;
            DateTime.TryParse(Convert.ToString(objDate), out CurrentDate);
            return CurrentDate.ToString("yyyy-MM-dd HH:mm");
        }
    }
}
