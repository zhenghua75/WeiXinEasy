using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.MiniShop;
using Model.MiniShop;
using Mozart.Common;

namespace Mozart.PalmShop.ShopCode
{
    public partial class SecHandList : System.Web.UI.Page
    {
        const string vsKey = "searchCriteria"; //ViewState key
        string strCID = string.Empty; string like = string.Empty;
        public static string cname = string.Empty;
        public static string errorscript = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["scid"] != null && Request["scid"] != "")
                {
                    AspNetPager1.CurrentPageIndex = 1;
                    strCID = Common.Common.NoHtml(Request["scid"]);
                    strCID = " and a.CID='" + strCID + "' ";
                    if (Request["like"] != null && Request["like"] != "")
                    {
                        like = " and a.ptitle like '%" + like + "%' ";
                    }
                    string where = string.Empty;
                    where = strCID + like;
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
            cname = Common.Common.NoHtml(Request["scid"]);
            try
            {
                cname = categoryDal.GetMSPCategoryValueByID("Cname",cname).ToString();
            }
            catch (Exception)
            {
            }
            
            MSProductDAL pruductDal = new MSProductDAL();
            DataSet ds = pruductDal.GetSecHandProduct(where);
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