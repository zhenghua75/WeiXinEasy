using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    public class GenerateResource
    {
        public FuncInfo bg { get; set; }
        public List<FuncInfo> nav { get; set; }
        public List<FuncInfo> fun { get; set; }
        public List<FuncInfo> menu { get; set; }
    }
}