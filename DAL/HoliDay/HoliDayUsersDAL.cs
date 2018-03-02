using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.HoliDay;
using Maticsoft.DBUtility;

namespace DAL.HoliDay
{
   public class HoliDayUsersDAL
    {
       public HoliDayUsersDAL() { ;}
       #region 用户注册
       /// <summary>
       /// 用户注册
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddHolidayUsers(HD_HoliDayUsers model)
       {
           string sql = @"INSERT INTO [HD_HoliDayUsers] ([ID],[HID],[OpenID],[Phone],[NickName],[Married],[Age],[AddTime],[IsDel])" +
                " VALUES (@ID,@HID,@OpenID,@Phone,@NickName,@Married,@Age,@AddTime,@IsDel)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@HID", model.HID),
                new System.Data.SqlClient.SqlParameter("@OpenID", model.OpenID),
                new System.Data.SqlClient.SqlParameter("@Phone", model.Phone),
                new System.Data.SqlClient.SqlParameter("@NickName", model.NickName),
                new System.Data.SqlClient.SqlParameter("@Married", model.Married),
                new System.Data.SqlClient.SqlParameter("@Age", model.Age),
                new System.Data.SqlClient.SqlParameter("@AddTime", DateTime.Now),
                new System.Data.SqlClient.SqlParameter("@IsDel",(model.IsDel==1?1:0))
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
       #region 用户信息修改
       /// <summary>
       /// 用户信息修改
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateHoliDayUsers(HD_HoliDayUsers model)
       {
           string safesql = " update HD_HoliDayUsers set ";
           if (model.HID != null && model.HID != "")
           {
               safesql += " HID='" + model.HID + "',";
           }
           if (model.OpenID != null && model.OpenID != "")
           {
               safesql += " OpenID='" + model.OpenID + "',";
           }
           if (model.Phone != null && model.Phone != "")
           {
               safesql += " Phone='" + model.Phone + "',";
           }
           if (model.NickName != null && model.NickName.ToString() != "")
           {
               safesql += " NickName='" + model.NickName + "',";
           }
           if (model.Married != null && model.Married.ToString() != "")
           {
               safesql += " Married='" + model.Married + "',";
           }
           if (model.Age != null && model.Age.ToString() != "")
           {
               safesql += " Age=" + model.Age + ",";
           }
           safesql += " IsDel=" + (model.IsDel == 1 ? 1 : 0);
           safesql += " where ID='" + model.ID + "' ";
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
       #region 获取用户属性
       /// <summary>
       /// 获取用户属性
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public object GetHoliDayUsersValue(string strValue, string strID)
       {
           string safesql = string.Empty; ;
           safesql = "select " + strValue + " from HD_HoliDayUsers where ID='" + strID + "' ";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion 
       #region 修改用户状态
       /// <summary>
       /// 修改用户状态
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateHDUsersSdate(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetHoliDayUsersValue("[IsDel]", strID));
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
           strSql.Append(" UPDATE HD_HoliDayUsers ");
           strSql.Append(" SET [IsDel] = @IsDel ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@IsDel", state)
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
       #region 获取有效的用户列表
       /// <summary>
       /// 获取有效的用户列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetHoliDayUsesList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT a.*,b.Htitle from HD_HoliDayUsers a,HD_HoliDay b WHERE a.IsDel=0 ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion 
       #region 获取用户详细
       /// <summary>
       /// 获取用户详细
       /// </summary>
       /// <param name="strUid"></param>
       /// <param name="strSiteCode"></param>
       /// <returns></returns>
       public DataSet GetHoliDayUsersDetail(string strUid, string strSiteCode)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT a.* ");
           strSql.Append(" FROM HD_HoliDay a,HD_HoliDay b ");
           strSql.Append(" where a.HID=b.ID and b.SiteCode=@SiteCode and a.ID=@ID ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strUid),
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion 
       #region 判断用户是否存在
       /// <summary>
       /// 判断用户是否存在
       /// </summary>
       /// <param name="strOpenID"></param>
       /// <param name="Phone"></param>
       /// <param name="strSiteCode"></param>
       /// <returns></returns>
       public bool ExistHoliDayUsers(string strOpenID, string Phone, string strSiteCode)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT count(a.ID) ");
           strSql.Append(" FROM HD_HoliDayUsers a,HD_HoliDay b WHERE a.IsDel=0 ");
           if (strOpenID.Trim() != null && strOpenID.Trim() != "")
           {
               strSql.Append(" and a.OpenID = '" + strOpenID + "' ");
           }
           if (Phone.Trim() != null && Phone.Trim() != "")
           {
               strSql.Append(" and a.Phone = '" + Phone + "' ");
           }
           if (strSiteCode.Trim() != null && strSiteCode.Trim() != "")
           {
               strSql.Append(" and b.SiteCode = '" + strSiteCode + "' ");
           }
           return DbHelperSQL.Exists(strSql.ToString());
       }
       #endregion 
    }
}
