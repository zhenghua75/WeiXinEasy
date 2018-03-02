using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.SYS;
using Maticsoft.DBUtility;

namespace DAL.SYS
{
   public class SysCategoryDAL
    {
       public SysCategoryDAL() { ;}

       #region 添加站点类别
       /// <summary>
       /// 添加站点类别
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddSysCateGory(SysCategory model)
       {
           string sqlname = ""; string sqlvalue = "";
           if (model.ID != null && model.ID != "")
           {
               sqlname += "ID,"; sqlvalue += "'" + model.ID + "',";
           }
           if (model.SiteName != null && model.SiteName != "")
           {
               sqlname += "SiteName,"; sqlvalue += "'" + model.SiteName + "',";
           }
           if (model.SiteDesc != null && model.SiteDesc != "")
           {
               sqlname += "SiteDesc,"; sqlvalue += "'" + model.SiteDesc + "',";
           }
           if (model.SiteOrder > -1)
           {
               sqlname += "SiteOrder,"; sqlvalue += model.SiteOrder+",";
           }
           sqlname += "IsDel"; sqlvalue +=(model.IsDel?0:1);
           sqlname = "insert into Sys_Category(" + sqlname + ") values(" + sqlvalue + ")";
           int rowsAffected = DbHelperSQL.ExecuteSql(sqlname.ToString());
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

       #region 更新站点类别
       /// <summary>
       /// 更新站点类别
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateSysCateGory(SysCategory model)
       {
           string safesql = " update Sys_Category set ";
           if (model.SiteName != null && model.SiteName != "")
           {
               safesql += " SiteName='" + model.SiteName + "',";
           }
           if (model.SiteDesc != null && model.SiteDesc != "")
           {
               safesql += " SiteDesc='" + model.SiteDesc + "',";
           }
           if (model.SiteOrder >0)
           {
               safesql += " SiteOrder=" + model.SiteOrder + ",";
           }
           safesql += " IsDel=" + (model.IsDel ? 0 : 1) + " where id='" + model.ID + "'";
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

       #region 返回所有站点类别
       /// <summary>
       /// 返回所有站点类别
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetSysCategoryList(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM Sys_Category ");
           if (!string.IsNullOrEmpty(where) && where.Trim() != null && where.Trim() != "")
           {
               strSql.Append(" WHERE " + where);
           }
           strSql.Append(" ORDER BY SiteOrder ASC");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion

       #region 返回生效的站点类别
       /// <summary>
       /// 返回生效的站点类别
       /// </summary>
       /// <param name="where">条件</param>
       /// <returns></returns>
       public DataSet GetNoDelSysCateGoryLsit(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM Sys_Category WHERE IsDel=0 ");
           if (!string.IsNullOrEmpty(where) && where.Trim() != null && where.Trim() != "")
           {
               strSql.Append(" AND " + where);
           }
           strSql.Append(" ORDER BY SiteOrder ASC");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion

       #region 根据ID获取相应的值
       /// <summary>
       /// 根据ID获取相应的值
       /// </summary>
       /// <param name="value"></param>
       /// <param name="syscategoryId"></param>
       /// <returns></returns>
       public object GetSysCategoryByValue(string value, string syscategoryId)
       {
           object obj = null;
           if (value.Trim() != null && value.Trim() != "")
           {
               string safesql = " select " + value + " from Sys_Category where id='" + syscategoryId + "' ";
               obj = DbHelperSQL.GetSingle(safesql);
           }
           return obj;
       }
       #endregion

       #region 获取排序最大值
       /// <summary>
       /// 获取排序最大值
       /// </summary>
       /// <returns></returns>
       public int GetMaxOrder()
       {
           string safesql = " select max(SiteOrder) from Sys_Category  ";
           int max = 0;
           try
           {
               max = Convert.ToInt32(DbHelperSQL.GetSingle(safesql));
           }
           catch (Exception)
           {
               max = 0;
           }
           return max;
       }
       #endregion

       #region 获取单条类别详细
       /// <summary>
       /// 获取单条类别详细
       /// </summary>
       /// <param name="syscategoryid"></param>
       /// <returns></returns>
       public DataSet GetSysCateGoryDetail(string syscategoryid)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM Sys_Category WHERE ID='" + syscategoryid + "' ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
    }
}
