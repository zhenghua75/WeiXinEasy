using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// GetCarouselImages 的摘要说明
    /// </summary>
    public class GetCarouselImages : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ReturnListObj<CarouselImage> ro = JsonConvert.DeserializeObject<ReturnListObj<CarouselImage>>(Helper.GetJson(context, "GetCarouselImages"));

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