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
    public partial class CheckOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strMobile = string.Empty;
            string strRealName = string.Empty;
            string strAddress = string.Empty;
            string strSiteCode = string.Empty;
            string strSiteName = string.Empty;
            string strErrInfo = string.Empty;
            string strCustomerID = string.Empty;
            if (null != Request.QueryString["SiteCode"] || null != Request.QueryString["CustomerID"])
            {
                strSiteCode = Common.Common.NoHtml(Request.QueryString["SiteCode"].ToString());
                strCustomerID = Common.Common.NoHtml(Request.QueryString["CustomerID"].ToString());
                CustomerSession.strSiteCode = strSiteCode;
                CustomerSession.strCustomerID = strCustomerID;
            }

            if (null != Request.QueryString["action"])
            {
                if (Request.QueryString["action"] == "checkout")
                {
                    strMobile = Common.Common.NoHtml(Request.Form["mobile"].ToString());
                    strRealName = Common.Common.NoHtml(Request.Form["realname"].ToString());
                    strAddress = Common.Common.NoHtml(Request.Form["address"].ToString());

                    if (string.IsNullOrEmpty(strMobile) || string.IsNullOrEmpty(strRealName))
                    {
                        strErrInfo = "请完整填写预订信息！";
                    }

                    DAL.Product.CartDAL dalCustomer = new DAL.Product.CartDAL();

                    try
                    {
                        if (dalCustomer.CheckOutOrder(strSiteCode, CustomerSession.strCustomerID, strRealName, strMobile, strAddress) == 1)
                        {
                            strErrInfo = "订单提交完成！";
                            Response.Redirect("MyOrder.aspx?SiteCode=" + strSiteCode + "&CustomerID=" + strCustomerID, false);
                            return;
                        }
                        else
                        {
                            strErrInfo = "订单提交有误！";
                        }
                    }
                    catch
                    {
                        strErrInfo = "订单提交有误！";
                    }
                }
            }
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Default/CheckOut.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["SiteCode"] = strSiteCode;
            context.TempData["Customer"] = strCustomerID;
            context.TempData["ErrInfo"] = strErrInfo;
            context.TempData["Title"] = "订单结算";
            context.TempData["SiteName"] = strSiteName;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}