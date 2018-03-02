using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// jump 的摘要说明
    /// </summary>
    public class jump : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string content = context.Request["content"];
            context.Response.ContentType = "text/html;charset=utf-8";
            string htmlFragment = Helper.GetHtmnlFragment(context, "jump_"+content);
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