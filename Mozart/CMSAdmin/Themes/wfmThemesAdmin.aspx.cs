using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.SYS;
using Model.SYS;

namespace Mozart.CMSAdmin.Themes
{
    public partial class wfmThemesAdmin : System.Web.UI.Page
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

            SysThemesDAL dal = new SysThemesDAL();
            int statein = 0;
            try
            {
                statein = Convert.ToInt32(dal.GetThemesvaluesById("IsState", strID));
            }
            catch (Exception)
            {
                statein = 0;
            }
            switch (statein)
            {
                case 0:
                    statein = 1;
                    break;
                default:
                    statein = 0;
                    break;
            }
            SysThemes model = new SysThemes();
            model.ID = strID;
            model.IsState = statein;
            switch (strAction)
            {
                case "del":
                    if (dal.UpdateSysThemes(model))
                    {
                        strMessage = "操作成功！";
                    }
                    else
                    {
                        strMessage = "操作失败！";
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