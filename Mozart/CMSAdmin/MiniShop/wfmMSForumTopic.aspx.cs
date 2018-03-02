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
    public partial class wfmMSForumTopic : System.Web.UI.Page
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
                if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != ""
                    && Session["strLoginName"].ToString().ToLower().Trim() == "vyigo")
                {
                    ddlforumlist.Items.Clear();
                    MSForumSetDAL ForumListDal = new MSForumSetDAL();
                    DataSet ds = new DataSet();
                    ds = ForumListDal.GetMSForumSetList("");
                    DataTable dt = ds.Tables[0];
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["ID"] = "";
                    dr["FTitle"] = "--请选择相关主题--";
                    dt.Rows.InsertAt(dr, 0);
                    ddlforumlist.DataSource = ds.Tables[0].DefaultView;
                    ddlforumlist.DataTextField = "FTitle";
                    ddlforumlist.DataValueField = "ID";
                    ddlforumlist.DataBind();
                    AspNetPager1.CurrentPageIndex = 1;
                    string s = "";
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
            if (txtName.Text.Trim() != null && txtName.Text.Trim() != "")
            {
                strWhere = " AND a.[TopicTitle] LIKE '%" + txtName.Text + "%' ";
            }
            if (ddlforumlist.SelectedValue != null && ddlforumlist.SelectedValue != "")
            {
                strWhere = " AND a.[FID] ='" + ddlforumlist.SelectedValue + "' ";
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
            MSForumTopicDAL TopicDal = new MSForumTopicDAL();
            DataSet ds = TopicDal.GetMSForumTopicList(strWhere);
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
                        lb_linktreview.Text = "<a onclick=\"AdminBizControl('wfmMSForumTopicAdmin.aspx?action=pass&id=" +
                        lb_linktreview.Text + "');\">设置通过</a>";
                    }
                    else
                    {
                        lb_linktreview.Text = "<a onclick=\"AdminBizControl('wfmMSForumTopicAdmin.aspx?action=pass&id=" +
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