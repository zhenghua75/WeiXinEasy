using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.WeiXin
{
    public class Menu
    {
        public Menu()
        {
            ID = Guid.NewGuid().ToString("N");
            AddTime = DateTime.Now;
        }

        public string ID { get; set; }

        public string WXConfigID { get; set; }

        public string MenuType { get; set; }

        public string ParentMenuID { get; set; }

        public string ButtonType { get; set; }

        public string ButtonName { get; set; }

        public string ButtonKey { get; set; }

        public string AccessLink { get; set; }

        public string RedirectScope { get; set; }

        public string RedirectState { get; set; }

        public DateTime? AddTime { get; set; }

        public int? OrderNum { get; set; }

        public int? Enabled { get; set; }
    }
}
