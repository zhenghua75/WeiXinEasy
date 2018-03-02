using Mozart.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.CMSAdmin.Coupon
{
    public partial class wfmCouponList : System.Web.UI.Page
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
                startTime.Attributes.Add("onclick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})");
                endTime.Attributes.Add("onclick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})");
                string s = "";
                if (Session["strRoleCode"].ToString() != "ADMIN")
                {
                    s = " a.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
                }



                ddlsiteactive.Items.Clear();
                DAL.ACT.SiteActivityDAL actdal = new DAL.ACT.SiteActivityDAL();
                DataSet ds = new DataSet();
                if (Session["strRoleCode"].ToString() == "ADMIN")
                {
                    ds = actdal.GeteffecitvActivity("");
                }
                else
                {
                    ds = actdal.GeteffecitvActivity(" SiteCode ='" + Session["strSiteCode"].ToString() + "' ");
                }
                DataTable dt = ds.Tables[0];

                DataRow dr = ds.Tables[0].NewRow();
                dr["ID"] = "0";
                dr["ActTitle"] = "--全部--";
                dt.Rows.InsertAt(dr, 0);

                ddlsiteactive.DataSource = ds.Tables[0].DefaultView;
                ddlsiteactive.DataTextField = "ActTitle";
                ddlsiteactive.DataValueField = "ID";
                ddlsiteactive.DataBind();


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
            string strWhere = "";
            if (Session["strRoleCode"].ToString() != "ADMIN")
            {
               strWhere= " a.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            }
            if (!string.IsNullOrEmpty(this.txtTitle.Text) && txtTitle.Text.Trim()!="")
            {
                if (strWhere.Trim() != null && strWhere.Trim() != "")
                {
                    strWhere = strWhere + " AND ";
                }
                strWhere = strWhere + "  b.ActTitle like '%" + this.txtTitle.Text + "%'";
            }
            switch (ddlsiteactive.SelectedValue)
            {
                case "0":
                    break;
                default:
                    if (strWhere.Trim() != null && strWhere.Trim() != "")
                    {
                        strWhere = strWhere + " AND ";
                    }
                    strWhere = strWhere + "  a.SiteActivityID ='" + ddlsiteactive.SelectedValue + "'";
                    break;
            }
            switch (ddlCategory.SelectedValue)
            {
                case "0":
                    break;
                case "1":
                    if (strWhere.Trim() != null && strWhere.Trim() != "")
                    {
                        strWhere = strWhere + " AND ";
                    }
                    strWhere = strWhere + "  a.CouponStatus = 0 ";
                    break;
                case "2":
                    if (strWhere.Trim() != null && strWhere.Trim() != "")
                    {
                        strWhere = strWhere + " AND ";
                    }
                    strWhere = strWhere + "  a.CouponStatus > 0 ";
                    break;
                default:
                    break;
            }
            if (startTime.Text.Trim() != null && startTime.Text.Trim() != "")
            {
                if (strWhere.Trim() != null && strWhere.Trim() != "")
                {
                    strWhere = strWhere + " AND ";
                }
                strWhere = strWhere + "   ISNULL(a.CheckTime,'')<>'' ";
                strWhere = strWhere + " AND a.CheckTime between '"+startTime.Text+"' ";
                if (endTime.Text.Trim() != null && endTime.Text.Trim() != "")
                {
                    strWhere = strWhere + " AND '" + endTime.Text + "' ";
                }
                else
                {
                    MessageBox.Show(this, "请输入结束时间后再操作！");
                    return;
                }
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
            DAL.ACT.CouponDAL dal = new DAL.ACT.CouponDAL();
            DataSet ds = dal.GetSiteCouponList(strWhere);
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