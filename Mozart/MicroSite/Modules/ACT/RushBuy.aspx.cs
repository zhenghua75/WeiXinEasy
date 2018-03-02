using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.ACT;
using DAL.WeiXin;
using Model.ACT;
using Mozart.Common;
using Newtonsoft.Json;
using WeiXinCore;
namespace Mozart.MicroSite
{
    public partial class RushBuy : BasePage
    {
        private string strReHtml = string.Empty;
        private string strMessage = string.Empty;
        protected override bool BeforeLoad()
        {
            string strGuid = string.Empty;
            

            if (null != Request.QueryString["sitecode"] && null != Request.QueryString["openid"] && null != Request.QueryString["couponid"])
            {
                CouponID = Common.Common.NoHtml(Request.QueryString["couponid"].ToString());
                SiteCode = Common.Common.NoHtml(Request.QueryString["sitecode"].ToString());

                if (Request.QueryString["sitecode"].ToString().Length > 6)
                {
                    return false;
                }

                if (Request.QueryString["couponid"].ToString().Length != 32)
                {
                    return false;
                }

                if (Request.QueryString["openid"].ToString().Length > 29 || Request.QueryString["openid"].ToString().Length < 25)
                {
                    return false;
                }
                else
                {
                    OpenID = Request.QueryString["openid"].ToString();
                }

                //插入优惠券                
                SiteActivityDAL dalActive = new SiteActivityDAL();
                DataSet dsActive = dalActive.GetActivityDetail(CouponID);
                if (null != dsActive && dsActive.Tables.Count > 0 && dsActive.Tables[0].Rows.Count > 0)
                {
                    strGuid = Guid.NewGuid().ToString("N");
                    CouponDAL cdal = new CouponDAL();
                    if (!cdal.ExistCoupon(SiteCode, CouponID, OpenID))
                    {
                        //判断是否可以参加
                        //取当前已经参加的人数
                        int iCount = 10000000;
                        if (null != dsActive.Tables[0].Rows[0]["DayLimit"].ToString())
                        {
                            string strCount = "SiteCode = '" + SiteCode + "' AND SiteActivityID = '" + CouponID + "' AND CONVERT(VARCHAR(10),AddTime,120) = CONVERT(VARCHAR(10),GETDATE(),120)";
                            iCount = cdal.GetCouponCount(strCount);
                        }
                        string strStartTime = dsActive.Tables[0].Rows[0]["StartTime"].ToString();
                        string strEndTime = dsActive.Tables[0].Rows[0]["EndTime"].ToString();
                        string strOpenTime = dsActive.Tables[0].Rows[0]["OpenTime"].ToString();
                        string strCloseTime = dsActive.Tables[0].Rows[0]["CloseTime"].ToString();
                        string strDayLimit = dsActive.Tables[0].Rows[0]["DayLimit"].ToString();

                        if (DateTime.Now >= Convert.ToDateTime(strStartTime)
                            && DateTime.Now < Convert.ToDateTime(strEndTime)
                            && DateTime.Now.Hour >= int.Parse(strOpenTime)
                            && DateTime.Now.Hour < int.Parse(strCloseTime)
                            )
                        {
                            if (iCount < int.Parse(strDayLimit))
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
                                strReHtml = "RushBuyOK.html";
                            }
                            else
                            {
                                strReHtml = "RushBuyOver.html";
                                strMessage = "抱歉！优惠券已经被抢完，请关注下期活动！";
                            }
                        }
                        else
                        {
                            strReHtml = "RushBuyOver.html";
                            strMessage = "抱歉！近期活动未开始或者已经结束，请关注公方微信优惠消息！";
                        }
                    }
                    else
                    {
                        strReHtml = "RushBuyOver.html";
                        strMessage = "抱歉！您已经参与了活动，请把机会留给其余的顾客。";
                    }
                }
                CouponID = strGuid;
            }
            else
            {
                Response.Write("<script>window.opener=null;window.close();</script>");
                return false;
            }
            return base.BeforeLoad();
        }
        protected override void SetTemplatePath()
        {
            TemplatePath = string.Format(CurrentModule.Theme, "/MicroSite/Themes/" + Theme + "/",strReHtml);
        }
        protected override void AddTempData(JinianNet.JNTemplate.TemplateContext context)
        {
            base.AddTempData(context);
            CouponDAL dal = new CouponDAL();
            DataSet ds = dal.GetCouponInfo(CouponID);

            MyCouponInfo model = new MyCouponInfo();
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model = DataConvert.DataRowToModel<MyCouponInfo>(ds.Tables[0].Rows[0]);
            }

            //读取模板内容

            //string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Default/" + strReHtml));
            //JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            if (strReHtml == "RushBuyOK.html" || strReHtml == "QJTVRushBuyOK.html")
            {
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
            else
            {
                #region 消费完成发生消息
                string strAppID = string.Empty;
                string strSecret = string.Empty;
                Model.WeiXin.WXConfig wc = new Model.WeiXin.WXConfig();
                WXConfigDAL wcdal = new WXConfigDAL();
                wc = wcdal.GetWXConfigBySiteCode(SiteCode);
                if (null != wc)
                {
                    strAppID = wc.WXAppID;
                    strSecret = wc.WXAppSecret;
                }
                string strToken = WeiXinHelper.GetAccessToken(strAppID, strSecret);

                var KeyToken = new { access_token = "" };
                var b = JsonConvert.DeserializeAnonymousType(strToken, KeyToken);
                string strRToken = b.access_token;

                WeiXinHelper.SendCustomTextMessage(strRToken, OpenID, strMessage);
                #endregion
                //Response.Write("<script>document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {	WeixinJSBridge.call('closeWindow');});</script>");
                //Response.Redirect("MyCoupon.aspx?SiteCode=" + strSiteCode + "&OpenID=" + strOpenID);
                //return;
            }
            context.TempData["strmsg"] = strMessage;
            context.TempData["OpenID"] = OpenID;
        }
    }
}