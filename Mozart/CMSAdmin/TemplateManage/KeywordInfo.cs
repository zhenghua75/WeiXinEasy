using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    public class KeywordInfo
    {
        public string LinkUrl { get; set; }
        public string Linkcontent { get; set; }
        public int MsgID { get; set; }
        public int BizModuleId { get; set; }
        public int ECID { get; set; }
        public int TemplateID { get; set; }
        public int ModuleID { get; set; }
        public int ModuleType { get; set; }
        public int ServiceType { get; set; }
        public string KeyWord { get; set; }
        public int ReplyType { get; set; }
        public string ReplyTxt { get; set; }
        public int ReplyImageID { get; set; }
        public string ReplyImageTitle { get; set; }
        public int Status { get; set; }
        public string Summary { get; set; }
        public bool DelFlag { get; set; }
        public DateTime AddTime { get; set; }
        public string ImageUrl { get; set; }
        public string BizModuleName { get; set; }
        public string showbizmodulename { get; set; }
    }
}