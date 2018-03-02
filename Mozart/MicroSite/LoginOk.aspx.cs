using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class LoginOk : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Default/LoginOK.html"));

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["title"] = "用户登录完成";
            if (null == Session["strName"])
            {
                context.TempData["username"] = "未登录";
            }
            context.TempData["username"] = Session["strName"].ToString();
            context.TempData["customerid"] = Session["strCustomerID"].ToString();
            context.TempData["sitecode"] = Session["strSiteCode"].ToString();
            context.TempData["footer"] = "奥琦微商易";
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}