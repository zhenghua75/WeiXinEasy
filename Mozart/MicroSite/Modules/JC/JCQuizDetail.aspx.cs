using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.JC;
using Model.JC;
using Mozart.Common;
using DAL.WeiXin;
using WeiXinCore;
using Newtonsoft.Json;
using WeiXinCore.Models;

namespace Mozart.MicroSite
{
    public partial class JCQuizDetail : System.Web.UI.Page
    {
        string strTheme = string.Empty;
        string strQuizID = string.Empty;
        string strSiteCode = string.Empty;
        string strOpenID = string.Empty;
        string strGuid = string.Empty;
        string strAction = string.Empty;
        JC_QuizDAL dalQuiz = new JC_QuizDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (null == Request["state"] || Request["state"] == "")
                {
                    return;
                }
                else
                {
                    strSiteCode = Common.Common.NoHtml(Request.QueryString["state"].ToString());
                    Session["strSiteCode"] = strSiteCode;
                }

                string code = Request.QueryString["code"] as string;
                if (!string.IsNullOrEmpty(code))
                {
                    WXConfigDAL dal = new WXConfigDAL();
                    Model.WeiXin.WXConfig wxConfig = dal.GetWXConfigBySiteCode(strSiteCode);
                    if (wxConfig != null)
                    {
                        WeiXinCore.Models.WeiXinConfig weixinConfig = new WeiXinCore.Models.WeiXinConfig()
                        {
                            ID = wxConfig.WXID,
                            Name = wxConfig.WXName,
                            Token = wxConfig.WXToken,
                            AppId = wxConfig.WXAppID,
                            AppSecret = wxConfig.WXAppSecret
                        };
                        WeiXinCore.WeiXin weixin = new WeiXinCore.WeiXin(weixinConfig);
                        Oauth2AccessToken oauth2AccessToken = weixin.GetOauth2AccessToken(code);
                        if (oauth2AccessToken != null)
                        {
                            strOpenID = oauth2AccessToken.OpenID;
                        }
                    }
                }

                //if (string.IsNullOrEmpty(strOpenID))
                //{
                //    return;
                //}

                if (Request["action"] != null && Request["action"] != "")
                {
                    if (Request["openid"] != null && Request["openid"] != "")
                    {
                        strOpenID = Request.QueryString["openid"].ToString();
                        Session["openid"] = strOpenID;
                    }
                }
                else
                {
                    if (null != Session["openid"])
                    {
                        strOpenID = Session["openid"].ToString();
                    }
                }

                if (Request["openid"] != null && Request["openid"] != "")
                {
                    strOpenID = Request.QueryString["openid"].ToString();
                    Session["openid"] = strOpenID;
                }


                //取有效竞猜ID
                DataSet dsQuiz = dalQuiz.GetJCQuizDataList(" DATEDIFF(MI,GETDATE(),StartTime) > 15 ");
                if (Request["id"] != null && Request["id"] != "")
                {
                    strQuizID = Common.Common.NoHtml(Request["id"].ToString());
                }
                else  if (null != dsQuiz && dsQuiz.Tables.Count > 0 && dsQuiz.Tables[0].Rows.Count > 0)
                {
                    strQuizID = dalQuiz.GetJCQuizDataList(" DATEDIFF(MI,GETDATE(),StartTime) > 15 ").Tables[0].Rows[0]["ID"].ToString();
                }
                else
                {
                    #region 消费完成发生消息
                    string strAppID = string.Empty;
                    string strSecret = string.Empty;
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

                    WeiXinHelper.SendCustomTextMessage(strRToken, strOpenID, "当前没有竞猜场次！<a href='http://114.215.108.27/MicroSite/MyQuiz.aspx?sitecode=" + strSiteCode + "&openid=" + strOpenID + "'>查看所有参加的竞猜</a>");
                    #endregion
                    Response.Write("<script>document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {	WeixinJSBridge.call('closeWindow');});</script>");
                    return;
                }

