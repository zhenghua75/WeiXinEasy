using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.MiniShop;
using Maticsoft.DBUtility;

namespace DAL.MiniShop
{
    public class MSShoppingCartDAL
    {
        public MSShoppingCartDAL() { ;}
/////////////////////////////////////购物车功能///////////////////////////////////////////////////
        #region 添加购物车
        /// <summary>
        /// 添加购物车
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddShoppingCart(MSShoppingCart model)
        {
            string sql = @"INSERT INTO [MS_ShoppingCart]
                        ([ID],[CustomerID],[Pid],[Quantity],[UnitCost],[OrderTime])
                 VALUES
                        (@ID,@CustomerID,@Pid,@Quantity,@UnitCost,@OrderTime)";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@CustomerID", model.CustomerID),
                new System.Data.SqlClient.SqlParameter("@Pid", model.Pid),
                new System.Data.SqlClient.SqlParameter("@Quantity", model.Quantity),
                new System.Data.SqlClient.SqlParameter("@UnitCost",model.UnitCost),
                new System.Data.SqlClient.SqlParameter("@OrderTime", DateTime.Now)
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
        #region 返回购物车某产品列表
        /// <summary>
        /// 返回购物车某产品列表
        /// </summary>
        /// <param name="strCustomerID">客户编号</param>
        /// <param name="strPID">产品编号</param>
        /// <returns></returns>
        public DataSet GetMyCartProduct(string strCustomerID, string strPID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM MS_ShoppingCart ");
            strSql.Append(" WHERE [CustomerID] = @CustomerID ");
            strSql.Append(" AND [PID] = @PID ORDER BY OrderTime DESC ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@CustomerID", strCustomerID),
                    new System.Data.SqlClient.SqlParameter("@PID", strPID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion
        #region 返回购物车列表
        /// <summary>
        /// 返回购物车列表
        /// </summary>
        /// <param name="strCustomerID">客户编号</param>
        /// <returns></returns>
        public DataSet GetMyCartProductList(string strCustomerID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM MS_ShoppingCart ");
            strSql.Append(" WHERE  [CustomerID] = @CustomerID ORDER BY OrderTime DESC ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@CustomerID", strCustomerID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion
        #region 返回购物车详细
        /// <summary>
        /// 返回购物车详细
        /// </summary>
        /// <param name="strCartID">购物车编号</param>
        /// <returns></returns>
        public DataSet GetCartDetail(string strCartID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM MS_ShoppingCart ");
            strSql.Append(" WHERE ID = @ID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strCartID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion
        #region 删除购物车产品
        /// <summary>
        /// 删除购物车产品
        /// </summary>
        /// <param name="strCartID">购物车编号</param>
        /// <returns></returns>
        public bool DeleteMyCart(string strCartID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" DELETE FROM MS_ShoppingCart ");
            strSql.Append(" WHERE ID = @ID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strCartID)
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
        #region 修改购物车的数量
        /// <summary>
        /// 修改购物车的数量
        /// </summary>
        /// <param name="strCustomerID">客户编号</param>
        /// <param name="strPID">产品编号</param>
        /// <param name="strFlag">数量 + - </param>
        /// <returns></returns>
        public string UpdateMyCartQuantity(string strCustomerID, string strPID, string strFlag)
        {
            string strReturn = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" UPDATE MS_ShoppingCart SET Quantity =Quantity ");
            if (strFlag.Trim().ToLower()== "+")
            {
                strSql.Append("  +1 ");
            }
            else
            {
                strSql.Append("  -1 ");
            }
            strSql.Append(" WHERE [CustomerID] = '" + strCustomerID + "' ");
            strSql.Append(" AND PID = '" + strPID + "' ;");
            strSql.Append(" SELECT Quantity FROM MS_ShoppingCart ");
            strSql.Append(" AND [CustomerID] = '" + strCustomerID + "' ");
            strSql.Append(" AND PID = '" + strPID + "' ");
            strSql.Append(" SELECT COUNT(ID) AS cc,SUM(Quantity) AS sq ,SUM(UnitCost * Quantity) AS sp ");
            strSql.Append(" FROM MS_ShoppingCart ");
            strSql.Append(" WHERE [CustomerID] = '" + strCustomerID + "' ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            if (null != ds && ds.Tables.Count > 1)
            {
                strReturn = ds.Tables[0].Rows[0]["Quantity"].ToString();
                strReturn = strReturn + "|" + ds.Tables[1].Rows[0]["cc"].ToString() + "|" + ds.Tables[1].Rows[0]["sq"].ToString() + "|" + ds.Tables[1].Rows[0]["sp"].ToString();
            }
            return strReturn;
        }
        #endregion
        #region 判断购物车是否存在
        /// <summary>
        /// 判断购物车是否存在
        /// </summary>
        /// <param name="strPID">产品编号</param>
        /// <param name="strCustomID">用户编号</param>
        /// <returns></returns>
        public bool ExistCart(string strPID, string strCustomID)
        {
            string strSql = string.Empty;
            strSql += " SELECT count(ID) FROM MS_ShoppingCart ";
            if (strPID.Trim() != null && strPID.Trim() != "")
            {
                if (strSql.ToString().Trim().ToLower().Contains("where"))
                {
                    strSql += " and ";
                }
                else
                {
                    strSql += " where ";
                }
                strSql += " [PID] ='" + strPID + "' ";
            }
            if (strCustomID.Trim() != null && strCustomID.Trim() != "")
            {
                if (strSql.ToString().Trim().ToLower().Contains("where"))
                {
                    strSql += " and ";
                }
                else
                {
                    strSql += " where ";
                }
                strSql += " [CustomerID] ='" + strCustomID + "' ";
            }
            return DbHelperSQL.Exists(strSql.ToString());
        }
        #endregion
        #region 返回购物车汇总信息
        /// <summary>
        /// 返回购物车汇总信息
        /// </summary>
        /// <param name="strCustomerID">客户编号</param>
        /// <returns></returns>
        public string ReturnCartSum(string strCustomerID)
        {
            string strReturn = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(ID) AS cc,SUM(Quantity) AS sq ,SUM(UnitCost * Quantity) AS sp ");
            strSql.Append(" FROM MS_ShoppingCart ");
            strSql.Append(" WHERE [CustomerID] = @CustomerID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@CustomerID", strCustomerID)
                };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), paras.ToArray());

            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                strReturn = "-1|" + ds.Tables[0].Rows[0]["cc"].ToString() + "|" + ds.Tables[0].Rows[0]["sq"].ToString() + "|" + ds.Tables[0].Rows[0]["sp"].ToString();
            }
            return strReturn;
        }
        #endregion
//////////////////////////////////////订单处理功能///////////////////////////////////////////

        #region 提交虚拟物品订单处理
        /// <summary>
        /// 提交订单处理
        /// </summary>
        /// <param name="strOpenID">用户OpenID</param>
        /// <param name="strPID">产品ID</param>
        /// <param name="strPnum">充值号码</param>
        /// <param name="strAmount">充值金额</param>
        /// 
        /// <returns></returns>
        public string SubVirtualProductOrder(string strOpenID, string strPnum,string strPID,string strAmount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Length = 0;
            strSql.Append(" DECLARE @Result AS INT; ");
            strSql.Append(" BEGIN TRAN; ");

            //插入订单数据
            string strOrderID = string.Empty; ;

            Random random = new Random();
            int r1 = (int)(random.Next(0, 9));//产生2个0-9的随机数
            int r2 = (int)(random.Next(0, 9));
            string now = DateTime.Now.ToString("yyMMddhhmmssf");//一个13位的时间戳
            String paymentID = r1.ToString() + r2.ToString() + now;// 订单ID
            strOrderID = paymentID;

            strSql.Append(" INSERT INTO MS_ProductOrder (ID,CartID,CustomerID,BuyName,Phone,LeaveMsg,PayWay,CarryWay,");
            strSql.Append(" ReveiveAddress,IsSend,IsReceive,OrderState,PayState,PayTime,AddTime) ");
            strSql.Append(" VALUES ('" + strOrderID + "','','" + strOpenID + "','" + strOpenID + "',");
            strSql.Append(" '','','wxpay|微支付','','',0,0,0,0,'',");
            strSql.Append(" '" + DateTime.Now.ToString() + "'); ");
            strSql.Append(" IF @@error <> 0 GOTO RB; ");

            //插入订单详细信息
            strSql.Append(" INSERT INTO MS_ProductOrderDetail(ID,OID,PID,MID,Quantity,Pnum,UnitCost)");
            strSql.Append(" VALUES ( '" + Guid.NewGuid().ToString("N").ToUpper() + "','" + strOrderID + "','" + strPID + "','',1,'" + strPnum + "'," + strAmount + " )");
            strSql.Append(" IF (@@ROWCOUNT = 0 OR @@error <> 0) GOTO RB; ");

            strSql.Append(" COMMIT TRAN; ");
            strSql.Append(" SET @Result = 1; ");
            strSql.Append(" GOTO ED; ");
            // 回滚事务
            strSql.Append(" RB: ");
            strSql.Append(" ROLLBACK TRAN; ");
            strSql.Append(" SET @Result = 0; ");
            // 返回结果
            strSql.Append(" ED: ");
            strSql.Append(" SELECT @Result; ");

            int result = Convert.ToInt32(DbHelperSQL.GetSingle(strSql.ToString()));
            if (result == 0)
            {
                strOrderID = string.Empty;
            }

            return strOrderID;
        }
        #endregion

        #region 提交订单处理
        /// <summary>
        /// 提交订单处理
        /// </summary>
        /// <param name="strCustomerID">客户编号</param>
        /// <param name="strBuyName">客户名称</param>
        /// <param name="strPhone">客户电话</param>
        /// <param name="strLeaveMsg">客户留言信息</param>
        /// <param name="strAddress">收件地址</param>
        /// <param name="strZipCode">邮编</param>
        /// <param name="strPID">产品编号</param>
        /// <param name="strCountCost">总花费</param>
        /// <param name="strQuantity">数量</param>
        /// <param name="strMID">型号编号</param>
        /// <returns></returns>
        public string SubOrder(string strCustomerID, string strBuyName, string strPhone,string strLeaveMsg,
            string strAddress, string strZipCode, string strPID, string strCountCost, string strQuantity, string strMID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Length = 0;
            strSql.Append(" DECLARE @Result AS INT; ");
            strSql.Append(" BEGIN TRAN; ");

            //插入订单数据
            string strOrderID = string.Empty; ;

            Random random = new Random();
            int r1 = (int)(random.Next(0, 9));//产生2个0-9的随机数
            int r2 = (int)(random.Next(0, 9));
            string now = DateTime.Now.ToString("yyMMddhhmmssf");//一个13位的时间戳
            String paymentID = r1.ToString() + r2.ToString() + now;// 订单ID
            strOrderID = paymentID;

            strSql.Append(" INSERT INTO MS_ProductOrder (ID,CartID,CustomerID,BuyName,Phone,LeaveMsg,PayWay,CarryWay,");
            strSql.Append(" ReveiveAddress,ZipCode,IsSend,IsReceive,OrderState,PayState,PayTime,AddTime) ");
            strSql.Append(" VALUES ('" + strOrderID + "','','" + strCustomerID + "','" + strBuyName + "',");
            strSql.Append(" '" + strPhone + "','" + strLeaveMsg + "','','','" + strAddress + "','" + strZipCode +
                "',0,0,0,0,'',");
            strSql.Append(" '" + DateTime.Now.ToString() + "'); ");
            strSql.Append(" IF @@error <> 0 GOTO RB; ");

            //插入订单详细信息
            strSql.Append(" INSERT INTO MS_ProductOrderDetail(ID,OID,PID,MID,Quantity,Pnum,UnitCost)");
            strSql.Append(" VALUES ( '" + Guid.NewGuid().ToString("N").ToUpper() + "','" + strOrderID + "','" + strPID + "','" + strMID + "'," + strQuantity + ",''," + strCountCost + " )");
            strSql.Append(" IF (@@ROWCOUNT = 0 OR @@error <> 0) GOTO RB; ");

            strSql.Append(" COMMIT TRAN; ");
            strSql.Append(" SET @Result = 1; ");
            strSql.Append(" GOTO ED; ");
            // 回滚事务
            strSql.Append(" RB: ");
            strSql.Append(" ROLLBACK TRAN; ");
            strSql.Append(" SET @Result = 0; ");
            // 返回结果
            strSql.Append(" ED: ");
            strSql.Append(" SELECT @Result; ");

            int result = Convert.ToInt32(DbHelperSQL.GetSingle(strSql.ToString()));
            if (result == 0)
            {
                strOrderID = string.Empty;
            }
            return strOrderID;
        }
        #endregion
        #region 返回我的订单列表
        /// <summary>
        /// 返回我的订单列表
        /// </summary>
        /// <param name="strCustomerID">客户编号</param>
        /// <returns></returns>
        public DataSet GetMyOrderListByCustomerID(string strCustomerID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.ID,a.CustomerID,a.BuyName,a.Phone,a.LeaveMsg,a.PayWay,a.CarryWay,a.ReveiveAddress,");
            strSql.Append(" case a.IsSend when 0 then '未发货' when 1 then '已发货' end as IsSend, ");
            strSql.Append(" case a.IsReceive when 0 then '未收件' when 1 then '已收件' end as IsReceive, ");
            strSql.Append(" a.OrderState,a.PayState,a.PayTime,a.AddTime,c.Ptitle,d.ShopName,e.PimgUrl ");
            strSql.Append(" from MS_ProductOrder a,MS_ProductOrderDetail b,MS_Product c,MS_Shop d,MS_ProductAtlas e ");
            strSql.Append(" where a.OrderState=0 and a.ID=b.OID and b.PID=c.ID and e.PID=c.ID and e.IsDefault=1 ");
            strSql.Append(" and a.[CustomerID] = '" + strCustomerID + "' ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@CustomerID", strCustomerID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion
        #region 返回我的有效订单列表
        /// <summary>
        /// 返回我的有效订单列表
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public DataSet GetMyOrderListByWhere(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.ID,a.CustomerID,a.BuyName,a.Phone,");
            strSql.Append(" a.LeaveMsg,a.PayWay,a.CarryWay,a.ReveiveAddress, ");
            strSql.Append(" case a.IsSend when 0 then '未发货' when 1 then '已发货' end as IsSend, ");
            strSql.Append(" case a.IsReceive when 0 then '未收件' when 1 then '已收件' end as IsReceive, ");
            strSql.Append(" case a.OrderState when 0 then '正常' when 1 then '已关闭' end as OrderState, ");
            strSql.Append(" case a.PayState when 0 then '未付款' when 1 then '已付款' end as PayState, ");
            strSql.Append(" a.PayTime,a.AddTime,b.PID,b.MID,b.Quantity,b.UnitCost,b.Pnum,b.MID,");
            strSql.Append(" c.Ptitle,c.Price,d.ShopName,e.PimgUrl,e.AtlasName ");
            strSql.Append(" from MS_ProductOrder a,MS_ProductOrderDetail b,MS_Product c, ");
            strSql.Append(" MS_Shop d,MS_ProductAtlas e  ");
            strSql.Append(" where a.OrderState=0 and a.ID=b.OID and b.PID=c.ID ");
            strSql.Append(" and d.ShopName!='' and d.ShopName is not null ");
            strSql.Append(" and e.PID=b.PID and e.IsDefault=1 and e.PID=c.ID ");
            if (strWhere.Trim() != null && strWhere.Trim() != "")
            {
                strSql.Append(" "+strWhere+" ");
            }
            strSql.Append(" order by a.AddTime desc ");
            return DbHelperSQL.Query(strSql.ToString());
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
            strSql.Append(" select a.*,b.Pnum,b.Quantity,b.UnitCost,b.MID, ");
            strSql.Append(" c.Ptitle,c.Pcontent,c.[SID],c.Price,d.ShopName, ");
            strSql.Append(" (select top 1 PimgUrl from MS_ProductAtlas  where ImgState=0 and PID=c.ID)pimg ");
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
        #region 返回订单详细
        /// <summary>
        /// 返回订单详细
        /// </summary>
        /// <param name="strOrderID">订单编号</param>
        /// <returns></returns>
        public DataSet GetMyOrderDetail(string strOrderID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.ID,a.CustomerID,a.BuyName,a.Phone,a.LeaveMsg,a.PayWay,a.CarryWay,a.ReveiveAddress,");
            strSql.Append(" case a.IsSend when 0 then '未发货' when 1 then '已发货' end as IsSend, ");
            strSql.Append(" case a.IsReceive when 0 then '未收件' when 1 then '已收件' end as IsReceive, ");
            strSql.Append(" a.OrderState,a.PayState,a.PayTime,a.AddTime,b.Quantity,b.UnitCost,b.Pnum,b.MID,");
            strSql.Append(" c.Ptitle,d.ShopName,e.PimgUrl ");
            strSql.Append(" from MS_ProductOrder a,MS_ProductOrderDetail b,MS_Product c,MS_Shop d,MS_ProductAtlas e ");
            strSql.Append(" where a.OrderState=0 and a.ID=b.OID and b.PID=c.ID and e.PID=c.ID and e.IsDefault=1 ");
            strSql.Append(" and b.OID = @OID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@OID", strOrderID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion
        #region 返回我的订单详细
        /// <summary>
        /// 返回我的订单详细
        /// </summary>
        /// <param name="strCustomerID">客户编号</param>
        /// <param name="strPID">产品编号</param>
        /// <returns></returns>
        public DataSet GetOrderDetailByCID(string strCustomerID, string strPID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.ID,a.CustomerID,a.BuyName,a.Phone,a.LeaveMsg,a.PayWay,a.CarryWay,a.ReveiveAddress,");
            strSql.Append(" case a.IsSend when 0 then '未发货' when 1 then '已发货' end as IsSend, ");
            strSql.Append(" case a.IsReceive when 0 then '未收件' when 1 then '已收件' end as IsReceive, ");
            strSql.Append(" a.OrderState,a.PayState,a.PayTime,a.AddTime,b.Quantity,b.UnitCost,b.Pnum,b.MID,");
            strSql.Append(" c.Ptitle,d.ShopName,e.PimgUrl ");
            strSql.Append(" from MS_ProductOrder a,MS_ProductOrderDetail b,MS_Product c,MS_Shop d,MS_ProductAtlas e ");
            strSql.Append(" where a.OrderState=0 and a.ID=b.OID and b.PID=c.ID and e.PID=c.ID and e.IsDefault=1 ");
            strSql.Append(" and a.CustomerID = @CustomerID and b.PID=@PID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@CustomerID", strCustomerID),
                    new System.Data.SqlClient.SqlParameter("@PID", strPID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion
    }
}
