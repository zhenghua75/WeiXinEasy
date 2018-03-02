using DAL.WeiXin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WeiXinCore;

namespace Mozart.Common
{
    public class JQDialog
    {
        private JQDialog() { }
        /// <summary>
        /// JQ弹框
        /// </summary>
        /// <param name="time">时间 以秒为单位-可不填</param>
        /// <param name="msg">要弹的内容</param>
        /// <param name="url">要跳转的链接-可不填</param>
        /// <param name="strico">ico/error:succeed</param>
        /// <returns></returns>
        public static string alertOKMsgBox(int time, string msg, string url,string strico)
        {
            string msglist = string.Empty;
            msglist = "$.dialog({";
            if(time>0)
            {
                msglist  += "time:"+time+",";
            }
            if(msg!=null&&msg!="")
            {
                msglist  += "content:\""+msg+"\",";
            }
            if (url != null && url != "")
            {
                url = "location.href =\"" + url + "\"";
            }
            if (strico != null && strico != "")
            {
                strico = "icon:\"" + strico + "\",";
            }
            msglist += "lock: true, fixed: true," + strico + "ok: function () {this.close();" + url + "}," +
                        "close: function () {" + url + "}";
            msglist += "});";
            return msglist;
        }
        /// <summary>
        /// JQ弹框点确认返回
        /// </summary>
        /// <param name="time">时间-以秒为单位</param>
        /// <param name="msg">消息内容</param>
        /// <param name="goback">是否返回</param>
        /// <returns></returns>
        public static string alertOKMsgBoxGoBack(int time, string msg, bool goback)
        {
            string msglist = string.Empty;
            msglist = "$.dialog({";
            if (time > 0)
            {
                msglist += "time:" + time + ",";
            }
            if (msg != null && msg != "")
            {
                msglist += "content:\"" + msg + "\",";
            }
            if (goback == true)
            {
                msglist += "lock: true, fixed: true, icon: 'error',ok: function () {this.close();history.back();}," +
                                       "close: function () {history.back();}";
            }
            else
            {
                msglist += "lock: true, fixed: true, icon: 'error',ok: function () {this.close();}";
            }
            msglist += "});";
            return msglist;
        }
        /// <summary>
        /// 弹出对话框并清除body内容
        /// </summary>
        /// <param name="time">时间(秒)</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static string alertOkMsgBoxClearBody(int time, string msg)
        {
            string msglist = string.Empty;
            msglist = "$(document.body).html(\"\");\r\n";
            msglist  += "$.dialog({";
            if (time > 0)
            {
                msglist += "time:" + time + ",";
            }
            if (msg != null && msg != "")
            {
                msglist += "content:\"" + msg + "\",";
            }
            msglist += "lock: true, fixed: true, icon: 'error',ok: function () {this.close();}";
            msglist += "});";
            return msglist;
        }
        /// <summary>
        /// 弹出组合消息对话框
        /// </summary>
        /// <param name="msg">消息内容</param>
        /// <param name="url">要跳转的链接地址</param>
        /// <param name="okVal">确定按钮值(如：确定) 默认为 确定</param>
        /// <param name="cancelVal">取消按钮值(如：撤销) 默认为你 取消</param>
        /// <returns></returns>
        public static string alertConfirmBox(string msg, string url,string okVal,string cancelVal)
        {
            string msglist = string.Empty;
            msglist = "$.dialog({";
            if (msg != null && msg != "")
            {
                msglist += "content:\"" + msg + "\",";
            }
            if (url != null && url != "")
            {
                url = "location.href=\"" + url + "\"";
            }
            if (okVal == null || okVal == "")
            {
                okVal = " 确 定 ";
            }
            if (cancelVal == null || cancelVal == "")
            {
                cancelVal = " 取 消 ";
            }
            msglist += "lock: true, fixed: true, ok: function () {" + url + "},okVal:\"" + okVal + 
                "\",cancelVal:\"" + cancelVal + "\",cancel: true";
            msglist += "});";
            return msglist;
        }

