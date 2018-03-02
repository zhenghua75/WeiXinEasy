using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Site
{
    public class ConRecords
    {
        public string ID { get; set; }
        public string SiteCode { get; set; }
        public string OpenID { get; set; }
        public string MemberShipNo { get; set; }
        public float Price { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
