using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// getCode 的摘要说明
    /// </summary>
    public class getCode : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html;charset=UTF-8";
            context.Response.Write("http://www.veshow.cn/uploadFiles/dimeCode/608989/homepage608989.jpg");
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