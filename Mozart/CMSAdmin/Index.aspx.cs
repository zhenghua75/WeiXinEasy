using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.CMSAdmin
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (null == Session["strAccountID"] || string.IsNullOrEmpty(Session["strAccountID"].ToString()))
            {
                Response.Redirect("Login.aspx", false);
            }
            if (null == Session["strRoleCode"] || string.IsNullOrEmpty(Session["strRoleCode"].ToString()))
            {
                Response.Redirect("Login.aspx", false);
            }
            if (null == Session["strSiteName"] || string.IsNullOrEmpty(Session["strSiteName"].ToString()))
            {
                Response.Redirect("Login.aspx", false);
            }
            else
            {
                labLoginName.Text = Session["strSiteName"].ToString();
            }
            if (!IsPostBack)
            {
                //try
                //{
                    DAL.SYS.MenuRoleDAL dal = new DAL.SYS.MenuRoleDAL();
                    string strRoleId = string.Empty;
                    if (!string.IsNullOrEmpty(Session["strRoleCode"].ToString()))
                    {
                        strRoleId = Session["strRoleCode"].ToString();
                    }
                    else
                    {
                        Response.Redirect("~/Login.aspx");
                    }
                    DataSet dsOut = dal.GetMenuDate(strRoleId);
                    StringBuilder sbNemu = new StringBuilder();
                    sbNemu.Append("<ul class=\"nav nav-list\">");

                    #region 公共功能
                    //一级菜单
                    DataRow[] drMenu0 = dsOut.Tables[0].Select(" Level=0 ");
                    string strMenuCode = "";
                    string strMenuName = "";
                    string strUrl = "";
                    string strIconImage = "";
                    if (Session["strSiteCode"] != null && Session["strSiteCode"].ToString() != "" &&
                        Session["strSiteCode"].ToString().ToLower() != "admin")
                    {
                        sbNemu.Append("<li><a data=\"line\"><i class=\"icon-th-large\"></i></i><span class=\"menu-text\">" + "基础功能" + "</span></a></li>");
                    }
                    for (int i = 0; i < drMenu0.Length; i++)
                    {
                        strMenuCode = drMenu0[i]["No"].ToString();
                        strMenuName = drMenu0[i]["Name"].ToString();
                        strIconImage = drMenu0[i]["Icon"].ToString();
                        strUrl = drMenu0[i]["Url"].ToString();
                        sbNemu.Append("<li><a href=\"#\" class=\"dropdown-toggle\"><i class=\"icon-desktop\"></i><span class=\"menu-text\">" + strMenuName + "</span><b class=\"arrow icon-angle-down\"></b></a>");
                        sbNemu.Append("<ul class=\"submenu\">");
                        //--获取下级菜单(二级菜单)
                        DataRow[] drMenu1 = dsOut.Tables[0].Select(" Level = 1 and vcParent = '" + strMenuCode + "' ");
                        for (int j = 0; j < drMenu1.Length; j++)
                        {
                            strMenuCode = drMenu1[j]["No"].ToString();
                            strMenuName = drMenu1[j]["Name"].ToString();
                            strIconImage = drMenu1[j]["Icon"].ToString();
                            strUrl = drMenu1[j]["Url"].ToString();
                            sbNemu.Append("<li><a href=\"" + strUrl + "\" target=\"rightFrame\"><i class=\"icon-double-angle-right\"></i>" + strMenuName + "</a></li>");
                        }
                        sbNemu.Append("</ul></li>");
                    }
                    #endregion
                    if (Session["strSiteCode"] != null && Session["strSiteCode"].ToString() != "" &&
                        Session["strSiteCode"].ToString().ToLower() != "admin")
                    {
                        #region 行业菜单
                        DAL.SYS.SYSMenuIndustryDAL dalIndustry = new DAL.SYS.SYSMenuIndustryDAL();
                        DataSet dsIndustry = dalIndustry.GetSYSMenuIndustryByCategory(Session["SiteCategory"].ToString());
                        if (null != dsIndustry && dsIndustry.Tables.Count > 0 && dsIndustry.Tables[0].Rows.Count > 0)
                        {
                            sbNemu.Append("<li><a data=\"line\"><i class=\"icon-th-large\"></i></i><span class=\"menu-text\">" + "行业功能" + "</span></a></li>");
                            DataRow[] drMenuIndustry0 = dsIndustry.Tables[0].Select(" Level=0 ");
                            for (int i = 0; i < drMenuIndustry0.Length; i++)
                            {
                                strMenuCode = drMenuIndustry0[i]["No"].ToString();
                                strMenuName = drMenuIndustry0[i]["Name"].ToString();
                                strIconImage = drMenuIndustry0[i]["Icon"].ToString();
                                strUrl = drMenuIndustry0[i]["Url"].ToString();
                                sbNemu.Append("<li><a href=\"" + strUrl + "\" class=\"dropdown-toggle\"><i class=\"icon-desktop\"></i><span class=\"menu-text\">" + strMenuName + "</span><b class=\"arrow icon-angle-down\"></b></a>");
                                sbNemu.Append("<ul class=\"submenu\">");
                                //--获取下级菜单(二级菜单)
                                DataRow[] drMenuIndustry1 = dsIndustry.Tables[0].Select(" Level=1 and vcParent = '" + strMenuCode + "' ");
                                for (int j = 0; j < drMenuIndustry1.Length; j++)
                                {
                                    strMenuCode = drMenuIndustry1[j]["No"].ToString();
                                    strMenuName = drMenuIndustry1[j]["Name"].ToString();
                                    strIconImage = drMenuIndustry1[j]["Icon"].ToString();
                                    strUrl = drMenuIndustry1[j]["Url"].ToString();
                                    sbNemu.Append("<li><a href=\"" + strUrl + "\" target=\"rightFrame\"><i class=\"icon-double-angle-right\"></i>" + strMenuName + "</a></li>");
                                }
                                sbNemu.Append("</ul></li>");
                            }
                        }
                        #endregion
                        #region 专属菜单
                        DAL.SYS.SYSMenuSiteCodeDAL dalSiteCode = new DAL.SYS.SYSMenuSiteCodeDAL();
                        DataSet dsSiteCode = dalSiteCode.GetSYSMenuSiteCodeBySiteCode(Session["strSiteCode"].ToString());
                        if (null != dsSiteCode && dsSiteCode.Tables.Count > 0 && dsSiteCode.Tables[0].Rows.Count > 0)
                        {
                            sbNemu.Append("<li><a data=\"line\"><i class=\"icon-th-large\"></i></i><span class=\"menu-text\">" + "专属功能" + "</span></a></li>");
                            DataRow[] drSiteCode0 = dsSiteCode.Tables[0].Select(" Level=0 ");
                            for (int i = 0; i < drSiteCode0.Length; i++)
                            {
                                strMenuCode = drSiteCode0[i]["No"].ToString();
                                strMenuName = drSiteCode0[i]["Name"].ToString();
                                strIconImage = drSiteCode0[i]["Icon"].ToString();
                                strUrl = drSiteCode0[i]["Url"].ToString();
                                sbNemu.Append("<li><a href=\"" + strUrl + "\" class=\"dropdown-toggle\"><i class=\"icon-desktop\"></i><span class=\"menu-text\">" + strMenuName + "</span><b class=\"arrow icon-angle-down\"></b></a>");
                                sbNemu.Append("<ul class=\"submenu\">");
                                //--获取下级菜单(二级菜单)
                                DataRow[] drSiteCode1 = dsSiteCode.Tables[0].Select(" Level=1 and vcParent = '" + strMenuCode + "' ");
                                for (int j = 0; j < drSiteCode1.Length; j++)
                                {
                                    strMenuCode = drSiteCode1[j]["No"].ToString();
                                    strMenuName = drSiteCode1[j]["Name"].ToString();
                                    strIconImage = drSiteCode1[j]["Icon"].ToString();
                                    strUrl = drSiteCode1[j]["Url"].ToString();
                                    sbNemu.Append("<li><a href=\"" + strUrl + "\" target=\"rightFrame\"><i class=\"icon-double-angle-right\"></i>" + strMenuName + "</a></li>");
                                }
                                sbNemu.Append("</ul></li>");
                            }
                        }
                        #endregion
                    }
                    sbNemu.Append("</ul>");
                    this.divMenu.InnerHtml = sbNemu.ToString();
                //}
                //catch
                //{
                //    Response.Redirect("login.aspx");
                //}
            }
        }
    }
}