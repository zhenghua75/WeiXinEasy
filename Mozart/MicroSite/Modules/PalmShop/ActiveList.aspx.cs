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
using Model.CMS;
using Mozart.Common;
using WeiXinCore.Models;


namespace Mozart.PalmShop.ShopCode
{
    public partial class ActiveList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strSiteCode = string.Empty;
                string strOpenID = string.Empty;

                //if (null == Request["state"] || Request["state"] == "")
                if (null == Request.QueryString["state"])
                {
                    return;
                }
                else
                {
                    strSiteCode = Common.Common.NoHtml(Request.QueryString["state"].ToString());
                    Session["strSiteCode"] = strSiteCode;
                }

                string code = Request.QueryString["code"] as string;
                if (!string.IsNullOrEmpty(code))
                {
                    WXConfigDAL dal = new WXConfigDAL();
                    Model.WeiXin.WXConfig wxConfig = dal.GetWXConfigBySiteCode(strSiteCode);
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
                            strOpenID = oauth2AccessToken.OpenID;
                        }
                    }
                    else
                    {
                        strOpenID = code;
                    }
                }

                Session["OpenID"] = strOpenID;

                //取所有参加的活动列表
                List<MyCouponInfo> liCoupon = new List<MyCouponInfo>();
                CouponDAL dalCoup = new CouponDAL();
                DataSet ds = dalCoup.GetCouponInfoList(strSiteCode, strOpenID);

                if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        MyCouponInfo model = DataConvert.DataRowToModel<MyCouponInfo>(row);
                        liCoupon.Add(model);
                    }
                }

                string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/ActiveList.html"));
                JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
                context.TempData["coupon_list"] = liCoupon;
                context.TempData["OpenID"] = strOpenID;
                JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
                t.Render(Response.Output);
            }
        }
    }
}