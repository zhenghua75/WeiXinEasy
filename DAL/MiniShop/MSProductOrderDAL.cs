using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.MiniShop;
using Maticsoft.DBUtility;

namespace DAL.MiniShop
{
    public class MSProductOrderDAL
    {
        public MSProductOrderDAL() { ;}
        #region 添加订单
        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddMSProductOrder(MSProductOrder model)
        {
            string sql = @"INSERT INTO [MS_ProductOrder]
                        ([ID],[CartID],[CustomerID],[BuyName],[Phone],[LeaveMsg],[PayWay],[CarryWay],[ReveiveAddress],
                         [ZipCode],[IsSend],[IsReceive],[OrderState],[PayState],[PayTime],[AddTime])
                 VALUES
                        (@ID,@CartID,@CustomerID,@BuyName,@Phone,@LeaveMsg,@PayWay,@CarryWay,@ReveiveAddress,
                      @ZipCode,@IsSend,@IsReceive,@OrderState,@PayState,@PayTime,@AddTime)";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@CartID", model.CartID),
                new System.Data.SqlClient.SqlParameter("@CustomerID", model.CustomerID),
                new System.Data.SqlClient.SqlParameter("@BuyName", model.BuyName),
                new System.Data.SqlClient.SqlParameter("@Phone", model.Phone),
                new System.Data.SqlClient.SqlParameter("@LeaveMsg", model.LeaveMsg),
                new System.Data.SqlClient.SqlParameter("@PayWay", model.PayWay),
                new System.Data.SqlClient.SqlParameter("@CarryWay", model.CarryWay),
                new System.Data.SqlClient.SqlParameter("@ReveiveAddress", model.ReveiveAddress),
                new System.Data.SqlClient.SqlParameter("@ZipCode", model.ZipCode),
                new System.Data.SqlClient.SqlParameter("@IsSend", model.IsSend==1?1:0),
                new System.Data.SqlClient.SqlParameter("@IsReceive", model.IsReceive==1?1:0),
                new System.Data.SqlClient.SqlParameter("@OrderState", model.OrderState==1?1:0),
                new System.Data.SqlClient.SqlParameter("@PayState",(model.PayState==1?1:0)),
                new System.Data.SqlClient.SqlParameter("@PayTime", model.PayTime),
                new System.Data.SqlClient.SqlParameter("@AddTime", DateTime.Now)
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
        #region 更新订单信息
        /// <summary>
        /// 更新订单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMSProductOrder(MSProductOrder model)
        {
            string safeslq =string.Empty;
            safeslq = "UPDATE MS_ProductOrder SET ";
            if (model.CartID != null && model.CartID != "")
            {
                safeslq += "CarID='" + model.CartID + "',";
            }
            if (model.CustomerID != null && model.CustomerID != "")
            {
                safeslq += "CustomerID='" + model.CustomerID + "',";
            }
            if (model.BuyName != null && model.BuyName.ToString() != "")
            {
                safeslq += "BuyName='" + model.BuyName + "',";
            }
            if (model.Phone != null && model.Phone.ToString() != "")
            {
                safeslq += "Phone='" + model.Phone + "',";
            }
            if (model.LeaveMsg != null && model.LeaveMsg != "")
            {
                safeslq += "LeaveMsg='" + model.LeaveMsg + "',";
            }
            if (model.PayWay != null && model.PayWay != "")
            {
                safeslq += "PayWay='" + model.PayWay + "',";
            }
            if (model.CarryWay != null && model.CarryWay != "")
            {
                safeslq += "CarryWay='" + model.CarryWay + "',";
            }
            if (model.ReveiveAddress != null && model.ReveiveAddress != "")
            {
                safeslq += "ReveiveAddress='" + model.ReveiveAddress + "',";
            }
            if (model.ZipCode != null && model.ZipCode != "")
            {
                safeslq += "ZipCode='" + model.ZipCode + "',";
            }
            safeslq += " IsSend=" + (model.IsSend == 1 ? 1 : 0) + ", ";
            safeslq += " IsReceive=" + (model.IsReceive == 1 ? 1 : 0) + ", ";
            safeslq += " OrderState=" + (model.OrderState == 1 ? 1 : 0) + ", ";
            safeslq += " PayState=" + (model.PayState == 1 ? 1 : 0) + " ";
            safeslq += " where ID='" + model.ID + "' ";
            int rowsAffected = DbHelperSQL.ExecuteSql(safeslq.ToString());
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
        #region 获取订单属性值
        /// <summary>
        /// 获取订单属性值
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="strID"></param>
        /// <returns></returns>
        public object GetOrderValueByID(string strValue,string strID)
        {
            string safesql = "";
            safesql = "select " + strValue + " from MS_ProductOrder where ID='" + strID + "'";
            return DbHelperSQL.GetSingle(safesql.ToString());
        }
        #endregion 
        #region 根据订单编号获取详细属性
        /// <summary>
        /// 根据订单编号获取详细属性
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="strOID"></param>
        /// <returns></returns>
        public object GetOrderDetailValueByOID(string strValue, string strOID)
        {
            string safesql = "";
            safesql = "select " + strValue + " from MS_ProductOrderDetail where OID='" + strOID + "'";
            return DbHelperSQL.GetSingle(safesql.ToString());
        }
        #endregion
        #region 更新订单状态
        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="strID"></param>
        /// <returns></returns>
        public bool UpdateOrderState(string strValue, string strID)
        {
            StringBuilder strSql = new StringBuilder();
            int state = 0;
            try
            {
                state = Convert.ToInt32(GetOrderValueByID(strValue, strID));
            }
            catch (Exception)
            {
                state = 0;
            }
            switch (state)
            {
                case 0:
                    state = 1;
                    break;
                default:
                    state = 0;
                    break;
            }
            strSql.Append(" UPDATE MS_ProductOrder ");
            strSql.Append(" SET " + strValue + " = @" + strValue + " ");
            strSql.Append(" WHERE ID = @ID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@"+strValue+"", state)
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
        #region 更新订单付款状态
        /// <summary>
        /// 更新订单付款状态
        /// </summary>
        /// <param name="strOrderID">订单号</param>
        /// <param name="strState">付款状态(0:未付款  1:已付款)</param>
        /// <returns></returns>
        public static bool UpdateOrderPayState(string strOrderID,string strState)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" UPDATE MS_ProductOrder ");
            strSql.Append(" SET PayTime='" + DateTime.Now + "',PayState=@State ");
            strSql.Append(" WHERE ID = @ID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@State", strState),
                new System.Data.SqlClient.SqlParameter("@ID", strOrderID)
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
        #region 获取有效的订单列表
        /// <summary>
        /// 获取有效的订单列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetOrderList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.ID,a.CustomerID,a.BuyName,a.Phone,a.LeaveMsg,a.PayWay,a.CarryWay,a.ReveiveAddress,");
            strSql.Append(" case a.IsSend when 0 then '未发货' when 1 then '已发货' end as IsSend, ");
            strSql.Append(" case a.IsReceive when 0 then '未收件' when 1 then '已收件' end as IsReceive, ");
            strSql.Append(" a.OrderState,a.PayState,a.PayTime,a.AddTime,c.Ptitle,d.ShopName,e.PimgUrl ");
            strSql.Append(" from MS_ProductOrder a,MS_ProductOrderDetail b,MS_Product c,MS_Shop d,MS_ProductAtlas e ");
            strSql.Append(" where a.OrderState=0 and a.ID=b.OID and e.PID=c.ID and e.IsDefault=1 ");
            if (strWhere.Trim() != null && strWhere.Trim() != "")
            {
                strSql.Append("  " + strWhere);
            }
            strSql.Append(" order by a.AddTime desc ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion
        #region 获取产品有效的订单列表
        /// <summary>
        /// 获取产品有效的订单列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetProductOrderList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.*,b.OID,b.PID,b.MID,b.Quantity,b.UnitCost,b.Pnum, ");
            strSql.Append(" c.Ptitle,c.ShopName,c.CID,d.PimgUrl,d.AtlasName ");
            strSql.Append(" from MS_ProductOrder a,MS_ProductOrderDetail b,MS_Product c,MS_ProductAtlas d ");
            strSql.Append(" where a.OrderState=0 and a.ID=b.OID and b.CID=c.ID and d.PID=c.ID ");
            strSql.Append(" and d.IsDefault=1 and d.ImgState=0 ");
            if (strWhere.Trim() != null && strWhere.Trim() != "")
            {
                strSql.Append("  " + strWhere);
            }
            strSql.Append(" order by a.AddTime desc ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion
        #region 销售订单列表
        /// <summary>
        /// 销售订单列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetSaleManager(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.*,b.OID,b.PID,b.MID,b.Quantity,b.UnitCost,b.Pnum, ");
            strSql.Append(" c.Ptitle,c.CID,d.PimgUrl,d.AtlasName, ");
            strSql.Append(" (select e.ShopName from MS_Shop e where c.[SID]=e.ID)ShopName ");
            strSql.Append(" from MS_ProductOrder a,MS_ProductOrderDetail b,MS_Product c,MS_ProductAtlas d ");
            strSql.Append(" where a.OrderState=0 and a.ID=b.OID  and d.PID=c.ID ");
            strSql.Append(" and d.IsDefault=1 and d.ImgState=0 ");
            if (strWhere.Trim() != null && strWhere.Trim() != "")
            {
                strSql.Append("  " + strWhere);
            }
            strSql.Append(" order by a.AddTime desc ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion
        #region 获取订单有效客户列表
        /// <summary>
        /// 获取订单有效客户列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetOrderUser(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.AddTime,b.NickName,b.HeadImg,b.Phone,MIN(b.ID)Cuid ");
            strSql.Append(" from MS_ProductOrder a,MS_Customers b,MS_ProductOrderDetail c,MS_Product d ");
            strSql.Append(" where a.OrderState=0 and a.CustomerID=b.ID  ");
            if (strWhere.Trim() != null && strWhere.Trim() != "")
            {
                strSql.Append("  " + strWhere);
            }
            strSql.Append(" group by a.AddTime,b.NickName,b.HeadImg,b.Phone ");
            strSql.Append(" order by a.AddTime desc ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion
        #region 更新客户订单信息
        /// <summary>
        /// 更新客户订单信息
        /// </summary>
        /// <param name="strID">订单编号</param>
        /// <param name="strPhone">客户电话</param>
        /// <param name="strBuyName">客户名称</param>
        /// <param name="strReveiveAddress">收件地址</param>
        /// <param name="strLeveaMsg">留言</param>
        /// <param name="strZipCode">邮编</param>
        /// <returns></returns>
        public bool UpdateCustomer(string strID, string strPhone, string strBuyName, 
            string strReveiveAddress,string strLeveaMsg,string strZipCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" UPDATE MS_ProductOrder SET ");
            if (strPhone.Trim() != null && strPhone.Trim() != "")
            {
                strSql.Append("Phone ='"+strPhone+"', ");
            }
            if (strBuyName.Trim() != null && strBuyName.Trim() != "")
            {
                strSql.Append("BuyName ='" + strBuyName + "', ");
            }
            if (strReveiveAddress.Trim() != null && strReveiveAddress.Trim() != "")
            {
                strSql.Append("ReveiveAddress ='" + strReveiveAddress + "', ");
            }
            if (strLeveaMsg.Trim() != null && strLeveaMsg.Trim() != "")
            {
                strSql.Append("LeaveMsg ='" + strLeveaMsg + "', ");
            }
            if (strZipCode.Trim() != null && strZipCode.Trim() != "")
            {
                strSql.Append("ZipCode ='" + strZipCode + "', ");
            }
            strSql.Append(" OrderState =0 ");
            strSql.Append(" WHERE ID ='" + strID + "' ");
            int rowsAffected = DbHelperSQL.ExecuteSql(strSql.ToString());
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
        #region 获取订单产品标题用于支付
        /// <summary>
        /// 获取订单产品标题用于支付
        /// </summary>
        /// <param name="strOID"></param>
        /// <returns></returns>
        public string GetOrderTitleByOID(string strOID)
        {
            string safesql =string.Empty;
            string pid = string.Empty; string ptitle = string.Empty;
            safesql = "select PID from MS_ProductOrderDetail where OID='" + strOID + "'";
            pid = DbHelperSQL.GetSingle(safesql.ToString()).ToString();
            if (pid != null && pid != "")
            {
                safesql = "select Ptitle from  MS_Product where ID='" + pid + "' ";
            }
           ptitle= DbHelperSQL.GetSingle(safesql.ToString()).ToString();
           return ptitle;
        }
        #endregion
        #region 更新支付方式
        /// <summary>
        /// 更新支付方式
        /// </summary>
        /// <param name="strOID">订单号</param>
        /// <param name="strPayWay">支付方式：alipay、  wxpay两种方式</param>
        /// <returns></returns>
        public bool UpdateOrderPayWay(string strOID,string strPayWay)
        {
            if (strPayWay != null && strPayWay != "")
            {
                switch (strPayWay.Trim().ToLower())
                {
                    case "alipay":
                        strPayWay = strPayWay+"|支付宝";
                        break;
                    case "wxpay":
                        strPayWay = strPayWay + "|微支付";
                        break;
                }
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" UPDATE MS_ProductOrder SET  ");
            if (strPayWay.Trim() != null && strPayWay.Trim() != "")
            {
                strSql.Append("PayWay ='" + strPayWay + "' ");
            }
            strSql.Append(" WHERE ID ='" + strOID + "' ");
            int rowsAffected = DbHelperSQL.ExecuteSql(strSql.ToString());
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
        #region 判断订单是否存在
        /// <summary>
        /// 判断订单是否存在
        /// </summary>
        /// <param name="strOrderNum"></param>
        /// <returns></returns>
        public bool ExsitOrderNum(string strOrderNum)
        {
            string strSql = string.Empty;
            strSql += " SELECT count(ID) FROM MS_ProductOrder where id='" + strOrderNum + "'";
            return DbHelperSQL.Exists(strSql.ToString());
        }
        #endregion
        #region 返回客户订单列表
        /// <summary>
        /// 返回客户订单列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetCustomerOrderList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.*,b.Pnum,b.Quantity,b.UnitCost,b.MID,b.PID, ");
            strSql.Append(" c.Ptitle,c.Pcontent,c.[SID],d.ShopName, ");
            //strSql.Append(" case  when c.Price=0  then (select Price from MS_ProductPara pa where pa.PID=c.ID) ");
            //strSql.Append(" when c.Price<>0 then c.Price end Price, ");
            //strSql.Append(" case  when c.Price=0  then (select ParName from MS_ProductPara pa where pa.PID=c.ID) ");
            //strSql.Append(" when c.Price<>0 then '' end ParName, ");
            strSql.Append(" (select top 1 PimgUrl from MS_ProductAtlas  where ImgState=0 and PID=c.ID)Pimg ");
            strSql.Append(" from MS_ProductOrder a,MS_ProductOrderDetail b,MS_Product c,MS_Shop d ");
            strSql.Append(" where a.OrderState=0 and a.ID=b.OID and b.PID=c.ID and c.[SID]=d.ID ");
            if (strWhere.Trim() != null && strWhere.Trim() != "")
            {
                strSql.Append(" " + strWhere + " ");
            }
            strSql.Append(" order by a.AddTime desc ");
            return DbHelperSQL.Query(strSql.ToString());
        }
        #endregion
        #region 更新订单状态(通过用户编号验证)
        /// <summary>
        /// 更新订单状态(通过用户编号验证)
        /// </summary>
        /// <param name="strUID">用户编号</param>
        /// <param name="strOID">订单编号</param>
        /// <returns></returns>
        public bool UpdateOrderStateByUID(string strUID, string strOID)
        {
            StringBuilder strSql = new StringBuilder();
            int state = 0;
            try
            {
                state = Convert.ToInt32(GetOrderValueByID("OrderState", strOID));
            }
            catch (Exception)
            {
                state = 0;
            }
            switch (state)
            {
                case 0:
                    state = 1;
                    break;
                default:
                    state = 0;
                    break;
            }
            strSql.Append(" UPDATE MS_ProductOrder ");
            strSql.Append(" SET OrderState = @OrderState ");
            strSql.Append(" WHERE ID = @ID and CustomerID=@CustomerID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strOID),
                new System.Data.SqlClient.SqlParameter("@CustomerID", strUID),
                new System.Data.SqlClient.SqlParameter("@OrderState", state)
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
        #region 验证用户订单是否存在(限购买多件)
        /// <summary>
        /// 验证用户订单是否存在(限购买多件)
        /// </summary>
        /// <param name="strUID">用户编号</param>
        /// <returns></returns>
        public bool ExistOrderByUID(string strUID,string strPID)
        {
            string strSql = string.Empty;
            strSql += " select COUNT(a.ID) from MS_ProductOrder a,MS_ProductOrderDetail b,"+
                "MS_Product c where a.ID=b.OID and b.PID=c.ID and a.OrderState=0 and"+
                " a.CustomerID='" + strUID + "' and b.PID='"+strPID+"'";
            return DbHelperSQL.Exists(strSql.ToString());
        }
        #endregion
        #region 返回充值缴费的列表
        /// <summary>
        /// 返回充值缴费的列表
        /// </summary>
        /// <param name="strOpenID">OPID</param>
        /// <returns></returns>
        public DataSet GetPOListByOpid(string strOpenID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT CONVERT(NVARCHAR,a.AddTime,20) AS AddTime, ");
            strSql.Append(" a.PayState,");
            strSql.Append(" CASE ");
            strSql.Append(" 	WHEN SUBSTRING(b.PID,1,2) = 'VQ' THEN 'Q币充值'  ");
            strSql.Append(" 	WHEN SUBSTRING(b.PID,1,2) = 'VH' THEN '话费充值'");
            strSql.Append(" END AS PID, ");
            strSql.Append(" b.Pnum, ");
            strSql.Append(" b.UnitCost ");
            strSql.Append(" FROM MS_ProductOrder a ");
            strSql.Append(" LEFT JOIN MS_ProductOrderDetail b ON (b.OID = a.ID) ");
            strSql.Append(" WHERE a.CustomerID = @OpenID ");
            strSql.Append(" ORDER BY a.AddTime DESC ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@OpenID", strOpenID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion


    }
}
