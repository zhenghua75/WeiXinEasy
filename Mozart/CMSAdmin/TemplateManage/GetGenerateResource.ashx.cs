using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// GetGenerateResource 的摘要说明
    /// </summary>
    public class GetGenerateResource : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ReturnObj<GenerateResource> ro = JsonConvert.DeserializeObject<ReturnObj<GenerateResource>>(Helper.GetJson(context, "GetGenerateResource"));

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