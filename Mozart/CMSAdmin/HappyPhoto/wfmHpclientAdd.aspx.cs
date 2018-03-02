using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.HP;
using Model.HP;
using Mozart.Common;

namespace Mozart.CMSAdmin.HappyPhoto
{
    public partial class wfmHpclientAdd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["strSiteCode"].ToString()) && Session["strRoleCode"].ToString() != "")
            {
                Response.Write("<script language=JavaScript>parent.location.href='../Index.aspx';</script>");
                Response.End();
            }
            if (!IsPostBack)
            {
                clientcode.Text = "";
                clientcode.Focus();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (clientcode.Text.Trim() != null && clientcode.Text.Trim() != "")
            {
                HP_Client model = new HP_Client();
                HPClientDAL dal = new HPClientDAL();
                if (clientcode.Text.Trim() != null && clientcode.Text.Trim() != "")
                {
                    model.ClientCode = clientcode.Text;
                }
                model.ID = Guid.NewGuid().ToString("N").ToUpper();
                model.IsDel = 0;
                model.SiteCode = Session["strSiteCode"].ToString();
                if (dal.IsExist(clientcode.Text))
                {
                    MessageBox.Show(this, "该信息已经存在，请不要重复添加！"); return;
                }
                if (dal.AddHPClient(model))
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
            clientcode.Text = "";
        }
    }
}