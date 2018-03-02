using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class VProductDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strProductID = string.Empty;
            string strSiteCode = string.Empty;
            string strOpenID = string.Empty;
            if (null == Request.QueryString["id"])
            {
                return;
            }
            if (null == Request.QueryString["sitecode"])
            {
                return;
            }
            if (null == Request.QueryString["openid"])
            {
                return;
            }
            strProductID = Common.Common.NoHtml(Request.QueryString["id"].ToString());
            strSiteCode = Common.Common.NoHtml(Request.QueryString["sitecode"].ToString());
            if (Request.QueryString["openid"].ToString().Length > 29 || Request.QueryString["openid"].ToString().Length < 25)
            {
                return;
            }
            else
            {
                strOpenID = Request.QueryString["openid"].ToString();
            }
            DAL.MiniShop.MSProductDAL dal = new DAL.MiniShop.MSProductDAL();
            DataSet ds = dal.GetProductByID(strProductID);

            Model.MiniShop.MSVProduct model = new Model.MiniShop.MSVProduct();
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model = DataConvert.DataRowToModel<Model.MiniShop.MSVProduct>(ds.Tables[0].Rows[0]);
            }

            //读取模板内容 
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Default/VProductDetail.html"));

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["sitecode"] = strSiteCode;
            context.TempData["openid"] = strOpenID;
            context.TempData["title"] = "商品详细信息";
            context.TempData["pDetail"] = model;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}