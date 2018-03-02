using DAL.SYS;
using Model.SYS;
using Mozart.Payment.Alipay.WapPay;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml;

namespace Mozart.Payment
{
    public abstract class AliWapPayNotifyBasePage : Page
    {
        public abstract void OnPaySucceed(AliWapPayNotifyInfo info);

        public abstract string GetSiteCode();

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!IsPostBack)
            {
                Dictionary<string, string> sPara = GetRequestPost();

                if (sPara.Count > 0)//判断是否有带返回参数
                {
                    bool verifyResult = false;

                    AlipayNotify aliNotify = new AlipayNotify();
                    verifyResult = aliNotify.VerifyNotify(sPara, Request.Form["sign"]);

                    if (verifyResult)//验证成功
                    {

                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //请在这里加上商户的业务逻辑程序代码


                        //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                        //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                        //解密（如果是RSA签名需要解密，如果是MD5签名则下面一行清注释掉）
                        //sPara = aliNotify.Decrypt(sPara);

                        //XML解析notify_data数据
                        try
                        {

                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(sPara["notify_data"]);

                            //交易状态
                            string trade_status = xmlDoc.SelectSingleNode("/notify/trade_status").InnerText;
                            AliWapPayNotifyInfo info = new AliWapPayNotifyInfo();
                            info.trade_status = trade_status;
                            info.subject = xmlDoc.SelectSingleNode("/notify/subject").InnerText;
                            info.trade_no = xmlDoc.SelectSingleNode("/notify/trade_no").InnerText;
                            info.buyer_email = xmlDoc.SelectSingleNode("/notify/buyer_email").InnerText;
                            info.gmt_create = xmlDoc.SelectSingleNode("/notify/gmt_create").InnerText;
                            info.quantity = xmlDoc.SelectSingleNode("/notify/quantity").InnerText;
                            info.out_trade_no = xmlDoc.SelectSingleNode("/notify/out_trade_no").InnerText;
                            info.notify_time = xmlDoc.SelectSingleNode("/notify/notify_time").InnerText;
                            info.seller_id = xmlDoc.SelectSingleNode("/notify/seller_id").InnerText;
                            info.total_fee = xmlDoc.SelectSingleNode("/notify/total_fee").InnerText;
                            info.seller_email = xmlDoc.SelectSingleNode("/notify/seller_email").InnerText;
                            info.price = xmlDoc.SelectSingleNode("/notify/price").InnerText;
                            //info.gmt_payment = xmlDoc.SelectSingleNode("/notify/gmt_payment").InnerText;
                            info.buyer_id = xmlDoc.SelectSingleNode("/notify/buyer_id").InnerText;
                            info.notify_id = xmlDoc.SelectSingleNode("/notify/notify_id").InnerText;
                            info.use_coupon = xmlDoc.SelectSingleNode("/notify/use_coupon").InnerText;
                            if (xmlDoc.SelectSingleNode("/notify/gmt_payment") != null)
                            {
                                info.gmt_payment = xmlDoc.SelectSingleNode("/notify/gmt_payment").InnerText;
                            }

                            if (trade_status == "TRADE_FINISHED" )
                            {
                                //判断该笔订单是否在商户网站中已经做过处理
                                //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                                //如果有做过处理，不执行商户的业务程序

                                //注意：
                                //该种交易状态只在两种情况下出现
                                //1、开通了普通即时到账，买家付款成功后。
                                //2、开通了高级即时到账，从该笔交易成功时间算起，过了签约时的可退款时限（如：三个月以内可退款、一年以内可退款等）后。

                                //微商易业务逻辑
                                OnPaySucceed(info);

                                Response.Write("success");  //请不要修改或删除
                            }
                            else if (trade_status == "TRADE_SUCCESS")
                            {
                                //判断该笔订单是否在商户网站中已经做过处理
                                //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                                //如果有做过处理，不执行商户的业务程序

                                //注意：
                                //该种交易状态只在一种情况下出现——开通了高级即时到账，买家付款成功后。

                                //微商易业务逻辑
                                OnPaySucceed(info);

                                Response.Write("success");  //请不要修改或删除
                            }
                            else
                            {
                                Response.Write(trade_status);
                            }

                        }
                        catch (Exception exc)
                        {
                            Response.Write(exc.ToString());
                        }



                        //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    }
                    else//验证失败
                    {
                        Response.Write("fail");
                    }
                }
                else
                {
                    Response.Write("无通知参数");
                }
            }
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public Dictionary<string, string> GetRequestPost()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }
    }

    public class AliWapPayNotifyInfo
    {
        /// <summary>
        /// 交易的状态
        /// WAIT_BUYER_PAY,交易创建，等待买家付款。
        /// TRADE_CLOSED,在指定时间段内未支付时关闭的交易；在交易完成全额退款成功时关闭的交易。
        /// TRADE_SUCCESS,交易成功，且可对该交易做操作，如：多级分润、退款等。
        /// TRADE_PENDING ,等待卖家收款（买家付款后，如果卖家账号被冻结）。
        /// TRADE_FINISHED,交易成功且结束，即不可再做任何操作
        /// </summary>
        public string trade_status { get; set; }

        /// <summary>
        /// 商品的标题/交易标题/订单标题/订单关键字等。
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        /// 该交易在支付宝系统中的交易流水号。
        /// </summary>
        public string trade_no { get; set; }

        /// <summary>
        /// 买家支付宝账号，可以是email或手机号码。
        /// </summary>
        public string buyer_email { get; set; }

        /// <summary>
        /// 该笔交易创建的时间。格式为yyyy-MM-dd HH:mm:ss。
        /// </summary>
        public string gmt_create { get; set; }

        /// <summary>
        /// 购买商品的数量。
        /// </summary>
        public string quantity { get; set; }
        /// <summary>
        /// 对应商户网站的订单系统中的唯一订单号，非支付宝交易号。
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 通知的发送时间。格式为yyyy-MM-dd HH:mm:ss。
        /// </summary>
        public string notify_time { get; set; }

        /// <summary>
        /// 卖家支付宝账号对应的支付宝唯一用户号。
        /// </summary>
        public string seller_id { get; set; }

        /// <summary>
        /// 该笔订单的总金额
        /// </summary>
        public string total_fee { get; set; }

        /// <summary>
        /// 卖家支付宝账号,卖家支付宝账号，可以是email和手机号码。
        /// </summary>
        public string seller_email { get; set; }

        /// <summary>
        /// 目前和total_fee值相同。单位：元。不应低于0.01元。
        /// </summary>
        public string price { get; set; }

        /// <summary>
        /// 该笔交易的买家付款时间。格式为yyyy-MM-dd HH:mm:ss。
        /// </summary>
        public string gmt_payment  { get; set; }

        /// <summary>
        /// 买家支付宝账号对应的支付宝唯一用户号。
        /// </summary>
        public string buyer_id { get; set; }

        /// <summary>
        /// 通知校验ID。唯一识别通知内容。重发相同内容的通知时，该值不变。
        /// </summary>
        public string notify_id { get; set; }

        /// <summary>
        /// 是否在交易过程中使用了红包
        /// </summary>
        public string use_coupon { get; set; }
    }
}