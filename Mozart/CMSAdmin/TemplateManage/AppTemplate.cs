using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    public class AppTemplate
    {
        public string app_name { get; set; }
        public string wap_url { get; set; }
        public int template_id { get; set; }
        public string template_name { get; set; }
        public string shell_type { get; set; }
        public string app_image_url { get; set; }
        public string weixin_image_url { get; set; }
        public string mp_image_url { get; set; }
        public string description { get; set; }
        public int enabled_flag { get; set; }
    }
}