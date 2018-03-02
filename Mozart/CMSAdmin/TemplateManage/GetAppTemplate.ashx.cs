using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// GetAppTemplate 的摘要说明
    /// </summary>
    public class GetAppTemplate : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
//biz_id: "80"
//ecid: 608989
//page_index: 1
//page_size: 9999
            ReturnObj<PageInfo<AppTemplate>> ro = JsonConvert.DeserializeObject<ReturnObj<PageInfo<AppTemplate>>>(Helper.GetJson(context, "GetAppTemplate"));

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