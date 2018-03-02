using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.MiniShop;
using Mozart.Common;
using Model.MiniShop;
using System.Text.RegularExpressions;

namespace Mozart.Comment
{
    public partial class Index : System.Web.UI.Page
    {
        public string errorscript = string.Empty;
        public string strfid = string.Empty;
        public string action = string.Empty;
        public string strUID = string.Empty;
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
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    strUID = Session["customerID"].ToString();
                }
                if (Request["action"] != null && Request["action"] != ""&&Request["tid"]!=null&&Request["tid"]!="")
                {
                    action = Request["action"];
                    if (strUID != null && strUID != "")
                    {
                        TopicLoveOrLike();
                        Response.End();
                    }
                    else
                    {
                        JQDialog.SetCookies("pageurl", "../../Comment/Index.aspx?fid=" + strfid, 2);
                        errorscript = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！",
                            "../PalmShop/ShopCode/UserLogin.aspx", "error");
                    }
                }
                GetTopicList();
            }
        }
        void GetTopicList()
        {
            #region---------------论坛详细----------------------
            MSForumSetDAL ForumDal = new MSForumSetDAL();
            MSForumTopicDAL topicDal = new MSForumTopicDAL();
            MSForumSet ForumModel= new MSForumSet();
            string pagetitle = string.Empty; int topicCount = 0; int forumvisit = 0;
            DataSet ForumDs = ForumDal.GetMSForumSetDetail(strfid);
            if (ForumDs != null && ForumDs.Tables.Count > 0 && ForumDs.Tables[0].Rows.Count > 0)
            {
                ForumModel = DataConvert.DataRowToModel<MSForumSet>(ForumDs.Tables[0].Rows[0]);
                pagetitle = ForumModel.FTitle;
                forumvisit = ForumModel.Visit;
            }
            try
            {
                topicCount = Convert.ToInt32(topicDal.GetMSForumTopicCount(strfid));
            }
            catch (Exception)
            {
                topicCount = 0;
            }
            forumvisit = forumvisit + 1;
            if (strUID != null && strUID != "")
            {
                ForumDal.UpdateForumVist(forumvisit, strfid);
            }
            #endregion
            #region--------------获取帖子列表------------------
            List<TopicModelList> topicModellist = new List<TopicModelList>();
            DataSet topiclistds = topicDal.GetMSForumTopicList(" and a.fid='"+strfid+"' ", 0);
            if (topiclistds != null && topiclistds.Tables.Count > 0 && topiclistds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in topiclistds.Tables[0].Rows)
                {
                    TopicModelList topicmodel = DataConvert.DataRowToModel<TopicModelList>(row);
                    if (topicmodel.NickName == null || topicmodel.NickName == "" || topicmodel.NickName.Length < 1)
                    {
                        topicmodel.NickName = "游客";
                    }
                    if (topicmodel.HeadImg == null || topicmodel.HeadImg == "")
                    {
                        topicmodel.HeadImg = "images/2.png";
                    }
                    string msg = topicmodel.TopicDesc;
                    msg = JQDialog.GetTextFromHTML(msg);
                    if (msg.Length > 250)
                    {
                        msg = msg.ToString().Substring(0, 100) + "...";
                    }
                    topicmodel.TopicDesc = msg;
                    topicModellist.Add(topicmodel);
                }
            }
            #endregion

            string text = System.IO.File.ReadAllText(Server.MapPath("HtmlPage/index.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            context.TempData["errorscript"] = errorscript;
            context.TempData["uid"] = strUID;
            context.TempData["title"] = pagetitle;
            context.TempData["ForumDetail"] = ForumModel;
            context.TempData["topicCount"] = topicCount;
            context.TempData["topiclist"] = topicModellist;
            context.TempData["forumvisit"] = forumvisit;
            context.TempData["footer"] = "奥琦微商易";
            t.Render(Response.Output);
        }
        public class TopicModelList 
        {
            private string iD;

            public string ID
            {
                get { return iD; }
                set { iD = value; }
            }
            private string fID;

            public string FID
            {
                get { return fID; }
                set { fID = value; }
            }
            private string uID;

            public string UID
            {
                get { return uID; }
                set { uID = value; }
            }
            private string topicTitle;

            public string TopicTitle
            {
                get { return topicTitle; }
                set { topicTitle = value; }
            }
            private string topicDesc;

            public string TopicDesc
            {
                get { return topicDesc; }
                set { topicDesc = value; }
            }
            private int topicState;

            public int TopicState
            {
                get { return topicState; }
                set { topicState = value; }
            }
            private int treview;

            public int Treview
            {
                get { return treview; }
                set { treview = value; }
            }
            private DateTime addTime;

            public DateTime AddTime
            {
                get { return addTime; }
                set { addTime = value; }
            }

            private string fTitle;

            public string FTitle
            {
                get { return fTitle; }
                set { fTitle = value; }
            }
            private string phone;

            public string Phone
            {
                get { return phone; }
                set { phone = value; }
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
            private int tlike;

            public int Tlike
            {
                get { return tlike; }
                set { tlike = value; }
            }
            private int tlove;

            public int Tlove
            {
                get { return tlove; }
                set { tlove = value; }
            }
            private int ccount;

            public int Ccount
            {
                get { return ccount; }
                set { ccount = value; }
            }
        }
        /// <summary>
        /// 添加或修改  点赞 喜欢
        /// </summary>
        void TopicLoveOrLike()
        {
            string tid = string.Empty; int count = 0;
            tid = Common.Common.NoHtml(Request["tid"]);
            action=action.ToString().ToLower().Trim();
            int addorupdate =0;
            if(action=="tlove"||action=="tlike")
            {
                MSForumTopicLoveDAL lovelike = new MSForumTopicLoveDAL();
                if (!lovelike.ExisTlikeOrlove(tid, strUID))
                {
                    if (!lovelike.AddTloveOrLike(tid, strUID, action))
                    {
                        Response.Write("{\"error\":true}");
                    }
                }
                else
                {
                    if (!lovelike.UpdateTloveOrLike(tid, strUID, action))
                    {
                        Response.Write("{\"error\":true}");
                    }
                }
                try
                {
                    addorupdate = Convert.ToInt32(lovelike.GetTloveOrLike(tid, strUID, action));
                }
                catch (Exception)
                {
                  addorupdate =0;
                }
                count = lovelike.GetLoveOrLikeCount(tid, action);
                Response.Write("{\"success\":true,\"count\":" + count + ",\"result\":" + addorupdate + "}");
            }
        }
    }
}