using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.Album;
using DAL.Album;

namespace Mozart.CMSAdmin.Album
{
    public partial class wfmUserPhoto : System.Web.UI.Page
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
                string s = string.Empty;
                s = " [state]=1 ";
                if (Session["strSiteCode"].ToString() != null && Session["strRoleCode"].ToString() != "ADMIN")
                {
                    if (s.Trim() != null && s.Trim() != "")
                    {
                        s = s + " and ";
                    }
                    s =s+ " SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
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
            string strWhere = " ";
            if (Session["strSiteCode"].ToString() != null && Session["strRoleCode"].ToString() != "ADMIN")
            {
                strWhere += " SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            }
            if (ddlstate.SelectedValue.Trim() != null && ddlstate.SelectedValue.Trim() != "")
            {
                if (strWhere.Trim() != null && strWhere.Trim() != "")
                {
                    strWhere += " and ";
                }
                strWhere += " [state] = " + ddlstate.SelectedValue + " ";
            }
            if (txtTitle.Text.Trim() != null && txtTitle.Text.Trim() != "")
            {
                if (strWhere.Trim() != null && strWhere.Trim() != "")
                {
                    strWhere += " and ";
                }
                strWhere += strWhere + "  name like '%" + this.txtTitle.Text + "%' ";
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
            txtTitle.Text = "";
            UserPhotoDAL dal = new UserPhotoDAL();
            DataSet ds = dal.GetPhotoList(strWhere);
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
                Label lb_state = (Label)e.Item.FindControl("state");
                if (lb_state.CssClass.Trim().ToLower() == "1")
                {
                    lb_state.Text = "已通过";
                }
                else
                {
                    lb_state.Text = "<a href=\"javascript:\" onclick=\"AdminBizControlConfirm('wfmUserPhotoAdmin.aspx?action=del&id=" + lb_state.Text + ">');\">通过</a>";
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