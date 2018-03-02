using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class PhotoPrint : System.Web.UI.Page
    {
        string strThumbID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (null == Request.QueryString["id"])
            {
                return;
            }
            strThumbID = Common.Common.NoHtml(Request.QueryString["id"].ToString());

            DAL.Album.UserPhotoDAL dal = new DAL.Album.UserPhotoDAL();
            DataSet ds = dal.GetMyThumb(strThumbID);
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                //读取模板内容 
                string text = System.IO.File.ReadAllText(Server.MapPath("Themes/PhotoList/PhotoPrint.html"));

                JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
                context.TempData["title"] = "照片信息";
                context.TempData["ThumbID"] = strThumbID;
                context.TempData["footer"] = "奥琦微商易";

                JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
                t.Render(Response.Output);
            }
            else
            {
                Response.Write("<script>alert('所要打印的照片不存在!')</script>");
                Response.Write("<script>document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {	WeixinJSBridge.call('closeWindow');});</script>");
            }
        }
    }
}