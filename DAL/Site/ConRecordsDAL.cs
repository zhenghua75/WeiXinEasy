using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Maticsoft.DBUtility;

namespace DAL.Site
{
    public class ConRecordsDAL
    {
        public ConRecordsDAL() { ; }

        #region 插入消费记录
        /// <summary>
        /// 插入优惠卷信息
        /// </summary>
        /// <param name="info"></param>
        public bool InsertInfo(Model.Site.ConRecords info)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" INSERT INTO [Site_ConRecords] ([ID],[SiteCode],[OpenID],[MemberShipNo],[CreateTime],[Price]) ");
            strSql.Append(" VALUES (@ID,@SiteCode,@OpenID,@MemberShipNo,@CreateTime,@Price)");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", info.ID),
                new System.Data.SqlClient.SqlParameter("@SiteCode", info.SiteCode),
                new System.Data.SqlClient.SqlParameter("@OpenID", info.OpenID),
                new System.Data.SqlClient.SqlParameter("@MemberShipNo", info.MemberShipNo),
                new System.Data.SqlClient.SqlParameter("@CreateTime", info.CreateTime),
                new System.Data.SqlClient.SqlParameter("@Price", info.Price)
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

        #region 取微会员消费记录
        /// <summary>
        /// 取微会员消费记录
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strOpenID">用户OpenID</param>
        /// <param name="strMemNo">用户账号</param>
        /// <returns></returns>
        public DataSet GetConRecords(string strSiteCode,string strOpenID,string strMemNo,string strTop)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT TOP @Top ID,SiteCode,OpenID,MemberShipNo,Price,CreateTime ");
            strSql.Append(" FROM Site_ConRecords ");
            strSql.Append(" WHERE SiteCode = @SiteCode ");
            strSql.Append(" AND REPLACE(OpenID,'-','') = @OpenID ");
            strSql.Append(" AND MemberShipNo = @MemberShipNo ");
            strSql.Append(" ORDER BY CreateTime DESC ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                    new System.Data.SqlClient.SqlParameter("@OpenID", strOpenID),
                    new System.Data.SqlClient.SqlParameter("@MemberShipNo", strMemNo),
                    new System.Data.SqlClient.SqlParameter("@Top", strTop),
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        /// <summary>
        /// 返回站点所有的所费记录
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public DataSet GetMemConRecord(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT MemberShipNo AS 用户账号,Price 消费金额,CreateTime 消费时间 ");
            strSql.Append(" FROM Site_ConRecords ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" WHERE " +strWhere);
            }
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
    }
}
