using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.RC;
using DAL.RC;

namespace Mozart.CMSAdmin.RC
{
    public partial class wfmRaceUsers : System.Web.UI.Page
    {
        const string vsKey = "searchCriteria"; //ViewState key
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ddlRaceList.Items.Clear();
                RC_RaceDAL dal = new RC_RaceDAL();
                DataSet ds = new DataSet();
                if (Session["strRoleCode"].ToString() == "ADMIN")
                {
                    ds = dal.GetRCRList("");
                }
                else
                {
                    ds = dal.GetRCRList("   SiteCode ='" + Session["strSiteCode"].ToString() + "' ");
                }
                DataTable dt = ds.Tables[0];

                DataRow dr = ds.Tables[0].NewRow();
                dr["ID"] = 0;
                dr["Rtitle"] = "--全部--";
                dt.Rows.InsertAt(dr, 0);

                this.ddlRaceList.DataSource = ds.Tables[0].DefaultView;
                this.ddlRaceList.DataTextField = "Rtitle";
                this.ddlRaceList.DataValueField = "ID";
                this.ddlRaceList.DataBind();

                AspNetPager1.CurrentPageIndex = 1;
                string s = "";
                if (Session["strRoleCode"].ToString() != "ADMIN")
                {
                    s = " and b.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
                }
                ViewState[vsKey] = s;
                LoadData(s);
            }
        }

        /// <summary>
        /// 单击"查询"按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string strWhere = "  ";
            if (Session["strRoleCode"].ToString() != "ADMIN")
            {
                strWhere = " and b.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            }
            if (ddliswin.SelectedValue.Trim() != null && ddliswin.SelectedValue.Trim() != "")
            {
                strWhere = " and a.IsWin =" + ddliswin.SelectedValue + " ";
            }
            if (ddlRaceList.SelectedValue.Trim() != null && ddlRaceList.SelectedValue.Trim() != "")
            {
                strWhere = " and a.RaceID ='" + ddlRaceList.SelectedValue + "' ";
            }
            if (!string.IsNullOrEmpty(this.txtName.Text) &&
                txtName.Text.Trim() != null && txtName.Text.Trim() != "")
            {
                if (strWhere.Trim() != null && strWhere.Trim() != "")
                {
                    strWhere = strWhere + " AND ";
                }
                strWhere = strWhere + " a.OpenID LIKE '%" + txtName.Text + "%' ";
            }
            AspNetPager1.CurrentPageIndex = 1;
            ViewState[vsKey] = strWhere;
            LoadData(strWhere);
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="strWhere">条件</param>
        void LoadData(string strWhere)
        {
            txtName.Text = "";
            RC_UsersDAL dal = new RC_UsersDAL();
            DataSet ds = dal.GetRcUserList(strWhere);
            DataView dv = ds.Tables[0].DefaultView;

            AspNetPager1.RecordCount = dv.Count;

            PagedDataSource pds = new PagedDataSource();
            pds.DataSource = dv;
            pds.AllowPaging = true;
            pds.CurrentPageIndex = AspNetPager1.CurrentPageIndex - 1;
            pds.PageSize = AspNetPager1.PageSize;
            Repeater1.DataSource = pds;
            Repeater1.DataBind();
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