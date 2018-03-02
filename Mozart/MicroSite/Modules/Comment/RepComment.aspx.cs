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
    public partial class RepComment : System.Web.UI.Page
    {
        public string errorscript = string.Empty;
        public string strfid = string.Empty;
        public string strTid = string.Empty;
        public string topictitle = string.Empty;
        public string strUid = string.Empty;
        public string strupid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                errorscript = "";
                if (Request["fid"] != null && Request["fid"] != "")
                {
                    strfid = Common.Common.NoHtml(Request["fid"]);
                }
                if (Request["upid"] != null && Request["upid"] != "")
                {
                    strupid = Common.Common.NoHtml(Request["upid"]);
                }
                else
                {
                    errorscript = JQDialog.alertOKMsgBoxGoBack(2, "操作失败<br/>请核对后再操作！",true);
                }
                if (Request["tid"] != null && Request["tid"] != "")
                {
                    strTid = Common.Common.NoHtml(Request["tid"]);
                }
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    strUid = Common.Common.NoHtml(Session["customerID"].ToString());
                    if (Request["action"] != null && Request["action"] != "" &&
                        Request["action"].ToString().ToLower() == "repmsg")
                    {
                        repcomment();
                    }
                }
                else
                {
                    JQDialog.SetCookies("pageurl", "../../Comment/CommentList.aspx?tid=" + strTid, 2);
                    errorscript = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！",
                        "../PalmShop/ShopCode/UserLogin.aspx", "error");
                }
                getTemplate();
            }
        }
        void getTemplate()
        {
            string text = System.IO.File.ReadAllText(Server.MapPath("HtmlPage/repcomment.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            context.TempData["errorscript"] = errorscript;
            context.TempData["tid"] = strTid;
            context.TempData["fid"] = strfid;
            context.TempData["upid"] = strupid;
            context.TempData["footer"] = "奥琦微商易";
            t.Render(Response.Output);
        }
        void repcomment()
        {
            string commentdesc = string.Empty;
            StreamReader rd = new StreamReader(Server.MapPath("emotion/key.txt"));
            string keyword = rd.ReadLine();
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
                commentdesc = JQDialog.FilterKeyWord(commentdesc, keyword);
                commentdesc = JQDialog.GetTextFromHTML(commentdesc);
                MSForumComment commentModel = new MSForumComment();
                MSForumCommentDAL commentDal = new MSForumCommentDAL();
                commentModel.ID = Guid.NewGuid().ToString("N").ToUpper();
                commentModel.Ctext = commentdesc;
                commentModel.Review = 1;
                commentModel.TID = strTid;
                commentModel.UID = strUid;
                commentModel.UpID =strupid;
                commentModel.Cstate = 0;
                if (commentDal.AddComment(commentModel))
                {
                    errorscript =
                        JQDialog.alertOKMsgBox(3, "回复成功", "MyCommentList.aspx?fid=" + strfid, "error");
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
    }
}