        /// <summary>
        /// 设置cookies
        /// </summary>
        /// <param name="CookiesName">cookie名称(一般用pageurl)</param>
        /// <param name="RequestUrl">要跳转的地址名称</param>
        /// <param name="time">存储时长(分)</param>
        public static void SetCookies(string CookiesName,string RequestUrl,int time)
        {
            HttpCookie delcookies = new HttpCookie(CookiesName);
            HttpResponse Response = HttpContext.Current.Response;
            delcookies.Expires = DateTime.Now.AddDays(-1);
             Response.AppendCookie(delcookies);
             HttpCookie cookies = new HttpCookie(CookiesName, RequestUrl);
            cookies.Expires = DateTime.Now.AddMinutes(time);
            Response.AppendCookie(cookies);
        }
        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="filePath">要压缩的图片的路径</param>
        /// <param name="filePath_ystp">压缩后的图片的路径</param>
        /// <param name="quality">压缩后的图片质量（范围0-100）</param>
        public static void ystp(string filePath, string filePath_ystp, int quality)
        {
            Bitmap bmp = null; ImageCodecInfo ici = null; EncoderParameters eptS = null;
            try
            {
                bmp = new Bitmap(filePath);
                #region --------获取图片类型---------
                ImageCodecInfo[] iciS = ImageCodecInfo.GetImageEncoders();
                foreach (ImageCodecInfo imgici in iciS)
                {
                    if (imgici.MimeType.Equals("image/jpeg"))
                        ici = imgici;
                }
                #endregion
                eptS = new EncoderParameters(1);
                eptS.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);
                bmp.Save(filePath_ystp, ici, eptS);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                bmp.Dispose();
                eptS.Dispose();
            }
        }
        /// <summary>
        /// 字符串过滤HTML标记
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string GetTextFromHTML(string str)
        {
            str = Regex.Replace(str, @"<[/]?table[^>]*>", "");
            str = Regex.Replace(str, @"<[/]?tbody[^>]*>", "");
            str = Regex.Replace(str, @"<[/]?tr[^>]*>", "");
            str = Regex.Replace(str, @"<[/]?td[^>]*>", "");
            str = Regex.Replace(str, @"<[/]?span[^>]*>", "");
            str = Regex.Replace(str, @"<[/]?font[^>]*>", "");
            str = Regex.Replace(str, @"<[/]?div[^>]*>", "");
            str = Regex.Replace(str, @"<[/]?p[^>]*>", "");
            str = Regex.Replace(str, @"[\n|\b|\t]", "");
            str = Regex.Replace(str, @"<script[^>]*?>.*?</script>", "");
            str = Regex.Replace(str, @"<>", "");
            str = Regex.Replace(str, @"-->", "");
            str = Regex.Replace(str, @"<!--.*", "");
            str = Regex.Replace(str, @"&(quot|#34);", "");
            str = Regex.Replace(str, @"&(amp|#38);", "");
            str = Regex.Replace(str, @"&(lt|#60);", "");
            str = Regex.Replace(str, @"&(gt|#62);", "");
            str = Regex.Replace(str, @"&(nbsp|#160);", "");
            str = Regex.Replace(str, @"&(iexcl|#161);", "");
            str = Regex.Replace(str, @"&(cent|#162);", "");
            str = Regex.Replace(str, @"&#(\d+);", "");
            return str;
        }
        /// <summary>
        /// 正则清除html标签
        /// </summary>
        /// <param name="str">需要清除html的字符串</param>
        /// <returns></returns>
        public static string ClearTextFromHtml(string str)
        {
            Regex regex = new Regex(@"<[^>]+>|</[^>]+>");
            str = regex.Replace(str, "");
            return str;
        }
        #region-------------------脏字过滤-----------------------------
        /// <summary>
        /// 脏字过滤
        /// </summary>
        /// <param name="text">输入的文本</param>
        /// <param name="repStr">关键字</param>
        /// <returns></returns>
        public static string FilterKeyWord(string text, string repStr)
        {
            string input = text;
            for (int i = 0; i < repStr.Split('｜').Length; i++)
            {
                MatchCollection matches = null;
                string temp = protectHtml(input, ref matches);
                temp = Filter(temp, repStr.Split('｜')[i]);
                input = restoreHtml(temp, matches);
            }
            return input;
        }
        private static string protectHtml(string input, ref MatchCollection matches)
        {
            //匹配html的正则
            Regex htmlReg =
                new Regex(@"\<.*?\>", RegexOptions.Multiline);
            //获取匹配集合
            matches = htmlReg.Matches(input);
            //设置替换字串
            string markFormat = "[[{0}]]";
            //替换html,记录位置
            for (int i = 0; i < matches.Count; i++)
            {
                input = input.Replace(matches[i].Value, string.Format(markFormat, i));
            }
            return input;
        }
        private static string restoreHtml(string input, MatchCollection matches)
        {
            //设置替换字串
            string markFormat = "[[{0}]]";
            for (int i = 0; i < matches.Count; i++)
            {
                input = input.Replace(string.Format(markFormat, i), matches[i].Value);
            }
            return input;
        }
        private static string Filter(string input, string replace)
        {
            //设置星号
            string replaceformat = "*";
            Regex reg = new Regex(String.Format("{0}", replace), RegexOptions.Multiline);
            return reg.Replace(input, string.Format(replaceformat, replace));
        }
        #endregion
        #region 发送微信消息
        /// <summary>
        /// 发送微信消息
        /// </summary>
        /// <param name="strSiteCode">SiteCode</param>
        /// <param name="strOpenID">OpenID</param>
        /// <param name="strMsg">消息内容</param>
        public static void SendWeiXinMsg(string strSiteCode, string strOpenID, string strMsg)
        {
            string strAppID = string.Empty;
            string strSecret = string.Empty;
            if (strSiteCode != null && strSiteCode != "")
            {
                Model.WeiXin.WXConfig wc = new Model.WeiXin.WXConfig();
                WXConfigDAL wcdal = new WXConfigDAL();
                wc = wcdal.GetWXConfigBySiteCode(strSiteCode);
                if (null != wc)
                {
                    strAppID = wc.WXAppID;
                    strSecret = wc.WXAppSecret;
                }
                string strToken = WeiXinHelper.GetAccessToken(strAppID, strSecret);
                var KeyToken = new { access_token = "" };
                var b = JsonConvert.DeserializeAnonymousType(strToken, KeyToken);
                string strRToken = b.access_token;
                WeiXinHelper.SendCustomTextMessage(strRToken, strOpenID, strMsg);
            }
        }
        #endregion
    }
}