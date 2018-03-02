using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.HP;
using DAL.HP;
using Mozart.Common;

namespace Mozart.CMSAdmin.HappyPhoto
{
    public partial class wfmHpclientUpdate : System.Web.UI.Page
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
                ShowInfo();
                #endregion
            }
        }
        void ShowInfo()
        {
            HPClientDAL dal = new HPClientDAL();
            DataSet ds = dal.GetHpClientDetail(strID);
            HP_Client model = DataConvert.DataRowToModel<HP_Client>(ds.Tables[0].Rows[0]);
            clientcode.Text = model.ClientCode;
            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
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
                model.ID =strID;
                model.IsDel = 0;
                if (dal.IsExist(clientcode.Text))
                {
                    MessageBox.Show(this, "该信息已经存在，请不要重复添加！"); return;
                }
                if (dal.UpdateHPClient(model))
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