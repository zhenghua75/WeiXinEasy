using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.WeiXin;
using Mozart.Common;
using Newtonsoft.Json;
using WeiXinCore;

namespace Mozart.WebService
{
    public partial class CustMemNo : System.Web.UI.Page
    {
        string strInfo = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string strOpenID = string.Empty;
            string strSiteCode = string.Empty;
            string strMemNo = string.Empty;
            string strAction = string.Empty;
            string strPrice = string.Empty;
            string strCode = string.Empty;
            if (null == Request.QueryString["SiteCode"])
            {
                return;
            }
            if (null == Request.QueryString["MemNo"])
            {
                return;
            }
            if (null == Request.QueryString["OpenId"])
            {
                return;
            }
            strOpenID = Common.Common.NoHtml(Request.QueryString["OpenId"].ToString());
            strSiteCode = Common.Common.NoHtml(Request.QueryString["SiteCode"].ToString());
            strMemNo = Common.Common.NoHtml(Request.QueryString["MemNo"].ToString());
            if (null != Request.QueryString["action"])
            {
                strAction = Request.QueryString["action"];
                if (string.IsNullOrEmpty(strSiteCode))
                {
                    return;
                }
                strPrice = Request.Form["txtFee"].ToString();
                strCode = Request.Form["txtCode"].ToString();
                
                if (strAction == "checkout" && strCode == "888666")
                {
                    DAL.Site.ConRecordsDAL dal = new DAL.Site.ConRecordsDAL();
                    Model.Site.ConRecords modelAdd = new Model.Site.ConRecords
                    {
                        ID = Guid.NewGuid().ToString("N").ToUpper(),
                        SiteCode = strSiteCode,
                        MemberShipNo = strMemNo,
                        OpenID = strOpenID,
                        CreateTime = DateTime.Now,
                        Price = float.Parse(strPrice)
                    };
                    if (dal.InsertInfo(modelAdd))
                    {
                        strInfo = "消费记录生成完成。";

                        #region 消费完成发生消息
                        //string strAppID = string.Empty;
                        //string strSecret = string.Empty;
                        //Model.WeiXin.WXConfig wc = new Model.WeiXin.WXConfig();
                        //WXConfigDAL wcdal = new WXConfigDAL();
                        //wc = wcdal.GetWXConfigBySiteCode(strSiteCode);
                        //if (null != wc)
                        //{
                        //    strAppID = wc.WXAppID;
                        //    strSecret = wc.WXAppSecret;
                        //}
                        //string strToken = WeiXinHelper.GetAccessToken(strAppID, strSecret);

                        //var KeyToken = new { access_token = "" };
                        //var b = JsonConvert.DeserializeAnonymousType(strToken, KeyToken);
                        //string strRToken = b.access_token;

                        //WeiXinHelper.SendCustomTextMessage(strRToken, strOpenID, "欢迎到海立方消费！");
                        #endregion
                        strOpenID = string.Empty;
                        strSiteCode = string.Empty;
                        strMemNo = string.Empty;
                        strAction = string.Empty;
                        strPrice = string.Empty;
                    }
                }
            }
            string text = System.IO.File.ReadAllText(Server.MapPath("CustMemNo.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            context.TempData["sitecode"] = strSiteCode;
            context.TempData["openid"] = strOpenID;
            context.TempData["memno"] = strMemNo;
            context.TempData["title"] = "会员消费处理";
            context.TempData["errinfo"] = strInfo;
            context.TempData["footer"] = "奥琦微商易";
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}