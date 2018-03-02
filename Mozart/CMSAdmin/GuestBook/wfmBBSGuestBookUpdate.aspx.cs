using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.BBS;
using Model.BBS;
using Mozart.Common;

namespace Mozart.CMSAdmin.GuestBook
{
    public partial class wfmBBSGuestBookUpdate : System.Web.UI.Page
    {
        static string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(Session["strSiteCode"].ToString()) && Session["strRoleCode"].ToString() != "ADMIN")
                {
                    Response.Write("<script language=JavaScript>parent.location.href='../Index.aspx';</script>");
                    Response.End();
                }
                if (!IsPostBack)
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
                    ShowActivityInfo(strID);
                    #endregion
                }
            }
        }
        public void ShowActivityInfo(string strID)
        {
            BBSGuestBookDAL dal = new BBSGuestBookDAL();
            DataSet ds = dal.GetGuestBookDetail(strID);
            BBS_GuestBook model = DataConvert.DataRowToModel<BBS_GuestBook>(ds.Tables[0].Rows[0]);
            UserName.Text = model.UserName;
            UserMobile.Text = model.UserMobile;
            Content.Text = model.Content;
            CreateTime.Text = model.CreateTime.ToString();
            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
                repinputdiv.Visible = false;
                showwddiv.Visible = true;
                if (model.Replay.Trim() != null && model.Replay.Trim() != "")
                {
                    repbtn.Visible = false; repcontr.Visible = true;
                    repcontent.Text = model.Replay;
                }
                else
                {
                    repbtn.Visible = true; repcontr.Visible = false;
                }
            }
            else
            {
                this.btnReset.Visible = true;
                this.btnSave.Visible = true;
                repinputdiv.Visible = true;
                repcontr.Visible = false;
                repbtn.Visible = false;
                showwddiv.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (repcontentinfo.Text.Trim() != null && repcontentinfo.Text.Trim() != "")
            {
                BBSGuestBookDAL dal = new BBSGuestBookDAL();
                if (dal.SaveReplay(repcontentinfo.Text, strID))
                {
                    MessageBox.Show(this, "回复成功！");
                }
                else
                {
                    MessageBox.Show(this, "回复失败！");
                }
            }
            else
            {
                MessageBox.Show(this, "请输入回复内容！");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfmBBSGuestBookUpdate.aspx?action=show&id=" + strID);
        }

        protected void repbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfmBBSGuestBookUpdate.aspx?action=update&id=" + strID);
        }
    }
}