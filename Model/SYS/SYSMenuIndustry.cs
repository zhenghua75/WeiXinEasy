using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.SYS
{
   public class SYSMenuIndustry
    {
        private string menuNo;
        private string category;
        public string MenuNo
        {
            get { return menuNo; }
            set { menuNo = value; }
        }
        public string Category
        {
            get { return category; }
            set { category = value; }
        }
    }
}
