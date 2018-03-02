using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maticsoft.DBUtility;

namespace DAL.WebSevice
{
    public class wsCouponDAL
    {
        public wsCouponDAL() { ; }

        #region 获取待打印的优惠券
        /// <summary>
        /// 获取待打印的优惠券
        /// </summary>
        /// <param name="strSiteCode">站点ID</param>
        /// <param name="strState">状态</param>
        /// <returns></returns>
        public DataSet GetCouponData(string strSiteCode, string strState)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT TOP 1 a.ID, CONVERT(varchar(100), a.AddTime, 20) AS AddTime ,CONVERT(varchar(100), a.CheckTime, 20) AS CheckTime,a.CouponCode ,b.ActTitle,b.[Discount],b.ActContent ");
            strSql.Append(" FROM ACT_Coupon a ");
            strSql.Append(" LEFT JOIN ACT_SiteActivity b ON (b.ID = a.SiteActivityID) ");
            //strSql.Append(" WHERE a.SiteCode = '" + strSiteCode + "' ");
            strSql.Append(" WHERE a.SiteCode = @SiteCode ");
            //strSql.Append(" AND a.CouponStatus = " + strState + " ");
            strSql.Append(" AND a.CouponStatus = @CouponStatus ");
            strSql.Append(" ORDER BY a.AddTime ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                new System.Data.SqlClient.SqlParameter("@CouponStatus",strState),
            };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(),paras);
            return ds;
        }
        #endregion

        #region 修改优惠券状态
        /// <summary>
        /// 修改优惠券状态
        /// </summary>
        /// <param name="strCouponID">优惠券ID</param>
        /// <param name="strState">状态</param>
        /// <returns></returns>
        public bool UpdateCouponState(string strSiteCode, string strState)
        {
            string strReturn = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" UPDATE ACT_Coupon ");
            //strSql.Append(" SET CouponStatus = " + strState );
            strSql.Append(" SET CouponStatus = @CouponStatus ");
            if (strState == "3")
            {
                strSql.Append(" ,CheckTime = GETDATE() ");
            }
            //strSql.Append(" WHERE ID = '" + strSiteCode + "' ");
            strSql.Append(" WHERE ID = @ID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@CouponStatus", strState),
                new System.Data.SqlClient.SqlParameter("@ID",strSiteCode),
            };
            int rowsAffected = DbHelperSQL.ExecuteSql(strSql.ToString(),paras);
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
