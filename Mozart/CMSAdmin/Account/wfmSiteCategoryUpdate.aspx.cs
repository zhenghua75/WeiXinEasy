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

namespace Mozart.CMSAdmin.Account
{
    public partial class wfmSiteCategoryUpdate : System.Web.UI.Page
    {
        static string strID = string.Empty;
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
            SysCategoryDAL dal = new SysCategoryDAL();
            DataSet ds = dal.GetSysCateGoryDetail(strID);
            SysCategory model = DataConvert.DataRowToModel<SysCategory>(ds.Tables[0].Rows[0]);
            SiteName.Text = model.SiteName;
            hd_SiteDesc.Value = model.SiteDesc;
            txtOrder.Text = model.SiteOrder.ToString();
            if (model.IsDel ==true)
            {
                isstateyes.Checked = true;
            }
            else
            {
                isstateno.Checked = true;
            }
            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SiteName.Text.Trim() != null && SiteName.Text.Trim() != "")
            {
                SysCategory model = new SysCategory();
                SysCategoryDAL dal = new SysCategoryDAL();
                model.SiteName = SiteName.Text;
                model.ID =strID;
                model.IsDel = isstateyes.Checked ? true : false;
                model.SiteDesc = hd_SiteDesc.Value;
                if (txtOrder.Text.Trim() != null && txtOrder.Text.Trim() != "")
                {
                    model.SiteOrder = Convert.ToInt32(txtOrder.Text);
                }
                else
                {
                    model.SiteOrder = 1;
                }
                if (dal.UpdateSysCateGory(model))
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
            SiteName.Text = "";
            hd_SiteDesc.Value = "";
            txtOrder.Text = "";
        }
    }
}