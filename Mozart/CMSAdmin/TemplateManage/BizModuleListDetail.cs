using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    public class BizModuleListDetail
    {
        public int bizmoduleid { get; set; }
        public int moduleid { get; set; }
        public bool onlyone { get; set; }
        public string bizmodulename { get; set; }
        public int customurlflag { get; set; }
        public string moduleurl { get; set; }
        public string bizdescription { get; set; }
        public bool iphone { get; set; }
        public bool android { get; set; }
        public bool mp { get; set; }
        public bool weixin { get; set; }
        public bool sms { get; set; }
        public bool permissions { get; set; }
        public string image1_url { get; set; }
        public string image2_url { get; set; }
        public string image3_url { get; set; }
        public int moduletypeid { get; set; }
    }
}