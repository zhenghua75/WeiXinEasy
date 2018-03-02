using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// BatchGetGenerateTaskByTag 的摘要说明
    /// </summary>
    public class BatchGetGenerateTaskByTag : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            int status = 0;

            if (context.Session!=null && context.Session.Count>0 && context.Session["BatchGetGenerateTaskByTag_status"] != null)
            {
                status = Convert.ToInt32(context.Session["BatchGetGenerateTaskByTag_status"]);
                if (status < 4)
                {
                    context.Session["BatchGetGenerateTaskByTag_status"] = status + 1;
                }
            }
            ReturnListObj<Generate> ro;
            if (status > 0)
            {
                ro = JsonConvert.DeserializeObject<ReturnListObj<Generate>>(Helper.GetJson(context, "BatchGetGenerateTaskByTag_" + status.ToString()));
            }
            else
            {
                ro = JsonConvert.DeserializeObject<ReturnListObj<Generate>>(Helper.GetJson(context, "BatchGetGenerateTaskByTag"));
            }
            Helper.ProcessContext(context, JsonConvert.SerializeObject(ro));
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