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
    public partial class WXJSAPIPayNotifyDemo : WXJSAPIPayNotifyBasePage
    {
        public override void OnPaySucceed(WXJSAPIPayNotifyInfo info)
        {
            ExceptionLog log = new ExceptionLog();
            log.Message = string.Format("Openid:{0},订单号：{1},附加消息：{2}",
                info.OpenId,info.OutTradeNo,info.Attach);
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