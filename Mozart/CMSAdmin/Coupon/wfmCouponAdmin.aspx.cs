using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.CMSAdmin.Coupon
{
    public partial class wfmCouponAdmin : System.Web.UI.Page
    {
        static string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (null != Common.Common.NoHtml(Request.QueryString["action"]))
            {
                strAction = Common.Common.NoHtml(Request.QueryString["action"]);
            }
            if (null != Common.Common.NoHtml(Request.QueryString["id"]))
            {
                strID = Common.Common.NoHtml(Request.QueryString["id"]);
            }

            DAL.ACT.CouponDAL dal = new DAL.ACT.CouponDAL();

            if (dal.UpdateCouponState(strID,strAction))
            {
                strMessage = "优惠券状态处理成功！";
            }
            else
            {
                strMessage = "优惠券状态处理失败！";
            }

            Response.Write(strMessage);
            Response.End();
        }
    }
}