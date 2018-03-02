using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.RC;
using Model.RC;

namespace Mozart.CMSAdmin.RC
{
    public partial class wfmRaceList : System.Web.UI.Page
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
                endtime.Attributes.Add("onclick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})");
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
                txtName.Text.Trim() != null && txtName.Text.Trim() != "")
            {
                if (strWhere.Trim() != null && strWhere.Trim() != "")
                {
                    strWhere = strWhere + " AND ";
                }
                strWhere = strWhere + " [Rtitle] LIKE '%" + txtName.Text + "%' ";
            }
            if (starttime.Text.Trim() != null && starttime.Text.Trim() != "")
            {
                if (strWhere.Trim() != null && strWhere.Trim() != "")
                {
                    strWhere = strWhere + " AND ";
                }
                strWhere = strWhere + " StartTime= '" + starttime.Text + "' ";
            }
            if (endtime.Text.Trim() != null && endtime.Text.Trim() != "")
            {
                if (strWhere.Trim() != null && strWhere.Trim() != "")
                {
                    strWhere = strWhere + " AND ";
                }
                strWhere = strWhere + " EndTime= '" + endtime.Text + "' ";
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
            RC_RaceDAL dal = new RC_RaceDAL();
            DataSet ds = dal.GetRCRList(strWhere);
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