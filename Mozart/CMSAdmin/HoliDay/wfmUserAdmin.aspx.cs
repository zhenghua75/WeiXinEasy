using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.HoliDay;

namespace Mozart.CMSAdmin.HoliDay
{
    public partial class wfmUserAdmin : System.Web.UI.Page
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
            HoliDayUsersDAL dal = new HoliDayUsersDAL();
            switch (strAction)
            {
                case "del":
                    if (dal.UpdateHDUsersSdate(strID))
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