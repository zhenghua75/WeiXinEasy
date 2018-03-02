using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.MicroSite.Themes.PhotoList.ajax
{
    public partial class thumbsOK : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strThumbID = string.Empty;
            string strOpenID = string.Empty;
            string strSiteCode = string.Empty;
            if (null == Request.QueryString["thumb"])
            {
                return;
            }
            strThumbID = Common.Common.NoHtml(Request.QueryString["thumb"].ToString());
            if (null == Session["OpenID"] || string.IsNullOrEmpty(Session["OpenID"].ToString()))
            {
                return;
            }
            strOpenID = Session["OpenID"].ToString();
            if (null == Session["strSiteCode"] || string.IsNullOrEmpty(Session["strSiteCode"].ToString()))
            {
                return;
            }
            strSiteCode = Session["strSiteCode"].ToString();
            //点赞功能
            DAL.Album.GreatuserPhotoDAL dal = new DAL.Album.GreatuserPhotoDAL();
            if (!dal.ExistGreatUserPhoto(strSiteCode, strThumbID, strOpenID))
            {
                Model.Album.GreatUserPhoto model = new Model.Album.GreatUserPhoto()
                {
                    SiteCode = strSiteCode,
                    OpenId = strOpenID,
                    ThumbID = strThumbID
                };

                dal.Insert(model);
                string strReturn = "点赞完成，谢谢参与！";
                Response.Write(strReturn);
            }
            else
            {
                string strReturn = "这张照片你已经点赞！";
                Response.Write(strReturn);
            }
        }
    }
}