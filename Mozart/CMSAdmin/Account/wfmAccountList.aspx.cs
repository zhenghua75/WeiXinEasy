using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.SYS;
using Mozart.Common;

namespace Mozart.CMSAdmin.Account
{
    public partial class wfmAccountList : System.Web.UI.Page
    {
        const string vsKey = "searchCriteria"; //ViewState key
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                this.txtName.Text = "";
                txtLoginName.Text = "";
                AspNetPager1.CurrentPageIndex = 1;
                string s = "";
                ViewState[vsKey] = s;
                txtLoginName.Focus();
                LoadData(s);
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string strWhere = string.Empty;

            if (txtLoginName.Text.Trim() != null && txtLoginName.Text.Trim() != "")
            {
                strWhere += " a.LoginName like '%" + txtLoginName.Text + "%' ";

                if (txtName.Text.Trim() != null && txtName.Text.Trim() != "")
                {
                    strWhere += " and a.Name = '" + txtName.Text + "' ";
                }
                if (ddlState.SelectedValue.Trim() != null && ddlState.SelectedValue.Trim() != "" && ddlState.SelectedValue.Trim() != "0")
                {
                    strWhere += " and a.Status='" + ddlState.SelectedValue + "' ";
                }
            }
            else
            {
                if (txtName.Text.Trim() != null && txtName.Text.Trim() != "")
                {
                    strWhere += "  a.Name = '" + txtName.Text + "' ";

                    if (ddlState.SelectedValue.Trim() != null && ddlState.SelectedValue.Trim() != "" && ddlState.SelectedValue.Trim() != "0")
                    {
                        strWhere += " and a.Status='" + ddlState.SelectedValue + "' ";
                    }
                }
                else
                {
                    if (ddlState.SelectedValue.Trim() != null && ddlState.SelectedValue.Trim() != "" && ddlState.SelectedValue.Trim() != "0")
                    {
                        strWhere += "  a.Status='" + ddlState.SelectedValue + "' ";
                    }
                }
            }
            AspNetPager1.CurrentPageIndex = 1;
            ViewState[vsKey] = strWhere;

            LoadData(strWhere);
        }

        void LoadData(string strWhere)
        {
            AccountDAL dal = new AccountDAL();
            DataSet ds = dal.GetAccountList(strWhere);
            DataView dv = ds.Tables[0].DefaultView;

            AspNetPager1.RecordCount = dv.Count;

            PagedDataSource pds = new PagedDataSource();
            pds.DataSource = dv;
            pds.AllowPaging = true;
            pds.CurrentPageIndex = AspNetPager1.CurrentPageIndex - 1;
            pds.PageSize = AspNetPager1.PageSize;
            Repeater1.DataSource = pds;
            Repeater1.DataBind();

            this.ddlState.Items.Clear();
            SYSDictionaryDAL didal = new SYSDictionaryDAL();
            DataSet dids = new DataSet();
            dids = didal.GetDictionaryList("");
            DataTable dt = dids.Tables[0];

            DataRow dr = dids.Tables[0].NewRow();
            dr["ID"] = "0";
            dr["Remark"] = "--全部--";
            dt.Rows.InsertAt(dr, 0);

            this.ddlState.DataSource = dids.Tables[0].DefaultView;
            this.ddlState.DataTextField = "Remark";
            this.ddlState.DataValueField = "ID";
            this.ddlState.DataBind();
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lb_no = (Label)e.Item.FindControl("no");
                lb_no.Text = (1 + e.Item.ItemIndex).ToString();
            }
        }

        protected void AspNetPager1_PageChanged(object src, EventArgs e)
        {
            LoadData((string)ViewState[vsKey]);
        }

        /// </summary>
        /// <param name="control"></param>
        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }
    }
}