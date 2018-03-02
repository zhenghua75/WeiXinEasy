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
    public partial class DelPhoto : System.Web.UI.Page
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

            //删除文件
            //System.IO.FileInfo pFile = new System.IO.FileInfo("~/User_Photo/" + strFilePath);
            System.IO.FileInfo pFile = new System.IO.FileInfo(Server.MapPath("../User_Photo/" + strFilePath));
            if (pFile.Exists)
            {
                pFile.Delete();//删除
            }

            //删除记录
            dal.DelMyThumb(strThumbID);

            //返回
            Response.Redirect("MyThumbList.aspx?SiteCode=" + strSiteCode + "&OpenID=" + strOpenID);
        }
    }
}