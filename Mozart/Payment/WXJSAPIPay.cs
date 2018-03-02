using DAL.SYS;
using Model.SYS;
using Mozart.Payment.wxpay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace Mozart.Payment
{
    public class WXJSAPIPay
    {
        private string mchid;
        private string appId;
        private string key;
        private string signType = "MD5";
        private string charset = "utf-8";

        /// <summary>
        /// payMode=wxpay
        /// </summary>
        /// <param name="siteCode"></param>
        public WXJSAPIPay(string siteCode)
        {
            bool flag = false;
            string payMode="wxpay";
            if (!string.IsNullOrEmpty(siteCode) && !string.IsNullOrEmpty(payMode))
            {
                PayConfig info=PayConfigDAL.CreateInstance().GetModel(siteCode, payMode);
                if (info != null)
                {
                    mchid = info.Partner;
                    appId = info.AppID;
                    signType = info.SignType;
                    key = info.SignKey;
                    charset=info.Charset;
                    flag = true;
                }
            }
            if(!flag)
            {
                throw new Exception("微信JSAPI支付对象创建失败！");
            }
        }

        /// <summary>
        /// 调用微信统一支付接口，获取JSAPI预支付标识
        /// </summary>
        /// <param name="body">商品描述</param>
        /// <param name="out_trade_no">商户系统内部的订单号 ,32个字符内、 可包含字母,确保在商户系统唯一</param>
        /// <param name="total_fee">订单总金额，单位为分，不能带小数点</param>
        /// <param name="spbill_create_ip">订单生成的机器 IP</param>
        /// <param name="openid">用户在商户 appid 下的唯一标识，trade_type 为 JSAPI时，此参数必传</param>
        /// <param name="notify_url">接收微信支付成功通知,用于支付成功时的订单处理</param>
        /// <param name="device_info">设备号，微信支付分配的终端设备号，可为空</param>
        /// <param name="attach">附加数据，原样返回，可为空</param>
        /// <returns>预支付ID，prepay_id，空表示未能成功调用</returns>
        public string GetJSAPIPrepayID(string body, string out_trade_no, int total_fee, string spbill_create_ip, string openid, string notify_url, string device_info = null, string attach = null)
        {
            string res = string.Empty;
            //微信分配的公众账号 ID
            string appid = appId;//不能为空
            //微信支付分配的商户号
            string mch_id = mchid;//不能为空
            //随机字符串，不长于 32 位
            string nonce_str = Guid.NewGuid().ToString("N");//不能为空
            //订单生成时间 ， 格 式 为yyyyMMddHHmms， 
            //如 2009 年12 月 25 日 9 点 10 分 10 秒表示为 20091225091010。时区为 GMT+8 beijing。 
            //该时间取自商户服务器
            string time_start = string.Empty;//DateTime.Now.ToString("yyyyMMddHHmmss");
            //订单失效时间
            string time_expire = string.Empty;
            //商品标记，该字段不能随便填，不使用请填空
            string goods_tag = string.Empty;

            string trade_type = "JSAPI";//JSAPI、NATIVE、APP
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
            string responseData = wxpayHelper.PostRequest(wxpayHelper.UNIFIED_ORDER_PAY, req_data);
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
        public string GetJSAPIParameters(string prepay_id)
        {
            string res = string.Empty;
            //时间戳，商户生成，从 1970 年 1 月 1日 00：00：00 至今的秒数，即当前的时间，且最终需要转换为字符串形式；
            string timeStamp = wxpayHelper.GetTimestamp();
            string nonceStr = Guid.NewGuid().ToString("N").Substring(0, 12);
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
            }
            temp.Append("}");
            res = temp.ToString();
            return res;
        }

        /// <summary>
        /// 直接支付
        /// </summary>
        /// <param name="body">商品描述</param>
        /// <param name="out_trade_no">商户系统内部的订单号 ,32个字符内、 可包含字母,确保在商户系统唯一</param>
        /// <param name="total_fee">订单总金额，单位为分，不能带小数点</param>
        /// <param name="spbill_create_ip">订单生成的机器 IP</param>
        /// <param name="openid">用户在商户 appid 下的唯一标识，trade_type 为 JSAPI时，此参数必传</param>
        /// <param name="notify_url">接收微信支付成功通知,用于支付成功时的订单处理,此参数必传</param>
        /// <param name="device_info">设备号，微信支付分配的终端设备号，可为空</param>
        /// <param name="attach">附加数据，原样返回，可为空</param>
        /// <returns>预支付ID，prepay_id，空表示未能成功调用</returns>
        public void DirectWXJSAPIPay(HttpResponse response,string body, string out_trade_no, int total_fee, string spbill_create_ip, string openid, string notify_url, string device_info = null, string attach = null)
        {
            string prepay_id = GetJSAPIPrepayID(body, out_trade_no, total_fee, spbill_create_ip, openid, notify_url, device_info, attach);
            response.Write("<script>document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {	WeixinJSBridge.invoke('getBrandWCPayRequest',"
                        + GetJSAPIParameters(prepay_id)
                        + ",function(res){ WeixinJSBridge.log(res.err_msg); if(res.err_msg == 'get_brand_wcpay_request:ok' )"
                        + " { WeixinJSBridge.call('closeWindow'); } });});</script>");//alert(res.err_code+res.err_desc+res.err_msg);
        }

        /// <summary>
        /// 直接支付
        /// </summary>
        /// <param name="body">商品描述</param>
        /// <param name="out_trade_no">商户系统内部的订单号 ,32个字符内、 可包含字母,确保在商户系统唯一</param>
        /// <param name="total_fee">订单总金额，单位为分，不能带小数点</param>
        /// <param name="spbill_create_ip">订单生成的机器 IP</param>
        /// <param name="openid">用户在商户 appid 下的唯一标识，trade_type 为 JSAPI时，此参数必传</param>
        /// <param name="notify_url">接收微信支付成功通知,用于支付成功时的订单处理,此参数必传</param>
        /// <param name="device_info">设备号，微信支付分配的终端设备号，可为空</param>
        /// <param name="attach">附加数据，原样返回，可为空</param>
        /// <param name="payOkScript">支付成功，执行的脚本，不含<script>可为空</param>
        /// <returns>预支付ID，prepay_id，空表示未能成功调用</returns>
        public void DirectWXJSAPIPay(HttpResponse response, string body, string out_trade_no, int total_fee, string spbill_create_ip, string openid, string notify_url, string device_info = null, string attach = null,string payOkScript="")
        {
            string prepay_id = GetJSAPIPrepayID(body, out_trade_no, total_fee, spbill_create_ip, openid, notify_url, device_info, attach);
            response.Write("<script>document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {	WeixinJSBridge.invoke('getBrandWCPayRequest',"
                        + GetJSAPIParameters(prepay_id)
                        + ",function(res){ WeixinJSBridge.log(res.err_msg); if(res.err_msg == 'get_brand_wcpay_request:ok' )"
                        + " { "+payOkScript+" } });});</script>");//alert(res.err_code+res.err_desc+res.err_msg);
        }

        /// <summary>
        /// 构建请求提交的数据
        /// </summary>
        /// <param name="sParaTemp">字典数据</param>
        /// <returns></returns>
        private string BuildRequestParaToString(Dictionary<string, string> sParaTemp)
        {
            string res = string.Empty;
            //过滤参数
            Dictionary<string, string> sPara = wxpayHelper.FilterPara(sParaTemp);
            //排序参数
            //sPara = SortPara(sPara);
            string mysign = GetSign(sPara);
            sPara.Add("sign", mysign);
            res = wxpayHelper.ParseParaToXML(sPara);
            return res;
        }

        private string GetSign(Dictionary<string, string> sParaTemp)
        {
            string res = string.Empty;
            Dictionary<string, string> sPara = wxpayHelper.SortPara(sParaTemp);
            //把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
            string prestr = wxpayHelper.CreateLinkString(sPara);
            string stringSignTemp = string.Format("{0}&key={1}", prestr, key);
            //string stringSignTemp = prestr + wxpayConfig.Key;
            //stringSignTemp = "appid=wxd930ea5d5a258f4f&auth_code=123456&body=test&device_info=123&mch_id=1900000109&nonce_str=960f228109051b9969f76c82bde183ac&out_trade_no=1400755861&spbill_create_ip=127.0.0.1&total_fee=1&key=8934e7d15453e97507ef794cf7b0519d";
            res = wxpayHelper.MD5Sign(stringSignTemp).ToUpper();
            return res;
        }

        /// <summary>
        /// 验证参数签名是否有效
        /// </summary>
        /// <param name="dictParam"></param>
        /// <returns></returns>
        public bool VerifyNotify(Dictionary<string, string> dictParam, string sign)
        {
            bool res = false;
            //过滤参数
            Dictionary<string, string> sPara = FilterPara(dictParam);
            string mysign = GetSign(sPara);
            res = sign == mysign;
            return res;
        }

        /// <summary>
        /// 除去数组中的空值和签名参数
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        public Dictionary<string, string> FilterPara(Dictionary<string, string> dicArrayPre)
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
    }
}