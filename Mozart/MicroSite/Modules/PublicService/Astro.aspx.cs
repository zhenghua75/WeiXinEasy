using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mozart.PublicService.ServiceCode
{
    public partial class Astro : System.Web.UI.Page
    {
        string strInfo = string.Empty;
        string strAction = string.Empty;
        string strbirthday = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (null != Request.QueryString["action"])
            {
                strAction = Common.Common.NoHtml(Request.QueryString["action"].ToString());
                strbirthday = Common.Common.NoHtml(Request.Form["txtNumber"].ToString());

                if (strAction == "query")
                {
                    strInfo = "今天运程：" + GetAstroInfo(strbirthday);
                }
            }
            string text = System.IO.File.ReadAllText(Server.MapPath("../ServicePage/Astro.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            context.TempData["title"] = "星座运程";
            context.TempData["errinfo"] = strInfo;
            context.TempData["footer"] = "奥琦微商易";
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }

        #region 查询当日运程
        public string GetAstroInfo(string strbirthday)
        {
            string strDay = strbirthday.Substring(0, 4) + "-" + strbirthday.Substring(4, 2) + "-" + strbirthday.Substring(6, 2);
            YangLiToYinLi.ChineseCalendar ss = new YangLiToYinLi.ChineseCalendar(DateTime.Parse(strDay));

            string strReturn = string.Empty;
            string strQueryInfo = Regex.Unescape(QueryAstroInfo(ss.ConstellationIndex));

            var KeyQuery = new { title = "", rank = "", value = "" };

            JArray ja = (JArray)JsonConvert.DeserializeObject(strQueryInfo);

            for (int i = 1; i < 11; i++)
            {
                JObject o = (JObject)ja[i];
                strReturn = strReturn 
                    + JsonConvert.DeserializeAnonymousType(o.ToString(), KeyQuery).title
                    + JsonConvert.DeserializeAnonymousType(o.ToString(), KeyQuery).rank
                    + JsonConvert.DeserializeAnonymousType(o.ToString(), KeyQuery).value 
                    + "<br>";
            }

           // strReturn = JsonConvert.DeserializeAnonymousType(o.ToString(), KeyQuery).title;


            return strReturn;
        }

        #region 星座查询接口调用
        /// <summary>
        /// 星座查询接口调用
        /// 
        public static string QueryAstroInfo(string ID)
        {
            string formUrl = "http://api.uihoo.com/astro/astro.http.php";

            string formDataFormat = "fun={0}&id={1}&format={2}";

            string formData = string.Format(formDataFormat, "day","0", "json");

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

        
        #endregion

    }
}