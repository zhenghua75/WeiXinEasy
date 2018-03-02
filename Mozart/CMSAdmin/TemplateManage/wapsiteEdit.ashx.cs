using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// wapsiteEdit 的摘要说明
    /// </summary>
    public class wapsiteEdit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string temId = context.Request["temId"];
            context.Response.ContentType = "text/html;charset=utf-8";
            string htmlFragment = Helper.GetHtmnlFragment(context, "wapsiteEdit_" + temId);
            context.Response.Write(htmlFragment);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}