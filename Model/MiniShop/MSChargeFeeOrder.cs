using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    public class MSChargeFeeOrder
    {
        public string ID { get; set; }
        public string OpenID { get; set; }
        public int ChargeType { get; set; }
        public string CustNo { get; set; }
        public int ChargeAmount { get; set; }
        public string ChargeIP { get; set; }
        public DateTime CreateTime { get; set; }
        public int State { get; set; }
        public string Remark { get; set; }
    }
}
