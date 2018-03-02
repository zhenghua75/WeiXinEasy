using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Product;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class ChangeCart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strProductID = Common.Common.NoHtml(Request.QueryString["productid"].ToString());
            string strFlag = Common.Common.NoHtml(Request.QueryString["jj"].ToString());
            string strRuturn = string.Empty;
            CartDAL dal = new CartDAL();
            if (strFlag == "0")
            {
                strRuturn = dal.UpdateCatList(CustomerSession.strSiteCode, CustomerSession.strCustomerID, strProductID, "-");
            }
            else
            {
                strRuturn = dal.UpdateCatList(CustomerSession.strSiteCode, CustomerSession.strCustomerID, strProductID, "+");
            }

            //返回说明 当前修改商品的数量|总种数|总金额|总件数
            //Response.Write("4|3|5|600");
            Response.Write(strRuturn);
        }
    }
}