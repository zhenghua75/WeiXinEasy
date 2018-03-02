using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.WeiXin;
using Mozart.Common;
using WeiXinCore.Models;

namespace Mozart.MicroSite
{
    public partial class PhotoList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strSiteID = string.Empty;
            string strSiteCode = string.Empty;
            string strTitle = string.Empty;
            string strTheme = string.Empty;
            string strAlbumTypeID = string.Empty;
            string strOpenID = string.Empty;
            //if (null == Request.QueryString["ID"])
            //{
            //    return;
            //}
            //if (null == Request.QueryString["OpenID"])
            //{
            //    return;
            //}
            //strSiteCode = Common.Common.NoHtml(Request.QueryString["ID"].ToString());
            //if (Request.QueryString["openid"].ToString().Length > 29 || Request.QueryString["openid"].ToString().Length < 25)
            //{
            //    strOpenID = Common.Common.NoHtml(Request.QueryString["openid"].ToString());
            //}
            //else
            //{
            //    strOpenID = Request.QueryString["openid"].ToString();
            //}




            if (null == Request["state"] || Request["state"] == "")
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
            }



            Session["OpenID"] = strOpenID;
            //取站点信息
            DAL.SYS.AccountDAL dalAccount = new DAL.SYS.AccountDAL();
            DAL.Album.PhotoList dalPhotoList = new DAL.Album.PhotoList();

            DataSet dsAccount = dalAccount.GetAExtDataBySiteCode(strSiteCode);

            if (null != dsAccount && dsAccount.Tables.Count > 0 && dsAccount.Tables[0].Rows.Count > 0)
            {
                strTheme = dsAccount.Tables[0].Rows[0]["Themes"].ToString();
                strTitle = dsAccount.Tables[0].Rows[0]["Name"].ToString();
                strSiteCode = dsAccount.Tables[0].Rows[0]["SiteCode"].ToString();
                strSiteID = dsAccount.Tables[0].Rows[0]["ID"].ToString();
                Session["strSiteCode"] = dsAccount.Tables[0].Rows[0]["SiteCode"].ToString();                
            }

            //取站点相册列表
            DataSet dsPhotoList = dalPhotoList.GetPhotoList(strSiteCode);
            List<Model.Album.PhotoList> liPhotoList = new List<Model.Album.PhotoList>();
            if (null != dsPhotoList && dsPhotoList.Tables.Count > 0 && dsPhotoList.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsPhotoList.Tables[0].Rows)
                {
                    Model.Album.PhotoList model = DataConvert.DataRowToModel<Model.Album.PhotoList>(row);
                    liPhotoList.Add(model);
                }
            }

            //读取模板内容
            string text = string.Empty;
            if (!File.Exists(Server.MapPath("Themes/" + strTheme + "/PhotoWall.html")))
            {
                text = System.IO.File.ReadAllText(Server.MapPath("Themes/PhotoWall/PhotoWall.html"));
            }
            else
            {
                text = System.IO.File.ReadAllText(Server.MapPath("Themes/" + strTheme + "/PhotoWall.html"));
            }


            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["title"] = strTitle;
            context.TempData["siteid"] = strSiteID;
            context.TempData["footer"] = "奥琦微商易";
            context.TempData["sitecode"] = strSiteCode;
            context.TempData["AlbumTypelist"] = liPhotoList;

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}