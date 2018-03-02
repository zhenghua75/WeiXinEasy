using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.Payment.Demo
{
    public partial class AliWapPayDemo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void BtnAlipay_Click(object sender, EventArgs e)
        {
            AliWapPay pay = new AliWapPay("VYIGO");
            string call_back_url = GetNotifyUrl("/Payment/Demo/AliWapPayCallBackDemo.aspx");
            string notify_url = GetNotifyUrl("/Payment/Demo/AliWapPayNotifyDemo.aspx");
            pay.DirectAliWayPay(this.Response,WIDsubject.Text.Trim(),WIDout_trade_no.Text.Trim(),
                WIDtotal_fee.Text.Trim(), "001",call_back_url,notify_url);
        }

        /// <summary>
        /// 获取服务器异步通知页面完整URL
        /// </summary>
        /// <returns></returns>
        public string GetNotifyUrl(string url)
        {
            return string.Format("{0}{1}",
                HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, ""),
                url);
        }
    }
}