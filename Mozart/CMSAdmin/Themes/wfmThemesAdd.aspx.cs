using Mozart.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.SYS;
using Model.SYS;

namespace Mozart.CMSAdmin.Themes
{
    public partial class wfmThemesAdd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["strSiteCode"].ToString()) && Session["strRoleCode"].ToString() != "ADMIN")
            {
                Response.Write("<script language=JavaScript>parent.location.href='../Index.aspx';</script>");
                Response.End();
            }
            if (!IsPostBack)
            {
                themesname.Text = "";
                themesname.Focus();
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
                model.ID = Guid.NewGuid().ToString("N").ToUpper();
                model.IsState = isstateyes.Checked ? 1 : 0;
                if (dal.AddSysTemes(model))
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