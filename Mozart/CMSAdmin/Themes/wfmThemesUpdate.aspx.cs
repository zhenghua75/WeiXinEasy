using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.SYS;
using Model.SYS;
using Mozart.Common;

namespace Mozart.CMSAdmin.Themes
{
    public partial class wfmThemesUpdate : System.Web.UI.Page
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
            SysThemesDAL dal = new SysThemesDAL();
            DataSet ds = dal.GetSysThemesds(strID);
            SysThemes model = DataConvert.DataRowToModel<SysThemes>(ds.Tables[0].Rows[0]);
            themesname.Text = model.Name;
            themesvalue.Text = model.Value;
            if (model.IsState == 0)
            {
                isstateyes.Checked = true;
            }
            else
            {
                isstateno.Checked = true;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (themesname.Text.Trim() != null && themesname.Text.Trim() != "")
            {
                SysThemes model = new SysThemes();
                SysThemesDAL dal = new SysThemesDAL();
                if (themesvalue.Text.Trim() != null && themesvalue.Text.Trim() != "")
                {
                    model.Value = themesvalue.Text;
                }
                model.Name = themesname.Text;
                model.ID =strID;
                model.IsState = isstateyes.Checked ? 1 : 0;
                if (dal.UpdateSysThemes(model))
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
                MessageBox.Show(this, "名称不能为空！");
                return;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            themesname.Text = "";
        }
    }
}