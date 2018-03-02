using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maticsoft.DBUtility;

namespace DAL.MiniShop
{

    public class MSChargeFeeOrder
    {
        #region 添加充值订单
        /// <summary>
        /// 添加充值订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddChargeFeeOrder(Model.MiniShop.MSChargeFeeOrder model)
        {
            string sql = @"INSERT INTO MS_ChargeFeeOrder (ID,OpenID,ChargeType,CustNo,ChargeAmount,CreateTime,ChargeIP,[State],Remark)" +
                 " VALUES (@ID,@OpenID,@ChargeType,@CustNo,@ChargeAmount,@CreateTime,@ChargeIP,@State,@Remark)";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID",model.ID),
                new System.Data.SqlClient.SqlParameter("@OpenID",model.OpenID),
                new System.Data.SqlClient.SqlParameter("@ChargeType",model.ChargeType),
                new System.Data.SqlClient.SqlParameter("@CustNo",model.CustNo),
                new System.Data.SqlClient.SqlParameter("@ChargeAmount",model.ChargeAmount),
                new System.Data.SqlClient.SqlParameter("@CreateTime", DateTime.Now),
                new System.Data.SqlClient.SqlParameter("@ChargeIP", model.ChargeIP),
                new System.Data.SqlClient.SqlParameter("@State", model.State),
                new System.Data.SqlClient.SqlParameter("@Remark", model.Remark)
            };
            int rowsAffected = DbHelperSQL.ExecuteSql(sql.ToString(), paras);
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
