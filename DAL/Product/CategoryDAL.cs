using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maticsoft.DBUtility;

namespace DAL.Product
{
    public class CategoryDAL
    {
        #region 返回站点所有商品类别
        /// <summary>
        /// 返回所有类别信息列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public DataSet GetSPCategory(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM SP_Category ");
            strSql.Append(" WHERE isDel = 0 ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" AND " + strWhere);
            }
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 插入新的商品分类信息
        /// <summary>
        /// 插入新的商品分类信息
        /// </summary>
        /// <param name="info"></param>
        public int InsertInfo(Model.SP.SP_Category info)
        {
            string sql = @"INSERT INTO [SP_Category]
                        ([ID]
                        ,[Name]
                        ,[SiteCode]
                        ,[IsDel]
                        ,[Desc]
                        )
                 VALUES
                        (@ID
                       ,@Name
                       ,@SiteCode
                       ,0
                       ,''
                       )";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", info.ID),
                new System.Data.SqlClient.SqlParameter("@Name", info.Name),
                new System.Data.SqlClient.SqlParameter("@SiteCode", info.SiteCode)
            };
            return DbHelperSQL.ExecuteSql(sql, paras);
        }
        #endregion

        #region 更新商品分类信息
        /// <summary>
        /// 更新商品信息
        /// </summary>
        /// <param name="model">商品信息</param>
        /// <returns></returns>
        public bool UpdateSPCategoryData(Model.SP.SP_Category model)
        {
            //[Name],[Photo],[Unit],[NormalPrice],[MemberPrice],[Pdate],[CatID],[Desc],[State],[SiteCode],[StartTime],[EndTime],[IsTop],[Credits]
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE SP_Category SET ");
            if (!string.IsNullOrEmpty(model.Name))
            {
                strSql.Append("Name='" + model.Name + "',");
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

        #region 修改商品分类状态
        /// <summary>
        /// 修改商品状态
        /// </summary>
        /// <param name="strProductID">商品信息</param>
        /// <returns></returns>
        public bool UpdateSPCategoryState(string strProductID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE SP_Category SET ");
            strSql.Append(" [IsDel] = 1 ");
            strSql.Append(" WHERE ID =@ID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strProductID)
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

        #region 返回商品分类详细信息
        /// <summary>
        /// 返回商品分类详细信息
        /// </summary>
        /// <param name="strSPCategoryID">商品分类ID</param>
        /// 
        public DataSet GetSPCategoryDetail(string strSPCategoryID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM SP_Category ");
            strSql.Append(" WHERE [ID] =@SPCategoryID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SPCategoryID", strSPCategoryID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());

        }
        #endregion
    }
}
