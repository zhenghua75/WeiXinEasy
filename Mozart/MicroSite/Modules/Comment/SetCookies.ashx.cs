using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mozart.Common;

namespace Mozart.Comment
{
    /// <summary>
    /// SetCookies 的摘要说明
    /// </summary>
    public class SetCookies : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string CookiesName = string.Empty;
            string RequestUrl = string.Empty;
            int time = 0;
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request["cname"] != null && Request["cname"] != "")
            {
                CookiesName = Request["cname"];
            }
            else
            {
                CookiesName = "pageurl";
            }
            if (Request["curl"] != null && Request["curl"] != "")
            {
                RequestUrl = Request["curl"];
            }
            if (Request["time"] != null && Request["time"] != "")
            {
                try
                {
                    time = Convert.ToInt32(Request["time"]);
                }
                catch (Exception)
                {
                    time = 2;
                }
            }
            else
            {
                time = 2;
            }
            JQDialog.SetCookies(CookiesName, RequestUrl, time);
            Response.Write("{\"succeed\":true,\"logurl\":\"../PalmShop/ShopCode/UserLogin.aspx\"}");
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