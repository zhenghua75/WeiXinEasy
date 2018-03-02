using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Product
{
    public class WebServiceDAL
    {
        /// <summary>
        /// 返回订单列表
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public DataSet GetSP_OrdersList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM dbo.SP_Orders");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" where ").Append(strWhere);
            }
            strSql.Append(" ORDER BY ShipDate DESC");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }

        /// <summary>
        /// 返回优惠活动列表
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public DataSet GetACT_CouponList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM dbo.ACT_Coupon");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" where ").Append(strWhere);
            }
            strSql.Append(" ORDER BY SiteActivityID DESC");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }

        /// <summary>
        /// 根据站点代码与优惠活动状态返回优惠活动列表
        /// </summary>
        /// <param name="siteCode">站点代码</param>
        /// <param name="couponStatus">优惠活动状态</param>
        /// <returns></returns>
        public DataSet GetACT_CouponList(string siteCode, int couponStatus)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM dbo.ACT_Coupon");
            if (!string.IsNullOrEmpty(siteCode))
            {
                strSql.Append(" where SiteCode='").Append(siteCode).Append("' and ");
            }
            strSql.Append(" where CouponStatus=").Append(couponStatus.ToString());
            strSql.Append(" ORDER BY SiteActivityID DESC");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }

        /// <summary>
        /// 返回订单详细信息数据列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetSP_OrderDetailsList(string oderID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM dbo.SP_OrderDetails");
            if (!string.IsNullOrEmpty(oderID))
            {
                strSql.Append(" where OrderID='").Append(oderID).Append("'");
            }
            strSql.Append(" ORDER BY ProductID");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }

        /// <summary>
        /// 根据站点活动代码返回优惠活动列表
        /// </summary>
        /// <param name="siteActivityID">站点活动ID</param>
        /// <returns></returns>
        public DataSet GetACT_CouponData(string siteActivityID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM dbo.ACT_Coupon");
            if (!string.IsNullOrEmpty(siteActivityID))
            {
                strSql.Append(" where SiteActivityID='").Append(siteActivityID).Append("'");
            }
            strSql.Append(" ORDER BY CouponCode");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }

        /// <summary>
        /// 返回站点活动信息列表
        /// </summary>
        /// <param name="siteActivityID">站点活动ID</param>
        /// <returns></returns>
        public DataSet GetACT_SiteActivityList(string siteActivityID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM dbo.ACT_SiteActivity");
            if (!string.IsNullOrEmpty(siteActivityID))
            {
                strSql.Append(" where ID='").Append(siteActivityID).Append("'");
            }
            strSql.Append(" ORDER BY StartTime DESC");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }

        /// <summary>
        /// 更新订单处理状态(0表示未处理(未打印),1表示已打印(未联系),2表示已处理完成(已联系))
        /// </summary>
        /// <param name="oderID">订单ID</param>
        /// <returns></returns>
        public int UpdateHasSendStatus(string oderID, int hasSend)
        {
            string updateSql = "UPDATE [dbo].[SP_Orders] SET [HasSend]=" + hasSend + " WHERE ID='" + oderID + "'";
            return DbHelperSQL.ExecuteSql(updateSql);
        }

        /// <summary>
        /// 更新优惠活动状态
        /// </summary>
        /// <param name="id">优惠活动ID</param>
        /// <param name="couponStatus"></param>
        /// <returns></returns>
        public int UpdateCouponStatus(string id, int couponStatus)
        {
            string updateSql = "UPDATE [dbo].[ACT_Coupon] SET [CouponStatus]=" + couponStatus + " WHERE ID='" + id + "'";
            return DbHelperSQL.ExecuteSql(updateSql);
        }
    }
}
