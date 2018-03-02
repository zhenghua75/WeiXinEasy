using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Maticsoft.DBUtility;

namespace DAL.PublicService
{
    public class aq_CeleWhere
    {
        #region 返回号码归属地
        /// <summary>
        /// 返回商品列表
        /// </summary>
        /// 
        public string GetLocalState(string strNumber)
        {
            string strLocal = "归属地未知";
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" SELECT Province_Code + State_Name + Operator_Name AS LocalState FROM AQ_CeleWhere ");
            sql.AppendFormat(" WHERE Seven_No=@Seven_No");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@Seven_No", strNumber.Substring(0,7))
                };
            DataSet ds = DbHelperSQL.Query(sql.ToString(), paras.ToArray());
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                strLocal = ds.Tables[0].Rows[0]["LocalState"].ToString();
            }
            return strLocal;
        }
        #endregion

        #region 返回号码归属省份
        /// <summary>
        /// 返回商品列表
        /// </summary>
        /// 
        public string GetLocalProvinceName(string strNumber)
        {
            string strLocal = "归属地未知";
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" SELECT Province_Code LocalState FROM AQ_CeleWhere ");
            sql.AppendFormat(" WHERE Seven_No=@Seven_No");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@Seven_No", strNumber.Substring(0,7))
                };
            DataSet ds = DbHelperSQL.Query(sql.ToString(), paras.ToArray());
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                strLocal = ds.Tables[0].Rows[0]["LocalState"].ToString();
            }
            return strLocal;
        }
        #endregion

        #region 返回号码归属地州
        /// <summary>
        /// 返回商品列表
        /// </summary>
        /// 
        public string GetLocalStateName(string strNumber)
        {
            string strLocal = "归属地未知";
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" SELECT State_Name AS LocalState FROM AQ_CeleWhere ");
            sql.AppendFormat(" WHERE Seven_No=@Seven_No");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@Seven_No", strNumber.Substring(0,7))
                };
            DataSet ds = DbHelperSQL.Query(sql.ToString(), paras.ToArray());
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                strLocal = ds.Tables[0].Rows[0]["LocalState"].ToString();
            }
            return strLocal;
        }
        #endregion
    }
}
