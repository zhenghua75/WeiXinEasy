using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.Payment.Demo
{
    public partial class wfmChagreFee : System.Web.UI.Page 
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            srChargeFee.wcRequestData scRequestData = new srChargeFee.wcRequestData();
            srChargeFee.wcResponseData scResponseData = new srChargeFee.wcResponseData();
            srChargeFee.OtherServiceSoapClient scChargeFee = new srChargeFee.OtherServiceSoapClient();

            scRequestData.ChargeType = "qbcz";
            scRequestData.OrderID = "TEST0001";
            scRequestData.ChargeNo = "6716808";
            scRequestData.ChargeAmount = "1";
            scRequestData.OpenID = "asanhbyn";
            scResponseData = scChargeFee.PutChargeFee(scRequestData);
        }
    }
}