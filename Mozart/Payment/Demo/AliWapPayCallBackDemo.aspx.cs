using DAL.SYS;
using Model.SYS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.Payment.Demo
{
    public partial class AliWapPayCallBackDemo : AliWapPayCallBackBasePage
    {
        public override void OnPaySucceed(AliWapPayCallBackInfo info)
        {
            ExceptionLog log = new ExceptionLog();
            log.Message = string.Format("[CallBackDemo]订单号：{0},支付宝交易号：{1}",
                info.out_trade_no, info.trade_no);
            ExceptionLogDAL.InsertExceptionLog(log);
        }

        public override string GetSiteCode()
        {
            return "VYIGO";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}