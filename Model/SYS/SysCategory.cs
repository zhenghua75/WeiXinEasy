using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.SYS
{
   public class SysCategory
    {
        private string iD;
        private string siteName;
        private string siteDesc;
        private int siteOrder;
        private bool isDel;

       public string ID
       {
           get { return iD; }
           set { iD = value; }
       }
       public string SiteName
       {
           get { return siteName; }
           set { siteName = value; }
       }
       public string SiteDesc
       {
           get { return siteDesc; }
           set { siteDesc = value; }
       }
       public int SiteOrder
       {
           get { return siteOrder; }
           set { siteOrder = value; }
       }
       public bool IsDel
       {
           get { return isDel; }
           set { isDel = value; }
       }
    }
}
