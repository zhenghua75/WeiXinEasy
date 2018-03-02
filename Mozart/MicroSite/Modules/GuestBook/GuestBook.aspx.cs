using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.WeiXin;
using Newtonsoft.Json;
using WeiXinCore;

namespace Mozart.MicroSite
{
    public partial class GuestBook : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strSiteID = string.Empty;
            string strTitle = string.Empty;
            string strTheme = string.Empty;
            string strSiteCode = string.Empty;
            if (null == Request.QueryString["ID"])
            {
                return;
            }
            strSiteID = Common.Common.NoHtml(Request.QueryString["ID"].ToString());
            Session["siteid"] = strSiteID;

            DAL.SYS.AccountDAL dalAccount = new DAL.SYS.AccountDAL();
            DataSet ds = dalAccount.GetAccountExtData(strSiteID);
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                strTheme = ds.Tables[0].Rows[0]["Themes"].ToString();
                strTitle = ds.Tables[0].Rows[0]["Name"].ToString();
                strSiteCode = ds.Tables[0].Rows[0]["SiteCode"].ToString();
                Session["strSiteCode"] = strSiteCode;
            }

            string strName = string.Empty;
            string strMobile = string.Empty;
            string strContext = string.Empty;
            if (null != Request.QueryString["action"])
            {
                if (Request.QueryString["action"].ToString() == "save")
                {
                    strName = Common.Common.NoHtml(Request.Form["realname"].ToString());
                    strMobile = Common.Common.NoHtml(Request.Form["mobile"].ToString());
                    strContext = Common.Common.NoHtml(Request.Form["message"].ToString());
                    if (strContext.Trim() != null && strContext.Trim()!="")
                    {
                        DAL.BBS.BBSGuestBookDAL gbDAL = new DAL.BBS.BBSGuestBookDAL();
                        if (!gbDAL.ExistMsg(strContext))
                        {
                            Model.BBS.BBS_GuestBook gbModel = new Model.BBS.BBS_GuestBook()
                            {
                                ID = Guid.NewGuid().ToString("N"),
                                UserName = strName,
                                UserMobile = strMobile,
                                Content = strContext,
                                Replay = "",
                                State = 0,
                                CreateTime = DateTime.Now,
                                SiteCode = strSiteCode
                            };
                            if (gbDAL.AddGuestBook(gbModel))
                            {
                                Response.Write("<script>alert('操作成功');</script>");
                                #region
                                //#region 消费完成发生消息
                                //string strAppID = string.Empty;
                                //string strSecret = string.Empty;
                                //Model.WeiXin.WXConfig wc = new Model.WeiXin.WXConfig();
                                //WXConfigDAL wcdal = new WXConfigDAL();
                                //wc = wcdal.GetWXConfigBySiteCode(strSiteCode);
                                //if (null != wc)
                                //{
                                //    strAppID = wc.WXAppID;
                                //    strSecret = wc.WXAppSecret;
                                //}
                                //string strToken = WeiXinHelper.GetAccessToken(strAppID, strSecret);

                                //var KeyToken = new { access_token = "" };
                                //var b = JsonConvert.DeserializeAnonymousType(strToken, KeyToken);
                                //string strRToken = b.access_token;

                                //WeiXinHelper.SendCustomTextMessage(strRToken, strOpenID, strMessage);
                                //#endregion
                                //Response.Write("<script>document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {	WeixinJSBridge.call('closeWindow');});</script>");
                                //return;
                                #endregion
                            }
                            else
                            {
                                Response.Write("<script>alert('操作失败，请稍后再试！');</script>");
                            }
                        }
                        else
                        {
                            Response.Write("<script>alert('该信息已提交过');</script>");
                        }
                    }
                }
            }

            //读取模板内容
            string text = string.Empty;
            if (!File.Exists(Server.MapPath("Themes/" + strTheme + "/GuestBook.html")))
            {
                text = System.IO.File.ReadAllText(Server.MapPath("Themes/GuestBook/GuestBook.html"));
            }
            else
            {
                text = System.IO.File.ReadAllText(Server.MapPath("Themes/" + strTheme + "/GuestBook.html"));
            }

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["title"] = strTitle;
            context.TempData["siteid"] = Session["siteid"];
            context.TempData["footer"] = "奥琦微商易";
            context.TempData["sitecode"] = Session["strSiteCode"].ToString();
            context.TempData["siteid"] = strSiteID;

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}