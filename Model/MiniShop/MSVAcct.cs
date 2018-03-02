using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    public class MSVAcct
    {
        private string custID;

        public string CustID
        {
            get { return custID; }
            set { custID = value; }
        }
        private int v_Amont;

        public int V_Amont
        {
            get { return v_Amont; }
            set { v_Amont = value; }
        }
        private string siteCode;

        public string SiteCode
        {
            get { return siteCode; }
            set { siteCode = value; }
        }
    }
}
