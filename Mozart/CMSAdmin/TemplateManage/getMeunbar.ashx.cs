using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// getMeunbar 的摘要说明
    /// </summary>
    public class getMeunbar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            context.Response.ContentType = "text/html;charset=utf-8";
            string htmlFragment = Helper.GetHtmnlFragment(context, "getMeunbar");
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