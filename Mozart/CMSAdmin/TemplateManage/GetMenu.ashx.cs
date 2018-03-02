using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// GetMenu 的摘要说明
    /// </summary>
    public class GetMenu : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //ecid: "608989"
            ReturnListObj<string> ro = JsonConvert.DeserializeObject<ReturnListObj<string>>(Helper.GetJson(context, "GetMenu"));

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