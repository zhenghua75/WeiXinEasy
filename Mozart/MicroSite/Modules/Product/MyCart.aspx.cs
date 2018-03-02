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
    public partial class MyCart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strSiteCode = string.Empty;
            string strCustomerID = string.Empty;
            if (null == Request.QueryString["SiteCode"] || null == Request.QueryString["CustomerID"])
            {
                if (null != Session["SiteCode"])
                {
                    Response.Redirect("Login.aspx?SiteCode=" + Session["SiteCode"].ToString(), false);
                }
                return;
            }
            strSiteCode = Common.Common.NoHtml(Request.QueryString["SiteCode"].ToString());
            strCustomerID = Common.Common.NoHtml(Request.QueryString["CustomerID"].ToString());

            List<SP_MyCart> liCart = new List<SP_MyCart>();

            CartDAL dal = new CartDAL();
            //DataSet ds = dal.GetProductList("KM_HLF", "31A10FB1-43C2-48A3-A2BA-5B451DE13276");
            //?SiteCode=KM_HLF&CatID=31A10FB1-43C2-48A3-A2BA-5B451DE13276
            //?SiteCode=KM_HLF&CatID=02F25D57-1BA6-438A-B965-428160A0AA68
            DataSet ds = dal.GetMyCartList(strSiteCode, strCustomerID);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                //SP_ShoppingCart model = DataConvert.DataRowToModel<SP_ShoppingCart>(row);
                SP_MyCart model = DataConvert.DataRowToModel<SP_MyCart>(row);
                liCart.Add(model);
            }


            string strReturn = dal.GetMyCatSum(strSiteCode, strCustomerID);

            //读取模板内容 
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Default/MyCart.html"));

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["title"] = "我的购物车";
            if (null == Session["strName"])
            {
                context.TempData["username"] = "未登录";
            }
            else
            {
                context.TempData["username"] = CustomerSession.strName;
            }
            context.TempData["sitecode"] = strSiteCode;
            context.TempData["customerid"] = strCustomerID;
            context.TempData["cart_list"] = liCart;
            context.TempData["footer"] = "奥琦微商易";

            context.TempData["cs"] = strReturn.Split('|')[1].ToString();
            context.TempData["ps"] = strReturn.Split('|')[2].ToString();
            context.TempData["sp"] = strReturn.Split('|')[3].ToString();

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}