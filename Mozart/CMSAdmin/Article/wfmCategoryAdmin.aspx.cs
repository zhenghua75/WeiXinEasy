using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.CMSAdmin.Article
{
    public partial class wfmCategoryAdmin : System.Web.UI.Page
    {
        static string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (null != Common.Common.NoHtml(Request.QueryString["action"]))
            {
                strAction = Common.Common.NoHtml(Request.QueryString["action"]);
            }
            if (null != Common.Common.NoHtml(Request.QueryString["id"]))
            {
                strID = Common.Common.NoHtml(Request.QueryString["id"]);
            }

            DAL.CMS.CategoryDAL dal = new DAL.CMS.CategoryDAL();
            switch (strAction)
            {
                case "del":
                    if (dal.UpdateCategoryState(strID))
                    {
                        strMessage = "删除文章类别完成！";
                    }
                    else
                    {
                        strMessage = "删除文章类别失败！";
                    }
                    break;
                default:
                    break;
            }
            Response.Write(strMessage);
            Response.End();
        }
    }
}