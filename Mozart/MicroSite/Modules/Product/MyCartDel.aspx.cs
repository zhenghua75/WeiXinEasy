using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Product;
using Model.SP;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class MyCartDel : System.Web.UI.Page
    {
        string strID = string.Empty;
        string strSiteCode = string.Empty;
        string strCustomerID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (null == Request.QueryString["id"])
            {
                return;
            }
            strID = Common.Common.NoHtml(Request.QueryString["id"].ToString());

            CartDAL dal = new CartDAL();

            DataSet ds = dal.GetMyCartProduct(strID);

            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                strSiteCode = ds.Tables[0].Rows[0]["SiteCode"].ToString();
                strCustomerID = ds.Tables[0].Rows[0]["CustomerID"].ToString();
                dal.DelMyCartProduct(strID);
                Response.Redirect("MyCart.aspx?SiteCode=" + strSiteCode + "&CustomerID=" + strCustomerID, false);
            }
        }
    }
}