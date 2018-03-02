using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.JC;
using Model.JC;
using Mozart.Common;


namespace Mozart.MicroSite
{
    public partial class MyQuiz : System.Web.UI.Page
    {
        string strInfo = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (null == Request.QueryString["sitecode"] && !string.IsNullOrEmpty(Request.QueryString["sitecode"].ToString()))
            {
                return;
            }
            if (null == Request.QueryString["openid"] && !string.IsNullOrEmpty(Request.QueryString["openid"].ToString()))
            {
                return;
            }

            string strSiteCode = Common.Common.NoHtml(Request.QueryString["sitecode"].ToString());
            string strOpenID = string.Empty;
            if (Request.QueryString["openid"].ToString().Length > 29 || Request.QueryString["openid"].ToString().Length < 25)
            {
                strOpenID = Common.Common.NoHtml(Request.QueryString["openid"].ToString());
            }
            else
            {
                strOpenID = Request.QueryString["openid"].ToString();
            }

            //取最新抢购的优惠活动
            List<Model.JC.MyQuiz> liQuizScroe = new List<Model.JC.MyQuiz>();
            JC_ScoreDAL dalQuizScroe = new JC_ScoreDAL();

            DataSet ds = dalQuizScroe.GetJCScoreList(" a.OpenID = '"+ strOpenID + "' ");

            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Model.JC.MyQuiz model = DataConvert.DataRowToModel<Model.JC.MyQuiz>(row);
                    liQuizScroe.Add(model);
                }
            }
            else
            {
                strInfo = "亲，你还没有参加过竞猜！";
            }
            //读取模板内容
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Quiz/MyQuiz.html"));

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["sitecode"] = strSiteCode;
            context.TempData["openid"] = strOpenID;
            context.TempData["title"] = "竞猜列表";
            context.TempData["quizlist"] = liQuizScroe;
            context.TempData["couponinfo"] = strInfo;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}