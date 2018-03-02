﻿using DAL.SYS;
using Model.SYS;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.Payment.Alipay.WapPay
{
    /// <summary>
    /// 功能：页面跳转同步通知页面
    /// 版本：3.3
    /// 日期：2012-07-10
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// 
    /// ///////////////////////页面功能说明///////////////////////
    /// 该页面可在本机电脑测试
    /// 可放入HTML等美化页面的代码、商户业务逻辑程序代码
    /// 该页面可以使用ASP.NET开发工具调试，也可以使用写文本函数LogResult进行调试
    /// </summary>
    public partial class call_back_url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<string, string> sPara = GetRequestGet();

            try
            {
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

                        //商户订单号
                        string out_trade_no = Request.QueryString["out_trade_no"];

                        //支付宝交易号
                        string trade_no = Request.QueryString["trade_no"];

                        //交易状态,只会为"success"
                        string result = Request.QueryString["result"];


                        //微商易业务逻辑
                        string url = string.Format("{0}/PalmShop/ShopCode/OrderHandle.aspx?oid={1}",
                            HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, ""),
                            out_trade_no);
                        Response.Redirect(url);
                        //打印页面
                        //Response.Write("验证成功<br />");

                        //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
            catch (Exception ex)
            {
                ExceptionLog log = new ExceptionLog();
                log.Message = "@发生异常" + ex.Message;
                ExceptionLogDAL.InsertExceptionLog(log);
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
}