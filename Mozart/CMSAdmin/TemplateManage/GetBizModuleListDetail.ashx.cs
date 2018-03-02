using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// GetBizModuleListDetail 的摘要说明
    /// </summary>
    public class GetBizModuleListDetail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ReturnObj<BizModuleListDetail> ro = JsonConvert.DeserializeObject<ReturnObj<BizModuleListDetail>>(Helper.GetJson(context, "GetBizModuleListDetail"));

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