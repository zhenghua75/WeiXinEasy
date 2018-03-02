using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.MiniShop;
using DAL.MiniShop;
using Mozart.Common;

namespace Mozart.PalmShop.ShopCode
{
    public partial class SearchProduct : System.Web.UI.Page
    {
        const string vsKey = "searchCriteria"; //ViewState key
        public static string errorscript = string.Empty;
        string like = string.Empty;
        string upid = string.Empty;
        string cid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["like"] != null && Request["like"] != "")
                {
                    AspNetPager1.CurrentPageIndex = 1;
                    like = Common.Common.NoHtml(Request["like"]);
                    like = " and a.ptitle like '%" + like + "%' ";
                    if (Request["cid"] != null && Request["cid"] != "")
                    {
                        cid = " and a.CID='" + Request["cid"] + "' ";
                    }
                    string where = string.Empty;
                    where = like +upid+cid;
                    ViewState[vsKey] = where;
                    errorscript = "";
                    LoadData(where);
                }
                else
                {
                    errorscript = JQDialog.alertOKMsgBoxGoBack(3, "操作失败<br/>您暂时没有权限访问！", true);
                }
            }
        }
        /// <summary>
        /// 获取二级列表
        /// </summary>
        void LoadData(string where)
        {
            MSProductCategoryDAL categoryDal = new MSProductCategoryDAL();
            MSProductDAL pruductDal = new MSProductDAL();
            DataSet ds = pruductDal.GetProductByTop(0,where);
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
        protected void AspNetPager1_PageChanged(object src, EventArgs e)
        {
            LoadData((string)ViewState[vsKey]);
        }
    }
}