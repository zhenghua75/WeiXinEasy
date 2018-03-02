using Maticsoft.DBUtility;
using Model.ACT;
/* ==============================================================================
 * 类名称：CouponDAL
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/3/22 14:51:38
 * 修改人：
 * 修改时间：
 * 修改备注：
 * @version 1.0
 * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DAL.ACT
{
    public class CouponDAL
    {
        #region 插入优惠卷信息
        /// <summary>
        /// 插入优惠卷信息
        /// </summary>
        /// <param name="info"></param>
        public void InsertInfo(Coupon info)
        {
            string sql = @"INSERT INTO [ACT_Coupon]
                        ([ID]
                       ,[SiteCode]
                       ,[SiteActivityID]
                       ,[OpenID]
                       ,[CouponCode]
                       ,[LimitTime]
                       ,[CouponStatus]
                       ,[Remark])
                 VALUES
                        (@ID
                       ,@SiteCode
                       ,@SiteActivityID
                       ,@OpenID
                       ,@CouponCode
                       ,@LimitTime
                       ,@CouponStatus
                       ,@Remark)";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", info.ID),
                new System.Data.SqlClient.SqlParameter("@SiteCode", info.SiteCode),
                new System.Data.SqlClient.SqlParameter("@SiteActivityID", info.SiteActivityID),
                new System.Data.SqlClient.SqlParameter("@OpenID", info.OpenID),
                new System.Data.SqlClient.SqlParameter("@CouponCode", info.CouponCode),
                new System.Data.SqlClient.SqlParameter("@LimitTime", info.LimitTime),
                new System.Data.SqlClient.SqlParameter("@CouponStatus", info.CouponStatus),
                new System.Data.SqlClient.SqlParameter("@Remark", info.Remark)
            };
            DbHelperSQL.ExecuteSql(sql, paras);
        }
        #endregion

        #region 判断是否已为用户生成优惠卷
        /// <summary>
        /// 判断是否已为用户生成优惠卷
        /// </summary>
        /// <param name="siteCode"></param>
        /// <param name="siteActivityID"></param>
        /// <returns></returns>
        public bool ExistCoupon(string siteCode, string siteActivityID,string openID)
        {
            string sql = @"SELECT count(ID) FROM ACT_Coupon 
                    WHERE SiteCode=@SiteCode 
                    AND SiteActivityID=@SiteActivityID
                    AND OpenID=@OpenID";
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", siteCode),
                    new System.Data.SqlClient.SqlParameter("@SiteActivityID", siteActivityID),
                    new System.Data.SqlClient.SqlParameter("@OpenID", openID)
                };
            return DbHelperSQL.Exists(sql, paras.ToArray());
        }
        #endregion

        #region 获取优惠卷列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSiteCode">站点ID</param>
        /// <param name="strOpenID">OpenID</param>
        /// <returns></returns>
        public DataSet GetCouponList(string strSiteCode, string strOpenID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM ACT_Coupon ");
            strSql.Append(" WHERE SiteCode=@SiteCode");
            strSql.Append(" AND REPLACE(OpenID,'-','')=@OpenID");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                    new System.Data.SqlClient.SqlParameter("@OpenID", strOpenID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 获取我优惠卷列表信息
        /// <summary>
        /// 获取我优惠卷列表信息
        /// </summary>
        /// <param name="strSiteCode">站点ID</param>
        /// <param name="strOpenID">OpenID</param>
        /// <returns></returns>
        public DataSet GetCouponInfoList(string strSiteCode, string strOpenID)
        {
            string sql = string.Empty;
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>();
            if (!string.IsNullOrEmpty(strOpenID))
            {
                sql = @" SELECT a.ID,b.ActTitle,b.Photo,b.ActContent,CASE a.CouponStatus WHEN 0 THEN '未使用' ELSE '已使用' END AS CouponStatus
                        FROM ACT_Coupon a LEFT JOIN ACT_SiteActivity b ON (b.ID = a.SiteActivityID)
                        WHERE a.SiteCode=@SiteCode AND a.OpenID = @OpenID ORDER BY a.AddTime DESC";
                        //WHERE a.SiteCode=@SiteCode AND REPLACE(a.OpenID,'-','') = @OpenID ORDER BY a.AddTime DESC";
                paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                    new System.Data.SqlClient.SqlParameter("@OpenID", strOpenID)
                };
            }
            else
            {
                sql = @" SELECT a.ID,b.ActTitle,b.ActContent,CASE a.CouponStatus WHEN 0 THEN '未使用' ELSE '已使用' END AS CouponStatus
                        FROM ACT_Coupon a LEFT JOIN ACT_SiteActivity b ON (b.ID = a.SiteActivityID)
                        WHERE a.SiteCode=@SiteCode";
                paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode)
                };
            }
            DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
            return ds;
        }
        #endregion

        #region 获取我优惠卷详细信息
        /// <summary>
        /// 获取我优惠卷详细信息
        /// </summary>
        /// <param name="strSiteCode">站点ID</param>
        /// <param name="strOpenID">OpenID</param>
        /// <returns></returns>
        public DataSet GetCouponInfo(string strCouponID)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append(" SELECT a.ID,b.ActTitle,CONVERT(varchar(100),a.AddTime,23) AS AddTime,CONVERT(varchar(100),a.CheckTime,23) AS CheckTime,DATEDIFF(DAY,GETDATE(),LTRIM(YEAR(a.AddTime)+1)+'-'+LTRIM(MONTH(a.AddTime)) +'-'+ LTRIM(DAY(a.AddTime))) AS RemainDay, ");
            strSql.Append(" SELECT a.ID,b.ActTitle,b.SiteCode,CONVERT(varchar(100),a.AddTime,23) AS AddTime,CONVERT(varchar(100),a.CheckTime,23) AS CheckTime,DATEDIFF(DAY,GETDATE(),ISNULL(b.CutOffTime,b.EndTime)) AS RemainDay, ");
            strSql.Append(" b.ActContent,CASE a.CouponStatus WHEN 0 THEN '未使用' ELSE '已经使用' END AS CouponStatus,a.CouponCode, b.Remark AS Remark ");
            strSql.Append(" FROM ACT_Coupon a ");
            strSql.Append(" LEFT JOIN ACT_SiteActivity b ON (b.ID = a.SiteActivityID)");
            strSql.Append(" WHERE a.ID = @ID");

            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strCouponID)
                };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
            return ds;
        }
        #endregion

        #region 修改我优惠券状态
        /// <summary>
        /// 修改我优惠券状态
        /// </summary>
        /// <param name="strCouponID">优惠券ID</param>
        /// <returns></returns>
        public bool UpdateCouponState(string strCouponID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" UPDATE ACT_Coupon SET CouponStatus = 1 ");
            strSql.Append(" FROM ACT_Coupon ");
            strSql.Append(" WHERE ID = @ID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strCouponID)
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

        #region 获取我优惠卷列表信息
        /// <summary>
        /// 获取我优惠卷列表信息
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public DataSet GetSiteCouponList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT a.ID,a.CouponCode,b.ActTitle,b.ActContent,CASE a.CouponStatus WHEN 0 THEN '未使用' ELSE '已使用' END AS CouponStatus, ");
            strSql.Append(" CONVERT(varchar(100),a.AddTime,20) AS AddTime,CONVERT(varchar(100),a.CheckTime,20) AS CheckTime ");
            strSql.Append(" FROM ACT_Coupon a ");
            strSql.Append(" LEFT JOIN ACT_SiteActivity b ON (b.ID = a.SiteActivityID)");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 修改优惠券状态
        /// <summary>
        /// 修改优惠券状态
        /// </summary>
        /// <param name="strCouponID">优惠券ID</param>
        /// <param name="strState">优惠券状态</param>
        /// <returns></returns>
        public bool UpdateCouponState(string strCouponID,string strState)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" UPDATE ACT_Coupon SET CouponStatus = @CouponStatus ");
            strSql.Append(" FROM ACT_Coupon ");
            strSql.Append(" WHERE ID = @ID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@CouponStatus", strState),
                new System.Data.SqlClient.SqlParameter("@ID", strCouponID)
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

        #region 获取优惠卷数量
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public int GetCouponCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT Count(*) ");
            strSql.Append(" FROM ACT_Coupon ");
            if (!string.IsNullOrWhiteSpace(strWhere))
            {
                strSql.Append(" WHERE " + strWhere );
            }
            return int.Parse(DbHelperSQL.Query(strSql.ToString()).Tables[0].Rows[0][0].ToString());
        }
        #endregion
    }
}
