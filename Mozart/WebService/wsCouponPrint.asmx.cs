using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using DAL.WebSevice;

namespace Mozart.WebService
{
    /// <summary>
    /// wsCouponPrint 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class wsCouponPrint : System.Web.Services.WebService
    {
        #region 返回用户扩展信息
        /// <summary>
        /// 返回用户扩展信息
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strState">优惠券状态</param>
        /// 
        [WebMethod]
        public DataSet GetCouponData(string strSiteCode, string strState)
        {
            string strReturn = string.Empty;
            wsCouponDAL dal = new wsCouponDAL();
            DataSet ds = dal.GetCouponData(strSiteCode, strState);
            return ds;
        }
        #endregion

        #region 返回修改优惠券状态
        /// <summary>
        /// 返回修改优惠券状态
        /// </summary>
        /// <param name="strCouponID">优惠券ID</param>
        /// <param name="strState">优惠券状态</param>
        /// 
        [WebMethod]
        public bool UpdateCouponState(string strCouponID, string strState)
        {
            wsCouponDAL dal = new wsCouponDAL();
            return dal.UpdateCouponState(strCouponID, strState);
        }
        #endregion
    }
}
