using Mozart.Payment.Alipay.WapPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.PalmShop.ShopCode
{
    public partial class OrderSubmit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string pname = string.Empty; string countcost = string.Empty;
                string oid = string.Empty; string uid = string.Empty;
                string payinfo = string.Empty;
                #region 订单付款处理
                if (Request["pname"] == null || Request["pname"] == "")
                {
                    return;
                }
                else
                {
                    pname = Common.Common.NoHtml(Request["pname"]);
                }
                if (Request["oid"] == null || Request["oid"] == "")
                {
                    return;
                }
                else
                {
                    oid = Common.Common.NoHtml(Request["oid"]);
                }
                if (Request["countcost"] == null || Request["countcost"] == "")
                {
                    return;
                }
                else
                {
                    countcost = Request["countcost"];
                }
                if (Request["uid"] == null || Request["uid"] == "")
                {
                    return;
                }
                else
                {
                    uid = Common.Common.NoHtml(Request["uid"]);
                }
                #endregion
                payinfo = WapPayHelper.BuildRequest(pname, oid, countcost, uid);
                Response.Write(payinfo);
            }
        }
    }
}