using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Maticsoft.DBUtility;

namespace DAL.Site
{
    public class SiteMemberNoDAL
    {
        public SiteMemberNoDAL() { ; }

        #region 返回一个用户会员账号
        /// <summary>
        /// 返回一个用户会员账号
        /// </summary>
        /// <returns></returns>
        public string GetMemberShipNo(string strSiteCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT TOP 1 ISNULL(MemberShipNo,0) AS MSNo FROM Site_MemberShipNO ");
            strSql.Append(" WHERE SiteCode = @SiteCode ");
            strSql.Append(" AND [State] = 0 ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode)
            };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), paras);
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["MSNo"].ToString();
            }
            else
            {
                return "0";
            }
        }
        #endregion

        #region 修改账号状态
        /// <summary>
        /// 修改账号状态
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strMemNo">站点客户账号</param>
        /// <param name="strState">账号状态</param>
        /// <returns></returns>
        public string UpdateMemNoState(string strSiteCode, string strMemNo,string strState)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" UPDATE Site_MemberShipNO SET [State] = @State" );
            strSql.Append(" WHERE SiteCode = @SiteCode ");
            strSql.Append(" AND MemberShipNo = @MemberShipNo ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                new System.Data.SqlClient.SqlParameter("@MemberShipNo", strMemNo),
                new System.Data.SqlClient.SqlParameter("@State", strState)
            };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), paras);
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["MSNo"].ToString();
            }
            else
            {
                return "0";
            }
        }
        #endregion
    }
}
