using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;
using DAL.WeiXin;
using WeiXinCore.Models;
using DAL.Vote;
using Model.Vote;

namespace Mozart.MicroSite
{
    public partial class Vote : System.Web.UI.Page
    {
       static string strTitle = "";
       static string strTheme = "";
       static string strID = "";
       static string strSubjectID = "";
       static string strSubjectTitle = "";
       static string strSubjectContent = "";
       static string strSiteCode = "";
       static string strOpenID = "";
       static string strEndTime = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (null == Request.QueryString["sitecode"])
            {
                return;
            }
            else
            {
                strSiteCode = Common.Common.NoHtml(Request.QueryString["sitecode"].ToString());
            }
            if (Request["subid"] != null && Request["subid"] != "")
            {
                strID = Common.Common.NoHtml(Request["subid"]);
            }
            else
            {
                return;
            }

            if (Request["action"] != null && Request["action"] != "" && Request["action"] == "save")
            {
                votesave();
            }
            else
            {                
                getvoteDetail();
            }
        }

        void getvoteDetail()
        {
            //
            DAL.Vote.IndexDAL dalIndex = new IndexDAL();

            strSubjectID = dalIndex.GetSubjectData(strID);

            if (string.IsNullOrEmpty(strSubjectID))
            {
                return;
            }

            //取投票主题与选项
            DAL.Vote.SubjectDAL dalSubject = new DAL.Vote.SubjectDAL();
            DataSet dsSubject = dalSubject.GetVoteDetail(strSubjectID,strSiteCode);
            if (null != dsSubject && dsSubject.Tables.Count > 0 && dsSubject.Tables[0].Rows.Count > 0)
            {
                strSubjectID = dsSubject.Tables[0].Rows[0]["ID"].ToString();
                strSubjectTitle = dsSubject.Tables[0].Rows[0]["Subject"].ToString();
                strSubjectContent = dsSubject.Tables[0].Rows[0]["Content"].ToString();
                strEndTime = dsSubject.Tables[0].Rows[0]["EndTime"].ToString();
                if (Convert.ToDateTime(strEndTime) > DateTime.Now)
                {
                    strEndTime = "show";
                }
            }
            else
            {
                return;
            }

            List<Model.Vote.VOTE_Option> liOption = new List<Model.Vote.VOTE_Option>();

            DataSet dsOptions = dalSubject.GetOptionsData(strSubjectID);
            if (null != dsOptions && dsOptions.Tables.Count > 0 && dsOptions.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsOptions.Tables[0].Rows)
                {
                    Model.Vote.VOTE_Option model = DataConvert.DataRowToModel<Model.Vote.VOTE_Option>(row);
                    liOption.Add(model);
                }
            }

