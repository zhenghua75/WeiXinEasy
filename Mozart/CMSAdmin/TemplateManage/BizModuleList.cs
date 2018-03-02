using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    public class BizModuleList
    {
        public int published { get; set; }
        public int bizmoduleid { get; set; }
        public int moduleid { get; set; }
        public int biztypeid { get; set; }
        public string bizmodulename { get; set; }
        public string showbizmodulename { get; set; }
        public int level { get; set; }
        public int moduletypeid { get; set; }
        public int app_status { get; set; }
        public int weixin_status { get; set; }
        public int mp_status { get; set; }
        public int multflag { get; set; }
        public string wapurl { get; set; }
    }
}