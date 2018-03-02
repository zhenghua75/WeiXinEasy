using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace Mozart.PublicService.ServiceCode
{
    public partial class QueryFee : System.Web.UI.Page
    {
        string strInfo = string.Empty;
        string strAction = string.Empty;
        string strPrice = string.Empty;
        string strNumber = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (null != Request.QueryString["action"])
            {
                strAction = Common.Common.NoHtml(Request.QueryString["action"].ToString());
                strNumber = Common.Common.NoHtml(Request.Form["txtNumber"].ToString());

                if (strAction == "query")
                {
                    strPrice = "当前余额为：" + QueryFeeByNumber(strNumber);
                }
            }
            strInfo = strPrice;
            string text = System.IO.File.ReadAllText(Server.MapPath("../ServicePage/QueryFee.html")); 
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            context.TempData["title"] = "话费查询";
            context.TempData["errinfo"] = strInfo;
            context.TempData["footer"] = "奥琦微商易";
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }

        #region 查询费用功能
        public string QueryFeeByNumber(string strTelNumber)
        {
            string strFee = string.Empty;

            //string strMobieNO = "134、135、136、137、138、139、147、150、151、152、157、158、159、182、183、184、187、188";

            //if (strMobieNO.IndexOf(strTelNumber.Substring(0, 3)) > -1)
            //{
            string strQueryInfo = TXFeeQuery.QueryNumberInfo("T08711142", "t11Aqi42", strTelNumber);

            var KeyQuery = new { custName = "", product = "", custNo = "", retCode = "", money = "", errorMsg = "" };
            if (JsonConvert.DeserializeAnonymousType(strQueryInfo, KeyQuery).retCode == "1")
            {
                strFee = JsonConvert.DeserializeAnonymousType(strQueryInfo, KeyQuery).money.Replace("\r\n", "") + "元";
            }
            else
            {
                strFee = JsonConvert.DeserializeAnonymousType(strQueryInfo, KeyQuery).errorMsg;
            }
            //}
            //else
            //{
            //    strFee = "当前只能查询昆明移动话费！";
            //}
            return strFee;
        }
        #endregion
    }
}