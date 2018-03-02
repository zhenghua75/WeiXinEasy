using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.CMS;
using Mozart.Common;

namespace Mozart.PalmShop.ShopCode
{
    public partial class HelpDoc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strSiteCode = string.Empty;
                string strCatID = string.Empty;
                if (null == Request.QueryString["SiteCode"])
                {
                    strSiteCode = "VYIGO";
                }
                if (null == Request.QueryString["CatID"])
                {
                    strCatID = "VYIGO";
                }
                strSiteCode = Common.Common.NoHtml(Request.QueryString["SiteCode"].ToString());
                strCatID = Common.Common.NoHtml(Request.QueryString["CatID"].ToString());

                DAL.CMS.ArticleDAL dalCat = new DAL.CMS.ArticleDAL();
                DataSet dsArt = dalCat.GetCategoryList(strSiteCode, strCatID);

                List<CMS_Article> liArtList = new List<CMS_Article>();
                if (null != dsArt && dsArt.Tables.Count > 0 && dsArt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsArt.Tables[0].Rows)
                    {
                        CMS_Article model = DataConvert.DataRowToModel<CMS_Article>(row);
                        liArtList.Add(model);
                    }
                }

                string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/helpdoc.html"));
                JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

                context.TempData["artList"] = liArtList;

                JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
                t.Render(Response.Output);
            }
        }
    }
}