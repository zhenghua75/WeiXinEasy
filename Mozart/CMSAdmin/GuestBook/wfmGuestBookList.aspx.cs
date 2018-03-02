using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.BBS;

namespace Mozart.CMSAdmin.GuestBook
{
    public partial class wfmGuestBookList : System.Web.UI.Page
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
                    s = " SiteCode = '" + Session["strSiteCode"].ToString() + "' AND [State]=0";
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
            string strWhere = " SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            if (txtTitle.Text.Trim() != null && txtTitle.Text.Trim()!="")
            {
                strWhere = strWhere + " AND UserName LIKE '%" + this.txtTitle.Text + "%'";
            }
            if (ddlisdel.SelectedValue.Trim() != null && ddlisdel.SelectedValue.Trim() != "")
            {
                strWhere = strWhere + " AND [state]=" + ddlisdel.SelectedValue + " ";
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
            BBSGuestBookDAL dal = new BBSGuestBookDAL();
            DataSet ds = dal.GetGuestBookList(strWhere);
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
                Label lb_isreplay = (Label)e.Item.FindControl("isreplay");
                Label lb_isdel = (Label)e.Item.FindControl("state");
                Label lb_isview = (Label)e.Item.FindControl("isview");
                if (lb_isreplay.Text.Trim() != null && lb_isreplay.Text.Trim() != "")
                {
                    lb_isreplay.Text = "已回复"; lb_isview.Text = "已查看";
                }
                else
                {
                    lb_isreplay.Text = "未回复"; lb_isview.Text = "未查看";
                }
                if (lb_isdel.Text.Trim() == "1")
                {
                    lb_isdel.Text = "已处理 &nbsp;|&nbsp;";
                }
                else
                {
                    lb_isdel.Text = "";
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