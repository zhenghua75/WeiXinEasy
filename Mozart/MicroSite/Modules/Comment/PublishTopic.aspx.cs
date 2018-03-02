using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.MiniShop;
using Mozart.Common;
using Model.MiniShop;
using System.Net;
using System.IO;

namespace Mozart.Comment
{
    public partial class PublishTopic : System.Web.UI.Page
    {
        public string errorscript = string.Empty;
        public string strfid = string.Empty;
        public string strTid = string.Empty;
        public string topictitle = string.Empty;
        public string strUid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["fid"] != null && Request["fid"] != "")
                {
                    errorscript = "";
                    strfid = Common.Common.NoHtml(Request["fid"]);
                }
                else
                {
                    errorscript = JQDialog.alertOkMsgBoxClearBody(2, "操作失败<br/>请核对后再操作！");
                }
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    strUid = Common.Common.NoHtml(Session["customerID"].ToString());
                    if (Request["action"] != null && Request["action"] != "" &&
                        Request["action"].ToString().ToLower() == "publish")
                    {
                        topicpublish();
                    }
                }
                else
                {
                    JQDialog.SetCookies("pageurl", "../../Comment/PublishTopic.aspx?fid=" + strfid, 2);
                    errorscript = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！",
                        "../PalmShop/ShopCode/UserLogin.aspx", "error");
                }
                getTemplate();
            }
        }
        void getTemplate()
        {
            string text = System.IO.File.ReadAllText(Server.MapPath("HtmlPage/newtrends.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            context.TempData["errorscript"] = errorscript;
            context.TempData["footer"] = "奥琦微商易";
            t.Render(Response.Output);
        }
        void topicpublish()
        {
            string topicdesc = string.Empty;
            #region --------获取标题和内容---------------
            try
            {
                topictitle = HttpContext.Current.Request.Form.Get("topictitle").ToString();
            }
            catch (Exception)
            {
                topictitle = "";
            }
            try
            {
                topicdesc = HttpContext.Current.Request.Form.Get("topicdeschidden").ToString();
            }
            catch (Exception)
            {
                topicdesc = "";
            }
            #endregion
            MSForumTopicDAL forumtopicDal = new MSForumTopicDAL();
            MSForumTopic forumtopicModel = new MSForumTopic();
            StreamReader rd = new StreamReader(Server.MapPath("emotion/key.txt"));
            string keyword = rd.ReadLine();
            if (topictitle != null && topictitle != "")
            {
                topictitle =JQDialog.FilterKeyWord(topictitle,keyword);
                topictitle = JQDialog.GetTextFromHTML(topictitle);
                forumtopicModel.TopicTitle = topictitle;
            }
            if (topicdesc != null && topicdesc != "")
            {
                topicdesc = JQDialog.FilterKeyWord(topicdesc, keyword);
                topicdesc = JQDialog.GetTextFromHTML(topicdesc);
                forumtopicModel.TopicDesc = topicdesc;
            }
            if (!forumtopicDal.ExistMSForumTopic(topictitle, strfid, strUid))
            {
                strTid = Guid.NewGuid().ToString("N").ToUpper();
                forumtopicModel.ID = strTid;
                forumtopicModel.TopicState = 0;
                forumtopicModel.FID = strfid;
                forumtopicModel.Treview = 1;
                forumtopicModel.UID = strUid;
                SaveImages();
                if (forumtopicDal.AddMSForumTopic(forumtopicModel))
                {
                    errorscript = JQDialog.alertOKMsgBox(5, "操作成功", "MyTopicList.aspx?fid=" + strfid,"succeed");
                }
                else
                {
                    errorscript = JQDialog.alertOKMsgBoxGoBack(5, "操作失败<br/>请核对后再操作！", true);
                }
            }
            else
            {
                errorscript = JQDialog.alertOKMsgBoxGoBack(5, "操作失败<br/>请核对后再操作！",true);
            }
        }
        /// <summary>
        /// 图集上传
        /// </summary>
        /// <returns></returns>
        private Boolean SaveImages()
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            try
            {
                MSForumTopicAtlas atlasModel = new MSForumTopicAtlas();
                atlasModel.TID = strTid;
                for (int iFile = 0; iFile < files.Count; iFile++)
                {
                    //检查文件扩展名字
                    HttpPostedFile postedFile = files[iFile];
                    string fileName, fileExtension, file_oldid, file_id;
                    //取出精确到毫秒的时间做文件的名称
                    string my_file_id = DateTime.Now.ToString("yyyyMMddHHmmssfff") + iFile.ToString();
                    fileName = System.IO.Path.GetFileName(postedFile.FileName);
                    fileExtension = System.IO.Path.GetExtension(fileName);
                    file_id = my_file_id + fileExtension;
                    if (fileName != "" && fileName != null && fileName.Length>0)
                    {
                        fileExtension = System.IO.Path.GetExtension(fileName);
                        string saveurl, modelimgurl;
                        saveurl = modelimgurl = "ForumImg/";
                        saveurl = Server.MapPath(saveurl);
                        if (!Directory.Exists(saveurl))
                        {
                            Directory.CreateDirectory(saveurl);
                        }

                        int length = postedFile.ContentLength;
                        if (length > 512000)
                        {
                            file_oldid = "old" + file_id;
                            postedFile.SaveAs(saveurl + file_oldid);
                            JQDialog.ystp(saveurl + file_oldid, saveurl + file_id, 15);
                            File.Delete(saveurl + file_oldid);
                        }
                        else
                        {
                            postedFile.SaveAs(saveurl + file_id);
                        }

                        atlasModel.ID = Guid.NewGuid().ToString("N").ToUpper();
                        atlasModel.ImgState = 0;
                        atlasModel.ImgUrl = modelimgurl + file_id;
                        atlasModel.ImgName = topictitle;
                        MSForumTopicAtlasDAL atlasDal = new MSForumTopicAtlasDAL();
                        atlasDal.AddMSForumTopicAtlas(atlasModel);
                    }
                }
                return true;
            }
            catch (System.Exception Ex)
            {
                return false;
            }
        }
    }
}