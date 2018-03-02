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

namespace Mozart.MicroSite
{
    public partial class JCQuizList : System.Web.UI.Page
    {
        string strSiteID = string.Empty;
        string strTitle = string.Empty;
        string strTheme = string.Empty;
        string strSiteCode = string.Empty;
        string strOpenID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetAccount();
                if (Request["action"] != null && Request["action"] != "")
                {
                    if (Request["action"].ToString().ToLower() != "save")
                    {
                        GetJCQuizList();
                    }
                    else
                    {
                        SaveJCGScore();
                    }
                }
                else
                {
                    GetJCQuizList();
                }
            }
        }
        void GetAccount()
        {
            if (Session["strOpenID"] != null && Session["strOpenID"].ToString() != "")
            {
                strOpenID = Session["strOpenID"].ToString();
            }
            else
            {
                return;
            }
            if (null == Request.QueryString["ID"])
            {
                strSiteID = "8C45500B9A6D4FBD8A4DE49AC19FE74E";
            }
            else
            {
                strSiteID = Common.Common.NoHtml(Request.QueryString["ID"].ToString());
            }
            Session["siteid"] = strSiteID;
            DAL.SYS.AccountDAL dalAccount = new DAL.SYS.AccountDAL(); ;
            DataSet ds = dalAccount.GetAccountExtData(strSiteID);
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                strTheme = ds.Tables[0].Rows[0]["Themes"].ToString();
                strTitle = ds.Tables[0].Rows[0]["Name"].ToString();
                strSiteCode = ds.Tables[0].Rows[0]["SiteCode"].ToString();
                Session["strSiteCode"] = strSiteCode;
            }
        }
        void GetJCQuizList()
        {
            string strSiteID = string.Empty;
            string strTitle = string.Empty;
            string strTheme = string.Empty;
            string strSiteCode = string.Empty;

            List<JC_Quiz> jcmodel = new List<JC_Quiz>();
            JC_QuizDAL dal = new JC_QuizDAL();
            DataSet jclistds = dal.GetJCQuizDataList("");
            foreach (DataRow row in jclistds.Tables[0].Rows)
            {
                JC_Quiz model = DataConvert.DataRowToModel<JC_Quiz>(row);
                jcmodel.Add(model);
            }

            //读取模板内容
            // string text = System.IO.File.ReadAllText(Server.MapPath("Themes/" + strTheme + "/NewHome.html"));
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Quiz/QuizList.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["title"] = "2014巴西世界杯比赛日程表一览";
            context.TempData["siteid"] = Session["siteid"];
            context.TempData["jcmodel"] = jcmodel;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        void SaveJCGScore()
        {
            string hscorevalue = string.Empty;
            string vscorevalue = string.Empty;
            if (Request["qid"] != null && Request["qid"] != "")
            {
                JC_ScoreDAL dal = new JC_ScoreDAL();
                if (dal.ExistJCScore(Request["qid"], strOpenID))
                {
                    Response.Write("{\"message\":\"您已经操作过了，请不要重复操作！\"}");
                }
                else
                {
                    JC_Score model = new JC_Score();
                    model.ID = Guid.NewGuid().ToString("N").ToUpper();
                    model.OpenID = strOpenID;
                    model.QuizId = Request["qid"].ToString();
                    if (Request["hscore"] != null && Request["hscore"] != "")
                    {
                        hscorevalue = Request["hscore"];
                    }
                    else
                    {
                        hscorevalue = "0";
                    }
                    if (Request["vscore"] != null && Request["vscore"] != "")
                    {
                        vscorevalue = Request["vscore"];
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
                        Response.Write("{\"message\":\"操作失败，请核对后再操作！\"}");
                    }
                }
            }
            else
            {
                Response.Write("{\"message\":\"操作失败，请核对后再操作！\"}");
            }
            Response.End();
        }
    }
}