using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.JC;
using DAL.JC;

namespace Mozart.CMSAdmin.JC
{
    public partial class wfmJCQuizList : System.Web.UI.Page
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
                starttime.Attributes.Add("onclick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})");
                Session.Remove("losewin");
                string s = "";
                if (Session["strRoleCode"].ToString() != "ADMIN")
                {
                    s = " SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
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
                strWhere = " SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            }
            if (!string.IsNullOrEmpty(this.txtName.Text) &&
                txtName.Text.Trim() != null &&txtName.Text.Trim()!="")
            {
                if (strWhere.Trim() != null && strWhere.Trim() != "")
                {
                    strWhere = strWhere + " AND ";
                }
                strWhere = strWhere + " [Name] LIKE '%" +txtName.Text + "%' ";
            }
            if (starttime.Text.Trim() != null && starttime.Text.Trim() != "")
            {
                if (strWhere.Trim() != null && strWhere.Trim() != "")
                {
                    strWhere = strWhere + " AND ";
                }
                strWhere = strWhere + " StartTime= '" + starttime.Text + "' ";
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
            JC_QuizDAL dal = new JC_QuizDAL();
            DataSet ds = dal.GetJCQuizDataList(strWhere);
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
                Label lb_HomeTeamImg = (Label)e.Item.FindControl("HomeTeamImg");
                Label lb_VisitingTeamImg = (Label)e.Item.FindControl("VisitingTeamImg");
                Label lb_losewin = (Label)e.Item.FindControl("ShowLoseWin");
                if (lb_HomeTeamImg.Text != null && lb_HomeTeamImg.Text != "")
                {
                    lb_HomeTeamImg.Text = "<img src=\"../../" + lb_HomeTeamImg.Text + 
                        "\" width=\"20px\" height=\"20px\" border=\"0px\">";
                }
                if (lb_VisitingTeamImg.Text != null && lb_VisitingTeamImg.Text != "")
                {
                    lb_VisitingTeamImg.Text = "<img src=\"../../" + lb_VisitingTeamImg.Text +
                        "\" width=\"20px\" height=\"20px\" border=\"0px\">";
                }
                if (lb_losewin.ToolTip.Trim() != null && lb_losewin.ToolTip.Trim() != "")
                {
                    lb_losewin.Text = "<a href=\"#\" onclick=\"Showlosewin('" + lb_losewin.Text + "');\">竞猜统计</a>&nbsp;|&nbsp;";
                }
                else
                {
                    lb_losewin.Text = ""; lb_losewin.ToolTip = "";
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