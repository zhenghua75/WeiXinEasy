﻿using Mozart.Payment.AliPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.Payment.Alipay
{
    public partial class TradeCreateByBuyerDemo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnAlipay_Click(object sender, EventArgs e)
        {
            ////////////////////////////////////////////请求参数////////////////////////////////////////////

            //支付类型
            string payment_type = "1";
            //必填，不能修改
            //服务器异步通知页面路径
            string notify_url = "http://www.xxx.com/trade_create_by_buyer-CSHARP-UTF-8/notify_url.aspx";
            //需http://格式的完整路径，不能加?id=123这类自定义参数

            //页面跳转同步通知页面路径
            string return_url = "http://www.xxx.com/trade_create_by_buyer-CSHARP-UTF-8/return_url.aspx";
            //需http://格式的完整路径，不能加?id=123这类自定义参数，不能写成http://localhost/

            //卖家支付宝帐户
            string seller_email = WIDseller_email.Text.Trim();
            //必填

            //商户订单号
            string out_trade_no = WIDout_trade_no.Text.Trim();
            //商户网站订单系统中唯一订单号，必填

            //订单名称
            string subject = WIDsubject.Text.Trim();
            //必填

            //付款金额
            string price = WIDprice.Text.Trim();
            //必填

            //商品数量
            string quantity = "1";
            //必填，建议默认为1，不改变值，把一次交易看成是一次下订单而非购买一件商品
            //物流费用
            string logistics_fee = "0.00";
            //必填，即运费
            //物流类型
            string logistics_type = "EXPRESS";
            //必填，三个值可选：EXPRESS（快递）、POST（平邮）、EMS（EMS）
            //物流支付方式
            string logistics_payment = "SELLER_PAY";
            //必填，两个值可选：SELLER_PAY（卖家承担运费）、BUYER_PAY（买家承担运费）
            //订单描述

            string body = WIDbody.Text.Trim();
            //商品展示地址
            string show_url = WIDshow_url.Text.Trim();
            //需以http://开头的完整路径，如：http://www.xxx.com/myorder.html

            //收货人姓名
            string receive_name = WIDreceive_name.Text.Trim();
            //如：张三

            //收货人地址
            string receive_address = WIDreceive_address.Text.Trim();
            //如：XX省XXX市XXX区XXX路XXX小区XXX栋XXX单元XXX号

            //收货人邮编
            string receive_zip = WIDreceive_zip.Text.Trim();
            //如：123456

            //收货人电话号码
            string receive_phone = WIDreceive_phone.Text.Trim();
            //如：0571-88158090

            //收货人手机号码
            string receive_mobile = WIDreceive_mobile.Text.Trim();
            //如：13312341234


            ////////////////////////////////////////////////////////////////////////////////////////////////

            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", AlipayConfig.Partner);
            sParaTemp.Add("_input_charset", AlipayConfig.Input_charset.ToLower());
            sParaTemp.Add("service", "trade_create_by_buyer");
            sParaTemp.Add("payment_type", payment_type);
            sParaTemp.Add("notify_url", notify_url);
            sParaTemp.Add("return_url", return_url);
            sParaTemp.Add("seller_email", seller_email);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("price", price);
            sParaTemp.Add("quantity", quantity);
            sParaTemp.Add("logistics_fee", logistics_fee);
            sParaTemp.Add("logistics_type", logistics_type);
            sParaTemp.Add("logistics_payment", logistics_payment);
            sParaTemp.Add("body", body);
            sParaTemp.Add("show_url", show_url);
            sParaTemp.Add("receive_name", receive_name);
            sParaTemp.Add("receive_address", receive_address);
            sParaTemp.Add("receive_zip", receive_zip);
            sParaTemp.Add("receive_phone", receive_phone);
            sParaTemp.Add("receive_mobile", receive_mobile);

            //建立请求
            string sHtmlText = AlipaySubmit.BuildRequest(sParaTemp, "get", "确认");
            Response.Write(sHtmlText);
        }
    }
}