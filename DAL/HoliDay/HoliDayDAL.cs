using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Maticsoft.DBUtility;
using Model.HoliDay;

namespace DAL.HoliDay
{
   public class HoliDayDAL
    {
       public HoliDayDAL() { ;}
       #region 节日添加
       /// <summary>
       /// 节日添加
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddHoliday(HD_HoliDay model)
       {
           string sql = @"INSERT INTO [HD_HoliDay] ([ID],[SiteCode],[Htitle],[Himg],[Hcontent],[HstartTime],[HendTime],[HaddTime],[Hisdel])" +
                 " VALUES (@ID,@SiteCode,@Htitle,@Himg,@Hcontent,@HstartTime,@HendTime,@HaddTime,@Hisdel)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@SiteCode", model.SiteCode),
                new System.Data.SqlClient.SqlParameter("@Htitle", model.Htitle),
                new System.Data.SqlClient.SqlParameter("@Himg", model.Himg),
                new System.Data.SqlClient.SqlParameter("@Hcontent", model.Hcontent),
                new System.Data.SqlClient.SqlParameter("@HstartTime", model.HstartTime),
                new System.Data.SqlClient.SqlParameter("@HendTime", model.HendTime),
                new System.Data.SqlClient.SqlParameter("@HaddTime", DateTime.Now),
                new System.Data.SqlClient.SqlParameter("@Hisdel",(model.Hisdel==1?1:0))
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
       #region 节日信息修改
       /// <summary>
       /// 节日信息修改
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateHoliday(HD_HoliDay model)
       {
           string safesql = " update HD_HoliDay set ";
           if (model.SiteCode != null && model.SiteCode != "")
           {
               safesql += " SiteCode='" + model.SiteCode + "',";
           }
           if (model.Htitle != null && model.Htitle != "")
           {
               safesql += " Htitle='" + model.Htitle + "',";
           }
           if (model.Himg != null && model.Himg != "")
           {
               safesql += " Himg='" + model.Himg + "',";
           }
           if (model.Hcontent != null && model.Hcontent != "")
           {
               safesql += " Hcontent='" + model.Hcontent + "',";
           }
           if (model.HstartTime != null && model.HstartTime.ToString() != "")
           {
               safesql += " HstartTime='" + model.HstartTime + "',";
           }
           if (model.HendTime != null && model.HendTime.ToString() != "")
           {
               safesql += " HendTime='" + model.HendTime + "',";
           }
           safesql += " Hisdel=" + (model.Hisdel == 1 ? 1 : 0);
           safesql += " where ID='" + model.ID + "'";
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
       #region 获取节日信息属性
       /// <summary>
       /// 获取节日信息属性
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public object GetHolidayValue(string strValue, string strID)
       {
           string safesql = string.Empty; ;
           safesql = "select " + strValue + " from HD_HoliDay where ID='" + strID + "' ";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion 
       #region 修改节日信息状态
       /// <summary>
       /// 修改节日信息状态
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool  UpdateHolidayState(string strID)
       {
           StringBuilder strSql = new StringBuilder();
            int state = 0;
            try
            {
                state = Convert.ToInt32(GetHolidayValue("[Hisdel]", strID));
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
            strSql.Append(" UPDATE HD_HoliDay ");
            strSql.Append(" SET [Hisdel] = @Hisdel ");
            strSql.Append(" WHERE ID = @ID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@Hisdel", state)
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
       #region 获取有效的节日列表
       /// <summary>
       /// 获取有效的节日列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetHoliDayList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT *,(select COUNT(b.ID) from HD_HoliDayUsers b where b.HID=a.ID) UserCount from HD_HoliDay a WHERE Hisdel=0 ");
           if (strWhere.Trim() != null && strWhere.Trim()!="")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by HaddTime asc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion 
       #region 获取节日详细
       /// <summary>
       /// 获取节日详细
       /// </summary>
       /// <param name="strID"></param>
       /// <param name="strSiteCode"></param>
       /// <returns></returns>
       public DataSet GetHoliDayDateil(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM HD_HoliDay ");
           strSql.Append(" WHERE [ID] = @ID");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion 
       #region 判断节日信息是否存在
       /// <summary>
       /// 判断节日信息是否存在
       /// </summary>
       /// <param name="strHname"></param>
       /// <param name="strSiteCode"></param>
       /// <returns></returns>
       public bool ExistHoliday(string strHname, string strSiteCode)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT count(ID) ");
           strSql.Append(" FROM HD_HoliDay ");
           strSql.Append(" WHERE [SiteCode] = @SiteCode ");
           strSql.Append(" AND [Htitle] = @Htitle ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                    new System.Data.SqlClient.SqlParameter("@Htitle", strHname)
                };
           return DbHelperSQL.Exists(strSql.ToString(), paras.ToArray());
       }
       #endregion
    }
}
