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
    public partial class MyCommentList : System.Web.UI.Page
    {
        public string errorscript = string.Empty;
        public string strUid = string.Empty;
        public string strfid = string.Empty;
        public string action = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["fid"] != null && Request["fid"] != "")
                {
                    errorscript = "";
                    strfid = Common.Common.NoHtml(Request["fid"]);
                }
                else
                {
                    errorscript = JQDialog.alertOkMsgBoxClearBody(2, "操作失败<br/>请核对后再操作！");
                }
                if (Request["uid"] != null && Request["uid"] != "")
                {
                    strUid = Common.Common.NoHtml(Request["uid"]);
                }
                else
                {
                    if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                    {
                        strUid = Session["customerID"].ToString();
                    }
                    else
                    {
                        JQDialog.SetCookies("pageurl", "../../Comment/MyCommentList.aspx?fid=" + strfid, 2);
                        errorscript = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！",
                            "../PalmShop/ShopCode/UserLogin.aspx", "error");
                    }
                }
                getTemplate();
            }
        }
        public void getTemplate()
        {
            #region -----------获取客户信息---------
            MSCustomers CustomerModel = new MSCustomers();
            MSCustomersDAL CustomerDal = new MSCustomersDAL();
            DataSet CustomerDs;
            if (strUid != null && strUid != "")
            {
                CustomerDs = CustomerDal.GetCustomerDetail(strUid);
                if (null != CustomerDs && CustomerDs.Tables.Count > 0 && CustomerDs.Tables[0].Rows.Count > 0)
                {
                    CustomerModel = DataConvert.DataRowToModel<MSCustomers>(CustomerDs.Tables[0].Rows[0]);
                    if (CustomerModel.NickName == null || CustomerModel.NickName == "" ||
                        CustomerModel.NickName.ToLower() == "null")
                    {
                        CustomerModel.NickName = "游客";
                    }
                }
            }
            #endregion
            #region ---------评论列表以及信息回复------------
            List<CommentListGetSet> commentModelList = new List<CommentListGetSet>();
            MSForumCommentDAL commentDal = new MSForumCommentDAL();
            DataSet commentDs = commentDal.GetCommentList(" and a.[uid]='" + strUid + "' ");
            DataSet repcommentds;
            List<CommentListGetSet> repcommentModelList = new List<CommentListGetSet>();
            int commentcount = 0;
            if (commentDs != null && commentDs.Tables.Count > 0 && commentDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in commentDs.Tables[0].Rows)
                {
                    CommentListGetSet commentModel = DataConvert.DataRowToModel<CommentListGetSet>(row);
                    if (commentModel.NickName == null || commentModel.NickName == "")
                    {
                        commentModel.NickName = "游客";
                    }
                    #region ----------------获取回复信息-----------------------
                    string repid = commentModel.ID;
                    repcommentds = commentDal.GetRepCommentList(" and a.[uid]='" + strUid + "' and a.UpID='"+ repid + "' ");
                    if (repcommentds != null && repcommentds.Tables.Count > 0 && repcommentds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow reprow in repcommentds.Tables[0].Rows)
                        {
                            CommentListGetSet repcommentModel = DataConvert.DataRowToModel<CommentListGetSet>(reprow);
                            if (repcommentModel.NickName == null || repcommentModel.NickName == "")
                            {
                                repcommentModel.NickName = "游客";
                            }
                            repcommentModelList.Add(repcommentModel);
                        }
                    }
                    #endregion
                    commentModelList.Add(commentModel);
                }
            }
            try
            {
                commentcount = commentDs.Tables[0].Rows.Count;
            }
            catch (Exception)
            {
                commentcount = 0;
            }
            #endregion
            #region --------------获取喜欢 总数------------------
            int likecount = 0;
            MSForumTopicLoveDAL likeDal = new MSForumTopicLoveDAL();
            try
            {
                likecount = likeDal.GetLoveOrLikeCountByUID(strUid, "tlike");
            }
            catch (Exception)
            {
                likecount = 0;
            }
            #endregion
            #region ------------获取消息总数---------------
            int msgcount = 0;
            try
            {
                msgcount = commentDal.GetCommentCountByUID(strUid);
            }
            catch (Exception)
            {
                msgcount = 0;
            }
            #endregion
            #region---------帖子总数-------------
            int topiccount = 0;
            MSForumTopicDAL topicdal=new MSForumTopicDAL ();
            try
            {
                topiccount = topicdal.GetTopicCountByUID(strUid);
            }
            catch (Exception)
            {
                topiccount = 0;
            }
            #endregion
            string text = System.IO.File.ReadAllText(Server.MapPath("HtmlPage/Mycommentlist.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            context.TempData["customer"] = CustomerModel;
            context.TempData["commentlist"] = commentModelList;
            context.TempData["repcommentlist"] = repcommentModelList;
            context.TempData["fid"] = strfid;
            context.TempData["likecount"] = likecount;
            context.TempData["msgcount"] = msgcount;
            context.TempData["topiccount"] = topiccount;
            context.TempData["errorscript"] = errorscript;
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
    }
}