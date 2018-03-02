using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    public class BaseConfigRequestObj
    {
        public int ecid { get; set; }
        public int? template_id { get; set; }
    }
    /// <summary>
    /// GetBaseConfig 的摘要说明
    /// </summary>
    public class GetBaseConfig : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var jsonString = Helper.GetRequestJsonObj(context);

            BaseConfigRequestObj requestObj = new BaseConfigRequestObj();
            if(!string.IsNullOrEmpty(jsonString))
            {
                requestObj = JsonConvert.DeserializeObject<BaseConfigRequestObj>(jsonString);
            }

            ReturnObj<BaseConfig> ro;
            if (requestObj.template_id.HasValue)
            {
                ro = JsonConvert.DeserializeObject<ReturnObj<BaseConfig>>(Helper.GetJson(context, "GetBaseConfig_" + requestObj.template_id.Value.ToString()));
            }
            else
            {
                ro = JsonConvert.DeserializeObject<ReturnObj<BaseConfig>>(Helper.GetJson(context, "GetBaseConfig"));
            }
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