using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.MicroSite.Themes.PhotoList.ajax
{
    public partial class thumbs : System.Web.UI.Page
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
            DataSet dsThumbList = dalUserPhoto.GetUserPhoto(strAlbumTypeID);
            List<Thumb> lstThumb = new List<Thumb>(); 
            foreach (DataRow row in dsThumbList.Tables[0].Rows)
            {
                Thumb p = new Thumb();
                p.src = "/USER_PHOTO/" + row["FilePath"].ToString();
                p.alt = "/USER_PHOTO/" + row["FilePath"].ToString();
                p.desc = row["ID"].ToString();
                //p.desc = row["ID"].ToString(); 
                lstThumb.Add(p);
            }
            string strReturn = ListToJson(lstThumb);
            Response.Write(strReturn);
        }

        #region List转换成Json
        /// <summary>
        /// List转换成Json
        /// </summary>
        public static string ListToJson<T>(IList<T> list)
        {
            object obj = list[0];
            return ListToJson<T>(list, obj.GetType().Name);
        }

        /// <summary>
        /// List转换成Json 
        /// </summary>
        public static string ListToJson<T>(IList<T> list, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName)) jsonName = list[0].GetType().Name;
            //Json.Append("{\"" + jsonName + "\":[");
            Json.Append("[");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    PropertyInfo[] pi = obj.GetType().GetProperties();
                    Json.Append("{");
                    for (int j = 0; j < pi.Length; j++)
                    {
                        Type type = pi[j].GetValue(list[i], null).GetType();
                        Json.Append("\"" + pi[j].Name.ToString() + "\":\"" + String.Format(pi[j].GetValue(list[i], null).ToString(), type));

                        if (j < pi.Length - 1)
                        {
                            Json.Append("\",");
                        }
                    }
                    Json.Append("\"}");
                    if (i < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            //Json.Append("]}");
            Json.Append("]");
            return Json.ToString();
        }
        #endregion
    }

    public class Thumb
    {
        public string src { get; set; }
        public string alt { get; set; }
        public string desc { get; set; }
    }  

}