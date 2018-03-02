using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strMobile = string.Empty;
            string strPassWord = string.Empty;
            string strSiteCode = string.Empty;
            string strTitle = string.Empty;
            string strTheme = string.Empty;
            if (null == Session["strSiteCode"])
            {
                return;                
            }
            strSiteCode = Session["strSiteCode"].ToString();
            if (null != Request.QueryString["action"])
            {
                if (Request.QueryString["action"] == "login")
                {
                    strMobile = Common.Common.NoHtml(Request.Form["mobile"].ToString());
                    strPassWord = Common.Common.NoHtml(Request.Form["password"].ToString());                   

                    DAL.SYS.CustomerDAL dal = new DAL.SYS.CustomerDAL();
                    DataSet ds = dal.GetCustomerData(strMobile, Common.Common.MD5(strPassWord), strSiteCode);

                    if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        CustomerSession.strCustomerID = ds.Tables[0].Rows[0]["ID"].ToString();
                        CustomerSession.strMobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                        CustomerSession.strSiteCode = ds.Tables[0].Rows[0]["SiteCode"].ToString();
                        CustomerSession.strOpenID = ds.Tables[0].Rows[0]["OpenID"].ToString();
                        CustomerSession.strName = ds.Tables[0].Rows[0]["Name"].ToString();                       
                        Session["strName"] = CustomerSession.strName;
                        Session["strCustomerID"] = CustomerSession.strCustomerID;
                        Session["strSiteCode"] = CustomerSession.strSiteCode;
                        Session["strOpenID"] = CustomerSession.strOpenID;
                    }
                    //Response.Redirect("ProductList.aspx?SiteCode=" + strSiteCode, false);
                    Response.Redirect("LoginOK.aspx", false);
                }
            }

            DAL.SYS.AccountDAL dalAccount = new DAL.SYS.AccountDAL();
            DataSet dsAccountExt = dalAccount.GetAExtDataBySiteCode(strSiteCode);
            if (null != dsAccountExt && dsAccountExt.Tables.Count > 0 && dsAccountExt.Tables[0].Rows.Count > 0)
            {
                strTheme = dsAccountExt.Tables[0].Rows[0]["Themes"].ToString();
                strTitle = dsAccountExt.Tables[0].Rows[0]["Name"].ToString();
            }

            string text = string.Empty;
            if (!File.Exists(Server.MapPath("Themes/" + strTheme + "/Login.html")))
            {
                text = System.IO.File.ReadAllText(Server.MapPath("Themes/Default/Login.html"));
            }
            else
            {
                text = System.IO.File.ReadAllText(Server.MapPath("Themes/" + strTheme + "/Login.html"));
            }
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["title"] = "用户登录";
            if (null == Session["strName"])
            {
                context.TempData["username"] = "未登录";
            }
            else
            {
                context.TempData["username"] = CustomerSession.strName;
            }
            if (null == Session["customerid"])
            {
                context.TempData["customerid"] = null;
            }
            else
            {
                context.TempData["customerid"] = CustomerSession.strCustomerID;
            }
            context.TempData["sitename"] = strTitle;
            context.TempData["sitecode"] = strSiteCode;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}