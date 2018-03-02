using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.MiniShop;
using DAL.WeiXin;
using WeiXinCore.Models;

namespace Mozart.PalmShop.ShopCode
{
    public partial class VServiceCentre : System.Web.UI.Page
    {
        string strOpenID = string.Empty;
        string strSiteCode = string.Empty;
        string errormsg = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["OpenID"] == null || Session["OpenID"].ToString() == "")
                {
                    GetUserOpenID();
                }
                else
                {
                    strOpenID = Session["OpenID"].ToString();
                }
                GetInfo();
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
            if (strOpenID != null && strOpenID != "")
            {
                Session["OpenID"] = strOpenID;
            }
        }
        void GetInfo()
        {
            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/VServiceCentre.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["errormsg"] = errormsg;
            context.TempData["sitecode"] = strSiteCode;
            context.TempData["openid"] = strOpenID;
            context.TempData["rnd"] = DateTime.Now.ToFileTime().ToString();

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}