                if (Request["action"] != null && Request["action"] != "")
                {
                    strAction = Common.Common.NoHtml(Request.QueryString["action"].ToString());
                }
                if (strAction.Trim() != null && strAction.Trim() != "" && strAction.Trim().ToLower() == "save")
                {
                    SaveJCGScore();
                }
                else
                {
                    GetDetailList();
                }
            }
        }

        void GetDetailList()
        {
            DataSet ds = dalQuiz.GetJCQuizDetail(strQuizID);

            JC_Quiz model = new JC_Quiz();
            string comptim = string.Empty;
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model = DataConvert.DataRowToModel<JC_Quiz>(ds.Tables[0].Rows[0]);
            }
            if (model.StartTime < DateTime.Now || model.StartTime == DateTime.Now)
            {
                comptim = model.StartTime.ToString();
            }
            else
            {
                comptim = "";
            }

            List<JC_Quiz> jcmodelList = new List<JC_Quiz>();
            DateTime smalldtm = dalQuiz.GetStartDateTime(); 
            string dtm = string.Empty;string nextdaytime = string.Empty;
            if (smalldtm.ToString() != null && smalldtm.ToString() != "")
            {
                DateTime bigdtm = dalQuiz.GetBigStartTime(smalldtm);
                if (DateTime.Now > bigdtm)
                {
                    dtm = bigdtm.AddDays(1).ToString("yyyy-MM-dd");
                    nextdaytime = bigdtm.AddDays(2).ToString("yyyy-MM-dd");
                }
                else
                {
                     dtm = smalldtm.ToString("yyyy-MM-dd");
                    nextdaytime = smalldtm.AddDays(1).ToString("yyyy-MM-dd");
                }
            }

            DataSet jclistds = dalQuiz.GetJCQuizDataList(" StartTime>='" + dtm + "' AND StartTime<'" + nextdaytime + "' ");
            foreach (DataRow row in jclistds.Tables[0].Rows)
            {
                JC_Quiz jcmodel = DataConvert.DataRowToModel<JC_Quiz>(row);
                jcmodelList.Add(jcmodel);
            }

            //读取模板内容 
            string text = string.Empty;
            if (model.QuizType == "0")
            {
                text = System.IO.File.ReadAllText(Server.MapPath("Themes/Quiz/QuizWinFailed.html"));
            }
            else
            {
                text = System.IO.File.ReadAllText(Server.MapPath("Themes/Quiz/QuizDetail.html"));
            }
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            QRCode qr = new QRCode();
            context.TempData["title"] = "竞猜详细信息";
            context.TempData["qDetail"] = model;
            context.TempData["comptim"] = comptim;
            context.TempData["jcmodel"] = jcmodelList;
            context.TempData["openid"] = strOpenID;
            context.TempData["QuizID"] = strQuizID;
            context.TempData["sitecode"] = strSiteCode;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        void SaveJCGScore()
        {
            string hscorevalue = string.Empty;
            string vscorevalue = string.Empty;
            if (strQuizID != null && strQuizID != "")
            {
                JC_ScoreDAL dal = new JC_ScoreDAL();
                if (!string.IsNullOrEmpty(strOpenID))
                {

                    if (dal.ExistJCScore(strQuizID, strOpenID))
                    {
                        Response.Write("{\"message\":\"不能重复提交！\"}");
                    }
                    else
                    {
                        JC_Score model = new JC_Score();
                        model.ID = Guid.NewGuid().ToString("N").ToUpper();
                        model.OpenID = strOpenID;
                        model.QuizId = strQuizID;
                        string hscore = Common.Common.NoHtml(Request["hscore"]);
                        string vscore = Common.Common.NoHtml(Request["vscore"]);
                        if (hscore != null && hscore != "")
                        {
                            hscorevalue = hscore;
                        }
                        else
                        {
                            hscorevalue = "0";
                        }
                        if (vscore != null && vscore != "")
                        {
                            vscorevalue = vscore;
                        }
                        else
                        {
                            vscorevalue = "0";
                        }
                        model.GuessScore = hscorevalue + ":" + vscorevalue;
                        model.State = 0;
                        model.SiteCode = Session["strSiteCode"].ToString();
                        if (dal.AddJCScore(model))
                        {
                            Response.Write("{\"message\":\"操作成功！\"}");
                        }
                        else
                        {
                            Response.Write("{\"message\":\"操作失败！\"}");
                        }
                    }
                }
                else
                {
                    Response.Write("{\"message\":\"不能重复提交！\"}");
                }
            }
            else
            {
                Response.Write("{\"message\":\"操作失败！\"}");
            }
            Response.End();
        }
    }
}