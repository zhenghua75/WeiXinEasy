using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.ACT
{
    public class RushCoupon
    {
        public string ID { get; set; }
        public string SiteCode { get; set; }
        public string Name {get;set;}
        public string Comments {get;set;}
        public int Amount { get; set; }
        public int State { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
