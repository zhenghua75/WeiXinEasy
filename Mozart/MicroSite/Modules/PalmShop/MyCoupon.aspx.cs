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
using WeiXinCore.Models;

namespace Mozart.PalmShop.ShopCode
{
    public partial class MyCoupon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strSiteCode = string.Empty;
                string strOpenID = string.Empty;

                //MyCoupon.aspx?sitecode=$sitecode&openid=$openid
                if (null == Request.QueryString["sitecode"])
                {
                    return;
                }
                else
                {
                    strSiteCode = Common.Common.NoHtml(Request.QueryString["sitecode"].ToString());
                    Session["strSiteCode"] = strSiteCode;
                }

                if (null == Request.QueryString["openid"])
                {
                    return;
                }
                else
                {
                    if (Request.QueryString["openid"].ToString().Length > 29 || Request.QueryString["openid"].ToString().Length < 25)
                    {
                        strOpenID = Common.Common.NoHtml(Request.QueryString["openid"].ToString());
                    }
                    else
                    {
                        strOpenID = Request.QueryString["openid"].ToString();                        
                    }
                    Session["openid"] = strOpenID;
                }

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

                string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/MyCoupon.html"));
                JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
                context.TempData["coupon_list"] = liCoupon;
                context.TempData["OpenID"] = strOpenID;
                JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
                t.Render(Response.Output);
            }
        }
    }
}