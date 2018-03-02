using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using DAL.CMS;
using Mozart.Common;
using Model.CMS;
using System.IO;

namespace Mozart.MicroSite
{
    public partial class ArticleList : BasePage
    {
        private string strCatID;
        protected override bool BeforeLoad()
        {
            string strTitle = string.Empty;
            string strTheme = string.Empty;
            if (null == Request.QueryString["SiteCode"])
            {
                return false;
            }
            if (null == Request.QueryString["CatID"])
            {
                return false;
            }

            SiteCode = Common.Common.NoHtml(Request.QueryString["sitecode"].ToString());
            strCatID = Common.Common.NoHtml(Request.QueryString["catid"].ToString());


            return base.BeforeLoad();
        }
        protected override void AddTempData(JinianNet.JNTemplate.TemplateContext context)
        {
            base.AddTempData(context);
            
            
            List<CMS_Article> liArticle = new List<CMS_Article>();
            ArticleDAL dal = new ArticleDAL();

            DataSet ds = dal.GetCategoryList(SiteCode, strCatID);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                CMS_Article model = DataConvert.DataRowToModel<CMS_Article>(row);
                liArticle.Add(model);
            }
            
            context.TempData["Article_list"] = liArticle;
        }
    }
}