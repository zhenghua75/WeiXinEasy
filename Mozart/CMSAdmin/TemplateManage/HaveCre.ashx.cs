using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// HaveCre 的摘要说明
    /// </summary>
    public class HaveCre : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ReturnObj<bool> ro = JsonConvert.DeserializeObject<ReturnObj<bool>>(Helper.GetJson(context, "HaveCre"));

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