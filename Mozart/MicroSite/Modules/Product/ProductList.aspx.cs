using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.SP;
using DAL.Product;
using System.Data;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class ProductList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strSiteCode = string.Empty;
            string strCatID = string.Empty;
            if (null == Request.QueryString["SiteCode"])
            {
                return;
            }         
            strSiteCode = Common.Common.NoHtml(Request.QueryString["SiteCode"].ToString());
            Session["strSiteCode"] = strSiteCode;
            if (null == Request.QueryString["CatID"])
            {
                strCatID = string.Empty;
            }
            else
            {
                strCatID = Common.Common.NoHtml(Request.QueryString["CatID"].ToString());
            }

            List<SP_Product> liProduct = new List<SP_Product>();

            ProductDAL dal = new ProductDAL();
            //DataSet ds = dal.GetProductList("KM_HLF", "31A10FB1-43C2-48A3-A2BA-5B451DE13276");
            //?SiteCode=KM_HLF&CatID=31A10FB1-43C2-48A3-A2BA-5B451DE13276
            //?SiteCode=KM_HLF&CatID=02F25D57-1BA6-438A-B965-428160A0AA68
            DataSet ds = dal.GetProductList(strSiteCode, strCatID);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                SP_Product model = DataConvert.DataRowToModel<SP_Product>(row);
                liProduct.Add(model);
            }


            //读取模板内容 
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Default/ProductList.html"));

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["sitecode"] = strSiteCode;
            context.TempData["title"] = "商品列表";
            if (null == Session["strName"])
            {
                context.TempData["username"] = "未登录";
            }
            else
            {
                context.TempData["username"] = CustomerSession.strName;
            }
            context.TempData["product_list"] = liProduct;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}