using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// GetBizModuleList 的摘要说明
    /// </summary>
    public class GetBizModuleList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ReturnListObj<BizModuleList> ro = JsonConvert.DeserializeObject<ReturnListObj<BizModuleList>>(Helper.GetJson(context, "GetBizModuleList"));

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