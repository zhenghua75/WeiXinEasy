using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Maticsoft.DBUtility;

namespace DAL.PublicService
{
    public class PS_Service
    {
        #region 返回便捷服务列表
        /// <summary>
        /// 返回商品列表
        /// </summary>
        /// 
        public DataSet GetServiceList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM PS_Service ");
            strSql.Append(" WHERE [State] = 1 ");
            return DbHelperSQL.Query(strSql.ToString());
        }
        #endregion
    }
}
