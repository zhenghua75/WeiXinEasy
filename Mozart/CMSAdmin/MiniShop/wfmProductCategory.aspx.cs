using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.MiniShop;
using DAL.MiniShop;
using Mozart;

namespace Mozart.CMSAdmin.MiniShop
{
    public partial class wfmProductCategory : System.Web.UI.Page
    {
        const string vsKey = "searchCriteria"; //ViewState key
        static string sid = string.Empty;
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != "")
                {
                    if (Request["sid"] != null && Request["sid"] != "")
                    {
                        sid = " and [SID]=" + Request["sid"] + " ";
                    }
                    else
                    {
                        sid = " and [SID]!='' ";
                    }
                    ddlcategorylist.Items.Clear();
                    MSProductCategoryDAL categoryDal = new MSProductCategoryDAL();
                    DataSet ds = new DataSet();
                    ds = categoryDal.GetCategoryList(" and UpID='' " + sid);
                    DataTable dt = ds.Tables[0];
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["ID"] = "";
                    dr["Cname"] = "--全部--";
                    dt.Rows.InsertAt(dr, 0);
                    DataRow firstdr = ds.Tables[0].NewRow();
                    firstdr["ID"] = "0";
                    firstdr["Cname"] = "顶级导航";
                    dt.Rows.InsertAt(firstdr, 1);
                    ddlcategorylist.DataSource = ds.Tables[0].DefaultView;
                    ddlcategorylist.DataTextField = "Cname";
                    ddlcategorylist.DataValueField = "ID";
                    ddlcategorylist.DataBind();

                    AspNetPager1.CurrentPageIndex = 1;
                    string s = " and UpID='' " + sid;
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
            if (sid != null && sid != "")
            {
                strWhere += sid;
            }
            if (txtName.Text.Trim() != null && txtName.Text.Trim() != "")
            {
                strWhere += " AND [Cname] LIKE '%" + txtName.Text + "%' ";
            }
            string parm = string.Empty;
            if (ddlcategorylist.SelectedValue.Trim() != null && ddlcategorylist.SelectedValue.Trim() != ""
                && ddlcategorylist.SelectedValue.Trim() != "0")
            {
                parm = ddlcategorylist.SelectedValue;
            }
            else
            {
                parm = "";
            }
            strWhere = strWhere + " AND [UpID]='" + parm + "' ";
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
            MSProductCategoryDAL categoryDal = new MSProductCategoryDAL();
            DataSet ds = categoryDal.GetCategoryList(strWhere);
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