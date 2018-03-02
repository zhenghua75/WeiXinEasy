using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.ACT;
using Model.ACT;
using Mozart.Common;


namespace Mozart.WebService
{
    public partial class QR : System.Web.UI.Page
    {
        string strCouponInfo = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string strCouponID = string.Empty;
            string strSiteCode = string.Empty;
            string strAction = string.Empty;

            if (null == Request.QueryString["id"])
            {
                return;
            }
            if (null != Request.QueryString["action"])
            {
                strCouponID = Common.Common.NoHtml(Request.QueryString["id"].ToString());
                strAction = Common.Common.NoHtml(Request.QueryString["action"].ToString());
                if (strAction == "checkout")
                {
                    CheckOutCoupon(strCouponID);
                }
            }
            strCouponID = Common.Common.NoHtml(Request.QueryString["id"].ToString());
            CouponDAL dal = new CouponDAL();
            DataSet ds = dal.GetCouponInfo(strCouponID);

            MyCouponInfo model = new MyCouponInfo();
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model = DataConvert.DataRowToModel<MyCouponInfo>(ds.Tables[0].Rows[0]);
            }

            string text = System.IO.File.ReadAllText(Server.MapPath("QRNotUse.html"));
            //读取模板内容 
            if (model.CouponStatus == "未使用")
            {
                text = System.IO.File.ReadAllText(Server.MapPath("QRNotUse.html"));
            }
            else
            {
                text = System.IO.File.ReadAllText(Server.MapPath("QRIsUse.html"));
            }
            
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            context.TempData["title"] = "奥琦微商易优惠券";
            context.TempData["note"] = model.ActTitle;
            context.TempData["id"] = strCouponID;
            context.TempData["content"] = model.Remark;
            context.TempData["errinfo"] = strCouponInfo;
            context.TempData["footer"] = "奥琦微商易";
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }

        public void CheckOutCoupon(string strCouponID)
        {
            CouponDAL dal = new CouponDAL();
            if (dal.UpdateCouponState(strCouponID))
            {
                strCouponInfo = "优惠券已经提交使用。";
            }
        }
    }
}