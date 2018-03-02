using Mozart.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.MiniShop;
using WeiXinCore.Models;
using DAL.WeiXin;

namespace Mozart.PalmShop.ShopCode
{
    public partial class ShopIndex : System.Web.UI.Page
    {
        string uid = string.Empty;
        string errowmsg = string.Empty;
        string sid = string.Empty;
        string strSiteCode = string.Empty;
        string strOpenID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                errowmsg = "";
                try
                {
                    if (Session["OpenID"] == null || Session["OpenID"].ToString() == "")
                    {
                        GetUserOpenID();
                    }
                    else
                    {
                        strOpenID = Session["OpenID"].ToString();
                    }
                    if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                    {
                        uid = Common.Common.NoHtml(Session["customerID"].ToString());
                        if (Session["SID"] == null || Session["SID"].ToString() == "")
                        {
                            GetShopID();
                            if (sid != null && sid != "")
                            {
                                JQDialog.SetCookies("pageurl", "ShopIndex.aspx", 5);
                                errowmsg = JQDialog.alertOKMsgBox(5, "您还没开通微店，请开通后再操作",
                                    "ApplyShop.aspx?action=reg", "error");
                            }
                        }
                    }
                    else
                    {
                        JQDialog.SetCookies("pageurl", "ShopIndex.aspx", 2);
                        errowmsg = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！",
                            "UserLogin.aspx", "error");
                    }
                }
                catch (Exception ex)
                {
                    errowmsg = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！",
                              "UserLogin.aspx", "error");
                }
                getHtmlpage();
            }
        }
        /// <summary>
        /// 获取用户OpenID
        /// </summary>
        void GetUserOpenID()
        {
            if (null == Request.QueryString["state"])
            {
                //return;
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
            if (strOpenID == null || strOpenID == "")
            {
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    MSCustomersDAL CustomerDal = new MSCustomersDAL();
                    try
                    {
                        strOpenID = CustomerDal.GetCustomerValueByID("OpenID", Session["customerID"].ToString()).ToString();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            if (strOpenID != null && strOpenID != "")
            {
                Session["OpenID"] = strOpenID;
            }
        }
        /// <summary>
        /// 获取店铺编号
        /// </summary>
        void GetShopID()
        {
            MSShopDAL shopDal = new MSShopDAL();
            if (uid != null && uid != "")
            {
                sid = shopDal.GetSidByUid("ID",uid).ToString();
            }
        }
        void getHtmlpage()
        {
            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/ShopIndex.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["errowmsg"] = errowmsg;

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}