using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// GetLogoImage 的摘要说明
    /// </summary>
    public class GetLogoImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //ecid: "608989"
            ReturnObj<BizType> ro = JsonConvert.DeserializeObject<ReturnObj<BizType>>(Helper.GetJson(context, "GetLogoImage"));

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