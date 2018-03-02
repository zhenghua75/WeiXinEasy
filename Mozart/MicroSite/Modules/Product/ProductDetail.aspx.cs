using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Product;
using Model.SP;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class ProductDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strProductID = string.Empty;   
            string strSiteCode = string.Empty;
            if (null == Request.QueryString["id"])
            {
                return;
            }
            strProductID = Common.Common.NoHtml(Request.QueryString["id"].ToString());
            ProductDAL dal = new ProductDAL();
            DataSet ds = dal.GetProductDetail(strProductID);

            SP_Product model = new SP_Product();
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model = DataConvert.DataRowToModel<SP_Product>(ds.Tables[0].Rows[0]);                
            }
            strSiteCode = model.SiteCode;

            //读取模板内容 
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Default/ProductDetail.html"));

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["sitecode"] = strSiteCode;
            context.TempData["title"] = "商品详细信息";
            context.TempData["pDetail"] = model;
            if (null == Session["strName"])
            {
                context.TempData["username"] = "未登录";
            }
            else
            {
                context.TempData["username"] = CustomerSession.strName;
            }
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}