using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maticsoft.DBUtility;
namespace DAL.SYS
{
    public class SYSDictionaryDAL
    {
        public SYSDictionaryDAL() { ; }

        #region 返回系统字典信息
        /// <summary>
        /// 返回系统字典信息
        /// </summary>
        /// <param name="strLoginName">登录名</param>
        /// <param name="strPassword">登录口令</param>
        /// 
        public DataSet GetDictionaryData(string strType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM SYS_Dictionary ");
            //strSql.Append(" WHERE [TYPE] = '" + strType + "' ");
            strSql.Append(" WHERE [TYPE] = @TYPE ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@TYPE", strType)
            };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(),paras);
            return ds;
        }
        #endregion

        #region 返回所以类别信息
        /// <summary>
        /// 返回所以类别信息
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public DataSet GetDictionaryList(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM SYS_Dictionary ");
            if (!string.IsNullOrEmpty(where))
            {
                strSql.Append(" WHERE " + where);
            }
            strSql.Append(" ORDER BY [Order]");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 返回某类别值
        /// <summary>
        /// 返回所以类别信息
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public string GetDictionaryValues(string strID)
        {
            string strDisCount = "100";
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT value ");
            strSql.Append(" FROM SYS_Dictionary ");
            strSql.Append(" WHERE ID = @strID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@strID", strID)
                };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                strDisCount = ds.Tables[0].Rows[0]["value"].ToString();
            }
            else
            {
                strDisCount = "100";
            }
            return strDisCount;
        }
        #endregion
    }
}
