using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using DAL;
using Maticsoft.DBUtility;

namespace Mozart.WebService
{
    /// <summary>
    /// wsCouponAdmin 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class wsCouponAdmin : System.Web.Services.WebService
    {
        /// <summary>
        /// 返回优惠活动列表
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetACT_CouponData(string strWhere)
        {
            WebServiceDAL wsdal = new WebServiceDAL();
            return wsdal.GetACT_CouponList(strWhere);
        }

        /// <summary>
        /// 根据站点活动代码返回优惠活动列表
        /// </summary>
        /// <param name="oderID">订单ID</param>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetACT_CouponData(string siteActivityID)
        {
            WebServiceDAL wsdal = new WebServiceDAL();
            return wsdal.GetACT_CouponData(siteActivityID);
        }

        /// <summary>
        /// 根据站点代码与优惠活动状态返回优惠活动列表
        /// </summary>
        /// <param name="siteCode">站点代码</param>
        /// <param name="couponStatus">优惠活动状态</param>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetACT_CouponData(string siteCode, int couponStatus)
        {
            WebServiceDAL wsdal = new WebServiceDAL();
            return wsdal.GetACT_CouponList(siteCode, couponStatus);
        }

        /// <summary>
        /// 返回站点活动信息列表
        /// </summary>
        /// <param name="siteActivityID"></param>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetACT_SiteActivityList(string siteActivityID)
        {
            WebServiceDAL wsdal = new WebServiceDAL();
            return wsdal.GetACT_SiteActivityList(siteActivityID);
        }

        /// <summary>
        /// 更新优惠活动状态
        /// </summary>
        /// <param name="id">优惠活动ID</param>
        /// <param name="couponStatus">优惠活动状态</param>
        /// <returns></returns>
        [WebMethod]
        public int UpdateCouponStatus(string id, int couponStatus)
        {
            WebServiceDAL wsdal = new WebServiceDAL();
            return wsdal.UpdateCouponStatus(id, couponStatus);
        }
    }
}
