using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.Payment.AliPay
{
    public class AlipayConfig
    {
        static AlipayConfig()
        {
            Partner = "2088511509671281";
            Key = "vsx72yr641s3odh88s4op7nhz94bx9g6";
            Input_charset = "utf-8";
            Sign_type = "MD5";
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
    }
}