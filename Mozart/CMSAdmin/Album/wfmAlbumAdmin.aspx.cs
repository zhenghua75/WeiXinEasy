﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Album;
using Model.PA;

namespace Mozart.CMSAdmin.Album
{
    public partial class wfmAlbumAdmin : System.Web.UI.Page
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

            AlbumDAL dal = new AlbumDAL();

            int statein = 0;
            try
            {
                statein = Convert.ToInt32(dal.GetAlbumValue("IsDel", strID));
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
            PA_Album model = new PA_Album();
            model.IsDel = statein;
            model.ID = strID;
            switch (strAction)
            {
                case "del":
                    if (dal.UpdateAlbum(model))
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