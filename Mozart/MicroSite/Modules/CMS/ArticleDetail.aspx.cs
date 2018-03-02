using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.CMS;
using Model;
using Mozart.Common;
using Model.CMS;

namespace Mozart.MicroSite
{
    public partial class ArticleDetail : BasePage
    {
        private CMS_Article model;
        protected override bool BeforeLoad()
        {
            string strArticleID = string.Empty;
            if (null == Request.QueryString["id"])
            {
                return false;
            }
            strArticleID = Common.Common.NoHtml(Request.QueryString["id"].ToString());
            ArticleDAL dal = new ArticleDAL();
            DataSet ds = dal.GetArticleDetail(strArticleID);

            model = new CMS_Article();
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model = DataConvert.DataRowToModel<CMS_Article>(ds.Tables[0].Rows[0]);
            }
            SiteCode = model.SiteCode;

            return base.BeforeLoad();
        }
        protected override void AddTempData(JinianNet.JNTemplate.TemplateContext context)
        {
            context.TempData["ADetail"] = model;
        }
    }
}