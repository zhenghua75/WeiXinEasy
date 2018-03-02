using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using Maticsoft.DBUtility;

namespace Mozart.WebService
{
    /// <summary>
    /// wsBookingAdmin 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class wsBookingAdmin : System.Web.Services.WebService
    {
        /// <summary>
        /// 返回订单数据集
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetSP_OrdersData(string strWhere)
        {
            WebServiceDAL wsdal = new WebServiceDAL();
            return wsdal.GetSP_OrdersList(strWhere);
        }

        /// <summary>
        /// 返回订单详细信息数据集
        /// </summary>
        /// <param name="oderID">订单ID</param>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetSP_OrderDetailsData(string oderID)
        {
            WebServiceDAL wsdal = new WebServiceDAL();
            return wsdal.GetSP_OrderDetailsList(oderID);
        }

        /// <summary>
        /// 更新订单处理状态
        /// </summary>
        /// <param name="oderID">订单ID</param>
        /// <param name="hasSend">订单处理状态</param>
        /// <returns></returns>
        [WebMethod]
        public int UpdateHasSendStatus(string oderID, int hasSend)
        {
            WebServiceDAL wsdal = new WebServiceDAL();
            return wsdal.UpdateHasSendStatus(oderID, hasSend);
        }
    }
}
