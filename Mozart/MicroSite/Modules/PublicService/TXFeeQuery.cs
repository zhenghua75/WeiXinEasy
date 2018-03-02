using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace Mozart.PublicService.ServiceCode
{
    public class TXFeeQuery
    {
        #region 天讯缴费接口调用
        /// <summary>
        /// 使用post方法，调用天讯查询接口
        /// </summary>
        /// <param name="custInfo">固定值为：custInfo</param>
        /// <param name="xml">返回数据的格式</param>
        /// <param name="userId">代理商工号</param>
        /// <param name="custNo">用户号码</param>
        /// <param name="md5Str">Md5 后字符串</param>
        /// http请求地址：http://www.qnt100.com/center.do
        /// http请求串：callName=charge&returnType=xml&userId=xxx&orderId=xxx&money=xxx&orderTime=xxx&retUrl=&repeat=0&goodsCode=YNHF01&custNo=xxx&md5Str=xxx
        /// MD5源串　源字符串= userId + orderId + custNo + money + goodsCode + orderTime + repeat 
        ///

        public static string ChargeFeeInfo(string strUserID, string strKey, string strOrderId, string strPhoneNo, string strPayAmount, string strChargeTime, string strGoodsCode)
        {
            //string strUserID = "T08711142";
            //string strKey = "t11Aqi42";
            string strMD5 = strUserID + strOrderId + strPhoneNo + strPayAmount + strGoodsCode + strChargeTime + "0" + strKey;

            strMD5 = MD5(strMD5);

            string formUrl = "http://www.qnt100.com/center.do";

            //callName=charge&returnType=xml&userId=xxx&orderId=xxx&money=xxx&orderTime=xxx&retUrl=&repeat=0&goodsCode=YNHF01&custNo=xxx&md5Str=xxx
            string formDataFormat = "callName={0}&returnType={1}&userId={2}&orderId={3}&money={4}&orderTime={5}&retUrl=&repeat=0&goodsCode={6}&custNo={7}&md5Str={8}";

            string formData = string.Format(formDataFormat, "charge", "json", strUserID, strOrderId, strPayAmount, strChargeTime, strGoodsCode, strPhoneNo, strMD5);

            //MessageBox.Show(formData);

            CookieContainer cookieContainer = new CookieContainer();
            // 将提交的字符串数据转换成字节数组 
            byte[] postData = Encoding.UTF8.GetBytes(formData);

            // 设置提交的相关参数 
            HttpWebRequest request = WebRequest.Create(formUrl) as HttpWebRequest;
            //Encoding myEncoding = Encoding.GetEncoding("gb2312");
            Encoding myEncoding = Encoding.GetEncoding("utf-8");
            request.Method = "POST";
            request.KeepAlive = false;
            request.AllowAutoRedirect = true;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
            request.CookieContainer = cookieContainer;
            request.ContentLength = postData.Length;
            // 提交请求数据 
            System.IO.Stream outputStream = request.GetRequestStream();
            outputStream.Write(postData, 0, postData.Length);
            outputStream.Close();
            HttpWebResponse response;
            Stream responseStream;
            StreamReader reader;
            string srcString;
            response = request.GetResponse() as HttpWebResponse;
            responseStream = response.GetResponseStream();
            reader = new System.IO.StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            srcString = reader.ReadToEnd();
            reader.Close();

            return srcString;
        }

        #endregion

        #region 天讯查询接口调用
        /// <summary>
        /// 使用post方法，调用天讯查询接口
        /// </summary>
        /// <param name="custInfo">固定值为：custInfo</param>
        /// <param name="xml">返回数据的格式</param>
        /// <param name="userId">代理商工号</param>
        /// <param name="custNo">用户号码</param>
        /// <param name="md5Str">Md5 后字符串</param>
        /// http://www.qnt100.com/center.do?callName=custInfo&returnType=xml&userId=xxx&custNo=xxx&md5Str=xxx
        /// 
        public static string QueryNumberInfo(string strUserID, string strKey, string strcustNo)
        {
            string strMD5 = strUserID + strcustNo + strKey;

            strMD5 = MD5(strMD5);

            string formUrl = "http://www.qnt100.com/center.do";

            string formDataFormat = "callName={0}&returnType={1}&userId={2}&custNo={3}&md5Str={4}";

            string formData = string.Format(formDataFormat, "custInfo", "json", strUserID, strcustNo, strMD5);

            CookieContainer cookieContainer = new CookieContainer();
            // 将提交的字符串数据转换成字节数组 
            byte[] postData = Encoding.UTF8.GetBytes(formData);

            // 设置提交的相关参数 
            HttpWebRequest request = WebRequest.Create(formUrl) as HttpWebRequest;
            //Encoding myEncoding = Encoding.GetEncoding("gb2312");
            Encoding myEncoding = Encoding.GetEncoding("utf-8");
            request.Method = "POST";
            request.KeepAlive = false;
            request.AllowAutoRedirect = true;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
            request.CookieContainer = cookieContainer;
            request.ContentLength = postData.Length;
            // 提交请求数据 
            System.IO.Stream outputStream = request.GetRequestStream();
            outputStream.Write(postData, 0, postData.Length);
            outputStream.Close();
            HttpWebResponse response;
            Stream responseStream;
            StreamReader reader;
            string srcString;
            response = request.GetResponse() as HttpWebResponse;
            responseStream = response.GetResponseStream();
            reader = new System.IO.StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            srcString = reader.ReadToEnd();
            reader.Close();

            return srcString;
        }

        #endregion

        #region 请求串MD5加密功能
        public static string MD5(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }

        #endregion

        #region 解析返回结果

        public static string GetTXReturnStr(string strJson)
        {
            string strResult = string.Empty;
            //string JsonStr = "{"custNo":"18908712627","retCode":1,"afterBalance":90.303,"state":3,"money":"1","errorMsg":"您好:每次充值最高为3000元，最低为10元!","orderId":"test007"}";
            var KeyPair = new { custNo = "", retCode = "", afterBalance = "", state = "", money = "", errorMsg = "", orderId = "" };
            var b = JsonConvert.DeserializeAnonymousType(strJson, KeyPair);
            //MessageBox.Show(b.errorMsg);
            strResult = b.custNo + "|" + b.retCode + "|" + b.afterBalance + "|" + b.state + "|" + b.money + "|" + b.errorMsg + "|" + b.orderId;
            return strResult;
        }
        #endregion
    }
}