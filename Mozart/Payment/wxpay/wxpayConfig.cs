using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.Payment.wxpay
{
    public class wxpayConfig
    {
        static string flagName="V易GO";
        static wxpayConfig()
        {
            switch(flagName)
            {
                case "V易GO":
                    appId = "wxc567715f14bab093";
                    Mchid = "10019421";
                    Key = "845BD828EB7A424F994FDA6E67FA692F";
                    break;
                default :
                    appId = "wx7fe93b025863dfeb";
                    Mchid = "10017995";
                    Key = "45563057E758489C9FCBE6E0AC31DF9E";
                    break;
            }
        }

        /// <summary>
        /// 微信公众号身份的唯一标识。审核通过后，在微信发送的邮件中查看。
        /// </summary>
        public static string appId { get; set; }

        /// <summary>
        /// 商户 ID，身份标识
        /// </summary>
        public static string Mchid { get; set; }

        /// <summary>
        /// 商户支付密钥 Key。审核通过后，在微信发送的邮件中查看。
        /// </summary>
        public static string Key { get; set; }

        /// <summary>
        /// JSAPI 接口中获取 openid，审核后在公众平台开启开发模式后可查看。
        /// </summary>
        public static string Appsecret { get; set; }
    }
}