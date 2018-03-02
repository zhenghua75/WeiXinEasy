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
using DAL.WeiXin;
using WeiXinCore.Models;

namespace Mozart.MicroSite
{
    public partial class ActiveList : BasePage
    {        
        protected override bool BeforeLoad()
        {            
            if (null == Request["state"] || Request["state"] == "")
            {
                return false;
            }
            else
            {
                SiteCode = Common.Common.NoHtml(Request.QueryString["state"].ToString());
            }
            string code = Request.QueryString["code"] as string;
            if (!string.IsNullOrEmpty(code))
            {
                WXConfigDAL dal = new WXConfigDAL();
                Model.WeiXin.WXConfig wxConfig = dal.GetWXConfigBySiteCode(SiteCode);
                if (wxConfig != null)
                {
                    WeiXinCore.Models.WeiXinConfig weixinConfig = new WeiXinCore.Models.WeiXinConfig()
                    {
                        ID = wxConfig.WXID,
                        Name = wxConfig.WXName,
                        Token = wxConfig.WXToken,
                        AppId = wxConfig.WXAppID,
                        AppSecret = wxConfig.WXAppSecret
                    };
                    WeiXinCore.WeiXin weixin = new WeiXinCore.WeiXin(weixinConfig);
                    Oauth2AccessToken oauth2AccessToken = weixin.GetOauth2AccessToken(code);
                    if (oauth2AccessToken != null)
                    {
                        OpenID = oauth2AccessToken.OpenID;
                    }
                }
            }

            Session["OpenID"] = OpenID;
            return base.BeforeLoad();
        }
        protected override void AddTempData(JinianNet.JNTemplate.TemplateContext context)
        {
            base.AddTempData(context);
            //取最新抢购的优惠活动
            List<Model.ACT.SiteActivity> liActive = new List<Model.ACT.SiteActivity>();
            DAL.ACT.SiteActivityDAL dalActive = new SiteActivityDAL();

            List<MyCouponInfo> liCoupon = new List<MyCouponInfo>();
            CouponDAL dalCoup = new CouponDAL();

            DataSet dsActive = dalActive.GetActivityList(" SiteCode = '" + SiteCode + "' AND ActStatus = 1 AND ActType = 'RushCoupon' AND StartTime < GETDATE() AND EndTime > GETDATE() ");
            if (null != dsActive && dsActive.Tables.Count > 0 && dsActive.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsActive.Tables[0].Rows)
                {
                    if (!dalCoup.ExistCoupon(SiteCode, row["ID"].ToString(), OpenID))
                    {
                        Model.ACT.SiteActivity modelActive = DataConvert.DataRowToModel<Model.ACT.SiteActivity>(row);
                        liActive.Add(modelActive);
                    }
                }
            }

            DataSet ds = dalCoup.GetCouponInfoList(SiteCode, OpenID);
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
                strInfo = "亲，你还没有参加过活动哦，多多关注我们的新活动！";
            }
            context.TempData["openid"] = OpenID;
            context.TempData["rushcoupon_list"] = liActive;
            context.TempData["coupon_list"] = liCoupon;
            context.TempData["couponinfo"] = strInfo;
        }
    }
}