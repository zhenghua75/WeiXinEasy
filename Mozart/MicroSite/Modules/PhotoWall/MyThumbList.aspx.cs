using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class MyThumbList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strSiteID = string.Empty;
            string strSiteCode = string.Empty;
            string strTitle = string.Empty;
            string strTheme = string.Empty;
            string strOpenID = string.Empty;
            if (null == Request.QueryString["openid"])
            {
                return;
            }
          
            if (Request.QueryString["openid"].ToString().Length > 29 || Request.QueryString["openid"].ToString().Length < 25)
            {
                strOpenID = Common.Common.NoHtml(Request.QueryString["openid"].ToString());
            }
            else
            {
                strOpenID = Request.QueryString["openid"].ToString();
            }
            if (null == Request.QueryString["sitecode"])
            {
                return;
            }

            strSiteCode = Common.Common.NoHtml(Request.QueryString["sitecode"].ToString());

            //取站点相册列表
            DAL.Album.UserPhotoDAL dalUserPhoto = new DAL.Album.UserPhotoDAL();
            DataSet dsThumbList = dalUserPhoto.GetMyPhoto(strSiteCode,strOpenID);
            List<Model.Album.UserPhoto> lstThumb = new List<Model.Album.UserPhoto>();
            foreach (DataRow row in dsThumbList.Tables[0].Rows)
            {
                Model.Album.UserPhoto model = DataConvert.DataRowToModel<Model.Album.UserPhoto>(row);
                lstThumb.Add(model);
            }


            //读取模板内容 
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/PhotoWall/MyThumbList.html"));

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["sitecode"] = strSiteCode;
            context.TempData["title"] = "我的照片";
            context.TempData["lstThumb"] = lstThumb;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}