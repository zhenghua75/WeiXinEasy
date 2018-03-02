using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maticsoft.DBUtility;
using Model.SP;

namespace DAL.Product
{
    public class ProductDAL
    {
        public ProductDAL() { ; }

        #region 返回商品列表
        /// <summary>
        /// 返回商品列表
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strCatID">商品类别ID</param>
        /// 
        public DataSet GetProductList(string strSiteCode,string strCatID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT [ID],[Name],[Photo],[Unit],[NormalPrice],[MemberPrice],[Pdate],[CatID],[Desc],[State],[SiteCode],[StartTime],[EndTime],[IsTop],[Credits] ");
            strSql.Append(" FROM SP_Product ");
            strSql.Append(" WHERE SiteCode = @SiteCode ");
            if (!string.IsNullOrEmpty(strCatID))
            {
                strSql.Append(" AND [CatID] = @CatID ");
            }
            strSql.Append(" ORDER BY [Order] ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                    new System.Data.SqlClient.SqlParameter("@CatID", strCatID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 返回商品列表
        /// <summary>
        /// 返回商品列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// 
        public DataSet GetProductList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT a.*,b.Name AS CatName ");
            strSql.Append(" FROM SP_Product a ");
            strSql.Append(" LEFT JOIN SP_Category b ON (b.ID = a.CatID) ");
            strSql.Append(" WHERE a.[State] = 0 ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" AND " + strWhere );
            }            
            strSql.Append(" ORDER BY a.[Order] ");
            return DbHelperSQL.Query(strSql.ToString());
        }
        #endregion

        #region 返回商品详细信息
        /// <summary>
        /// 返回商品详细信息
        /// </summary>
        /// <param name="strProductID">商品ID</param>
        /// 
        public DataSet GetProductDetail(string strProductID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT [ID],[Name],[Photo],[Unit],[NormalPrice],[MemberPrice],[Pdate],[CatID],[Desc],[State],[SiteCode],[StartTime],[EndTime],[IsTop],[Credits],[Order] ");
            strSql.Append(" FROM SP_Product ");
            strSql.Append(" WHERE [ID] =@ProductID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ProductID", strProductID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());

        }
        #endregion

        #region 插入新的商品信息
        /// <summary>
        /// 插入新的商品信息
        /// </summary>
        /// <param name="info"></param>
        public int InsertInfo(SP_Product info)
        {
            string sql = @"INSERT INTO [SP_Product]
                        ([ID]
                        ,[Name]
                        ,[Photo]
                        ,[Unit]
                        ,[NormalPrice]
                        ,[MemberPrice]
                        ,[Pdate]
                        ,[CatID]
                        ,[Desc]
                        ,[State]
                        ,[SiteCode]
                        ,[StartTime]
                        ,[EndTime]
                        ,[IsTop]
                        ,[Credits]
                        ,[Order])
                 VALUES
                        (@ID
                       ,@Name
                       ,@Photo
                       ,@Unit
                       ,@NormalPrice
                       ,@MemberPrice
                       ,@Pdate
                       ,@CatID
                       ,@Desc
                       ,@State
                       ,@SiteCode
                       ,@StartTime
                       ,@EndTime
                       ,@IsTop
                       ,@Credits
                       ,@Order)";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", info.ID),
                new System.Data.SqlClient.SqlParameter("@Name", info.Name),
                new System.Data.SqlClient.SqlParameter("@Photo", info.Photo),
                new System.Data.SqlClient.SqlParameter("@Unit", info.Unit),
                new System.Data.SqlClient.SqlParameter("@NormalPrice", info.NormalPrice),
                new System.Data.SqlClient.SqlParameter("@MemberPrice", info.MemberPrice),
                new System.Data.SqlClient.SqlParameter("@Pdate", info.Pdate),
                new System.Data.SqlClient.SqlParameter("@CatID", info.CatID),
                new System.Data.SqlClient.SqlParameter("@Desc", info.Desc),
                new System.Data.SqlClient.SqlParameter("@State", info.State),
                new System.Data.SqlClient.SqlParameter("@SiteCode", info.SiteCode),
                new System.Data.SqlClient.SqlParameter("@StartTime", info.StartTime),
                new System.Data.SqlClient.SqlParameter("@EndTime", info.EndTime),
                new System.Data.SqlClient.SqlParameter("@IsTop", info.IsTop),
                new System.Data.SqlClient.SqlParameter("@Credits", info.Credits),
                new System.Data.SqlClient.SqlParameter("@Order", info.Order)
            };
            return DbHelperSQL.ExecuteSql(sql, paras);

            //StringBuilder strSql = new StringBuilder();
            //strSql.Append(String.Format(" UPDATE SP_Product SET ID = '{0}'", info.ID));
            //strSql.Append(String.Format(" WHERE ID ='{0}' ", info.ID));
            //int rowsAffected = DbHelperSQL.ExecuteSql(strSql.ToString());
            //if (rowsAffected > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }
        #endregion

        #region 更新商品信息
        /// <summary>
        /// 更新商品信息
        /// </summary>
        /// <param name="model">商品信息</param>
        /// <returns></returns>
        public bool UpdateProductData(SP_Product model)
        {
            //[Name],[Photo],[Unit],[NormalPrice],[MemberPrice],[Pdate],[CatID],[Desc],[State],[SiteCode],[StartTime],[EndTime],[IsTop],[Credits]
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE SP_Product SET ");
            if (!string.IsNullOrEmpty(model.Name))
            {
                strSql.Append("Name='" + model.Name + "',");
            }
            if (!string.IsNullOrEmpty(model.Photo))
            {
                strSql.Append("Photo='" + model.Photo + "',");
            }
            if (!string.IsNullOrEmpty(model.Desc))
            {
                strSql.Append("[Desc]='" + model.Desc + "',");
            }
            if (!string.IsNullOrEmpty(model.Unit))
            {
                strSql.Append("Unit='" + model.Unit + "',");
            }
            if (!string.IsNullOrEmpty(model.NormalPrice.ToString()))
            {
                strSql.Append("NormalPrice=" + model.NormalPrice + ",");
            }
            if (!string.IsNullOrEmpty(model.MemberPrice.ToString()))
            {
                strSql.Append("MemberPrice=" + model.MemberPrice + ",");
            }
            if (!string.IsNullOrEmpty(model.CatID))
            {
                strSql.Append("CatID='" + model.CatID + "',");
            }
            if (model.Credits > 0)
            {
                strSql.Append("Credits=" + model.Credits + ",");
            }
            if (!string.IsNullOrEmpty(model.IsTop.ToString()))
            {
                strSql.Append("IsTop=" + model.IsTop + ",");
            }
            if (!string.IsNullOrEmpty(model.Pdate.ToString()))
            {
                strSql.Append("Pdate='" + model.Pdate + "',");
            }
            if (!string.IsNullOrEmpty(model.SiteCode))
            {
                strSql.Append("SiteCode='" + model.SiteCode + "',");
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

        #region 修改商品状态
        /// <summary>
        /// 修改商品状态
        /// </summary>
        /// <param name="strProductID">商品信息</param>
        /// <returns></returns>
        public bool UpdateProductState(string strProductID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE SP_Product SET ");
            strSql.Append(" [State] = 1 ");
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
    }
}
