using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace Mozart.PublicService.ServiceCode
{
    public partial class QueryFeeNoPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strNumber = Common.Common.NoHtml(Request.QueryString["QN"].ToString());
            string strPrice = "  余额：" + QueryFeeByNumber(strNumber);
            Response.Write(strPrice);
        }
        #region 查询费用功能
        public string QueryFeeByNumber(string strTelNumber)
        {
            string strFee = string.Empty;
            string strLocal = string.Empty;
            string strQueryInfo = TXFeeQuery.QueryNumberInfo("T08711142", "t11Aqi42", strTelNumber);

            var KeyQuery = new { custName = "", product = "", custNo = "", retCode = "", money = "", errorMsg = "" };
            if (JsonConvert.DeserializeAnonymousType(strQueryInfo, KeyQuery).retCode == "1")
            {
                strFee = JsonConvert.DeserializeAnonymousType(strQueryInfo, KeyQuery).money.Replace("\r\n", "") + "元";               
            }
            else
            {
                //strFee = JsonConvert.DeserializeAnonymousType(strQueryInfo, KeyQuery).errorMsg;
                strFee = "未查询到";
            }
            //取号码所属地
            if (strTelNumber.Substring(0, 1) == "1")
            {
                DAL.PublicService.aq_CeleWhere dal = new DAL.PublicService.aq_CeleWhere();
                strLocal = dal.GetLocalState(strTelNumber);
            }
            else
            {
                switch (strTelNumber.Substring(0, 4))
                {
                    case "0691":
                        strLocal = "云南版纳固话";
                        break;
                    case "0692":
                        strLocal = "云南德宏固话";
                        break;
                    case "0870":
                        strLocal = "云南昭通固话";
                        break;
                    case "0871":
                        strLocal = "云南昆明固话";
                        break;
                    case "0872":
                        strLocal = "云南大理固话";
                        break;
                    case "0873":
                        strLocal = "云南红河固话";
                        break;
                    case "0874":
                        strLocal = "云南曲靖固话";
                        break;
                    case "0875":
                        strLocal = "云南保山固话";
                        break;
                    case "0876":
                        strLocal = "云南文山固话";
                        break;
                    case "0877":
                        strLocal = "云南玉溪固话";
                        break;
                    case "0878":
                        strLocal = "云南楚雄固话";
                        break;
                    case "0879":
                        strLocal = "云南思茅固话";
                        break;
                    case "0883":
                        strLocal = "云南临沧固话";
                        break;
                    case "0886":
                        strLocal = "云南怒江固话";
                        break;
                    case "0887":
                        strLocal = "云南迪庆固话";
                        break;
                    case "0888":
                        strLocal = "云南丽江固话";
                        break;
                    default:
                        strLocal = "未知";
                        break;
                }
            }
            return strFee + " 归属：" + strLocal;
        }
        #endregion

    }
}