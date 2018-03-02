using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.WeiXin;
using WeiXinCore.Models;

namespace Mozart.PalmShop.ShopCode
{
    public partial class ChargeFee : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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

            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/ChargeFee.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            context.TempData["OpenID"] = strOpenID;
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}