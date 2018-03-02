using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    public class ReturnListObj<T>
    {
        public int code { get; set; }
        public string notice { get; set; }
        public List<T> data { get; set; }
    }
    public class ReturnObj<T>
    {
        public int code { get; set; }
        public string notice { get; set; }
        public T data { get; set; }
    }
}