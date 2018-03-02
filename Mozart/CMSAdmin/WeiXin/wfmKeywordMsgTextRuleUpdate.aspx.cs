using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.WeiXin;
using DAL.WeiXin;
using Mozart.Common;

namespace Mozart.CMSAdmin.WeiXin
{
    public partial class wfmKeywordMsgTextRuleUpdate : System.Web.UI.Page
    {
        static string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;
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
        public void ShowActivityInfo(string strID)
        {
            MsgAutoRuleDAL dal = new MsgAutoRuleDAL();
            DataSet ds = dal.GetMsuAutoRuleDetail(strID);
            MsgAutoRule model = DataConvert.DataRowToModel<MsgAutoRule>(ds.Tables[0].Rows[0]);
            keyword.Text = model.MatchPattern;
            repmsgcontent.Value = model.MsgValue;
            sort.Text = model.Order.ToString();
            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
                keyword.ReadOnly = true;
                sort.ReadOnly = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (null == Session["strSiteName"] || null == Session["strSiteCode"] || null == Session["strLoginName"])
            {
                Response.Write("<script language=JavaScript>;parent.location.href='../Index.aspx';</script>");
                Response.End();
            }

            if (keyword.Text.Trim() != null && keyword.Text.Trim() != "")
            {
                MsgAutoRule model = new MsgAutoRule();
                MsgAutoRuleDAL dal = new MsgAutoRuleDAL();
                model.MatchPattern = keyword.Text;
                model.Order = Convert.ToInt32(sort.Text);
                model.MsgValue = repmsgcontent.Value;
                model.ID = strID;
                model.Enabled = 1;
                model.LastModTime = DateTime.Now;
                //model.MatchType = "keywords";
                //model.MsgType = "text";
                //model.Handle = "Mozart.WeiXin.SubscribeCouponActHandle";
                if (dal.UpdateMsgAutoRule(model))
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
                MessageBox.Show(this, "请输入信息名称后再操作！");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            keyword.Text = ""; repmsgcontent.Value = ""; sort.Text = "";
        }
    }
}