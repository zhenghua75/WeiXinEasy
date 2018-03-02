using Maticsoft.DBUtility;
using Model.WeiXin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DAL.Common;
using WeiXinCore.Models;
using WeiXinCore;
using System.Web;

namespace DAL.WeiXin
{
    public class MenuDAL
    {
        public const string TABLE_NAME = "WX_Menu";

        public static MenuDAL CreateInstance()
        {
            return new MenuDAL();
        }

        /// <summary>
        /// 创建微信菜单
        /// </summary>
        /// <param name="siteCode"></param>
        public static void CreateWeiXinMenu(string wxConfigID)
        {
            if (!string.IsNullOrEmpty(wxConfigID))
            {
                WXConfigDAL dal = new WXConfigDAL();
                WXConfig wxConfig = dal.GetWXConfigByID(wxConfigID);
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
                    MenuDAL dal2 = CreateInstance();
                    List<Menu> menus = dal2.GetWeiXinMenu(wxConfig.ID);
                    MenuInfo menuInfo = new MenuInfo();
                    var buttons = from x in menus
                                  where x.MenuType.ToLower() == "button"
                                  orderby x.OrderNum ascending
                                  select x;
                    foreach (Menu button in buttons)
                    {
                        if (!string.IsNullOrEmpty(button.ButtonType))
                        {
                            switch (button.ButtonType.ToLower())
                            {
                                case "view":
                                    string url = button.AccessLink;
                                    if (!string.IsNullOrEmpty(button.RedirectScope))
                                    {
                                        url = WeiXinHelper.GenerateAuthorizeUrl(weixinConfig.AppId, HttpUtility.UrlEncode(url), button.RedirectScope, button.RedirectState);
                                    }
                                    menuInfo.Buttons.Add(new MenuViewBtton()
                                    {
                                        Name = button.ButtonName,
                                        Type = button.ButtonType,
                                        Url = url
                                    });
                                    break;
                                case "click":
                                    menuInfo.Buttons.Add(new MenuClickButton()
                                    {
                                        Name = button.ButtonName,
                                        Type = button.ButtonType,
                                        Key = button.ButtonKey
                                    });
                                    break;
                                default:
                                    menuInfo.Buttons.Add(new MenuEventButton()
                                    {
                                        Name = button.ButtonName,
                                        Type = button.ButtonType,
                                        Key = button.ButtonKey
                                    });
                                    break;
                            }
                        }
                        else
                        {
                            MenuSubButton subButton = new MenuSubButton()
                            {
                                Name = button.ButtonName
                            };
                            var subButtons = from x in menus
                                             where x.MenuType.ToLower() == "sub_button" &&
                                                x.ParentMenuID == button.ID
                                             orderby x.OrderNum ascending
                                             select x;
                            foreach (Menu subButton1 in subButtons)
                            {
                                switch (subButton1.ButtonType.ToLower())
                                {
                                    case "view":
                                        string url = subButton1.AccessLink;
                                        if (!string.IsNullOrEmpty(subButton1.RedirectScope))
                                        {
                                            url = WeiXinHelper.GenerateAuthorizeUrl(weixinConfig.AppId, HttpUtility.UrlEncode(url), subButton1.RedirectScope, subButton1.RedirectState);
                                        }       
                                        subButton.SubButtons.Add(new MenuViewBtton()
                                        {
                                            Name = subButton1.ButtonName,
                                            Type = subButton1.ButtonType,
                                            Url = url
                                        });
                                        break;
                                    case "click":
                                        subButton.SubButtons.Add(new MenuClickButton()
                                        {
                                            Name = subButton1.ButtonName,
                                            Type = subButton1.ButtonType,
                                            Key = subButton1.ButtonKey
                                        });
                                        break;
                                    default:
                                        subButton.SubButtons.Add(new MenuEventButton()
                                        {
                                            Name = subButton1.ButtonName,
                                            Type = subButton1.ButtonType,
                                            Key = subButton1.ButtonKey
                                        });
                                        break;
                                }
                            }
                            menuInfo.Buttons.Add(subButton);
                        }
                    }
                    string strJosn = menuInfo.ToJson();
                    weixin.CreateCustomMenu(menuInfo);
                }
            }
        }

