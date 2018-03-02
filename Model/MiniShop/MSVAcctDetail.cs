using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    public class MSVAcctDetail
    {
        private string custID;

        public string CustID
        {
            get { return custID; }
            set { custID = value; }
        }
        private int amount;

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        private string chargeType;

        public string ChargeType
        {
            get { return chargeType; }
            set { chargeType = value; }
        }
        private string siteCode;

        public string SiteCode
        {
            get { return siteCode; }
            set { siteCode = value; }
        }
        private string ext_Fld1;

        public string Ext_Fld1
        {
            get { return ext_Fld1; }
            set { ext_Fld1 = value; }
        }
        private int isReceive;

        public int IsReceive
        {
            get { return isReceive; }
            set { isReceive = value; }
        }

        private DateTime vdate;

        public DateTime Vdate
        {
            get { return vdate; }
            set { vdate = value; }
        }
    }
}
