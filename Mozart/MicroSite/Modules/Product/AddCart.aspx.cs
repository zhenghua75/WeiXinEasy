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
    public partial class AddCart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if ( CustomerSession.strCustomerID == null) 
            string strSiteCode = string.Empty;
            string strCustomerID = string.Empty;
            if (Session["strCustomerID"] != null)
            {
                strCustomerID = Session["strCustomerID"].ToString();
                strSiteCode = CustomerSession.strSiteCode;
                string strProductID = string.Empty;
                if (null == Request.QueryString["id"])
                {
                    return;
                }
                CartDAL dal = new CartDAL();
                strProductID = Common.Common.NoHtml(Request.QueryString["id"].ToString());
                DataSet dsMyProduct = dal.GetMyCartProduct(strSiteCode, strCustomerID, strProductID);
                if (null != dsMyProduct && dsMyProduct.Tables.Count > 0 && dsMyProduct.Tables[0].Rows.Count > 0)
                { 
                    Response.Redirect("MyCart.aspx?SiteCode=" + CustomerSession.strSiteCode + "&CustomerID=" + CustomerSession.strCustomerID, false);
                    return;
                }
                ProductDAL dalProduct = new ProductDAL();
                DataSet ds = dalProduct.GetProductDetail(strProductID);
                decimal dUintCost = decimal.Parse(ds.Tables[0].Rows[0]["MemberPrice"].ToString());
                SP_ShoppingCart modelAdd = new SP_ShoppingCart()
                {
                    ID = Guid.NewGuid().ToString("N").ToUpper(),
                    CustomerID = CustomerSession.strCustomerID,
                    ProductID = strProductID,
                    UnitCost = dUintCost,
                    Quantity = 1,
                    OrderTime = DateTime.Now,
                    SiteCode = GlobalSession.strSiteCode
                };
                if (dal.AddCartData(modelAdd))
                {
                    Response.Redirect("MyCart.aspx?SiteCode=" + CustomerSession.strSiteCode + "&CustomerID=" + CustomerSession.strCustomerID, false);
                }
            }
            else
            {
                Response.Redirect("Login.aspx?SiteCode='" + strSiteCode + "'", false);
            }
        }
    }
}