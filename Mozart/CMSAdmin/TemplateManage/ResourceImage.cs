using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    public class ResourceImage
    {
        public int imageid { get; set; }
        public int partflag { get; set; }
        public bool isself { get; set; }
        public string image_url { get; set; }
        public string resourcetype { get; set; }
        public string themesimagesremark { get; set; }
    }
}