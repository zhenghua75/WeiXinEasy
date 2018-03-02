using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.MiniShop;
using Model.MiniShop;
using Mozart.Common;

namespace Mozart.CMSAdmin.MiniShop
{
    public partial class wfmMSForumTopicUpdate : System.Web.UI.Page
    {
        static string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;
        static string oldimg = string.Empty;
        public static string topicaltaslist = string.Empty;
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != ""
                    && Session["strLoginName"].ToString().ToLower().Trim() == "vyigo")
                {
                    #region 初始化界面
                    if (null != Common.Common.NoHtml(Request.QueryString["action"]))
                    {
                        strAction = Common.Common.NoHtml(Request.QueryString["action"]);
                    }
                    if (null != Common.Common.NoHtml(Request.QueryString["id"]))
                    {
                        strID = Common.Common.NoHtml(Request.QueryString["id"]);
                    }
                    showdetailinfo();
                    #endregion
                }
                else
                {
                    return;
                }
            }
        }
        void showdetailinfo()
        {
            MSForumTopicDAL topicDal = new MSForumTopicDAL();
            DataSet ds = topicDal.GetTopicDetail(strID);
            MSForumTopic topicModel = DataConvert.DataRowToModel<MSForumTopic>(ds.Tables[0].Rows[0]);
            topictitle.Text = topicModel.TopicTitle;
            hd_content.Value = topicModel.TopicDesc;

            MSForumTopicAtlasDAL AtlasDal = new MSForumTopicAtlasDAL();
            DataSet atlasds = AtlasDal.GetMSFTAtlasList(" AND TID='"+strID+"' ");
            if (atlasds != null && atlasds.Tables.Count > 0 && atlasds.Tables[0].Rows.Count > 0)
            {
                topicaltaslist = ""; string atlasimg = "";
                for (int i = 0; i < atlasds.Tables[0].Rows.Count; i++)
                {
                    atlasimg = atlasds.Tables[0].Rows[i]["ImgUrl"].ToString();
                    topicaltaslist += "<img src=\"../../Comment/" + atlasimg + "\" />";
                }
            }
            

            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != "")
            {
                if (topictitle.Text.Trim() != null && topictitle.Text.Trim() != "")
                {
                    MSForumTopicDAL topicDal = new MSForumTopicDAL();
                    MSForumTopic topicModel = new MSForumTopic();
                    topicModel.ID = strID;
                    topicModel.TopicTitle = topictitle.Text; ;
                    topicModel.TopicDesc = hd_content.Value;
                    topicModel.TopicState = 0;
                    if (topicDal.UpdateMSForumTopic(topicModel))
                    {
                        MessageBox.Show(this, "操作成功！");
                    }
                    else
                    {
                        MessageBox.Show(this, "操作失败！");
                    }
                }
                else
                {
                    MessageBox.Show(this, "请输入相应标题！");
                }
            }
            else
            {
                return;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            topictitle.Text = "";
            hd_content.Value = ""; 
        }
    }
}