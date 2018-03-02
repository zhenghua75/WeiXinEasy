using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.PA;
using Mozart.Common;


namespace Mozart.MicroSite
{
    public partial class UserPhoto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strSiteID = string.Empty;
            string strSiteCode = string.Empty;
            string strTitle = string.Empty;
            string strTheme = string.Empty;
            string strAlbumTypeID = string.Empty;
            if (null == Request.QueryString["ID"])
            {
                return;
            }
            strAlbumTypeID = Common.Common.NoHtml(Request.QueryString["ID"].ToString());

            //取站点信息
            DAL.SYS.AccountDAL dalAccount = new DAL.SYS.AccountDAL();
            DAL.Album.UserPhotoDAL dalUserPhoto = new DAL.Album.UserPhotoDAL();

            DataSet dsAccount = dalUserPhoto.GetAccountData(strAlbumTypeID);

            if (null != dsAccount && dsAccount.Tables.Count > 0 && dsAccount.Tables[0].Rows.Count > 0)
            {
                strTheme = dsAccount.Tables[0].Rows[0]["Themes"].ToString();
                strTitle = dsAccount.Tables[0].Rows[0]["Name"].ToString();
                strSiteCode = dsAccount.Tables[0].Rows[0]["SiteCode"].ToString();
                strSiteID = dsAccount.Tables[0].Rows[0]["ID"].ToString();
                Session["strSiteCode"] = dsAccount.Tables[0].Rows[0]["SiteCode"].ToString();
            }

            //取站点相册列表
            DataSet dsAlbumList = dalUserPhoto.GetUserPhoto(strAlbumTypeID);
            List<Model.Album.UserPhoto> liAlbumList = new List<Model.Album.UserPhoto>();
            if (null != dsAlbumList && dsAlbumList.Tables.Count > 0 && dsAlbumList.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsAlbumList.Tables[0].Rows)
                {
                    Model.Album.UserPhoto model = DataConvert.DataRowToModel<Model.Album.UserPhoto>(row);
                    liAlbumList.Add(model);
                }
            }

            //读取模板内容
            string text = string.Empty;
            if (!File.Exists(Server.MapPath("Themes/" + strTheme + "/UserPhoto.html")))
            {
                text = System.IO.File.ReadAllText(Server.MapPath("Themes/PhotoList/UserPhoto.html"));
            }
            else
            {
                text = System.IO.File.ReadAllText(Server.MapPath("Themes/" + strTheme + "/UserPhoto.html"));
            }

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["title"] = strTitle;
            context.TempData["siteid"] = strSiteID;
            context.TempData["footer"] = "奥琦微商易";
            context.TempData["sitecode"] = strSiteCode;
            context.TempData["albumlist"] = liAlbumList;

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}