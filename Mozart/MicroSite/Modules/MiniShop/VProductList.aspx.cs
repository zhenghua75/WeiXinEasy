using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Mozart.Common;
using DAL.WeiXin;
using WeiXinCore.Models;

namespace Mozart.MicroSite
{
    public partial class VProductList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strSiteCode = string.Empty;
            string strOpenID = string.Empty;

            if (null == Request.QueryString["state"])
            {
                return;
            }
            else
            {
                strSiteCode = Common.Common.NoHtml(Request.QueryString["state"].ToString());
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

            strOpenID = code;
            List<Model.MiniShop.MSVProduct> liProduct = new List<Model.MiniShop.MSVProduct>();

            DAL.MiniShop.MSProductDAL dalProduct = new DAL.MiniShop.MSProductDAL();
            DataSet ds = dalProduct.GetProductListBySiteCode(strSiteCode);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Model.MiniShop.MSVProduct model = DataConvert.DataRowToModel<Model.MiniShop.MSVProduct>(row);
                liProduct.Add(model);
            }


            //读取模板内容 
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Default/VProductList.html"));

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["sitecode"] = strSiteCode;
            context.TempData["openid"] = strOpenID;
            context.TempData["product_list"] = liProduct;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}