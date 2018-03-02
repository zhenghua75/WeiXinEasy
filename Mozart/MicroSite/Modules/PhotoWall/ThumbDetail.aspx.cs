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
    public partial class ThumbDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strProductID = string.Empty;
            string strSiteCode = string.Empty;
            if (null == Request.QueryString["id"])
            {
                return;
            }
            strProductID = Common.Common.NoHtml(Request.QueryString["id"].ToString());
            DAL.Album.PhotoWallDAL dal = new DAL.Album.PhotoWallDAL();
            DataSet ds = dal.GetUserThumb(strProductID);

            Model.Album.UserPhoto model = new Model.Album.UserPhoto();
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model = DataConvert.DataRowToModel<Model.Album.UserPhoto>(ds.Tables[0].Rows[0]);
            }
            strSiteCode = model.SiteCode;

            //读取模板内容 
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/PhotoWall/ThumbDetail.html"));

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["sitecode"] = strSiteCode;
            context.TempData["title"] = "照片信息";
            context.TempData["pDetail"] = model;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}