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

namespace Mozart.MicroSite
{
    public partial class CouponDetail : BasePage
    {        
        protected override bool BeforeLoad()
        {
            if (null == Request.QueryString["id"])
            {
                return false;
            }
            CouponID = Common.Common.NoHtml(Request.QueryString["id"].ToString());
            string strGuid = string.Empty;
            if (null != Request.QueryString["sitecode"] && null != Request.QueryString["openid"])
            {
                SiteCode = Common.Common.NoHtml(Request.QueryString["sitecode"].ToString());
                OpenID = Common.Common.NoHtml(Request.QueryString["openid"].ToString());
                //插入优惠券
                SiteActivityDAL dalActive = new SiteActivityDAL();
                DataSet dsActive = dalActive.GetActivityDetail(CouponID);
                if (null != dsActive && dsActive.Tables.Count > 0 && dsActive.Tables[0].Rows.Count > 0)
                {
                    strGuid = Guid.NewGuid().ToString("N");
                    CouponDAL cdal = new CouponDAL();
                    if (!cdal.ExistCoupon(SiteCode, CouponID, OpenID))
                    {
                        Coupon coupon = new Coupon()
                        {
                            ID = strGuid,
                            SiteCode = SiteCode,
                            SiteActivityID = CouponID,
                            OpenID = OpenID,
                            CouponStatus = 0
                        };
                        cdal.InsertInfo(coupon);
                    }
                }
                CouponID = strGuid;
            }
            else
            {
                SiteActivityDAL dalActive = new SiteActivityDAL();
                DataSet dsActive = dalActive.GetActivityDetail(CouponID);
                if (null != dsActive && dsActive.Tables.Count > 0 && dsActive.Tables[0].Rows.Count > 0)
                {
                    SiteCode = dsActive.Tables[0].Rows[0]["SIteCode"].ToString();
                }
            }
            return base.BeforeLoad();
        }
        protected override void AddTempData(JinianNet.JNTemplate.TemplateContext context)
        {
            CouponDAL dal = new CouponDAL();
            DataSet ds = dal.GetCouponInfo(CouponID);

            MyCouponInfo model = new MyCouponInfo();
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model = DataConvert.DataRowToModel<MyCouponInfo>(ds.Tables[0].Rows[0]);
            }

            this.SetQRCode(context, model.ID);
            context.TempData["pDetail"] = model;
            if (model.CouponStatus == "已经使用")
            {
                context.TempData["RemainDay"] = "";
            }
            else
            {
                if (int.Parse(model.RemainDay) > -1)
                {
                    context.TempData["RemainDay"] = "有效期：还剩" + model.RemainDay + "天";
                }
                else
                {
                    context.TempData["RemainDay"] = "有效期：此券已经过期！";
                }
            }
        }
    }
}