using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// StartGenerate 的摘要说明
    /// </summary>
    public class StartGenerate : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Session["BatchGetGenerateTaskByTag_status"] = 1;
            ReturnObj<Generate> ro = JsonConvert.DeserializeObject<ReturnObj<Generate>>(Helper.GetJson(context, "StartGenerate"));

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