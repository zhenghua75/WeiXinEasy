using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// GetAutoReply 的摘要说明
    /// </summary>
    public class GetAutoReply : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
//ecid: "608989"
//service_type: "3"
            ReturnObj<AutoReply> ro = JsonConvert.DeserializeObject<ReturnObj<AutoReply>>(Helper.GetJson(context, "GetAutoReply"));

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