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
    public partial class Reg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strMobile = string.Empty;
            string strUserName = string.Empty;
            string strPassWord = string.Empty;
            string strPassWord1 = string.Empty;
            string strSiteCode = string.Empty;
            string strSiteName = string.Empty;
            string strErrInfo = string.Empty;
            if (null != Request.QueryString["SiteCode"])
            {
                strSiteCode = Common.Common.NoHtml(Request.QueryString["SiteCode"].ToString());
                CustomerSession.strSiteCode = strSiteCode;
            }
            //strSiteID = "KM_HLF";
            DAL.SYS.AccountDAL dalAccount = new DAL.SYS.AccountDAL();
            DataSet dsAccount = dalAccount.GetSiteData(strSiteCode);
            if (null != dsAccount && dsAccount.Tables.Count > 0 && dsAccount.Tables[0].Rows.Count > 0)
            {
                strSiteCode = dsAccount.Tables[0].Rows[0]["SiteCode"].ToString();
                strSiteName = dsAccount.Tables[0].Rows[0]["Name"].ToString();
                GlobalSession.strSiteCode = strSiteCode;
            }

            if (null != Request.QueryString["action"])
            {
                if (Common.Common.NoHtml(Request.QueryString["action"].ToString()) == "save")
                {
                    strMobile = Request.Form["mobile"].ToString();
                    strUserName = Request.Form["username"].ToString();
                    strPassWord = Request.Form["password"].ToString();
                    strPassWord1 = Request.Form["password1"].ToString();

                    if (string.IsNullOrEmpty(strMobile) || string.IsNullOrEmpty(strUserName) || string.IsNullOrEmpty(strPassWord) || string.IsNullOrEmpty(strPassWord1))
                    {
                        strErrInfo = "请完整填写注册信息！";
                    }
                    if (strPassWord != strPassWord1)
                    {
                        strErrInfo = "两次输入的密码不一致！";
                    }

                    DAL.SYS.CustomerDAL dalCustomer = new DAL.SYS.CustomerDAL();

                    if (dalCustomer.CheckMobile(strMobile))
                    {
                        strErrInfo = "此手机号码已经注册！";
                    }
                    else
                    {
                        Model.SYS.SYS_Customer modelAdd = new Model.SYS.SYS_Customer
                        {
                            ID = Guid.NewGuid().ToString("N").ToUpper(),
                            Mobile = strMobile,
                            Name = strUserName,
                            PassWord = Common.Common.MD5(strPassWord),
                            OpenID = "",
                            SiteCode = GlobalSession.strSiteCode
                        };
                        if (dalCustomer.AddCustomerData(modelAdd))
                        {
                            strErrInfo = "账户添加成功！";
                            Response.Redirect("ProductList.aspx?SiteCode=" + strSiteCode, false);
                            
                        }
                        else
                        {
                            strErrInfo = "账户添加失败！";
                        }
                    }
                }
            }
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Default/Reg.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["SiteCode"] = GlobalSession.strSiteCode;
            context.TempData["ErrInfo"] = strErrInfo;
            context.TempData["Title"] = "用户注册";
            context.TempData["SiteName"] = strSiteName;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}