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
    public partial class wfmPhotoSubmit : System.Web.UI.Page
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
                if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != "")
                {
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
                strWhere = " and [OrderNum] = '" + txtName.Text + "' ";
            }
            if (selectreview.SelectedValue != null && selectreview.SelectedValue != "")
            {
                strWhere = strWhere + " and Reivew=" + selectreview.SelectedValue + " ";
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
            MSPhotoSubmitDAL shopdal = new MSPhotoSubmitDAL();
            DataSet ds = shopdal.GetPhotoSubmitList(strWhere);
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
                Label img1 = (Label)e.Item.FindControl("img1");
                Label img2 = (Label)e.Item.FindControl("img2");
                Label labelbtn = (Label)e.Item.FindControl("labelbtn");
                if (img1.Text != null && img1.Text != "")
                {
                    img1.Text = "<img src=\"../../PalmShop/ShopCode/" + img1.Text +
                        "\" width=\"20px\" height=\"20px\" border=\"0px\">";
                }
                if (img2.Text != null && img2.Text != "")
                {
                    img2.Text = "<img src=\"../../PalmShop/ShopCode/" + img2.Text +
                        "\" width=\"20px\" height=\"20px\" border=\"0px\">";
                }
                if (labelbtn.Text == "未通过" || labelbtn.Text == "0")
                {
                    labelbtn.Text = "<a onclick=\"AdminBizControl('wfmPhotoSubmitAdmin.aspx?action=pass&id=" +
                    labelbtn.ToolTip + "');\">设置通过</a>";
                    labelbtn.ToolTip = "未通过";
                }
                else
                {
                    labelbtn.Text = "<a onclick=\"AdminBizControl('wfmPhotoSubmitAdmin.aspx?action=pass&id=" +
                                           labelbtn.ToolTip + "');\">锁定</a>";
                    labelbtn.ToolTip = "审核通过";
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