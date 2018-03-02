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
    public partial class WallPhoto : System.Web.UI.Page
    {
        string strThumbID = string.Empty;
        string strSiteCode = string.Empty;
        string strOpenID = string.Empty;
        string strFilePath = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (null == Request.QueryString["id"])
            {
                return;
            }
            strThumbID = Common.Common.NoHtml(Request.QueryString["id"].ToString());
            DAL.Album.UserPhotoDAL dal = new DAL.Album.UserPhotoDAL();
            DataSet ds = dal.GetMyThumb(strThumbID);

            Model.Album.UserPhoto model = new Model.Album.UserPhoto();
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model = DataConvert.DataRowToModel<Model.Album.UserPhoto>(ds.Tables[0].Rows[0]);
            }
            strSiteCode = model.SiteCode;
            strOpenID = model.OpenId;
            strFilePath = model.FilePath;

            //插入照片墙记录
            DAL.Album.PhotoWallDAL dalPhotoWall = new DAL.Album.PhotoWallDAL();
            Model.Album.PhotoWall modelPhotoWall = new Model.Album.PhotoWall();

            modelPhotoWall.OpenId = model.OpenId;
            modelPhotoWall.Name = model.Name;
            modelPhotoWall.SiteCode = model.SiteCode;
            modelPhotoWall.OpenId = model.OpenId;
            modelPhotoWall.FilePath = model.FilePath;
            modelPhotoWall.Remark = model.Remark;
            modelPhotoWall.AddTime = model.AddTime;

            dalPhotoWall.Insert(modelPhotoWall);

            //复制文件
            System.IO.FileInfo pFile = new System.IO.FileInfo(Server.MapPath("../User_Photo/" + strFilePath));
            if (pFile.Exists)
            {
                pFile.CopyTo(Server.MapPath("../WALL_Photo/" + strFilePath),true);
            }
            Response.Write("<script>alert('照片上传照片墙完成!');window.location.href='MyThumbList.aspx?SiteCode=" + strSiteCode + "&OpenID=" + strOpenID + "'</script>");
            //返回
            //Response.Redirect("MyThumbList.aspx?SiteCode=" + strSiteCode + "&OpenID=" + strOpenID);
        }
    }
}