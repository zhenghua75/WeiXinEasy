using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.ACT;
using DAL.Game;
using Model.Game;
using Mozart.Common;

namespace Mozart.CMSAdmin.Game
{
    public partial class wfmLuckyAwardUpdate : System.Web.UI.Page
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
            if (string.IsNullOrEmpty(Session["strSiteCode"].ToString()) && Session["strRoleCode"].ToString() != "ADMIN")
            {
                Response.Write("<script language=JavaScript>parent.location.href='../Index.aspx';</script>");
                Response.End();
            }
            if (!IsPostBack)
            {
                #region 初始化界面

                ddlactlist.Items.Clear();
                SiteActivityDAL actDal = new SiteActivityDAL();
                DataSet ds = new DataSet();
                if (Session["strRoleCode"].ToString() == "ADMIN")
                {
                    ds = actDal.GetActivityListByState("");
                }
                else
                {
                    ds = actDal.GetActivityListByState("  AND SiteCode ='" + Session["strSiteCode"].ToString() + "' ");
                }
                ddlactlist.DataSource = ds.Tables[0].DefaultView;
                ddlactlist.DataTextField = "ActTitle";
                ddlactlist.DataValueField = "ID";
                ddlactlist.DataBind();

                awardsort.Items.Add(new ListItem("大转盘", "0"));
                awardsort.Items.Add(new ListItem("刮刮奖", "1"));

                if (null != Common.Common.NoHtml(Request.QueryString["action"]))
                {
                    strAction = Common.Common.NoHtml(Request.QueryString["action"]);
                }
                if (null != Common.Common.NoHtml(Request.QueryString["id"]))
                {
                    strID = Common.Common.NoHtml(Request.QueryString["id"]);
                }
                ShowInfo(strID);
                #endregion
            }
        }
        void ShowInfo(string strID)
        {
            LuckyAwardDAL dal = new LuckyAwardDAL();
            DataSet ds = dal.GetAwardDetail(strID);
            LuckyAward model = DataConvert.DataRowToModel<LuckyAward>(ds.Tables[0].Rows[0]);
            AwardName.Text = model.Award;
            hd_content.Value = model.AwardContent;
            AwardNum.Text = model.AwardNum.ToString();
            ddlactlist.SelectedIndex = ddlactlist.Items.IndexOf(ddlactlist.Items.FindByValue(model.ActID));
            awardsort.SelectedIndex = awardsort.Items.IndexOf(awardsort.Items.FindByValue(model.AwardSort.ToString()));
            AwardPro.Text = model.AwardPro.ToString();
            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (null == Session["strSiteName"] || null == Session["strSiteCode"] || null == Session["strLoginName"])
            {
                Response.Write("<script language=JavaScript>;parent.location.href='../Index.aspx';</script>");
                Response.End();
            }
            if (AwardName.Text.Trim() != null && AwardName.Text.Trim() != "")
            {
                LuckyAward model = new LuckyAward();
                LuckyAwardDAL dal = new LuckyAwardDAL();
                model.Award = AwardName.Text;
                model.AwardContent = hd_content.Value;
                if (AwardNum.Text.Trim() != null && AwardNum.Text.Trim() != "")
                {
                    model.AwardNum = Convert.ToInt32(AwardNum.Text);
                }
                if (AwardPro.Text.Trim() != null && AwardPro.Text.Trim() != "")
                {
                    model.AwardPro = Convert.ToInt32(AwardPro.Text);
                }
                if (ddlactlist.SelectedValue.Trim() != null && ddlactlist.SelectedValue.Trim() != "")
                {
                    model.ActID = ddlactlist.SelectedValue;
                }
                model.AwardSort = Convert.ToInt32(awardsort.SelectedValue);
                model.IsDel = 0;
                model.ID = strID;
                if (dal.UpdateAward(model))
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
                MessageBox.Show(this, "请输入相应标题名称！");
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            AwardName.Text = ""; AwardNum.Text = "";
            hd_content.Value = "";
        }
    }
}