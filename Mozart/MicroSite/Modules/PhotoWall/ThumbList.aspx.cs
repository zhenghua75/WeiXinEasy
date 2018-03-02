using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class ThumbList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strSiteID = string.Empty;
            string strSiteCode = string.Empty;
            string strTitle = string.Empty;
            string strTheme = string.Empty;
            string strAlbumTypeID = string.Empty;
            if (null == Request.QueryString["album"])
            {
                return;
            }
            strAlbumTypeID = Common.Common.NoHtml(Request.QueryString["album"].ToString());

            //取站点信息
            DAL.SYS.AccountDAL dalAccount = new DAL.SYS.AccountDAL();
            DAL.Album.PhotoWallDAL dalPhotoWall = new DAL.Album.PhotoWallDAL();

            DataSet dsAccount = dalPhotoWall.GetAccountData(strAlbumTypeID);

            if (null != dsAccount && dsAccount.Tables.Count > 0 && dsAccount.Tables[0].Rows.Count > 0)
            {
                strTheme = dsAccount.Tables[0].Rows[0]["Themes"].ToString();
                strTitle = dsAccount.Tables[0].Rows[0]["Name"].ToString();
                strSiteCode = dsAccount.Tables[0].Rows[0]["SiteCode"].ToString();
                strSiteID = dsAccount.Tables[0].Rows[0]["ID"].ToString();
                Session["strSiteCode"] = dsAccount.Tables[0].Rows[0]["SiteCode"].ToString();
            }

            //取站点相册列表
            DataSet dsThumbList = dalPhotoWall.GetPhotoWall(strAlbumTypeID);
            List<Model.Album.PhotoWall> lstThumb = new List<Model.Album.PhotoWall>();
            foreach (DataRow row in dsThumbList.Tables[0].Rows)
            {
                Model.Album.PhotoWall model = DataConvert.DataRowToModel<Model.Album.PhotoWall>(row);
                lstThumb.Add(model);
            }


            //读取模板内容 
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/PhotoWall/ThumbList.html"));

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["sitecode"] = strSiteCode;
            context.TempData["title"] = "照片列表";
            context.TempData["lstThumb"] = lstThumb;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}