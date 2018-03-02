using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.CMSAdmin.Product
{
    public partial class wfmProductList : System.Web.UI.Page
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
                this.ddlCategory.Items.Clear();
                DAL.Product.CategoryDAL dal = new DAL.Product.CategoryDAL();
                DataSet ds = new DataSet();
                if (Session["strRoleCode"].ToString() == "ADMIN")
                {
                    ds = dal.GetSPCategory("");
                }
                else
                {
                    ds = dal.GetSPCategory(" SiteCode ='" + Session["strSiteCode"].ToString() + "' ");
                }
                DataTable dt = ds.Tables[0];

                DataRow dr = ds.Tables[0].NewRow();
                dr["ID"] = "0";
                dr["Name"] = "--全部--";
                dt.Rows.InsertAt(dr, 0);

                this.ddlCategory.DataSource = ds.Tables[0].DefaultView;
                this.ddlCategory.DataTextField = "Name";
                this.ddlCategory.DataValueField = "ID";
                this.ddlCategory.DataBind();

                AspNetPager1.CurrentPageIndex = 1;
                string s = "";
                if (Session["strRoleCode"].ToString() != "ADMIN")
                {
                    s = " a.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
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
            string strWhere = " a.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            if (!string.IsNullOrEmpty(this.txtTitle.Text))
            {
                strWhere = strWhere + " AND a.Name LIKE '%" + Common.Common.NoHtml(this.txtTitle.Text) + "%'";
            }
            if (ddlCategory.SelectedValue != "0")
            {
                strWhere = strWhere + " AND a.CatID = '" + ddlCategory.SelectedValue + "' ";
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
            DAL.Product.ProductDAL dal = new DAL.Product.ProductDAL();
            DataSet ds = dal.GetProductList(strWhere);
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