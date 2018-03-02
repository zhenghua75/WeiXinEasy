using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    public class PageInfo<T>
    {
        public int page_index { get; set; }
        public int total_pages { get; set; }
        public int total_items { get; set; }
        public int page_size { get; set; }
        public bool has_more { get; set; }
        public List<T> items { get; set; }
    }
}