            List<VOTE_Subject> lisubjectlist = new List<VOTE_Subject>();
            DataSet dssublist = dalSubject.GetSubjectDataList(strSiteCode);
            int sublistcount = dssublist.Tables[0].Rows.Count;
            if (null != dssublist && dssublist.Tables.Count > 0 && dssublist.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dssublist.Tables[0].Rows)
                {
                    VOTE_Subject model = DataConvert.DataRowToModel<VOTE_Subject>(row);
                    lisubjectlist.Add(model);
                }
            }

            //读取模板内容
            string text = string.Empty;
            if (!File.Exists(Server.MapPath("Themes/" + strTheme + "/Vote.html")))
            {
                text = System.IO.File.ReadAllText(Server.MapPath("Themes/Vote/Vote.html"));
            }
            else
            {
                text = System.IO.File.ReadAllText(Server.MapPath("Themes/" + strTheme + "/Vote.html"));
            }

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["title"] = strTitle;
            context.TempData["siteid"] = Session["siteid"];
            context.TempData["footer"] = "奥琦微商易";
            context.TempData["sitecode"] =strSiteCode;

            context.TempData["openid"] = strOpenID;
            context.TempData["subjectID"] = strSubjectID;
            context.TempData["SubjectTitle"] = strSubjectTitle;
            context.TempData["SubjectContent"] = strSubjectContent;
            context.TempData["SubjectEndTime"] = strEndTime;

            context.TempData["option_list"] = liOption;

            context.TempData["subjectlist"] = lisubjectlist;
            context.TempData["sublistcount"] = sublistcount;
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        void votesave()
        {
            if (Request["action"] != null && Request["action"] != "")
            {
                if (Request["openid"] != null && Request["openid"] != "")
                {
                    strOpenID = Request.QueryString["openid"].ToString();
                }
            }
            string voteID = string.Empty;
            string subID = string.Empty;
            string phonenum = string.Empty;
            if (Request["option"] != null && Request["option"] != "")
            {
                voteID = Common.Common.NoHtml(Request.QueryString["option"].ToString());
            }
            if (Request["voteid"] != null && Request["voteid"] != "")
            {
                subID = Common.Common.NoHtml(Request.QueryString["voteid"].ToString());
            }
            if (Request["phonenum"] != null && Request["phonenum"] != "")
            {
                phonenum = Common.Common.NoHtml(Request.QueryString["phonenum"].ToString());
            }
            if (strOpenID.Trim() == null || strOpenID.Trim() == "")
            {
                if (phonenum.Trim() == null || phonenum.Trim() == "")
                {
                    Response.Write("{\"message\":\"操作失败，未找到相关的用户信息！\"}"); return;
                }
            }
            if (voteID.Trim() != null && voteID.Trim() != "")
            {
                VoteUsers modeluser = new VoteUsers();
                VoteUsersDAL daluser = new VoteUsersDAL();
                if (strOpenID.Trim() != null && strOpenID.Trim() != "")
                {
                    if (daluser.VoteIsRepeat(voteID, strOpenID))
                    {
                        Response.Write("{\"message\":\"不能重复提交！\"}"); return;
                    }
                    else
                    {
                        if (daluser.SubjectIsRepeat(subID, strOpenID))
                        {
                            Response.Write("{\"message\":\"您已经投过票了！\"}"); return;
                        }
                    }
                }
                else
                {
                    if (phonenum.Trim() != null && phonenum.Trim() != "")
                    {
                        if (daluser.UsreIsRepeat(voteID, phonenum))
                        {
                            Response.Write("{\"message\":\"不能重复提交！\"}"); return;
                        }
                        else
                        {
                            if (daluser.SubjectIsRepeatUser(subID, phonenum))
                            {
                                Response.Write("{\"message\":\"您已经投过票了！\"}"); return;
                            }
                        }
                    }
                    else
                    {
                        Response.Write("{\"message\":\"请输入相关信息后再操作！\"}"); return;
                    }
                }
                string struip = Request.UserHostAddress;
                if (daluser.UserIsRepeat(struip, voteID) > 100)
                {
                    //Response.Write("{\"message\":\"您已经投过了！\"}"); 
                    return;
                }
                modeluser.ID = Guid.NewGuid().ToString("N").ToUpper();
                modeluser.VoteID = voteID;
                modeluser.IsDel = 1;
                modeluser.UserIP = struip;
                if (strOpenID.Trim() != null && strOpenID.Trim() != "")
                {
                    modeluser.OpenID = strOpenID;
                }
                if (phonenum.Trim() != null && phonenum.Trim() != "")
                {
                    modeluser.UserName = phonenum;
                }
                modeluser.SubjectID = subID;
                if (daluser.AddVoteUsers(modeluser))
                {
                    Response.Write("{\"message\":\"操作成功！\"}"); return;
                }
                else
                {
                    Response.Write("{\"message\":\"操作失败！\"}"); return;
                }
            }
            else
            {
                Response.Write("{\"message\":\"操作失败！\"}"); return;
            }
            Response.End();
        }
    }
}
