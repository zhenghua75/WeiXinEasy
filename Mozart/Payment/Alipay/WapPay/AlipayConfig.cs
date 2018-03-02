using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.Payment.Alipay.WapPay
{
    public class AlipayConfig
    {
        static AlipayConfig()
        {
            Partner = Mozart.Payment.AliPay.AlipayConfig.Partner;
            Key = Mozart.Payment.AliPay.AlipayConfig.Key;
            Input_charset = "utf-8";
            Sign_type = "MD5";
            Private_key = "";
            Public_key = "";
        }

        /// <summary>
        /// 获取或设置合作者身份ID
        /// </summary>
        public static string Partner { get; set; }

        /// <summary>
        /// 获取或设交易安全校验码
        /// </summary>
        public static string Key { get; set; }

        /// <summary>
        /// 获取字符编码格式
        /// </summary>
        public static string Input_charset { get; set; }

        /// <summary>
        /// 获取签名方式
        /// </summary>
        public static string Sign_type { get; set; }

         /// <summary>
        /// 获取或设置商户的私钥
        /// </summary>
        public static string Private_key { get; set; }

        /// <summary>
        /// 获取或设置支付宝的公钥
        /// </summary>
        public static string Public_key { get; set; }
    }
}