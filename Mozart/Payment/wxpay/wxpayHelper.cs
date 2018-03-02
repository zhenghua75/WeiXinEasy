using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace Mozart.Payment.wxpay
{
    public class wxpayHelper
    {
        //统一支付接口，可接受 JSAPI/NATIVE/APP 下预支付订单，返回预支付订单号。
        public static string UNIFIED_ORDER_PAY = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        //服务器异步通知页面路径
        static string _notify_url = "/Payment/wxpay/notify_url.aspx";
        //http://114.215.108.27/Payment/wxpay/wxpayDemo.aspx
        /// <summary>
        /// 调用微信统一支付接口，获取JSAPI预支付标识
        /// </summary>
        /// <param name="body">商品描述</param>
        /// <param name="out_trade_no">商户系统内部的订单号 ,32个字符内、 可包含字母,确保在商户系统唯一</param>
        /// <param name="total_fee">订单总金额，单位为分，不能带小数点</param>
        /// <param name="spbill_create_ip">订单生成的机器 IP</param>
        /// <param name="openid">用户在商户 appid 下的唯一标识，trade_type 为 JSAPI时，此参数必传</param>
        /// <param name="device_info">设备号，微信支付分配的终端设备号，可为空</param>
        /// <param name="attach">附加数据，原样返回，可为空</param>
        /// <returns>预支付ID，prepay_id，空表示未能成功调用</returns>
        public static string GetJSAPIPrepayID(string body, string out_trade_no, int total_fee, string spbill_create_ip, string openid, string device_info = null, string attach = null)
        {
            string res = string.Empty;
            //微信分配的公众账号 ID
            string appid = wxpayConfig.appId;//不能为空
            //微信支付分配的商户号
            string mch_id = wxpayConfig.Mchid;//不能为空
            //随机字符串，不长于 32 位
            string nonce_str = Guid.NewGuid().ToString("N");//不能为空
            //订单生成时间 ， 格 式 为yyyyMMddHHmms， 
            //如 2009 年12 月 25 日 9 点 10 分 10 秒表示为 20091225091010。时区为 GMT+8 beijing。 
            //该时间取自商户服务器
            string time_start = string.Empty;//DateTime.Now.ToString("yyyyMMddHHmmss");
            //订单失效时间
            string time_expire=string.Empty;
            //商品标记，该字段不能随便填，不使用请填空
            string goods_tag=string.Empty;
            //接收微信支付成功通知
            string notify_url = GetNotifyUrl();//不能为空
            string trade_type="JSAPI";//JSAPI、NATIVE、APP
            string product_id = string.Empty;//只在 trade_type 为 NATIVE时需要填写。 此 id 为二维码中包含的商品 ID，商户自行维护。
            Dictionary<string, string> sParaTemp = new Dictionary<string, string>();
            sParaTemp.Add("appid", appid);
            sParaTemp.Add("mch_id", mch_id);
            sParaTemp.Add("nonce_str", nonce_str);
            sParaTemp.Add("body", body);//HttpUtility.UrlEncode(body, Encoding.UTF8));
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("total_fee", total_fee.ToString());
            sParaTemp.Add("spbill_create_ip", spbill_create_ip);
            sParaTemp.Add("openid", openid);
            sParaTemp.Add("device_info", device_info);
            sParaTemp.Add("attach", attach);
            sParaTemp.Add("time_start", time_start);
            sParaTemp.Add("time_expire", time_expire);
            sParaTemp.Add("goods_tag", goods_tag);
            sParaTemp.Add("notify_url", notify_url);
            sParaTemp.Add("trade_type", trade_type);
            sParaTemp.Add("product_id", product_id);

            string req_data = BuildRequestParaToString(sParaTemp);//不能为空
            string responseData = PostRequest(UNIFIED_ORDER_PAY, req_data);
            if (!string.IsNullOrEmpty(responseData))
            {
                XElement xml = XElement.Parse(responseData);
                if (xml.Element("return_code").Value == "SUCCESS")
                {
                    if (xml.Element("result_code").Value == "SUCCESS")
                    {
                        res = xml.Element("prepay_id").Value;
                    }
                    else
                    {
                        throw new Exception(xml.Element("err_code_des").Value);
                    }
                }
                else
                {
                    throw new Exception(xml.Element("return_msg").Value);
                }
            }
            return res;
        }

        /// <summary>
        /// 设置获取getBrandWCPayRequest方法参数，基于JSON字符串返回
        /// </summary>
        /// <param name="prepay_id">微信预支付标识</param>
        /// <returns>JSON字符串</returns>
        public static string GetJSAPIParameters(string prepay_id)
        {
            string res = string.Empty;
            //公众号ID
            string appId = wxpayConfig.appId;
            //时间戳，商户生成，从 1970 年 1 月 1日 00：00：00 至今的秒数，即当前的时间，且最终需要转换为字符串形式；
            string timeStamp = GetTimestamp();
            string nonceStr = Guid.NewGuid().ToString("N").Substring(0,12);
            string signType = "MD5";
            string package = string.Format("prepay_id={0}", prepay_id);

            Dictionary<string, string> sPara = new Dictionary<string, string>();
            sPara.Add("appId", appId);
            sPara.Add("timeStamp", timeStamp);
            sPara.Add("nonceStr", nonceStr);
            sPara.Add("package", package);
            sPara.Add("signType", signType);
            string paySign = GetSign(sPara);
            sPara.Add("paySign", paySign);

            StringBuilder temp = new StringBuilder();
            temp.Append("{");
            bool flag = false;
            foreach (KeyValuePair<string, string> kv in sPara)
            {
                if (flag)
                {
                    temp.AppendFormat(",\"{0}\":\"{1}\"", kv.Key, kv.Value);
                }
                else
                {
                    temp.AppendFormat("\"{0}\":\"{1}\"", kv.Key, kv.Value);
                    flag = true;
                }
                //temp.AppendFormat("\"{0}\":\"{1}\"", kv.Key,kv.Value);
            }
            //temp.AppendFormat("\"appId\":\"{0}\"",appId);
            //temp.AppendFormat("\"timeStamp\":\"{0}\"", timeStamp);
            //temp.AppendFormat("\"nonceStr\":\"{0}\"", nonceStr);
            //temp.AppendFormat("\"package\":\"{0}\"", package);
            //temp.AppendFormat("\"signType\":\"{0}\"", signType);
            //temp.AppendFormat("\"paySign\":\"{0}\"", paySign);
            temp.Append("}");
            res = temp.ToString();
            return res;
        }

        #region 私有方法
        /// <summary>
        /// 获取服务器异步通知页面完整URL
        /// </summary>
        /// <returns></returns>
        public static string GetNotifyUrl()
        {
            return string.Format("{0}{1}", 
                HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, ""),
                _notify_url);
        }

        /// <summary>
        /// 构建请求提交的数据
        /// </summary>
        /// <param name="sParaTemp">字典数据</param>
        /// <returns></returns>
        public static string BuildRequestParaToString(Dictionary<string, string> sParaTemp)
        {
            string res = string.Empty;
            //过滤参数
            Dictionary<string, string> sPara = FilterPara(sParaTemp);
            //排序参数
            //sPara = SortPara(sPara);
            string mysign = GetSign(sPara);
            sPara.Add("sign", mysign);
            res = ParseParaToXML(sPara);
            return res;
        }

        public static string GetSign(Dictionary<string, string> sParaTemp)
        {
            string res = string.Empty;
            Dictionary<string,string> sPara=SortPara(sParaTemp);
            //把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
            string prestr = CreateLinkString(sPara);
            string stringSignTemp = string.Format("{0}&key={1}", prestr, wxpayConfig.Key);
            //string stringSignTemp = prestr + wxpayConfig.Key;
            //stringSignTemp = "appid=wxd930ea5d5a258f4f&auth_code=123456&body=test&device_info=123&mch_id=1900000109&nonce_str=960f228109051b9969f76c82bde183ac&out_trade_no=1400755861&spbill_create_ip=127.0.0.1&total_fee=1&key=8934e7d15453e97507ef794cf7b0519d";
            res = MD5Sign(stringSignTemp).ToUpper();
            return res;
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        /// <summary>
        /// 签名字符串
        /// </summary>
        /// <param name="prestr">需要签名的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="_input_charset">编码格式</param>
        /// <returns>签名结果</returns>
        public static string MD5Sign(string prestr)
        {
            string res = string.Empty;
            StringBuilder sb = new StringBuilder();
           
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.UTF8.GetBytes(prestr));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            //res=System.Text.Encoding.GetEncoding("GBK").GetString(t);
            res = sb.ToString();
            return res;
        }

        public static string MD5ForPHP(string stringToHash)
        {
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] emailBytes = Encoding.UTF8.GetBytes(stringToHash.ToLower());
            byte[] hashedEmailBytes = md5.ComputeHash(emailBytes);
            StringBuilder sb = new StringBuilder();
            foreach (var b in hashedEmailBytes)
            {
                sb.Append(b.ToString("x2").ToLower());
            }
            return sb.ToString();
        } 


        /// <summary>
        /// 除去数组中的空值和签名参数
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        public static Dictionary<string, string> FilterPara(Dictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (temp.Key.ToLower() != "sign" && temp.Value != "" && temp.Value != null)
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }
            return dicArray;
        }

        /// <summary>
        /// 根据字母a到z的顺序把参数排序
        /// </summary>
        /// <param name="dicArrayPre">排序前的参数组</param>
        /// <returns>排序后的参数组</returns>
        public static Dictionary<string, string> SortPara(Dictionary<string, string> dicArrayPre)
        {
            SortedDictionary<string, string> dicTemp = new SortedDictionary<string, string>(dicArrayPre);
            Dictionary<string, string> dicArray = new Dictionary<string, string>(dicTemp);

            return dicArray;
        }

        /// <summary>
        /// 解析参数TOXML
        /// </summary>
        /// <param name="dicArrayPre"></param>
        /// <returns></returns>
        public static string ParseParaToXML(Dictionary<string, string> dicArrayPre)
        {
            string res = string.Empty;
            StringBuilder prestr = new StringBuilder();
            prestr.Append("<xml>");
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                string[] ar = { };//"attach", "body", "sign", "openid" };// 
                if (ar.Contains(temp.Key))
                //if (!Regex.IsMatch(temp.Value, @"^[0-9.]$"))
                {
                    prestr.AppendFormat("<{0}>![CDATA[{1}]]</{0}>", temp.Key, temp.Value);
                }
                else
                {
                    prestr.AppendFormat("<{0}>{1}</{0}>", temp.Key, temp.Value);
                }
                //prestr.AppendFormat("<{0}>![CDATA[{1}]]</{0}>", temp.Key, temp.Value);
                //prestr.AppendFormat("<{0}>{1}</{0}>", temp.Key, temp.Value);
            }
            prestr.Append("</xml>");
            res = prestr.ToString();
            return res;
        }

        /// <summary>
        /// 建立请求，以模拟远程HTTP的POST请求方式构造并获取支付宝的处理结果
        /// </summary>
        /// <param name="GATEWAY_NEW">网关地址</param>
        /// <param name="sParaTemp">请求参数</param>
        /// <returns>支付宝处理结果</returns>
        public static string PostRequest(string GATEWAY_NEW, string  requestData)
        {
            Encoding code = Encoding.UTF8;

            //把数组转换成流中所需字节数组类型
            byte[] bytesRequestData = code.GetBytes(requestData);

            //构造请求地址
            string strUrl = GATEWAY_NEW;

            //请求远程HTTP
            string strResult = "";
            try
            {
                //设置HttpWebRequest基本信息
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                myReq.Method = "post";
                myReq.ContentType = "application/x-www-form-urlencoded";

                //填充POST数据
                myReq.ContentLength = bytesRequestData.Length;
                Stream requestStream = myReq.GetRequestStream();
                requestStream.Write(bytesRequestData, 0, bytesRequestData.Length);
                requestStream.Close();

                //发送POST数据请求服务器
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();

                //获取服务器返回信息
                StreamReader reader = new StreamReader(myStream, code);
                StringBuilder responseData = new StringBuilder();
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    responseData.Append(line);
                }

                //释放
                myStream.Close();

                strResult = responseData.ToString();
            }
            catch (Exception exp)
            {
                strResult = "报错：" + exp.Message;
            }

            return strResult;
        }

        /// <summary>
        /// 验证参数签名是否有效
        /// </summary>
        /// <param name="dictParam"></param>
        /// <returns></returns>
        public static bool VerifyNotify(Dictionary<string,string> dictParam,string sign)
        {
            bool res = false;
            //过滤参数
            Dictionary<string, string> sPara = FilterPara(dictParam);
            string mysign = GetSign(sPara);
            res = sign == mysign;
            return res;
        }

        public static string GetTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        #endregion
    }
}