using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.Vote;
using DAL.Vote;

namespace Mozart.CMSAdmin.Vote
{
    public partial class wfmOptionList : System.Web.UI.Page
    {
        const string vsKey = "searchCriteria"; //ViewState key
        static string strID = string.Empty;
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
                    s = " and b.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
                }
                if (null != Common.Common.NoHtml(Request.QueryString["id"]))
                    {
                        strID = Common.Common.NoHtml(Request.QueryString["id"]);
                        s=s+" and SubjectID='"+strID+"' ";
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
            string strWhere ="";
            if (Session["strRoleCode"].ToString() != "ADMIN")
            {
            strWhere=" and b.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            }
            if (txtTitle.Text.Trim()!=null&&txtTitle.Text.Trim()!="")
            {
                strWhere = strWhere + " AND  [Title] LIKE '%" + this.txtTitle.Text + "%' ";
            }
            if(strID.Trim()!=null&&strID.Trim()!="")
            {
                strWhere=strWhere+" and SubjectID='"+strID+"' ";
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
            OptionDAL dal=new OptionDAL ();
            DataSet ds = dal.GetOptionDataList(strWhere);
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