using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    public class FuncInfo
    {
        public int function_id { get; set; }
        public int id { get; set; }
        public int imageid { get; set; }
        public string image_url { get; set; }
        public string wap_url { get; set; }
        public int ecid { get; set; }
        public string resourcetype { get; set; }
        public string shelltype { get; set; }
        public int partflag { get; set; }
        public bool ishome { get; set; }
        public string text { get; set; }
        public string textcolor { get; set; }
        public string backgroundcolor { get; set; }
        public int moduleid { get; set; }
        public int bizmoduleid { get; set; }
        public int moduletypeid { get; set; }
        public string moduleurl { get; set; }
        public string themesimagesremark { get; set; }
        public int ordernum { get; set; }
        public string showbizmodulename { get; set; }
    }
}