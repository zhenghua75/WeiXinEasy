using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    public class ResourceImageRequestObj
    {
        public int ecid { get; set; }
        public int page_index { get; set; }
        public int page_size { get; set; }
        public string resourcetype { get; set; }
        public int template_id { get; set; }
    }
    /// <summary>
    /// GetResourceImages 的摘要说明
    /// </summary>
    public class GetResourceImages : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var jsonString = Helper.GetRequestJsonObj(context);
            ResourceImageRequestObj requestObj = new ResourceImageRequestObj();
            if (!string.IsNullOrEmpty(jsonString))
            {
                requestObj = JsonConvert.DeserializeObject<ResourceImageRequestObj>(jsonString);
            }
            ReturnObj<PageInfo<ResourceImage>> ro;
            switch (requestObj.resourcetype)
            {
                case "top":
                    ro = JsonConvert.DeserializeObject<ReturnObj<PageInfo<ResourceImage>>>(Helper.GetJson(context, "GetResourceImages_top"));                    
                    break;
                //case "help":
                //    ro = JsonConvert.DeserializeObject<ReturnObj<PageInfo<ResourceImage>>>(Helper.GetJson(context, "GetResourceImages_help"));                    
                //    break;
                //case "cover":
                //    ro = JsonConvert.DeserializeObject<ReturnObj<PageInfo<ResourceImage>>>(Helper.GetJson(context, "GetResourceImages_cover"));                    
                //    break;
                //case "bg":
                //    ro = JsonConvert.DeserializeObject<ReturnObj<PageInfo<ResourceImage>>>(Helper.GetJson(context, "GetResourceImages_bg"));                    
                //    break;
                default:
                    ro = JsonConvert.DeserializeObject<ReturnObj<PageInfo<ResourceImage>>>(Helper.GetJson(context, "GetResourceImages_all"));                    
                    break;
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