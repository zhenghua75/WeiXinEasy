using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.Vote;
using Model.Vote;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class MyVote : System.Web.UI.Page
    {
        string strInfo = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (null == Request.QueryString["sitecode"] && !string.IsNullOrEmpty(Request.QueryString["sitecode"].ToString()))
            {
                return;
            }
            if (Request["openid"] == null || Request["openid"] == "")
            {
                if (Request["username"] == null || Request["username"] == "")
                {
                    return;
                }
            }

            string strSiteCode = Common.Common.NoHtml(Request.QueryString["sitecode"].ToString());
            string strOpenID = string.Empty;
            string strUserName = string.Empty;
            if (Request["openid"] != null && Request["openid"] != "")
            {
                if (Request.QueryString["openid"].ToString().Length > 29 || Request.QueryString["openid"].ToString().Length < 25)
                {
                    strOpenID = Common.Common.NoHtml(Request.QueryString["openid"].ToString());
                }
                else
                {
                    strOpenID = Request.QueryString["openid"].ToString();
                }
            }
            else
            {
                strUserName = Common.Common.NoHtml(Request["username"]);
            }

            //取最新抢购的优惠活动
            List<VOTE_Option> MyVoteList = new List<VOTE_Option>();
            VoteUsersDAL voteuserdal = new VoteUsersDAL();

            DataSet myvoteds;
            if (Request["openid"] != null && Request["openid"] != "")
            {
                myvoteds = voteuserdal.GetMyVoteList(" AND a.OpenID = '" + strOpenID + "' ");
            }
            else
            {
                myvoteds = voteuserdal.GetMyVoteList(" AND a.UserName = '" + strUserName + "' ");
            }
            string VoteTitle = string.Empty;
            string VoteIco = string.Empty;
            string VoteContent = string.Empty;
            if (null != myvoteds && myvoteds.Tables.Count > 0 && myvoteds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in myvoteds.Tables[0].Rows)
                {
                    //VoteUsers model = DataConvert.DataRowToModel<VoteUsers>(row);
                    VOTE_Option model = DataConvert.DataRowToModel<VOTE_Option>(row);
                    MyVoteList.Add(model);
                }
            }
            else
            {
                strInfo = "亲，你还没有参加过任何投票！";
            }
            //读取模板内容
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Vote/MyVote.html"));

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["sitecode"] = strSiteCode;
            context.TempData["openid"] = strOpenID;
            context.TempData["title"] = "投票列表";
            context.TempData["MyVoteList"] = MyVoteList;
            context.TempData["noinfo"] = strInfo;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}