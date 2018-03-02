using Mozart.Payment.Alipay.WapPay;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Mozart.Payment
{
    public abstract class AliWapPayCallBackBasePage : Page
    {
        public abstract void OnPaySucceed(AliWapPayCallBackInfo info);

        public abstract string GetSiteCode();

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!IsPostBack)
            {
                Dictionary<string, string> sPara = GetRequestGet();

                if (sPara.Count > 0)//判断是否有带返回参数
                {

                    AlipayNotify aliNotify = new AlipayNotify();
                    bool verifyResult = aliNotify.VerifyReturn(sPara, Request.QueryString["sign"]);

                    if (verifyResult)//验证成功
                    {
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //请在这里加上商户的业务逻辑程序代码


                        //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                        //获取支付宝的通知返回参数，可参考技术文档中页面跳转同步通知参数列表

                        AliWapPayCallBackInfo info = new AliWapPayCallBackInfo();
                        info.sign = Request.QueryString["sign"];
                        info.result = Request.QueryString["result"];
                        info.out_trade_no = Request.QueryString["out_trade_no"];
                        info.trade_no = Request.QueryString["trade_no"];
                        info.request_token = Request.QueryString["request_token"];
                        OnPaySucceed(info);
                    }
                    else//验证失败
                    {
                        Response.Write("验证失败");
                    }
                }
                else
                {
                    Response.Write("无返回参数");
                }
                
            }
        }

        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public Dictionary<string, string> GetRequestGet()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }
    }

    public class AliWapPayCallBackInfo
    {
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 判断支付结果及交易状态。只有支付成功时（即result=success），
        /// 才会跳转到支付成功页面，result有且只有success一个交易状态。
        /// </summary>
        public string result { get; set; }

        /// <summary>
        /// 支付宝合作商户网站唯一订单号。
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 该交易在支付宝系统中的交易流水号。
        /// 最短16位，最长64位。
        /// </summary>
        public string trade_no { get; set; }

        /// <summary>
        /// 授权令牌，调用“手机网页即时到账授权接口(alipay.wap.trade.create.direct)”成功后返回的值。
        /// </summary>
        public string request_token { get; set; }
    }
}