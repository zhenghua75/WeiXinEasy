using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Maticsoft.DBUtility;

namespace DAL.Vote
{
    public class IndexDAL
    {
        public IndexDAL() { ; }

        #region 返回站点投票信息
        /// <summary>
        /// 返回站点投票信息
        /// </summary>
        /// <param name="strID">索引代码</param>
        /// 
        public string GetSubjectData(string strID)
        {
            //SELECT * FROM Vote_Index WHERE ID = 
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT TOP 1 SubjectID ");
            strSql.Append(" FROM Vote_Index ");
            //strSql.Append(" WHERE SiteCode = '" + strSiteCode + "' ");
            strSql.Append(" WHERE ID = @strID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@strID", strID)
            };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), paras);
            if (null == ds && ds.Tables.Count < 1 && ds.Tables[0].Rows.Count < 1)
            {
                return string.Empty;
            }
            else
            {
                return ds.Tables[0].Rows[0]["SubjectID"].ToString();
            }
        }
        #endregion
    }
}
