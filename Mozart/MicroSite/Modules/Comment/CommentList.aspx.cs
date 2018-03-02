using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Mozart.Common;
using DAL.MiniShop;
using Model.MiniShop;

namespace Mozart.Comment
{
    public partial class CommentList : System.Web.UI.Page
    {
        public string errorscript = string.Empty;
        public string strUid = string.Empty;
        public string strfid = string.Empty;
        public string strtid = string.Empty;
        public string action = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                errorscript = "";
                if (Request["fid"] != null && Request["fid"] != "")
                {
                    strfid = Common.Common.NoHtml(Request["fid"]);
                }
                else
                {
                    errorscript = JQDialog.alertOkMsgBoxClearBody(2, "操作失败<br/>请核对后再操作！");
                }
                if (Request["tid"] != null && Request["tid"] != "")
                {
                    strtid = Common.Common.NoHtml(Request["tid"]);
                }
                else
                {
                    errorscript = JQDialog.alertOkMsgBoxClearBody(2, "操作失败<br/>请核对后再操作！");
                }
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    strUid = Session["customerID"].ToString();
                }
                if (Request["action"] != null && Request["action"] != "" && Request["tid"] != null && Request["tid"] != "")
                {
                    action = Request["action"];
                    if (strUid != null && strUid != "")
                    {
                        if (action.Trim().ToLower() == "delrep")
                        {
                            DelRepComment();
                        }
                        else
                        {
                            TopicLoveOrLike();
                        }
                        Response.End();
                    }
                    else
                    {
                        JQDialog.SetCookies("pageurl", "../../Comment/Index.aspx?fid=" + strfid + "&tid=" + strtid, 2);
                        errorscript = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！",
                            "../PalmShop/ShopCode/UserLogin.aspx", "error");
                    }
                }
                getTemplate();
            }
        }
        void getTemplate()
        {
            #region--------------帖子详细--------------------------
            MSForumTopic TopicModel = new MSForumTopic();
            MSForumTopicDAL TopicDel = new MSForumTopicDAL();
            DataSet TopicDs = TopicDel.GetTopicDetail(strtid);
            if (null != TopicDs && TopicDs.Tables.Count > 0 && TopicDs.Tables[0].Rows.Count > 0)
            {
                TopicModel = DataConvert.DataRowToModel<MSForumTopic>(TopicDs.Tables[0].Rows[0]);
            }
            #endregion
            #region-----------帖子图集----------------
            List<MSForumTopicAtlas> atlasModelList = new List<MSForumTopicAtlas>();
            MSForumTopicAtlasDAL atlasDal = new MSForumTopicAtlasDAL();
            DataSet altasDs = atlasDal.GetMSFTAtlasList(" and tid='" + strtid + "'");
            if (altasDs != null && altasDs.Tables.Count > 0 && altasDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in altasDs.Tables[0].Rows)
                {
                    MSForumTopicAtlas atlasModel = DataConvert.DataRowToModel<MSForumTopicAtlas>(row);
                    atlasModelList.Add(atlasModel);
                }
            }
            #endregion
            #region---------点赞或喜欢------------
            MSForumTopicLoveDAL lovelikeDal = new MSForumTopicLoveDAL();
            string likecount = lovelikeDal.GetLoveOrLikeCount(strtid, "tlike").ToString();
            string lovecount = lovelikeDal.GetLoveOrLikeCount(strtid, "tlove").ToString();
            #endregion
            #region ---------评论列表以及图集信息------------ 
            List<CommentListGetSet> commentModelList = new List<CommentListGetSet>();
            MSForumCommentDAL commentDal = new MSForumCommentDAL();
            DataSet commentDs = commentDal.GetCommentList(" and a.tid='" + strtid + "' ");
            List<MSForumTopicAtlas> commatlasModelList = new List<MSForumTopicAtlas>();
            int commentcount = 0;
            if (commentDs != null && commentDs.Tables.Count > 0 && commentDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in commentDs.Tables[0].Rows)
                {
                    CommentListGetSet commentModel = DataConvert.DataRowToModel<CommentListGetSet>(row);
                    string cmmid = commentModel.ID;
                    if (commentModel.NickName == null || commentModel.NickName == "")
                    {
                        commentModel.NickName = "游客";
                    }
                    #region------------评论图集-----------------------
                    DataSet commaltasDs = atlasDal.GetMSFTAtlasList(" and tid='comm" + cmmid + "'");
                    if (commaltasDs != null && commaltasDs.Tables.Count > 0 && commaltasDs.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow commrow in commaltasDs.Tables[0].Rows)
                        {
                            MSForumTopicAtlas atlasModel = DataConvert.DataRowToModel<MSForumTopicAtlas>(commrow);
                            commatlasModelList.Add(atlasModel);
                        }
                    }
                    #endregion

                    commentModelList.Add(commentModel);
                }
            }
            try
            {
                commentcount= commentDs.Tables[0].Rows.Count;
            }
            catch (Exception)
            {
               commentcount = 0;
            }
            #endregion
            string text = System.IO.File.ReadAllText(Server.MapPath("HtmlPage/commentlist.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            context.TempData["topicdetail"] = TopicModel;
            context.TempData["altaslist"] = atlasModelList;
            context.TempData["commentlist"] = commentModelList;
            context.TempData["commentatlaslist"] = commatlasModelList;
            context.TempData["commentcount"] = commentcount;
            context.TempData["errorscript"] = errorscript;
            context.TempData["likecount"] = likecount;
            context.TempData["lovecount"] = lovecount;
            context.TempData["fid"] = strfid;
            context.TempData["uid"] = strUid;
            context.TempData["footer"] = "奥琦微商易";
            t.Render(Response.Output);
        }
        /// <summary>
        /// 评论构造器
        /// </summary>
        public class CommentListGetSet
        {
            private string iD;

            public string ID
            {
                get { return iD; }
                set { iD = value; }
            }
            private string upID;

            public string UpID
            {
                get { return upID; }
                set { upID = value; }
            }
            private string tID;

            public string TID
            {
                get { return tID; }
                set { tID = value; }
            }
            private string uID;

            public string UID
            {
                get { return uID; }
                set { uID = value; }
            }
            private string ctext;

            public string Ctext
            {
                get { return ctext; }
                set { ctext = value; }
            }
            private int review;

            public int Review
            {
                get { return review; }
                set { review = value; }
            }
            private int cstate;

            public int Cstate
            {
                get { return cstate; }
                set { cstate = value; }
            }
            private DateTime addTime;

            public DateTime AddTime
            {
                get { return addTime; }
                set { addTime = value; }
            }
            private string nickName;

            public string NickName
            {
                get { return nickName; }
                set { nickName = value; }
            }
            private string headImg;

            public string HeadImg
            {
                get { return headImg; }
                set { headImg = value; }
            }
        }
        /// <summary>
        /// 添加或修改  点赞 喜欢
        /// </summary>
        void TopicLoveOrLike()
        {
            string tid = string.Empty; int count = 0;
            tid = Common.Common.NoHtml(Request["tid"]);
            action = action.ToString().ToLower().Trim();
            int addorupdate = 0;
            if (action == "tlove" || action == "tlike")
            {
                MSForumTopicLoveDAL lovelike = new MSForumTopicLoveDAL();
                if (!lovelike.ExisTlikeOrlove(tid, strUid))
                {
                    if (!lovelike.AddTloveOrLike(tid, strUid, action))
                    {
                        Response.Write("{\"error\":true}");
                    }
                }
                else
                {
                    if (!lovelike.UpdateTloveOrLike(tid, strUid, action))
                    {
                        Response.Write("{\"error\":true}");
                    }
                }
                try
                {
                    addorupdate = Convert.ToInt32(lovelike.GetTloveOrLike(tid, strUid, action));
                }
                catch (Exception)
                {
                    addorupdate = 0;
                }
                count = lovelike.GetLoveOrLikeCount(tid, action);
                Response.Write("{\"success\":true,\"count\":" + count + ",\"result\":" + addorupdate + "}");
            }
        }
        /// <summary>
        /// 删除评论
        /// </summary>
        void DelRepComment()
        {
            if (strtid != null && strtid != "")
            {
                MSForumCommentDAL commentDal = new MSForumCommentDAL();
                if (commentDal.UpdateCommentState(strtid))
                {
                    Response.Write("{\"success\":true}");
                }
                else
                {
                    Response.Write("{\"error\":true}");
                }
            }
            else
            {
                Response.Write("{\"error\":true}");
            }
        }
    }
}