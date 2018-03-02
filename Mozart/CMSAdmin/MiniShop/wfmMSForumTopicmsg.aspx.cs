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
    public partial class wfmMSForumTopicmsg : System.Web.UI.Page
    {
        const string vsKey = "searchCriteria"; //ViewState key
        public static string strtid = string.Empty;
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["tid"] != null && Request["tid"] != "")
                {
                    strtid = Common.Common.NoHtml(Request.QueryString["tid"]);
                    if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != ""
                        && Session["strLoginName"].ToString().ToLower().Trim() == "vyigo")
                    {
                        AspNetPager1.CurrentPageIndex = 1;
                        string s = "";
                        if (strtid != null && strtid != "")
                        {
                            s = " AND a.[TID] = '" + strtid + "' ";
                        }
                        ViewState[vsKey] = s;
                        LoadData(s);
                    }
                    else
                    {
                        return;
                    }
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
            if (strtid != null && strtid != "")
            {
                strWhere += " AND a.[TID] = '" + strtid + "' ";
            }
            if (txtName.Text.Trim() != null && txtName.Text.Trim() != "")
            {
                strWhere  += " AND a.[Ctext] LIKE '%" + txtName.Text + "%' ";
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
            MSForumCommentDAL commDal = new MSForumCommentDAL();
            DataSet ds = commDal.GetCommentList(strWhere);
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
                Label lb_linktreview = (Label)e.Item.FindControl("linktreview");
                if (lb_linktreview.ToolTip != null && lb_linktreview.ToolTip != "")
                {
                    if (lb_linktreview.ToolTip == "未通过" || lb_linktreview.ToolTip == "0")
                    {
                        lb_linktreview.Text = "<a onclick=\"AdminBizControl('wfmMSForumTopicmsgAdmin.aspx?action=pass&id=" +
                        lb_linktreview.Text + "');\">设置通过</a>";
                    }
                    else
                    {
                        lb_linktreview.Text = "<a onclick=\"AdminBizControl('wfmMSForumTopicmsgAdmin.aspx?action=pass&id=" +
                                               lb_linktreview.Text + "');\">锁定</a>";
                    }
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