using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.Payment.wxpay
{
    public partial class wxpayDemo : System.Web.UI.Page
    {
        protected string JSAPIParameters;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ViewState["openid"]= this.Request.QueryString["openid"];
            }
            if(ViewState["pap"]!=null)
            {
                JSAPIParameters = ViewState["pap"].ToString();
            }
        }

        protected void btnUnifiedOrder_Click(object sender, EventArgs e)
        {
            try
            {
                string openid = ViewState["openid"].ToString();
                if (!string.IsNullOrEmpty(openid))
                {
                    string ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    //ip = "127.0.0.1";
                    string prepay_id = wxpayHelper.GetJSAPIPrepayID("我的订单", DateTime.Now.ToString("yyyyMMddHHmmss"), 100, ip, openid,attach:txtMobile.Text);
                    ViewState["pap"] = wxpayHelper.GetJSAPIParameters(prepay_id);
                    JSAPIParameters = ViewState["pap"].ToString();
                    lblDesc.Text = string.Format("将为您的手机：{0}充值话费1元。确认请点击支付按钮！", txtMobile.Text);
                    //Response.Write(string.Format("将为您的手机：{0}充值话费1元。确认请点击支付按钮！",txtMobile.Text));
                }  
            }
            catch (Exception ex)
            {
                lblDesc.Text = ex.Message;
                //Response.Write(ex.Message);
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
                    string prepay_id = wxpayHelper.GetJSAPIPrepayID("我的订单", DateTime.Now.ToString("yyyyMMddHHmmss"), 100, ip, openid, attach: txtMobile.Text);
                    string script = "<script>document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {	WeixinJSBridge.invoke('getBrandWCPayRequest',"
                        + wxpayHelper.GetJSAPIParameters(prepay_id)
                        + ",function(res){ WeixinJSBridge.log(res.err_msg); if(res.err_msg == 'get_brand_wcpay_request:ok' )"
                        + " { alert(res.err_code+res.err_desc+res.err_msg); } });});</script>";
                    Response.Write(script);
                }
            }
            catch (Exception ex)
            {
                lblDesc.Text = ex.Message;
                //Response.Write(ex.Message);
            }
        }
    }
}