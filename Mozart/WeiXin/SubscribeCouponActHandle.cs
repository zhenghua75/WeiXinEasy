using DAL.ACT;
using Model.ACT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiXinCore.Models;
using WeiXinCore.Models.RequestMsgModels;
using WeiXinCore.Models.ResponseMsgModels;

namespace Mozart.WeiXin
{
    public class SubscribeCouponActHandle : IRequestMsgHandle
    {
        public SubscribeCouponActHandle()
        {
            Params = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Params { get; set; }

        public string Process(RequestMsgModel msg)
        {
            SubscribeEventRequestMsgModel msgMode = msg as SubscribeEventRequestMsgModel;
            if(msgMode!=null)
            {
                SiteActivityDAL dal = new SiteActivityDAL();
                SiteActivity activity = dal.GetSiteAct(Params["SiteCode"].ToString(), "Coupon");
                if (activity != null)
                {
                    CouponDAL cdal = new CouponDAL();
                    if (!cdal.ExistCoupon(Params["SiteCode"].ToString(), activity.ID,msgMode.FromUserName))
                    {
                        Coupon coupon = new Coupon()
                        {
                            SiteCode = Params["SiteCode"].ToString(),
                            SiteActivityID = activity.ID,
                            OpenID=msgMode.FromUserName,
                            CouponCode = GetCouponCode(msgMode.FromUserName),
                            CouponStatus = 0
                        };
                        cdal.InsertInfo(coupon);
                    }
                }
            }
            return string.Empty;
        }

        public string GetCouponCode(string openID)
        {
            return openID;
        }
    }
}