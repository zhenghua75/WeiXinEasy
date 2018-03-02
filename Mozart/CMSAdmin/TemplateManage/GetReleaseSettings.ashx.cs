using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// GetReleaseSettings 的摘要说明
    /// </summary>
    public class GetReleaseSettings : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ReturnObj<ReleaseSetting> ro = JsonConvert.DeserializeObject<ReturnObj<ReleaseSetting>>(Helper.GetJson(context, "GetReleaseSettings"));

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