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
    public partial class wfmMSForumTopicmsgUpdate : System.Web.UI.Page
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
            MSForumCommentDAL CommDal = new MSForumCommentDAL();
            DataSet ds = CommDal.GetCommentDetail(strID);
            MSForumComment CommModel = DataConvert.DataRowToModel<MSForumComment>(ds.Tables[0].Rows[0]);
            hd_content.Value = CommModel.Ctext;

            MSForumTopicAtlasDAL AtlasDal = new MSForumTopicAtlasDAL();
            DataSet atlasds = AtlasDal.GetMSFTAtlasList(" AND TID='comm" + strID + "' ");
            if (atlasds != null && atlasds.Tables.Count > 0 && atlasds.Tables[0].Rows.Count > 0)
            {
                topicaltaslist = ""; string atlasimg = "";
                for (int i = 0; i < atlasds.Tables[0].Rows.Count; i++)
                {
                    atlasimg = atlasds.Tables[0].Rows[i]["ImgUrl"].ToString();
                    topicaltaslist += "<img src=\"../../Comment/" + atlasimg + "\" />";
                }
            }

        }
    }
}