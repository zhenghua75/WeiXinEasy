using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;
using DAL;
using System.Data;


namespace Mozart.CMSAdmin
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string strUserName = this.txtName.Value.Trim();
            string strPassWrod = this.txtPD.Value.Trim();
            if (strUserName.Equals("") || strPassWrod.Equals(""))
            {
                MessageBox.Show(this, "登录名与口令不能这空！");
                return;
            }

            //if (string.Compare(Session["CheckCode"].ToString(), this.txtRand.Text, true) != 0)
            //{
            //    MessageBox.Show(this, "验证码错误，请输入正确的验证码。");
            //    return;
            //}

            DAL.SYS.AccountDAL dal = new DAL.SYS.AccountDAL();

            DataSet dsAccount = dal.GetAccountData(strUserName, Common.Common.MD5(strPassWrod));

            if (dsAccount.Tables[0].Rows.Count < 1 || string.IsNullOrEmpty(dsAccount.Tables[0].Rows[0]["ID"].ToString()))
            {
                MessageBox.Show(this, "用户名或口令不正确，请核对后重新登录！");
                return;
            }
            //if (!isinip(Page.Request.UserHostAddress, "192.168.1.0", "192.168.1.255"))
            //{

            //}
            Session["strAccountID"] = dsAccount.Tables[0].Rows[0]["ID"].ToString();
            Session["strLoginName"] = dsAccount.Tables[0].Rows[0]["LoginName"].ToString();
            Session["strSiteName"] = dsAccount.Tables[0].Rows[0]["Name"].ToString();
            Session["strRoleCode"] = dsAccount.Tables[0].Rows[0]["RoleID"].ToString();
            Session["strIP"] = Page.Request.UserHostAddress.ToString();
            Session["strSiteCode"] = dsAccount.Tables[0].Rows[0]["SiteCode"].ToString();
            Session["SiteCategory"] = dsAccount.Tables[0].Rows[0]["SiteCategory"].ToString();

            Response.Redirect("Index.aspx", false);
        }
    }
}