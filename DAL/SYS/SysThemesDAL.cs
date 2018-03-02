using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Maticsoft.DBUtility;
using Model.SYS;

namespace DAL.SYS
{
   public class SysThemesDAL
    {
       public SysThemesDAL() { ; }

       #region 获取所有主题列表
       /// <summary>
       /// 获取所有主题列表
       /// </summary>
       /// <param name="where">条件</param>
       /// <returns></returns>
       public DataSet GetSysThemesList(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM SYS_Themes ");
           if (!string.IsNullOrEmpty(where)&&where.Trim()!=null&&where.Trim()!="")
           {
               strSql.Append(" WHERE " + where);
           }
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion

       #region 获取生效的主题
       /// <summary>
       /// 获取生效的主题
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetSysThemesListByState(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM SYS_Themes where isstate=1 ");
           if (!string.IsNullOrEmpty(where) && where.Trim() != null && where.Trim() != "")
           {
               strSql.Append(" AND " + where);
           }
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion

       #region 获取单条数据
       /// <summary>
       /// 获取所有主题列表
       /// </summary>
       /// <param name="where">条件</param>
       /// <returns></returns>
       public DataSet GetSysThemesds(string themesid)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM SYS_Themes WHERE ID=@ID");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", themesid),
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion

       #region 添加主题
       /// <summary>
       /// 添加主题
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddSysTemes(SysThemes model)
       {
           string sql = @"INSERT INTO [SYS_Account_Ext]
                        ([ID]
                        ,[Name]
                        ,[Value]
                        ,[IsState])
                 VALUES
                        (@ID
                       ,@Name
                       ,@Value
                       ,@IsState)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@Name", model.Name),
                new System.Data.SqlClient.SqlParameter("@Value", model.Value),
                new System.Data.SqlClient.SqlParameter("@IsState", model.IsState)
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

       #region 根据ID获取相应的值
       /// <summary>
       /// 根据ID获取相应的值
       /// </summary>
       /// <param name="value"></param>
       /// <param name="ID"></param>
       /// <returns></returns>
       public object GetThemesvaluesById(string value, string ID)
       {
           object obj = null;
           if (value.Trim() != null && value.Trim() != "")
           {
               string safesql = " select "+value+" from sys_themes where id='"+ID+"' ";
               obj = DbHelperSQL.GetSingle(safesql);
           }
           return obj;
       }
       #endregion

       #region 更新主题信息
       /// <summary>
       /// 更新主题信息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateSysThemes(SysThemes model)
       {
           string safesql = " update SYS_Themes set ";
           if (model.Name != null && model.Name != "")
           {
               safesql += " name='"+model.Name+"',";
           }
           if (model.Value != null && model.Value != "")
           {
               safesql += " Value='" + model.Value + "',";
           }
           safesql += " isstate="+model.IsState+" where id='"+model.ID+"'";
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
    }
}
