using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.ACT;
using Mozart.Common;
using Model.ACT;


namespace Mozart.MicroSite
{
    public partial class MyCoupon : BasePage
    {
        
        protected override bool BeforeLoad()
        {
            if (null == Request.QueryString["sitecode"])
            {
                return false;
            }
            if (null == Request.QueryString["openid"])
            {
                return false;
            }

            SiteCode = Common.Common.NoHtml(Request.QueryString["sitecode"].ToString());
            OpenID = string.Empty;
            if (Request.QueryString["openid"].ToString().Length > 29 || Request.QueryString["openid"].ToString().Length < 25)
            {
                OpenID = Common.Common.NoHtml(Request.QueryString["openid"].ToString());
            }
            else
            {
                OpenID = Request.QueryString["openid"].ToString();
            }
            return base.BeforeLoad();
        }
        protected override void AddTempData(JinianNet.JNTemplate.TemplateContext context)
        {
            base.AddTempData(context);
            //取最新抢购的优惠活动
            List<Model.ACT.SiteActivity> liActive = new List<Model.ACT.SiteActivity>();
            DAL.ACT.SiteActivityDAL dalActive = new SiteActivityDAL();

            List<MyCouponInfo> liCoupon = new List<MyCouponInfo>();
            CouponDAL dal = new CouponDAL();

            DataSet dsActive = dalActive.GetActivityList(" SiteCode = '" + SiteCode + "' AND ActStatus = 1 AND ActType = 'RushCoupon' AND StartTime < GETDATE() AND EndTime > GETDATE() ");
            if (null != dsActive && dsActive.Tables.Count > 0 && dsActive.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsActive.Tables[0].Rows)
                {
                    if (!dal.ExistCoupon(SiteCode, row["ID"].ToString(), OpenID))
                    {
                        Model.ACT.SiteActivity modelActive = DataConvert.DataRowToModel<Model.ACT.SiteActivity>(row);
                        liActive.Add(modelActive);
                    }
                }
            }

            DataSet ds = dal.GetCouponInfoList(SiteCode, OpenID);
            string strInfo = string.Empty;
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    MyCouponInfo model = DataConvert.DataRowToModel<MyCouponInfo>(row);
                    liCoupon.Add(model);
                }
            }
            else
            {
                strInfo = "亲，你还没有优惠券哦，多多关注我们的新活动！";
            }
            context.TempData["openid"] = OpenID;
            context.TempData["rushcoupon_list"] = liActive;
            context.TempData["coupon_list"] = liCoupon;
            context.TempData["couponinfo"] = strInfo;
        }
    }
}