using DAL.SYS;
using Model.SYS;
using Mozart.Payment.wxpay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml.Linq;

namespace Mozart.Payment
{
    public abstract class WXJSAPIPayNotifyBasePage:Page
    {
        //public Action<WXJSAPIPayNotifyInfo> OnPaySucceed;

        public abstract void OnPaySucceed(WXJSAPIPayNotifyInfo info);
        public abstract string GetSiteCode();

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!IsPostBack)
            {
                string siteCode = GetSiteCode();
                string return_code = string.Empty;
                string return_msg = string.Empty;
                WXJSAPIPay pay = new WXJSAPIPay(siteCode);
                Dictionary<string, string> dict = GetRequestPost();

                //ExceptionLog log = new ExceptionLog();
                //log.Message = dict.ToArray().ToString();
                //ExceptionLogDAL.InsertExceptionLog(log);

                if (pay.VerifyNotify(dict, dict["sign"]))
                {
                    return_code = "SUCCESS";
                    if (dict["return_code"] == "SUCCESS")
                    {
                        if (dict["result_code"] == "SUCCESS")
                        {
                            WXJSAPIPayNotifyInfo info = new WXJSAPIPayNotifyInfo();
                            if (dict.ContainsKey("openid"))
                                info.OpenId = dict["openid"];
                            if (dict.ContainsKey("total_fee"))
                                info.TotalFee = int.Parse(dict["total_fee"]);
                            //string trade_type = dict["trade_type"];
                            if (dict.ContainsKey("transaction_id"))
                                info.TransactionId = dict["transaction_id"];
                            if (dict.ContainsKey("out_trade_no"))
                                info.OutTradeNo = dict["out_trade_no"];
                            if (dict.ContainsKey("attach"))
                            {
                                info.Attach = dict["attach"];
                            }
                            
                            if (!string.IsNullOrEmpty(info.OpenId) &&
                                info.TotalFee > 0 &&
                                !string.IsNullOrEmpty(info.TransactionId) &&
                                !string.IsNullOrEmpty(info.OutTradeNo))
                            {

                                OnPaySucceed(info);
                            }
                            else
                            {
                                return_code = "FAIL";
                                return_msg = "参数格式校验错误";
                            }
                        }
                    }
                }
                else
                {
                    return_code = "FAIL";
                    return_msg = "签名失败";
                }
                string returnValue = string.Format("<xml><return_code>{0}</return_code><return_msg>{1}</return_msg></xml>",
                    return_code, return_msg);
            }
        }

        public Dictionary<string, string> GetRequestPost()
        {
            Dictionary<string, string> res = null;
            byte[] byts = new byte[Request.InputStream.Length];
            Request.InputStream.Read(byts, 0, byts.Length);
            string req = System.Text.Encoding.UTF8.GetString(byts);
            //req = Server.UrlDecode(req);
            if (!string.IsNullOrEmpty(req))
            {
                res = new Dictionary<string, string>();
                XElement xml = XElement.Parse(req);

                //ExceptionLog log = new ExceptionLog();
                //log.Message = xml.ToString();
                //ExceptionLogDAL.InsertExceptionLog(log);

                foreach (XElement xe in xml.Elements())
                {
                    res.Add(xe.Name.ToString(), xe.Value);
                }
            }
            return res;
        }
    }

    public class WXJSAPIPayNotifyInfo
    {
        /// <summary>
        /// 用户在商户 appid 下的唯一标识
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 订单总金额，单位为分
        /// </summary>
        public int TotalFee { get; set; }

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// 商户系统的订单号，与请求一致
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 商家数据包，原样返回
        /// </summary>
        public string Attach { get; set; }

        /// <summary>
        /// 支 付 完 成 时 间 ， 格 式 为yyyyMMddhhmmss
        /// </summary>
        public string TimeEnd { get; set; }
    }
}