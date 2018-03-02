using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.CMSAdmin
{
    public partial class UpdatePD : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (this.txtNewPD.Text.Equals("") || this.txtSecPD.Text.Equals(""))
            {
                Common.MessageBox.Show(this, "输入的新密码不能为空，请重新输入！");
                return;
            }

            if (this.txtOldPD.Text == this.txtNewPD.Text)
            {
                Common.MessageBox.Show(this, "新密码与旧密码不能一样，请重新输入！");
                return;
            }

            if (this.txtNewPD.Text != this.txtSecPD.Text)
            {
                Common.MessageBox.Show(this, "两次输入的新密码不一致，请重新输入！");
                return;
            }

            //取旧口令
            DAL.SYS.AccountDAL dal = new DAL.SYS.AccountDAL();
            if (null == Session["strAccountID"])
            {
                Session.Abandon();
                Response.Write("<script language=JavaScript>location.href='Login.aspx';</script>");
                Response.End();
            }
            DataSet ds = dal.GetAccountByID(Session["strAccountID"].ToString());
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (Common.Common.MD5(this.txtOldPD.Text) != ds.Tables[0].Rows[0]["Password"].ToString())
                {
                    Common.MessageBox.Show(this, "原密码不对，请重新输入！");
                    return;
                }

                if (dal.UpdateAccountPD(Session["strAccountID"].ToString(), Common.Common.MD5(this.txtSecPD.Text)))
                {
                    Response.Write("<script language='javascript'>alert('修改完成，请重新登录！');</script>");
                    Session.Abandon();
                    Response.Write("<script language=JavaScript>parent.location.href='Login.aspx';</script>");
                    Response.End();
                }
                else
                {
                    Common.MessageBox.Show(this, "修改失败，请重新登录！");
                    return;
                }
            }
            else
            {
                Session.Abandon();
                Response.Write("<script language=JavaScript>parent.location.href='Login.aspx';</script>");
                Response.End();
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtOldPD.Text = "";
            this.txtNewPD.Text = "";
            this.txtSecPD.Text = "";
        }
    }
}