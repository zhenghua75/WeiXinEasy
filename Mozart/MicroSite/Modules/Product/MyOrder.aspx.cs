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
    public partial class MyOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strSiteCode = string.Empty;
            string strCustomerID = string.Empty;

            if (null != Request.QueryString["SiteCode"])
            {
                strSiteCode = Common.Common.NoHtml(Request.QueryString["SiteCode"].ToString());
                Session["strSiteCode"] = strSiteCode;
            }

            if (null == Request.QueryString["CustomerID"])
            {
                if ( null == Session["strCustomerID"])
                {
                    Response.Redirect("Login.aspx?SiteCode=" + Session["strSiteCode"].ToString(), false);
                    return;
                }
                else
                {                    
                    strCustomerID = Session["strCustomerID"].ToString();
                }
            }

            List<SP_Order> liOrder = new List<SP_Order>();

            CartDAL dal = new CartDAL();
            DataSet ds = dal.GetMyOrderList(strSiteCode, strCustomerID);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                SP_Order model = DataConvert.DataRowToModel<SP_Order>(row);
                liOrder.Add(model);
            }

            //读取模板内容 
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Default/MyOrder.html"));

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["title"] = "我的订单";
            if (null == Session["strName"])
            {
                context.TempData["username"] = "未登录";
            }
            else
            {
                context.TempData["username"] = CustomerSession.strName;
            }
            context.TempData["stiecode"] = strSiteCode;
            context.TempData["customerid"] = strCustomerID;
            if (null == Session["strName"])
            {
                context.TempData["username"] = "未登录";
            }
            else
            {
                context.TempData["username"] = CustomerSession.strName;
            }
            context.TempData["order_list"] = liOrder;
            context.TempData["footer"] = "奥琦微商易";
            
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}