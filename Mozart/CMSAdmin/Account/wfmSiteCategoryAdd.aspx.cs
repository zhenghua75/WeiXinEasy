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
    public partial class wfmSiteCategoryAdd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SiteName.Text = "";
                SiteName.Focus();
                SysCategoryDAL dal = new SysCategoryDAL();
                int max = dal.GetMaxOrder();
                max = max + 1;
                txtOrder.Text = max.ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SiteName.Text.Trim() != null && SiteName.Text.Trim() != "")
            {
                SysCategory model = new SysCategory();
                SysCategoryDAL dal = new SysCategoryDAL();
                model.SiteName = SiteName.Text;
                model.ID = Guid.NewGuid().ToString("N").ToUpper();
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
                if (dal.AddSysCateGory(model))
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