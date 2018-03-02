using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.CR;
using Model.CR;

namespace Mozart.CMSAdmin.CR
{
    public partial class wfmChatUsers : System.Web.UI.Page
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

                ddlRoomList.Items.Clear();
                ChatRoomDAL rmdal = new ChatRoomDAL();
                DataSet ds = new DataSet();
                if (Session["strRoleCode"].ToString() == "ADMIN")
                {
                    ds = rmdal.GetChatRoomList("");
                }
                else
                {
                    ds = rmdal.GetChatRoomList("  AND SiteCode ='" + Session["strSiteCode"].ToString() + "' ");
                }
                DataTable dt = ds.Tables[0];

                DataRow dr = ds.Tables[0].NewRow();
                dr["ID"] =0;
                dr["RoomName"] = "--全部--";
                dt.Rows.InsertAt(dr, 0);

                this.ddlRoomList.DataSource = ds.Tables[0].DefaultView;
                this.ddlRoomList.DataTextField = "RoomName";
                this.ddlRoomList.DataValueField = "ID";
                this.ddlRoomList.DataBind();

                AspNetPager1.CurrentPageIndex = 1;
                string s = "";
                if (Session["strRoleCode"].ToString() != "ADMIN")
                {
                    s = " AND b.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
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
                strWhere = " AND b.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            }
            if (OpenID.Text.Trim() != null && OpenID.Text.Trim() != "")
            {
                strWhere = strWhere + " AND a.OpenID like '%" + OpenID.Text + "%' ";
            }
            if (ddlRoomList.SelectedValue.Trim() != null && ddlRoomList.SelectedValue.Trim() != ""
                && ddlRoomList.SelectedValue.Trim() != "0")
            {
                strWhere = strWhere + " AND b.ID =" + ddlRoomList.SelectedValue + " ";
            }
            if (ddliswin.SelectedValue.Trim() != null && ddliswin.SelectedValue.Trim() != ""
                && ddliswin.SelectedValue.Trim() != "0")
            {
                strWhere = strWhere + " AND a.IsWin =" + ddliswin.SelectedValue + " ";
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
            OpenID.Text = "";
            ChatUsersDAL dal = new ChatUsersDAL();
            DataSet ds = dal.GetChatUsersList(strWhere);
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