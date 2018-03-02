using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.MiniShop;

namespace Mozart.CMSAdmin.MiniShop
{
    public partial class wfmSecHandProduct : System.Web.UI.Page
    {
        const string vsKey = "searchCriteria"; //ViewState key
        static string sechand = string.Empty;
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != ""
                    && Session["strLoginName"].ToString().ToLower().Trim() == "vyigo")
                {
                    if (Request["sch"] != null && Request["sch"] != "")
                    {
                        sechand = " and c.CsecHand=" + Request["sch"] + " ";
                    }
                    else
                    {
                        sechand = "";
                    }
                    AspNetPager1.CurrentPageIndex = 1;
                    string s = "";
                    s = sechand;
                    ViewState[vsKey] = s;
                    LoadData(s);
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 单击"查询"按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string strWhere = string.Empty;
            if (sechand != null && sechand != "")
            {
                strWhere += sechand;
            }
            if (selectreview.SelectedValue != null && selectreview.SelectedValue != "")
            {
                strWhere = strWhere + " AND a.Review=" + selectreview.SelectedValue;
            }
            if (txtName.Text.Trim() != null && txtName.Text.Trim() != "")
            {
                strWhere = strWhere + " AND a.[ptitle] LIKE '%" + txtName.Text + "%' ";
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
            MSProductDAL shopdal = new MSProductDAL();
            DataSet ds = shopdal.GetSecHandProduct(strWhere);
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
                Label link_review = (Label)e.Item.FindControl("linkreview");
                Label lb_review = (Label)e.Item.FindControl("Review");
                if (link_review.ToolTip == "未通过" || link_review.ToolTip == "0")
                {
                    link_review.Text = "<a onclick=\"AdminBizControl('wfmProductAdmin.aspx?action=pass&id=" +
                        link_review.Text + "');\">设置通过</a>";
                   link_review.ToolTip= lb_review.Text= "未通过";
                }
                else
                {
                    link_review.Text = "<a onclick=\"AdminBizControl('wfmProductAdmin.aspx?action=pass&id=" +
                                           link_review.Text + "');\">锁定</a>";
                    link_review.ToolTip=lb_review.Text = "通过";
                }
                Label lb_sechand = (Label)e.Item.FindControl("sechand");
                if (lb_sechand.Text == "是" || lb_sechand.Text == "1" || lb_sechand.Text == "二手"||
                     lb_sechand.Text == "是二手")
                {
                     lb_sechand.Text = "是";
                }
                else
                {
                    lb_sechand.Text = "否";
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