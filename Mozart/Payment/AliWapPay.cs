using DAL.SYS;
using Model.SYS;
using Mozart.Payment.Alipay.WapPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mozart.Payment
{
    public class AliWapPay
    {
        private string partner = "2088511509671281";
        private string key = "vsx72yr641s3odh88s4op7nhz94bx9g6";
        private string input_charset = "utf-8";
        private string sign_type = "MD5";
        //默认卖家的支付宝账号。交易成功后，买家资金会转移到该账户中。
        private string seller_account_name = "1228855790@qq.com";
        private string gateway_new = "http://wappaygw.alipay.com/service/rest.htm?";

        /// <summary>
        /// payMode=alipay
        /// </summary>
        /// <param name="siteCode"></param>
        public AliWapPay(string siteCode)
        {
            //bool flag = false;
            //string payMode = "alipay";
            //if (!string.IsNullOrEmpty(siteCode) && !string.IsNullOrEmpty(payMode))
            //{
            //    PayConfig info = PayConfigDAL.CreateInstance().GetModel(siteCode, payMode);
            //    if (info != null)
            //    {
            //        partner = info.Partner;
            //        sign_type = info.SignType;
            //        key = info.SignKey;
            //        input_charset = info.Charset;
            //        seller_account_name = info.MoreParams;
            //        flag = true;
            //    }
            //}
            //if (!flag)
            //{
            //    throw new Exception("微信JSAPI支付对象创建失败！");
            //}
        }

        /// <summary>
        /// 调用授权接口获取授权令牌
        /// </summary>
        /// <param name="response">订单处理网页Response对象</param>
        /// <param name="subject">用户购买的商品名称，不可空</param>
        /// <param name="out_trade_no">支付宝合作商户网站唯一订单号。不可空</param>
        /// <param name="total_fee">该笔订单的资金总额，单位为RMB-Yuan。取值范围为[0.01，100000000.00]，精确到小数点后两位。不可空</param>
        /// <param name="call_back_url">支付成功跳转页面路径,不可空</param>
        /// <param name="out_user">买家在商户系统的唯一标识。当该买家支付成功一次后，再次支付金额在30元内时，不需要再次输入密码，可为空</param>
        /// <param name="notify_url">服务器异步通知页面路径。可为空</param>
        /// <param name="merchant_url">用户付款中途退出返回商户的地址。可为空</param>
        /// <param name="pay_expire">交易自动关闭时间。可为空</param>
        public string GetAliPayToken(string subject, string out_trade_no, string total_fee,
            string out_user, string call_back_url, string notify_url="", string merchant_url="",string pay_expire="")
        {
            string res = string.Empty;
            Dictionary<string, string> sParaTempToken = new Dictionary<string, string>();
            sParaTempToken.Add("service", "alipay.wap.trade.create.direct");
            sParaTempToken.Add("format", "xml");
            sParaTempToken.Add("v", "2.0");
            sParaTempToken.Add("partner", partner);
            sParaTempToken.Add("sec_id", sign_type.ToUpper());
            sParaTempToken.Add("_input_charset", input_charset.ToLower());

            sParaTempToken.Add("req_data", BuildReqData(subject, out_trade_no, total_fee,
                seller_account_name, call_back_url, notify_url, out_user,
                merchant_url, pay_expire));
            string sHtmlTextToken = AlipaySubmit.BuildRequest(gateway_new, sParaTempToken);
            //URLDECODE返回的信息
            Encoding code = Encoding.GetEncoding(input_charset);
            sHtmlTextToken = HttpUtility.UrlDecode(sHtmlTextToken, code);
            //解析远程模拟提交后返回的信息
            Dictionary<string, string> dicHtmlTextToken = AlipaySubmit.ParseResponse(sHtmlTextToken);
            //获取token
            res = dicHtmlTextToken["request_token"];
            return res;
        }

         /// <summary>
        /// 调用支付接入进行支付
        /// </summary>
        /// <param name="response">订单处理网页Response对象</param>
        /// <param name="request_token">授权令牌，不可空</param>
        public void RequestWapPayment(HttpResponse response, string request_token)
        {
            string req_data = "<auth_and_execute_req><request_token>" + request_token + "</request_token></auth_and_execute_req>";
            //必填
            Dictionary<string, string> sParaTempToken = new Dictionary<string, string>();
            sParaTempToken.Add("service", "alipay.wap.auth.authAndExecute");
            sParaTempToken.Add("format", "xml");
            sParaTempToken.Add("v", "2.0");
            sParaTempToken.Add("partner", partner);
            sParaTempToken.Add("sec_id", sign_type.ToUpper());
            sParaTempToken.Add("_input_charset", input_charset.ToLower());

            sParaTempToken.Add("req_data", req_data);

            //建立请求
            string sHtmlText = AlipaySubmit.BuildRequest(gateway_new, sParaTempToken, "get", "确认");
            response.Write(sHtmlText);
        }

        /// <summary>
        /// 直接请求手机网页即时到账支付
        /// </summary>
        /// <param name="response">订单处理网页Response对象</param>
        /// <param name="subject">用户购买的商品名称，不可空</param>
        /// <param name="out_trade_no">支付宝合作商户网站唯一订单号。不可空</param>
        /// <param name="total_fee">该笔订单的资金总额，单位为RMB-Yuan。取值范围为[0.01，100000000.00]，精确到小数点后两位。不可空</param>
        /// <param name="call_back_url">支付成功跳转页面路径,不可空</param>
        /// <param name="out_user">买家在商户系统的唯一标识。当该买家支付成功一次后，再次支付金额在30元内时，不需要再次输入密码，可为空</param>
        /// <param name="notify_url">服务器异步通知页面路径。可为空</param>
        /// <param name="merchant_url">用户付款中途退出返回商户的地址。可为空</param>
        /// <param name="pay_expire">交易自动关闭时间。可为空</param>
        public void DirectAliWayPay(HttpResponse response,string subject, string out_trade_no, string total_fee,
            string out_user, string call_back_url, string notify_url = "", string merchant_url = "", string pay_expire = "")
        {
            string token = GetAliPayToken(subject, out_trade_no, total_fee,out_user, call_back_url,
                notify_url, merchant_url, pay_expire);
            if (!string.IsNullOrEmpty(token))
            {
                RequestWapPayment(response,token);
            }
        }

        /// <summary>
        /// 根据传入参数构建请求业务参数
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="out_trade_no"></param>
        /// <param name="total_fee"></param>
        /// <param name="seller_account_name"></param>
        /// <param name="call_back_url"></param>
        /// <param name="notify_url"></param>
        /// <param name="out_user"></param>
        /// <param name="merchant_url"></param>
        /// <param name="pay_expire"></param>
        /// <returns></returns>
        protected string BuildReqData(string subject, string out_trade_no, string total_fee,
            string seller_account_name, string call_back_url, string notify_url, string out_user,
            string merchant_url, string pay_expire)
        {
            if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(out_trade_no) && !string.IsNullOrEmpty(total_fee) &&
                !string.IsNullOrEmpty(seller_account_name) && !string.IsNullOrEmpty(call_back_url))
            {
                StringBuilder res = new StringBuilder();
                res.Append("<direct_trade_create_req>");
                res.AppendFormat("<subject>{0}</subject>", subject);
                res.AppendFormat("<out_trade_no>{0}</out_trade_no>", out_trade_no);
                res.AppendFormat("<total_fee>{0}</total_fee>", total_fee);
                res.AppendFormat("<seller_account_name>{0}</seller_account_name>", seller_account_name);
                res.AppendFormat("<call_back_url>{0}</call_back_url>", call_back_url);
                if (!string.IsNullOrEmpty(notify_url))
                    res.AppendFormat("<notify_url>{0}</notify_url>", notify_url);
                if (!string.IsNullOrEmpty(out_user))
                    res.AppendFormat("<out_user>{0}</out_user>", out_user);
                if (!string.IsNullOrEmpty(merchant_url))
                    res.AppendFormat("<merchant_url>{0}</merchant_url>", merchant_url);
                if (!string.IsNullOrEmpty(pay_expire))
                    res.AppendFormat("<pay_expire>{0}</pay_expire>", pay_expire);
                res.Append("</direct_trade_create_req>");
                return res.ToString();
            }
            else
            {
                throw new Exception("对不起，请传递的正确的参数！");
            }
        }
    }
}