        #region 添加微信菜单
        /// <summary>
        /// 添加微信菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddWeiXinMenu(Menu model)
        {
            string sql = @"INSERT INTO [WX_Menu] ([ID],[WXConfigID],[MenuType],[ParentMenuID],[ButtonType],[ButtonName]," +
                "[ButtonKey],[AccessLink],[RedirectScope],[RedirectState],[AddTime],[OrderNum],[Enabled])" +
                 " VALUES (@ID,@WXConfigID,@MenuType,@ParentMenuID,@ButtonType,@ButtonName,@ButtonKey," +
                 "@AccessLink,@RedirectScope,@RedirectState,@AddTime,@OrderNum,@Enabled)";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@WXConfigID", model.WXConfigID),
                new System.Data.SqlClient.SqlParameter("@MenuType", model.MenuType),
                new System.Data.SqlClient.SqlParameter("@ParentMenuID", model.ParentMenuID),
                new System.Data.SqlClient.SqlParameter("@ButtonType", model.ButtonType),
                new System.Data.SqlClient.SqlParameter("@ButtonName", model.ButtonName),
                new System.Data.SqlClient.SqlParameter("@ButtonKey", model.ButtonKey),
                new System.Data.SqlClient.SqlParameter("@AccessLink", model.AccessLink),
                new System.Data.SqlClient.SqlParameter("@RedirectScope", model.RedirectScope),
                new System.Data.SqlClient.SqlParameter("@RedirectState", model.RedirectState),
                new System.Data.SqlClient.SqlParameter("@AddTime", DateTime.Now),
                new System.Data.SqlClient.SqlParameter("@OrderNum", model.OrderNum),
                new System.Data.SqlClient.SqlParameter("@Enabled", model.Enabled)
            };
            int rowsAffected = DbHelperSQL.ExecuteSql(sql.ToString(), paras);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region 修改微信菜单
        /// <summary>
        /// 修改微信菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateWeiXinMenu(Menu model)
        {
            string safesql = " update WX_Menu set ";
            if (model.WXConfigID != null && model.WXConfigID != "")
            {
                safesql += " WXConfigID='" + model.WXConfigID + "',";
            }
            if (model.MenuType != null && model.MenuType != "")
            {
                safesql += " MenuType='" + model.MenuType + "',";
            }
            if (model.ParentMenuID != null && model.ParentMenuID != "")
            {
                safesql += " ParentMenuID='" + model.ParentMenuID + "',";
            }
            if (model.ButtonType != null && model.ButtonType != "")
            {
                safesql += " ButtonType='" + model.ButtonType + "',";
            }
            if (model.ButtonName != null && model.ButtonName != "")
            {
                safesql += " ButtonName='" + model.ButtonName + "',";
            }
            if (model.ButtonKey != null && model.ButtonKey != "")
            {
                safesql += " ButtonKey='" + model.ButtonKey + "',";
            }
            if (model.AccessLink != null && model.AccessLink != "")
            {
                safesql += " AccessLink='" + model.AccessLink + "',";
            }
            if (model.RedirectScope != null && model.RedirectScope != "")
            {
                safesql += " RedirectScope='" + model.RedirectScope + "',";
            }
            if (model.RedirectState != null && model.RedirectState != "")
            {
                safesql += " RedirectState='" + model.RedirectState + "',";
            }
            if (model.OrderNum != null)
            {
                safesql += " OrderNum=" + model.OrderNum + ",";
            }
            else {
                safesql += " OrderNum=0,";
            }
            if (model.Enabled != null)
            {
                safesql += " AccessLink=" + model.Enabled;
            }
            else
            {
                safesql += " AccessLink=1";
            }
            safesql +=" where ID='" + model.ID + "'";
            int rowsAffected = DbHelperSQL.ExecuteSql(safesql.ToString());
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region 获取微信菜单属性
        /// <summary>
        /// 获取微信菜单属性
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="strID"></param>
        /// <returns></returns>
        public object GetWeiXinMenuValue(string strValue, string strID)
        {
            string safesql = string.Empty; ;
            safesql = "select " + strValue + " from WX_Menu where ID='" + strID + "' ";
            return DbHelperSQL.GetSingle(safesql.ToString());
        }
        #endregion
        #region 更新菜单状态
        /// <summary>
        /// 更新菜单状态
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public bool UpdateWeiXinMenuEnabled(string strID)
        {
            StringBuilder strSql = new StringBuilder();
            int state = 0;
            try
            {
                state = Convert.ToInt32(GetWeiXinMenuValue("[Enabled]", strID));
            }
            catch (Exception)
            {
                state = 0;
            }
            switch (state)
            {
                case 0:
                    state = 1;
                    break;
                default:
                    state = 0;
                    break;
            }
            strSql.Append(" UPDATE WX_Menu ");
            strSql.Append(" SET [Enabled] = @Enabled ");
            strSql.Append(" WHERE ID = @ID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@Enabled", state)
            };
            int rowsAffected = DbHelperSQL.ExecuteSql(strSql.ToString(), paras);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region 判断微信菜单是否存在
        /// <summary>
        /// 判断微信菜单是否存在
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public bool ExistWeiXinMenu(string strWhere)
        {
            if (strWhere.Trim() != null && strWhere.Trim() != "")
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" SELECT count(a.ID) ");
                strSql.Append(" from WX_Menu a,WX_WXConfig b where b.ID=a.WXConfigID ");
                strSql.Append(" and " + strWhere);
                return DbHelperSQL.Exists(strSql.ToString());
            }
            return false;
        }
        #endregion
        #region 获取菜单总数
        /// <summary>
        /// 获取菜单总数
        /// </summary>
        /// <param name="strParentID"></param>
        /// <returns></returns>
        public int GetWeiXinMenuCount(string strParentID,string strSiteCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT count(a.ID) ");
            strSql.Append(" from WX_Menu a,WX_WXConfig b where b.ID=a.WXConfigID ");
            if (strParentID.Trim() != null && strParentID.Trim() != "")
            {
                strParentID = "='" + strParentID + "'";
            }
            else
            {
                strParentID = " is null ";
            }
            strSql.Append(" and ParentMenuID " + strParentID);
            if (strSiteCode.Trim() != null && strSiteCode.Trim() != "")
            {
                strSql.Append(" and b.SiteCode='" + strSiteCode + "' ");
            }
            int count = 0;
            try
            {
                count = Convert.ToInt32(DbHelperSQL.GetSingle(strSql.ToString()));
            }
            catch (Exception)
            {
            }
            return count;
        }
        #endregion
        /// <summary>
        /// 根据微信配置ID获取相应的菜单
        /// </summary>
        /// <param name="wxConfigID"></param>
        /// <returns></returns>
        public List<Menu> GetWeiXinMenu(string wxConfigID)
        {
            List<Menu> res=null;
            if (!string.IsNullOrEmpty(wxConfigID))
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("SELECT * FROM [{0}]", TABLE_NAME);
                sql.AppendFormat(" WHERE WXConfigID=@WXConfigID");
                sql.AppendFormat(" AND Enabled=1");
                IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@WXConfigID", wxConfigID)
                };
                DataSet ds = DbHelperSQL.Query(sql.ToString(), paras.ToArray());
                res = ds.ConvertToList<Menu>();
            }
            return res;
        }

        /// <summary>
        /// 根据微信SiteCode获取相应的菜单
        /// </summary>
        /// <param name="strSiteCode"></param>
        /// <returns></returns>
        public List<Menu> GetWeiXinMenuBySiteCode(string strSiteCode)
        {
            List<Menu> res = null;
            WXConfigDAL dal = new WXConfigDAL();
            //WXConfig wxConfig = dal.GetWXConfigByID(strSiteCode);
            WXConfig wxConfig = dal.GetWXConfigBySiteCode(strSiteCode);
            if (wxConfig != null)
            {
                if (!string.IsNullOrEmpty(wxConfig.ID))
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendFormat("SELECT * FROM [{0}]", TABLE_NAME);
                    sql.AppendFormat(" WHERE WXConfigID=@WXConfigID");
                    sql.AppendFormat(" AND Enabled=1");
                    IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@WXConfigID", wxConfig.ID)
                };
                    DataSet ds = DbHelperSQL.Query(sql.ToString(), paras.ToArray());
                    res = ds.ConvertToList<Menu>();
                }
            }
            return res;
        }
    }
}
