using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// GetKeyword 的摘要说明
    /// </summary>
    public class GetKeyword : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ReturnListObj<KeywordInfo> ro = JsonConvert.DeserializeObject<ReturnListObj<KeywordInfo>>(Helper.GetJson(context, "GetKeyword"));

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