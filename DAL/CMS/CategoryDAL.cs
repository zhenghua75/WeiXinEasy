using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.CMS
{
    public class CategoryDAL
    {
        #region 返回所有类别信息列表
        /// <summary>
        /// 返回所有类别信息列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public DataSet GetAllCategory(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM Cms_Category ");
            strSql.Append(" WHERE isDel = 0 ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" AND " + strWhere);
            }
            strSql.Append(" ORDER BY [Order]");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        } 
        #endregion

        #region 返回某站点类别信息列表
        /// <summary>
        /// 返回某站点类别信息列表
        /// </summary>
        /// <param name="strID">类别ID</param>
        /// <returns></returns>
        public DataSet GetCategoryByID(string strID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT [SiteCode],[Name],[Pic],Summary,[Content],[Link],[Order] ");
            strSql.Append(" FROM Cms_Category ");
            strSql.Append(" WHERE ID = @ID");
            strSql.Append(" ORDER BY [Order] ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[] 
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
            };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(),paras.ToArray());
            return ds;
        } 
        #endregion

        #region 添加类别
        /// <summary>
        /// 添加类别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddCategoryData(Model.CMS.CMS_Category model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
            strSql1.Append("ID,");
            strSql2.Append(String.Format("'{0}',", model.ID));
            if (model.SiteCode != null)
            {
                strSql1.Append("SiteCode,");
                strSql2.Append(String.Format("'{0}',", model.SiteCode));
            }
            if (model.Name != null)
            {
                strSql1.Append("Name,");
                strSql2.Append(String.Format("'{0}',", model.Name));
            }
            if (model.Pic != null)
            {
                strSql1.Append("Pic,");
                strSql2.Append(String.Format("'{0}',", model.Pic));
            }
            if (model.Summary != null)
            {
                strSql1.Append("Summary,");
                strSql2.Append(String.Format("'{0}',", model.Summary));
            }
            if (model.Content != null)
            {
                strSql1.Append("Content,");
                strSql2.Append(String.Format("'{0}',", model.Content));
            }
            if (model.Link != null)
            {
                strSql1.Append("Link,");
                strSql2.Append(String.Format("'{0}',", model.Link));
            }
            if (model.Order > 0)
            {
                strSql1.Append("[Order],");
                strSql2.Append(String.Format("{0},", model.Order));
            }
            strSql1.Append("IsDel,");
            strSql2.Append((model.IsDel ? 1 : 0) + ",");
            strSql.Append("INSERT INTO Cms_Category(");
            strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
            strSql.Append(")");
            strSql.Append(" VALUES (");
            strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
            strSql.Append(");");
            strSql.Append(String.Format(" UPDATE SYS_Account SET ID = '{0}'", model.ID));
            strSql.Append(String.Format(" WHERE ID ='{0}' ", model.ID));
            int rowsAffected = DbHelperSQL.ExecuteSql(strSql.ToString());
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

        #region 更新类别
        /// <summary>
        /// 更新类别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCategoryData(Model.CMS.CMS_Category model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Cms_Category SET ");
            if (model.Name != null)
            {
                strSql.Append("Name='" + model.Name + "',");
            }
            if (!string.IsNullOrEmpty(model.Pic))
            {
                strSql.Append("Pic='" + model.Pic + "',");
            }
            if (model.Content != null)
            {
                strSql.Append("Content='" + model.Content + "',");
            }
            if (model.Link != null)
            {
                strSql.Append("Link='" + model.Link + "',");
            }
            if (model.Order > 0)
            {
                strSql.Append("[Order]=" + model.Order + ",");
            }
            int n = strSql.ToString().LastIndexOf(",");
            strSql.Remove(n, 1);
            strSql.Append(" WHERE ID ='" + model.ID + "' ");
            int rowsAffected = DbHelperSQL.ExecuteSql(strSql.ToString());
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

        #region 修改文章状态
        /// <summary>
        /// 修改文章类别状态
        /// </summary>
        /// <param name="strArticleID">文章类别ID</param>
        /// <returns></returns>
        public bool UpdateCategoryState(string strCategoryID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" UPDATE Cms_Category SET ");
            strSql.Append(" IsDel = 1 ");
            strSql.Append(" WHERE ID ='" + strCategoryID + "' ");
            int rowsAffected = DbHelperSQL.ExecuteSql(strSql.ToString());
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
