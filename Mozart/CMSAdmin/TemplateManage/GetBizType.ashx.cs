using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// GetBizType 的摘要说明
    /// </summary>
    public class GetBizType : IHttpHandler
    {
        
        public void ProcessRequest(HttpContext context)
        {
            //ecid: 608989
            ReturnListObj<BizType> ro = JsonConvert.DeserializeObject<ReturnListObj<BizType>>(Helper.GetJson(context,"GetBizType"));

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