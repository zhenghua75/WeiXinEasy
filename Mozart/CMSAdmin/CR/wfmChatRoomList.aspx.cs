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
    public partial class wfmChatRoomList : System.Web.UI.Page
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
                AspNetPager1.CurrentPageIndex = 1;
                string s = "";
                if (Session["strRoleCode"].ToString() != "ADMIN")
                {
                    s = " AND SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
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
                strWhere = " AND SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            }
            if (txtName.Text.Trim() != null && txtName.Text.Trim() != "")
            {
                strWhere = strWhere + " AND [RoomName] LIKE '%" + txtName.Text + "%' ";
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
            ChatRoomDAL dal = new ChatRoomDAL();
            DataSet ds = dal.GetChatRoomList(strWhere);
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

                Label lb_roomimg = (Label)e.Item.FindControl("roomimg");
                if (lb_roomimg.Text.Trim() != null && lb_roomimg.Text.Trim() != "")
                {
                    lb_roomimg.Text = "<img src=\"../../WXWall/" + lb_roomimg.Text + "\" width=\"20px\" height=\"20px\" border=\"0px\" >";
                }
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