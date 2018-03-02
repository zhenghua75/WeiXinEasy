using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Mozart.Common;
using Model.MiniShop;
using DAL.MiniShop;
using System.IO;

namespace Mozart.Comment
{
    public partial class CommentView : System.Web.UI.Page
    {
        public string errorscript = string.Empty;
        public string strfid = string.Empty;
        public string strTid = string.Empty;
        public string topictitle = string.Empty;
        public string strUid = string.Empty;
        public string strcommid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                errorscript = "";
                if (Request["fid"] != null && Request["fid"] != "")
                {
                    strfid = Common.Common.NoHtml(Request["fid"]);
                }
                else
                {
                    errorscript = JQDialog.alertOkMsgBoxClearBody(2, "操作失败<br/>请核对后再操作！");
                }
                if (Request["tid"] != null && Request["tid"] != "")
                {
                    strTid = Common.Common.NoHtml(Request["tid"]);
                }
                else
                {
                    errorscript = JQDialog.alertOkMsgBoxClearBody(2, "操作失败<br/>请核对后再操作！");
                }
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    strUid = Common.Common.NoHtml(Session["customerID"].ToString());
                    if (Request["action"] != null && Request["action"] != "" &&
                        Request["action"].ToString().ToLower() == "comment")
                    {
                        pubcomment();
                    }
                }
                else
                {
                    JQDialog.SetCookies("pageurl", "../../Comment/CommentView.aspx?tid=" + strTid+"&fid="+strfid, 2);
                    errorscript = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！",
                        "../PalmShop/ShopCode/UserLogin.aspx","error");
                }
                getTemplate();
            }
        }
        void getTemplate()
        {
            string text = System.IO.File.ReadAllText(Server.MapPath("HtmlPage/commentview.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            context.TempData["errorscript"] = errorscript;
            context.TempData["tid"] = strTid;
            context.TempData["fid"] = strfid;
            context.TempData["footer"] = "奥琦微商易";
            t.Render(Response.Output);
        }
        void pubcomment()
        {
            string commentdesc = string.Empty;
            try
            {
                commentdesc = HttpContext.Current.Request.Form.Get("commentdesc").ToString();
            }
            catch (Exception)
            {
                commentdesc = "";
            }
            if (commentdesc.Trim() != null && commentdesc.Trim() != "")
            {
                MSForumComment commentModel = new MSForumComment();
                MSForumCommentDAL commentDal = new MSForumCommentDAL();
                strcommid =Guid.NewGuid().ToString("N").ToUpper();
                commentModel.ID = strcommid; 
                commentModel.Ctext = commentdesc;
                commentModel.Review = 1;
                commentModel.TID =strTid;
                commentModel.UID = strUid;
                commentModel.UpID = "";
                commentModel.Cstate = 0;
                SaveImages();
                if (commentDal.AddComment(commentModel))
                {
                    errorscript = 
                        JQDialog.alertOKMsgBox(3, "评论成功", "CommentList.aspx?fid=" + 
                        strfid + "&tid=" + strTid,"succeed");
                }
                else
                {
                    errorscript = JQDialog.alertOKMsgBoxGoBack(3, "操作失败<br/>请核对后再操作！", true);
                }
            }
            else
            {
                errorscript = JQDialog.alertOKMsgBoxGoBack(3, "操作失败<br/>请重新再操作！", true);
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
                atlasModel.TID ="comm"+ strcommid;
                for (int iFile = 0; iFile < files.Count; iFile++)
                {
                    //检查文件扩展名字
                    HttpPostedFile postedFile = files[iFile];
                    string fileName, fileExtension, file_oldid, file_id;
                    //取出精确到毫秒的时间做文件的名称
                    string my_file_id = DateTime.Now.ToString("yyyyMMddHHmmssfff") + iFile.ToString();
                    fileName = System.IO.Path.GetFileName(postedFile.FileName);
                    fileExtension = System.IO.Path.GetExtension(fileName);
                    file_id = "comm" + my_file_id + fileExtension;
                    if (fileName != "" && fileName != null && fileName.Length > 0)
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