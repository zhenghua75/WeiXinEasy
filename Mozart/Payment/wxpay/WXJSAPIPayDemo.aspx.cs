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
    public partial class WXJSAPIPayDemo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ViewState["openid"] = this.Request.QueryString["openid"];
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string openid = ViewState["openid"].ToString();
                if (!string.IsNullOrEmpty(openid))
                {
                    string ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    //ip = "127.0.0.1";
                    string notify_url = GetNotifyUrl("/Payment/wxpay/WXJSAPIPayNotifyDemo.aspx");
                    WXJSAPIPay pay=new WXJSAPIPay("VYIGO");
                    string prepay_id = pay.GetJSAPIPrepayID("我的订单", DateTime.Now.ToString("yyyyMMddHHmmss"), 100, ip, openid, notify_url, attach: txtMobile.Text);
                    string script = "<script>document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {	WeixinJSBridge.invoke('getBrandWCPayRequest',"
                        + pay.GetJSAPIParameters(prepay_id)
                        + ",function(res){ WeixinJSBridge.log(res.err_msg); if(res.err_msg == 'get_brand_wcpay_request:ok' )"
                        + " { alert(res.err_code+res.err_desc+res.err_msg); } });});</script>";
                    //lblDesc.Text = script;
                    ExceptionLog log = new ExceptionLog();
                    log.Message = script;
                    ExceptionLogDAL.InsertExceptionLog(log);

                    Response.Write(script);
                    //pay.DirectWXJSAPIPay(this.Response, "我的订单", DateTime.Now.ToString("yyyyMMddHHmmss"), 100, ip, openid, notify_url, attach: txtMobile.Text);
                }
            }
            catch (Exception ex)
            {
                lblDesc.Text = ex.Message;
                //Response.Write(ex.Message);
            }
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