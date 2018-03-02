using System;
using System.Collections.Generic;
using System.Web;

namespace Mozart.Common
{
    public partial class GlobalSession
    {
        /// <summary>
        /// 用户ID号
        /// </summary>
        public static string strAccountID
        {
            set { HttpContext.Current.Session["strAccountID"] = value; }
            get { return HttpContext.Current.Session["strAccountID"].ToString(); }
        } 

        /// <summary>
        /// 用户登录名称
        /// </summary>
        public static string strLoginName
        {
            set { HttpContext.Current.Session["strLoginName"] = value; }
            get { return HttpContext.Current.Session["strLoginName"].ToString(); }
        }

        /// <summary>
        /// 用户名称
        /// </summary>
        public static string strName
        {
            set { HttpContext.Current.Session["strName"] = value; }
            get { return HttpContext.Current.Session["strName"].ToString(); }
        }

        /// <summary>
        /// 登录IP地址
        /// </summary>
        public static string strIP
        {
            set { HttpContext.Current.Session["strIP"] = value; }
            get { return HttpContext.Current.Session["strIP"].ToString(); }
        }

        ///// <summary>
        ///// 角色组名称
        ///// </summary>
        //public static string strRoleName
        //{
        //    set { HttpContext.Current.Session["strRoleName"] = value; }
        //    get { return HttpContext.Current.Session["strRoleName"].ToString(); }
        //}

        /// <summary>
        /// 角色组代码
        /// </summary>
        public static string strRoleCode
        {
            set { HttpContext.Current.Session["strRoleCode"] = value; }
            get { return HttpContext.Current.Session["strRoleCode"].ToString(); }
        }

        /// <summary>
        /// 站点代码
        /// </summary>
        public static string strSiteCode
        {
            set { HttpContext.Current.Session["strSiteCode"] = value; }
            get { return HttpContext.Current.Session["strSiteCode"].ToString(); }
        }
    }

    public partial class CustomerSession
    {
        /// <summary>
        /// 用户ID号
        /// </summary>
        public static string strCustomerID
        {
            set { HttpContext.Current.Session["strCustomerID"] = value; }
            get { return HttpContext.Current.Session["strCustomerID"].ToString(); }
        }

        /// <summary>
        /// 用户名称
        /// </summary>
        public static string strName
        {
            set { HttpContext.Current.Session["strName"] = value; }
            get { return HttpContext.Current.Session["strName"].ToString(); }
        }

        /// <summary>
        /// 站点代码
        /// </summary>
        public static string strSiteCode
        {
            set { HttpContext.Current.Session["strSiteCode"] = value; }
            get { return HttpContext.Current.Session["strSiteCode"].ToString(); }
        }

        /// <summary>
        /// 联系电话
        /// </summary>
        public static string strMobile
        {
            set { HttpContext.Current.Session["strMobile"] = value; }
            get { return HttpContext.Current.Session["strMobile"].ToString(); }
        }

        /// <summary>
        /// OpenID
        /// </summary>
        public static string strOpenID
        {
            set { HttpContext.Current.Session["strOpenID"] = value; }
            get { return HttpContext.Current.Session["strOpenID"].ToString(); }
        }

        public void GlobalSession()
        {
            strName = string.Empty;
        }
    }
}