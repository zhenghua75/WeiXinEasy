using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.WeiXin;
using System.Data;

namespace Mozart.CMSAdmin.WXConfig
{
    public partial class wfmWXMenu : System.Web.UI.Page
    {
        public static string action = string.Empty;
        string menuname = string.Empty;
        string menutype = string.Empty;
        string parentmenuid = string.Empty;
        string buttonmenutype = string.Empty;
        string redirectscope = string.Empty;
        string redirectstate = string.Empty;
        string ordernum = string.Empty;
        string wxconfigID = string.Empty;
        string menuid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["strSiteCode"].ToString()) && Session["strRoleCode"].ToString() != "ADMIN")
            {
                Response.Write("<script language=JavaScript>parent.location.href='../Index.aspx';</script>");
                Response.End();
            }
            if (!IsPostBack)
            {
                if (Request["action"] != null && Request["action"] != "")
                {
                    action = Common.Common.NoHtml(Request["action"]).ToString().ToLower();
                    AddOrUpdateMenu();
                    Response.End();
                }
            }
        }
        #region 菜单添加或修改
        /// <summary>
        /// 菜单添加或修改
        /// </summary>
        void AddOrUpdateMenu()
        {
            #region 获取请求参数值
            if (action != "del")
            {
                if (Request["menuname"] != null && Request["menuname"] != "")
                {
                    menuname = Common.Common.NoHtml(Request["menuname"]);
                }
                if (Request["menutype"] != null && Request["menutype"] != "")
                {
                    menutype = Common.Common.NoHtml(Request["menutype"]);
                }
                if (Request["parentmenuid"] != null && Request["parentmenuid"] != "")
                {
                    parentmenuid = Common.Common.NoHtml(Request["parentmenuid"]);
                }
                if (Request["buttonmenutype"] != null && Request["buttonmenutype"] != "")
                {
                    buttonmenutype = Common.Common.NoHtml(Request["buttonmenutype"]);
                }
                if (Request["redirectscope"] != null && Request["redirectscope"] != "")
                {
                    redirectscope = Common.Common.NoHtml(Request["redirectscope"]);
                }
                if (Request["redirectstate"] != null && Request["redirectstate"] != "")
                {
                    redirectstate = Common.Common.NoHtml(Request["redirectstate"]);
                }
                if (Request["ordernum"] != null && Request["ordernum"] != "")
                {
                    ordernum = Common.Common.NoHtml(Request["ordernum"]);
                }
            }
            if (Request["menuid"] != null && Request["menuid"] != "")
            {
                menuid = Common.Common.NoHtml(Request["menuid"]);
            }
        #endregion
            if (Session["strSiteCode"] != null && Session["strSiteCode"].ToString() != "")
            {
                Model.WeiXin.Menu menumodel = new Model.WeiXin.Menu();
                WXConfigDAL wxconfigdal = new WXConfigDAL();
                MenuDAL menudal = new MenuDAL();
                #region  设置Model参数值
                if (action != "del")
                {
                    if (menuname.Trim() != null && menuname.Trim() != "")
                    {
                        menumodel.ButtonName = menuname;
                    }
                    if (menutype.Trim() != null && menutype.Trim() != "")
                    {
                        menumodel.MenuType = menutype;
                    }
                    if (buttonmenutype.Trim() != null && buttonmenutype.Trim() != "")
                    {
                        menumodel.ButtonType = buttonmenutype;
                    }
                    if (redirectscope.Trim() != null && redirectscope.Trim() != "")
                    {
                        menumodel.RedirectScope = redirectscope;
                    }
                    if (redirectstate.Trim() != null && redirectstate.Trim() != "")
                    {
                        menumodel.RedirectState = redirectstate;
                    }
                    if (ordernum.Trim() != null && ordernum.Trim() != "")
                    {
                        menumodel.OrderNum = Convert.ToInt32(ordernum);
                    }
                    else
                    {
                        menumodel.OrderNum = 1;
                    }
                    try
                    {
                        wxconfigID = wxconfigdal.GetWXConfigValue("ID", Session["strSiteCode"].ToString()).ToString();
                    }
                    catch (Exception)
                    {
                    }
                    menumodel.WXConfigID = wxconfigID;
                }
                #endregion
                #region 数据请求操作
                switch (action.Trim())
                {
                    case "update":
                        if (menuid.Trim() != null && menuid.Trim() != "" && menuid.Trim() != "0")
                        {
                            menumodel.ID = menuid;
                            if (menudal.UpdateWeiXinMenu(menumodel))
                            {
                                Response.Write("{\"message\":\"操作成功!\"}"); return;
                            }
                            else
                            {
                                Response.Write("{\"message\":\"操作失败，请核对后再操作!\"}"); return;
                            }
                        }
                        break;
                    case "add":
                        if (menudal.ExistWeiXinMenu(" ButtonName='" + menuname +
                         "' and b.SiteCode='" + Session["strSiteCode"] + "' "))
                        {
                            Response.Write("{\"message\":\"操作失败，该菜单已经存在!\"}"); return;
                        }
                        else
                        {
                            int count = menudal.GetWeiXinMenuCount(parentmenuid, Session["strSiteCode"].ToString());
                            if (parentmenuid.Trim() != null && parentmenuid.Trim() != "")
                            {
                                if (count >= 5)
                                {
                                    Response.Write("{\"message\":\"二级菜单最多能添加5个!\"}"); return;
                                }
                            }
                            else
                            {
                                if (count >= 3)
                                {
                                    Response.Write("{\"message\":\"主菜单最多能添加3个!\"}"); return;
                                }
                            }
                            menumodel.ID = Guid.NewGuid().ToString("N").ToUpper();
                            menumodel.Enabled = 1;
                            if (parentmenuid.Trim() != null && parentmenuid.Trim() != "")
                            {
                                menumodel.ParentMenuID = parentmenuid;
                            }
                            if (menudal.AddWeiXinMenu(menumodel))
                            {
                                Response.Write("{\"message\":\"操作成功!\"}"); return;
                            }
                            else
                            {
                                Response.Write("{\"message\":\"操作失败，请核对后再操作!\"}"); return;
                            }
                        }
                    case "del":
                        if (menuid.Trim() != null && menuid.Trim() != "")
                        {
                            if (menudal.UpdateWeiXinMenuEnabled(menuid))
                            {
                                Response.Write("{\"message\":\"操作成功!\"}"); return;
                            }
                            else
                            {
                                Response.Write("{\"message\":\"操作失败，请核对后再操作!\"}"); return;
                            }
                        }
                        else
                        {
                            Response.Write("{\"message\":\"操作失败，请核对后再操作!\"}"); return;
                        }
                }
                #endregion
            }
        }
        #endregion 
    }
}