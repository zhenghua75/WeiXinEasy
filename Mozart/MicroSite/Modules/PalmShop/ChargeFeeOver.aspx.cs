using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.SYS;
using Model.SYS;
using Mozart.Payment;

namespace Mozart.PalmShop.ShopCode
{
    public partial class ChargeFeeOver : WXJSAPIPayNotifyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override string GetSiteCode()
        {
            return "VYIGO";
        }

        public override void OnPaySucceed(WXJSAPIPayNotifyInfo info)
        {
            //ExceptionLog log = new ExceptionLog();
            //log.Message = string.Format("Openid:{0},订单号：{1},附加消息：{2}", info.OpenId, info.OutTradeNo, info.Attach);
            //ExceptionLogDAL.InsertExceptionLog(log);
            try
            {
                srChargeFee.wcRequestData scRequestData = new srChargeFee.wcRequestData();
                srChargeFee.wcResponseData scResponseData = new srChargeFee.wcResponseData();
                srChargeFee.OtherServiceSoapClient scChargeFee = new srChargeFee.OtherServiceSoapClient();

                if (info.Attach.Split('|')[0] == "hfcz")
                {
                    scRequestData.ChargeType = "hfcz";
                }
                else
                {
                    scRequestData.ChargeType = "qbcz";
                }
                scRequestData.OrderID = info.OutTradeNo;
                scRequestData.ChargeNo = info.Attach.Split('|')[1].ToString();
                scRequestData.ChargeAmount = info.Attach.Split('|')[2].ToString();// (Int64.Parse(info.TotalFee.ToString()) / 100).ToString();
                scRequestData.OpenID = info.OpenId;
                scResponseData = scChargeFee.PutChargeFee(scRequestData);

                //log.Message = string.Format("充值结果：{1},充值说明：{2}", scResponseData.Ret, scResponseData.Msg);
                //ExceptionLogDAL.InsertExceptionLog(log);
            }
            catch
            {
                ExceptionLog log = new ExceptionLog();
                log.Message = string.Format("Openid:{0},订单号：{1},附加消息：{2}，出错！", info.OpenId, info.OutTradeNo, info.Attach);
                ExceptionLogDAL.InsertExceptionLog(log);
            }
        }
    }
}