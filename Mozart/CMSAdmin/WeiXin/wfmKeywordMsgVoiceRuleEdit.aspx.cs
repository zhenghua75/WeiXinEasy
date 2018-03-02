using DAL.WeiXin;
using Model.WeiXin;
using Mozart.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiXinCore;
using WeiXinCore.Models;

namespace Mozart.CMSAdmin.WeiXin
{
    public partial class wfmKeywordMsgVoiceRuleEdit : System.Web.UI.Page
    {
        string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["strSiteCode"].ToString()) && Session["strRoleCode"].ToString() != "ADMIN")
            {
                Response.Write("<script language=JavaScript>parent.location.href='../Index.aspx';</script>");
                Response.End();
            }
            if (!IsPostBack)
            {
                if (null != Common.Common.NoHtml(Request.QueryString["action"]))
                {
                    strAction = Common.Common.NoHtml(Request.QueryString["action"]);
                }
                if (null != Common.Common.NoHtml(Request.QueryString["id"]))
                {
                    strID = Common.Common.NoHtml(Request.QueryString["id"]);
                }
                MsgAutoRuleDAL dal = new MsgAutoRuleDAL();
                switch (strAction)
                {
                    case "del":
                        if (dal.UpdateMsgAutoRuleEnable(strID))
                        {
                            strMessage = "操作成功！";
                        }
                        else
                        {
                            strMessage = "操作失败！";
                        }
                        Response.Write(strMessage);
                        Response.End();
                        break;
                    default:
                        break;
                }
                
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (null == Session["strSiteName"] || null == Session["strSiteCode"] || null == Session["strLoginName"])
            {
                Response.Write("<script language=JavaScript>;parent.location.href='../Index.aspx';</script>");
                Response.End();
            }

            if (!string.IsNullOrEmpty(keyword.Text.Trim()) 
                && fudVoice.HasFile 
                && fudVoice.PostedFile.ContentLength<=1024*1024*2
                && (fudVoice.PostedFile.FileName.ToLower().Contains(".mp3") || fudVoice.PostedFile.FileName.ToLower().Contains(".amr")))
            {//语音（voice）：2M，播放长度不超过60s，支持AMR\MP3格式 
                try
                {
                    //先上传并插入微信媒体表
                    string fileDir = string.Format("/Uploads/{0}/",DateTime.Now.ToString("yyyyMM"));
                    if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(fileDir)))
                    {
                        System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(fileDir));
                    }
                    string ext=System.IO.Path.GetExtension(fudVoice.FileName);
                    string fileName = string.Format("{0}{1}",Guid.NewGuid().ToString("N"),ext);
                    string filePath = fileDir + fileName;
                    fudVoice.SaveAs(HttpContext.Current.Server.MapPath(filePath));
                    Media media = new Media()
                    {
                        MediaName = fudVoice.FileName,
                        MediaFile = filePath,
                        MediaType = MediaUploadType.Voice.ToString().ToLower()
                    };
                    //选中自动同步时，将语音文件自动同步至微信服务器
                    if (cboIsSyn.Checked)
                    {
                        WXConfigDAL cdal = new WXConfigDAL();
                        Model.WeiXin.WXConfig wxConfig = cdal.GetWXConfigBySiteCode(Session["strSiteCode"].ToString());
                        if (wxConfig != null)
                        {
                            WeiXinCore.Models.WeiXinConfig weixinConfig = new WeiXinCore.Models.WeiXinConfig()
                            {
                                ID = wxConfig.WXID,
                                Name = wxConfig.WXName,
                                Token = wxConfig.WXToken,
                                AppId = wxConfig.WXAppID,
                                AppSecret = wxConfig.WXAppSecret
                            };
                            WeiXinCore.WeiXin weixin = new WeiXinCore.WeiXin(weixinConfig);
                            MediaUploadResult obj = weixin.PostMedia(HttpContext.Current.Server.MapPath(filePath), MediaUploadType.Voice);
                            media.MediaID = obj.MediaID;
                            media.LastSynTime = DateTime.Now;
                        }
                    }

                    MediaDAL.CreateInstance().InsertInfo(media);


                    //插入自动回复匹配表
                    WXConfigDAL configdal = new WXConfigDAL();
                    DataSet wxconfigds = configdal.GetWXConfigDataList(Session["strSiteCode"].ToString());
                    MsgAutoRule model = new MsgAutoRule();
                    MsgAutoRuleDAL dal = new MsgAutoRuleDAL();
                    model.MatchPattern = keyword.Text;
                    model.Order = Convert.ToInt32(sort.Text);
                    model.MsgValue = media.ID;//对应微信媒体文件ID，因微信上传文件有效期为两天，所以需要调度程序定期自动上传
                    model.ID = Guid.NewGuid().ToString("N").ToUpper();
                    model.Enabled = 1;
                    model.LastModTime = DateTime.Now;
                    model.MatchType = "keywords";
                    model.MsgType = "voice";
                    if (wxconfigds != null && wxconfigds.Tables.Count > 0 && wxconfigds.Tables[0].Rows.Count > 0)
                    {
                        model.WXConfigID = wxconfigds.Tables[0].Rows[0]["ID"].ToString();
                    }
                    if (dal.AddMsgAutoRule(model))
                    {
                        MessageBox.Show(this, "操作成功！");
                    }
                    else
                    {
                        MessageBox.Show(this, "操作失败！");
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(this, ex.Message);
                }
            }
            else
            {
                MessageBox.Show(this, "请核对信息后再操作！");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            keyword.Text = ""; sort.Text = ""; 
        }
    }
}