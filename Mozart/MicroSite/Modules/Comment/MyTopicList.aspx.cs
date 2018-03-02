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
    public partial class MyTopicList : System.Web.UI.Page
    {
        public string errorscript = string.Empty;
        public string strUid = string.Empty;
        public string strfid = string.Empty;
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
                        JQDialog.SetCookies("pageurl", "../../Comment/MyTopicList.aspx?fid=" + strfid, 2);
                        errorscript = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！",
                            "../PalmShop/ShopCode/UserLogin.aspx", "error");
                    }
                }
                 getTemplate();
            }
        }
        void getTemplate()
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
                    if (CustomerModel.NickName == null || CustomerModel.NickName == ""||
                        CustomerModel.NickName.ToLower() =="null")
                    {
                        CustomerModel.NickName = "游客";
                    }
                }
            }
            #endregion
            #region ----------获取帖子列表----------------
            MSForumTopicDAL topicDal = new MSForumTopicDAL();
            List<TopicModelList> topicModellist = new List<TopicModelList>();
            int topicCount = 0;
            DataSet topiclistds = topicDal.GetMSForumTopicList(" and a.[UID]='" + strUid + "' ", 0);
            if (topiclistds != null && topiclistds.Tables.Count > 0 && topiclistds.Tables[0].Rows.Count > 0)
            {
                topicCount = topiclistds.Tables[0].Rows.Count;
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
            MSForumCommentDAL commentDal = new MSForumCommentDAL();
            try
            {
                msgcount = commentDal.GetCommentCountByUID(strUid);
            }
            catch (Exception)
            {
                msgcount = 0;
            }
            #endregion
            string text = System.IO.File.ReadAllText(Server.MapPath("HtmlPage/postlist.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            context.TempData["mytopiclist"] = topicModellist;
            context.TempData["topiccount"] = topicCount;
            context.TempData["customer"] = CustomerModel;
            context.TempData["fid"] = strfid;
            context.TempData["likecount"] = likecount;
            context.TempData["msgcount"] = msgcount;
            context.TempData["errorscript"] = errorscript;
            context.TempData["uid"] = strUid;
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
    }
}