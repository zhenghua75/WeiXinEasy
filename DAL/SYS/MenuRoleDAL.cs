using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maticsoft.DBUtility;

namespace DAL.SYS
{
    public class MenuRoleDAL
    {
        public MenuRoleDAL() { ; }

        #region 返回当前角色的菜单
        /// <summary>
        /// 返回当前角色的菜单
        /// </summary>
        /// <param name="strRoleID">角色ID</param>
        /// 
        public DataSet GetMenuDate(string strRoleID)
        {
            StringBuilder strSql = new StringBuilder();
            List<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>();
            if (strRoleID != "ADMIN")
            {
                strSql.Append(" SELECT a.[No],a.[Name],a.[Order],a.[Parent] AS vcParent,a.[Level],a.[Url],a.[Icon],a.[target] ");
                strSql.Append(" FROM SYS_Menu a, SYS_MenuRole b ");
                strSql.Append(" WHERE a.No = b.MenuNo ");
                //strSql.Append(" AND b.RoleNo = '" + strRoleID + "' ");     
                strSql.Append(" AND b.RoleNo = @RoleNo ");
                paras.Add(
                    new System.Data.SqlClient.SqlParameter("@RoleNo", strRoleID));
            }
            else
            {
                strSql.Append(" SELECT a.[No],a.[Name],a.[Order],a.[Parent] AS vcParent,a.[Level],a.[Url],a.[Icon],a.[target] ");
                strSql.Append(" FROM SYS_Menu a ");
            }
            strSql.Append(" ORDER BY a.No,a.[Order] ");
            DataSet ds = null;
            if (paras.Count>0)
            {
                ds = DbHelperSQL.Query(strSql.ToString(),paras.ToArray());
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
        /// <param name="strWhere">条件语句</param>
        /// 
        public DataSet GetMenuListByWhere(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT a.[No],a.[Name],a.[Order],a.[Parent] AS vcParent,a.[Level],a.[Url],a.[Icon],a.[target] ");
            strSql.Append(" FROM SYS_Menu a ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ORDER BY a.[No],a.[Order] ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 返回所有角色
        /// <summary>
        /// 返回当前角色的菜单
        /// </summary>
        /// 
        public DataSet GetRoleList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM SYS_Role ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 取角色的所有菜单
        /// <summary>
        /// 取角色的所有菜单
        /// </summary>
        /// <param name="strRoleID">角色ID</param>
        /// 
        //public DataSet GetMenuByRole(string strRoleID)
        public string GetMenuByRole(string strRoleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT RoleNo ");
            strSql.Append(" ,MenuNo=STUFF( ");
            strSql.Append(" ( ");
            strSql.Append(" 	SELECT ','+ MenuNo FROM SYS_MenuRole t WHERE RoleNo=ml.RoleNo FOR XML PATH('')  ");
            strSql.Append(" 	), 1, 1, '') ");
            strSql.Append(" FROM SYS_MenuRole AS ml ");
            //strSql.Append(" WHERE RoleNo = '" + strRoleID + "' ");
            strSql.Append(" WHERE RoleNo = @RoleNo ");
            strSql.Append(" GROUP BY RoleNo");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@RoleNo", strRoleID)
            };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(),paras);
            //return ds;
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

        #region 修改角色权限
        /// <summary>
        /// 修改角色权限
        /// </summary>
        /// <param name="strRoleId">角色ID</param>
        /// <param name="strMenu">菜单串</param>
        public int UpdateRoleMenu(string strRoleId, string strMenu)
        {
            int result = 0;

            StringBuilder strSql = new StringBuilder();
            strSql.Length = 0;
            strSql.Append(" DECLARE @Result AS INT; ");
            strSql.Append(" BEGIN TRAN; ");
            strSql.Append(" DELETE SYS_MenuRole WHERE RoleNo = '" + strRoleId + "'; ");
            strSql.Append(" IF @@error <> 0 GOTO RB; ");

            if (strMenu != null && strMenu.Length > 0)
            {
                string[] arrTeam = strMenu.Split(',');
                for (int i = 0; i < arrTeam.Count(); i++)
                {
                    strSql.Append(" INSERT INTO SYS_MenuRole(MenuNo,RoleNo) VALUES ('" + arrTeam[i] + "','" + strRoleId + "'); ");
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
