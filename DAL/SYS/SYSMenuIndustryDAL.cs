using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.SYS;
using Maticsoft.DBUtility;

namespace DAL.SYS
{
   public class SYSMenuIndustryDAL
    {
       public SYSMenuIndustryDAL() { ;}
       #region 添加行业菜单
       /// <summary>
       /// 添加行业菜单
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddSYSMenuIndustry(SYSMenuIndustry model)
       {
           string sql = @"INSERT INTO [SYS_MenuIndustry](MenuNo,Category)VALUES (@MenuNo,@Category)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@MenuNo", model.MenuNo),
                new System.Data.SqlClient.SqlParameter("@Category", model.Category)
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
       #region 根据行业类型返回菜单列表
       /// <summary>
       /// 根据行业类型返回菜单列表
       /// </summary>
       /// <param name="strCategory"></param>
       /// <returns></returns>
       public DataSet GetSYSMenuIndustryByCategory(string strCategory)
       {
           StringBuilder strSql = new StringBuilder();
           List<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>();
           if (strCategory != null && strCategory!="")
           {
               strSql.Append(" SELECT a.[No],a.[Name],a.[Order],a.[Parent] AS vcParent,a.[Level],a.[Url],a.[Icon],a.[target] ");
               strSql.Append(" FROM SYS_Menu a, SYS_MenuIndustry b ");
               strSql.Append(" WHERE a.No = b.MenuNo ");
               strSql.Append(" AND b.Category = @Category ");
               paras.Add(
                   new System.Data.SqlClient.SqlParameter("@Category", strCategory));
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
       #region 根据行业类别获取所有菜单
       /// <summary>
       /// 根据行业类别获取所有菜单
       /// </summary>
       /// <param name="strCateGory"></param>
       /// <returns></returns>
       public string GetSYSMenuByCategory(string strCateGory)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT Category,MenuNo=STUFF( ");
           strSql.Append(" (SELECT ','+ MenuNo FROM SYS_MenuIndustry t WHERE Category=ml.Category FOR XML PATH('')  ");
           strSql.Append(" 	), 1, 1, '') ");
           strSql.Append(" FROM SYS_MenuIndustry AS ml WHERE Category= @Category ");
           strSql.Append(" GROUP BY Category");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@Category", strCateGory)
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
       #region 根据条件返回行业类型菜单
       /// <summary>
       /// 根据条件返回行业类型菜单
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetSYSMenuIndustryByWhere(string strWhere)
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
       #region 修改行业菜单
       /// <summary>
       /// 修改行业菜单
       /// </summary>
       /// <param name="strMenuNo"></param>
       /// <param name="strCategory"></param>
       /// <returns></returns>
       public int UpdateSYSMenuIndustry(string strCategory, string strMenuNo)
       {
           int result = 0;
           StringBuilder strSql = new StringBuilder();
           strSql.Length = 0;
           strSql.Append(" DECLARE @Result AS INT; ");
           strSql.Append(" BEGIN TRAN; ");
           strSql.Append(" DELETE SYS_MenuIndustry WHERE Category = '" + strCategory + "'; ");
           strSql.Append(" IF @@error <> 0 GOTO RB; ");
           if (strMenuNo != null && strMenuNo.Length > 0)
           {
               string[] arrTeam = strMenuNo.Split(',');
               for (int i = 0; i < arrTeam.Count(); i++)
               {
                   strSql.Append(" INSERT INTO SYS_MenuIndustry(MenuNo,Category) VALUES ('" + arrTeam[i] + "','" + strCategory + "'); ");
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
       #region 判断某菜单是否存在
       /// <summary>
       /// 判断某菜单是否存在
       /// </summary>
       /// <param name="strMenuNo"></param>
       /// <param name="strCatrgory"></param>
       /// <returns></returns>
       public bool ExistMenu(string strMenuNo,string strCatrgory)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT count(*) ");
           strSql.Append(" FROM SYS_MenuIndustry ");
           strSql.Append(" WHERE [MenuNo] = @MenuNo ");
           strSql.Append(" AND [Category] = @Category ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@MenuNo", strMenuNo),
                    new System.Data.SqlClient.SqlParameter("@Category", strCatrgory)
                };
           return DbHelperSQL.Exists(strSql.ToString(), paras.ToArray());
       }
       #endregion 
    }
}
