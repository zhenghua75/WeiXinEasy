using DAL.WeiXin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.CMSAdmin.WeiXin
{
    public partial class wfmKeywordMsgVoicePlay : System.Web.UI.Page
    {
        string mediaID = string.Empty;
        protected string mediaFile=string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (null != Common.Common.NoHtml(Request.QueryString["id"]))
            {
                mediaID = Common.Common.NoHtml(Request.QueryString["id"]);
            }
            if (!string.IsNullOrEmpty(mediaID))
            {
                Model.WeiXin.Media media=MediaDAL.CreateInstance().GetMediaByID(mediaID);
                mediaFile = media.MediaFile;
            }
            else
            {
                Response.Write("无法播放指定的文件！");
                Response.End();
            }
        }
    }
}