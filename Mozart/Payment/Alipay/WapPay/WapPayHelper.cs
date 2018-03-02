using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mozart.Payment.Alipay.WapPay
{
    public class WapPayHelper
    {

        //支付宝网关地址
        static string GATEWAY_NEW = "http://wappaygw.alipay.com/service/rest.htm?";

        //服务器异步通知页面路径
        static string _notify_url = "/Payment/Alipay/WapPay/notify_url.aspx";
        //需http://格式的完整路径，不允许加?id=123这类自定义参数

        //页面跳转同步通知页面路径
        static string _call_back_url = "/Payment/Alipay/WapPay/call_back_url.aspx";
        //需http://格式的完整路径，不允许加?id=123这类自定义参数

        //操作中断返回地址
        static string _merchant_url = string.Empty;//"/Payment/Alipay/WapPay/merchant_url.aspx";
        //用户付款中途退出返回商户的地址。需http://格式的完整路径，不允许加?id=123这类自定义参数

        //默认卖家的支付宝账号。交易成功后，买家资金会转移到该账户中。
        static string seller_account_name = "1228855790@qq.com";

        public static string GetSiteUrl()
        {
            return HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, "");
        }

        /// <summary>
        /// 获取默认参数字典信息
        /// </summary>
        /// <returns></returns>
        protected static Dictionary<string, string> CreateDefaultParamsDict(string interfaceName)
        {
            Dictionary<string, string> sParaTempToken = new Dictionary<string, string>();
            sParaTempToken.Add("service", interfaceName);
            sParaTempToken.Add("format", "xml");
            sParaTempToken.Add("v", "2.0");
            sParaTempToken.Add("partner", AlipayConfig.Partner);
            sParaTempToken.Add("sec_id", AlipayConfig.Sign_type.ToUpper());
            sParaTempToken.Add("_input_charset", AlipayConfig.Input_charset.ToLower());
            return sParaTempToken;
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
        protected static string BuildReqData(string subject, string out_trade_no, string total_fee,
            string seller_account_name, string call_back_url, string notify_url, string out_user,
            string merchant_url, string pay_expire)
        {
            if(!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(out_trade_no) && !string.IsNullOrEmpty(total_fee) &&
                !string.IsNullOrEmpty(seller_account_name) && !string.IsNullOrEmpty(call_back_url))
            {
                StringBuilder res = new StringBuilder();
                res.Append("<direct_trade_create_req>");
                res.AppendFormat("<subject>{0}</subject>",subject);
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

        /// <summary>
        /// 请求手机网页即时到账授权接口
        /// </summary>
        /// <param name="subject">用户购买的商品名称，不可空</param>
        /// <param name="out_trade_no">支付宝合作商户网站唯一订单号。不可空</param>
        /// <param name="total_fee">该笔订单的资金总额，单位为RMB-Yuan。取值范围为[0.01，100000000.00]，精确到小数点后两位。不可空</param>
        /// <param name="seller_account_name">卖家的支付宝账号。交易成功后，买家资金会转移到该账户中。不可空</param>
        /// <param name="call_back_url">支付成功后的跳转页面链接。支付成功才会跳转。不可空</param>
        /// <param name="notify_url">支付宝服务器主动通知商户网站里指定的页面http路径。可为空</param>
        /// <param name="out_user">买家在商户系统的唯一标识。当该买家支付成功一次后，再次支付金额在30元内时，不需要再次输入密码，可为空</param>
        /// <param name="merchant_url">用户付款中途退出返回商户的地址。可为空</param>
        /// <param name="pay_expire">交易自动关闭时间，单位为分钟。默认值21600（即15天）。可为空</param>
        /// <returns></returns>
        public static string RequestTradeCreateDirect(string subject, string out_trade_no, string total_fee,
            string seller_account_name, string call_back_url, string notify_url, string out_user,
            string merchant_url, string pay_expire)
        {
            Dictionary<string, string> sParaTempToken = CreateDefaultParamsDict("alipay.wap.trade.create.direct");
            sParaTempToken.Add("req_data", BuildReqData(subject,  out_trade_no,  total_fee,
                seller_account_name,  call_back_url,  notify_url,  out_user,
                merchant_url,  pay_expire));
            string sHtmlTextToken = AlipaySubmit.BuildRequest(GATEWAY_NEW, sParaTempToken);
            //URLDECODE返回的信息
            Encoding code = Encoding.GetEncoding(AlipayConfig.Input_charset);
            sHtmlTextToken = HttpUtility.UrlDecode(sHtmlTextToken, code);
            return sHtmlTextToken;
        }

        /// <summary>
        /// 请求手机网页即时到账授权接口
        /// </summary>
        /// <param name="subject">用户购买的商品名称，不可空</param>
        /// <param name="out_trade_no">支付宝合作商户网站唯一订单号。不可空</param>
        /// <param name="total_fee">该笔订单的资金总额，单位为RMB-Yuan。取值范围为[0.01，100000000.00]，精确到小数点后两位。不可空</param>
        /// <param name="out_user">买家在商户系统的唯一标识。当该买家支付成功一次后，再次支付金额在30元内时，不需要再次输入密码，可为空</param>
        /// <returns></returns>
        public static string RequestTradeCreateDirect(string subject, string out_trade_no, string total_fee,
            string out_user)
        {
            return RequestTradeCreateDirect(subject, out_trade_no, total_fee,
                seller_account_name, string.Format("{0}{1}", GetSiteUrl(), _call_back_url), 
                string.Format("{0}{1}", GetSiteUrl(), _notify_url), out_user,
                string.Format("{0}{1}", GetSiteUrl(), _merchant_url), string.Empty);
        }

        /// <summary>
        /// 请求手机网页即时到账支付
        /// </summary>
        /// <param name="response">订单处理网页Response对象</param>
        /// <param name="subject">用户购买的商品名称，不可空</param>
        /// <param name="out_trade_no">支付宝合作商户网站唯一订单号。不可空</param>
        /// <param name="total_fee">该笔订单的资金总额，单位为RMB-Yuan。取值范围为[0.01，100000000.00]，精确到小数点后两位。不可空</param>
        /// <param name="out_user">买家在商户系统的唯一标识。当该买家支付成功一次后，再次支付金额在30元内时，不需要再次输入密码，可为空</param>
        public static void RequestWapPayment(HttpResponse response, string subject, string out_trade_no, string total_fee,
            string out_user)
        {
            //建立请求
            string sHtmlText = BuildRequest(subject,out_trade_no,total_fee,out_user);
            response.Write(sHtmlText);
        }

        /// <summary>
        /// 生成手机网页即时到账支付表单
        /// </summary>
        /// <param name="subject">用户购买的商品名称，不可空</param>
        /// <param name="out_trade_no">支付宝合作商户网站唯一订单号。不可空</param>
        /// <param name="total_fee">该笔订单的资金总额，单位为RMB-Yuan。取值范围为[0.01，100000000.00]，精确到小数点后两位。不可空</param>
        /// <param name="out_user">买家在商户系统的唯一标识。当该买家支付成功一次后，再次支付金额在30元内时，不需要再次输入密码，可为空</param>
        public static string BuildRequest(string subject, string out_trade_no, string total_fee,
            string out_user)
        {
            ////////////////////////////////////////////调用授权接口alipay.wap.trade.create.direct获取授权码token////////////////////////////////////////////
            string sHtmlTextToken = RequestTradeCreateDirect(subject, out_trade_no, total_fee, out_user);
            //解析远程模拟提交后返回的信息
            Dictionary<string, string> dicHtmlTextToken = AlipaySubmit.ParseResponse(sHtmlTextToken);
            //获取token
            string request_token = dicHtmlTextToken["request_token"];

            ////////////////////////////////////////////根据授权码token调用交易接口alipay.wap.auth.authAndExecute////////////////////////////////////////////
            //业务详细
            string req_data = "<auth_and_execute_req><request_token>" + request_token + "</request_token></auth_and_execute_req>";
            //必填
            //把请求参数打包成数组
            Dictionary<string, string> sParaTemp = CreateDefaultParamsDict("alipay.wap.auth.authAndExecute");
            sParaTemp.Add("req_data", req_data);

            //建立请求
            string sHtmlText = AlipaySubmit.BuildRequest(GATEWAY_NEW, sParaTemp, "get", "确认");
            return sHtmlText;
        }
    }
}