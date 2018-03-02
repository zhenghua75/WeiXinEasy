using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.SYS;
using Maticsoft.DBUtility;

namespace DAL.SYS
{
   public class SYSMenuSiteCodeDAL
    {
       public SYSMenuSiteCodeDAL() { ;}
       #region 添加定制菜单
       /// <summary>
       /// 添加定制菜单
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddSYSMenuSiteCode(SYSMenuSiteCode model)
       {
           string sql = @"INSERT INTO [SYS_MenuSiteCode](MenuNo,SiteCode)VALUES (@MenuNo,@SiteCode)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@MenuNo", model.MenuNo),
                new System.Data.SqlClient.SqlParameter("@SiteCode", model.SiteCode)
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
       #region 返回当前用户定制菜单
       /// <summary>
       /// 返回当前用户定制菜单
       /// </summary>
       /// <param name="strSiteCode"></param>
       /// <returns></returns>
       public DataSet GetSYSMenuSiteCodeBySiteCode(string strSiteCode)
       {
           StringBuilder strSql = new StringBuilder();
           List<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>();
           if (strSiteCode != "ADMIN")
           {
               strSql.Append(" SELECT a.[No],a.[Name],a.[Order],a.[Parent] AS vcParent,a.[Level],a.[Url],a.[Icon],a.[target] ");
               strSql.Append(" FROM SYS_Menu a, SYS_MenuSiteCode b ");
               strSql.Append(" WHERE a.No = b.MenuNo ");
               strSql.Append(" AND b.SiteCode = @SiteCode ");
               paras.Add(
                   new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode));
           }
           //else
           //{
           //    strSql.Append(" SELECT a.[No],a.[Name],a.[Order],a.[Parent] AS vcParent,a.[Level],a.[Url],a.[Icon],a.[target] ");
           //    strSql.Append(" FROM SYS_Menu a ");
           //}
           strSql.Append(" ORDER BY a.No,a.[Order] ");
           DataSet ds = null;
           if (paras.Count > 0)
           {
               ds = DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
           }
           else
           {
               ds = DbHelperSQL.Query(strSql.ToString());
           }
           return ds;
       }
       #endregion
       #region 根据条件返回菜单
       /// <summary>
       /// 根据条件返回菜单
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetSYSMenuSiteCodeByWhere(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT a.[No],a.[Name],a.[Order],a.[Parent] AS vcParent,a.[Level],a.[Url],a.[Icon],a.[target] ");
           strSql.Append(" FROM SYS_Menu a");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append(" WHERE " + strWhere);
           }
           strSql.Append(" ORDER BY a.[No],a.[Order] ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 根据SiteCode获取相应的菜单
       /// <summary>
       /// 根据SiteCode获取相应的菜单
       /// </summary>
       /// <param name="strSiteCode"></param>
       /// <returns></returns>
       public string GetStringSYSMenuSiteCode(string strSiteCode)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT SiteCode,MenuNo=STUFF( ");
           strSql.Append(" (SELECT ','+ MenuNo FROM SYS_MenuSiteCode t WHERE SiteCode=ml.SiteCode FOR XML PATH('')  ");
           strSql.Append(" 	), 1, 1, '') ");
           strSql.Append(" FROM SYS_MenuSiteCode AS ml WHERE SiteCode= @SiteCode ");
           strSql.Append(" GROUP BY SiteCode");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode)
            };
           DataSet ds = DbHelperSQL.Query(strSql.ToString(), paras);
           if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
           {
               return ds.Tables[0].Rows[0]["MenuNo"].ToString();
           }
           else
           {
               return "";
           }
       }
       #endregion
       #region 修改定制菜单权限
       /// <summary>
       /// 修改定制菜单权限
       /// </summary>
       /// <param name="strMenuNo"></param>
       /// <param name="strSiteCode"></param>
       /// <returns></returns>
       public int UpdateSYSMenuSiteCode(string strSiteCode, string strMenuNo)
       {
           int result = 0;
           StringBuilder strSql = new StringBuilder();
           strSql.Length = 0;
           strSql.Append(" DECLARE @Result AS INT; ");
           strSql.Append(" BEGIN TRAN; ");
           strSql.Append(" DELETE SYS_MenuSiteCode WHERE SiteCode = '" + strSiteCode + "'; ");
           strSql.Append(" IF @@error <> 0 GOTO RB; ");
           if (strMenuNo != null && strMenuNo.Length > 0)
           {
               string[] arrTeam = strMenuNo.Split(',');
               for (int i = 0; i < arrTeam.Count(); i++)
               {
                   strSql.Append(" INSERT INTO SYS_MenuSiteCode(MenuNo,SiteCode) VALUES ('" + arrTeam[i] + "','" + strSiteCode + "'); ");
                   strSql.Append(" IF (@@ROWCOUNT = 0 OR @@error <> 0) GOTO RB; ");
               }
           }
           strSql.Append(" COMMIT TRAN; ");
           strSql.Append(" SET @Result = 1; ");
           strSql.Append(" GOTO ED; ");
           // 回滚事务
           strSql.Append(" RB: ");
           strSql.Append(" ROLLBACK TRAN; ");
           strSql.Append(" SET @Result = 0; ");
           // 返回结果
           strSql.Append(" ED: ");
           strSql.Append(" SELECT @Result; ");
           result = Convert.ToInt32(DbHelperSQL.GetSingle(strSql.ToString()));
           return result;
       }
       #endregion
    }
}
