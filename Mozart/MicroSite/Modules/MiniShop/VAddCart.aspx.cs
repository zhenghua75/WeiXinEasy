using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.MicroSite
{
    public partial class VAddCart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strProductID = string.Empty;
            string strSiteCode = string.Empty;
            string strOpenID = string.Empty;
            if (null == Request.QueryString["id"])
            {
                return;
            }
            if (null == Request.QueryString["sitecode"])
            {
                return;
            }
            if (null == Request.QueryString["openid"])
            {
                return;
            }
            strProductID = Common.Common.NoHtml(Request.QueryString["id"].ToString());
            strSiteCode = Common.Common.NoHtml(Request.QueryString["sitecode"].ToString());
            if (Request.QueryString["openid"].ToString().Length > 29 || Request.QueryString["openid"].ToString().Length < 25)
            {
                return;
            }
            else
            {
                strOpenID = Request.QueryString["openid"].ToString();
            }
        }
    }
}