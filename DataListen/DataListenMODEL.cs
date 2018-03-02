using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataListen
{
    public class DataListenMODEL
    {
        public class CouponInfo
        {
            public string ID { get; set; }
            public string OpenID { get; set; }
            public string ActTitle { get; set; }
            public string ActContent { get; set; }
            public string CouponStatus { get; set; }
            public string AddTime { get; set; }
            public string CheckTime { get; set; }
            public string CouponNo { get; set; }
        }
    }
}
