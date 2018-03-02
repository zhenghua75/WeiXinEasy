using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    public class Generate
    {
        public string platform_type{get;set;}
        public int task_id{get;set;}
        public int generate_count{get;set;}
        public int regenerate_count{get;set;}
        public int wait_count{get;set;}
        public int status{get;set;}
        public int generate_percent{get;set;}
        public string client_download_url{get;set;}
        public string client_qr_url{get;set;}
        public bool havecer { get; set; }
    }
}