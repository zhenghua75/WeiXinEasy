using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maticsoft.DBUtility;


namespace DAL.Product
{
    public class CartDAL
    {
        public CartDAL() { ; }

        #region 添加购物车信息
        /// <summary>
        /// 添加购物车信息
        /// </summary>
        /// <param name="model">购物车信息</param>
        /// 
        public bool AddCartData(Model.SP.SP_ShoppingCart model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
            if (model.ID != null)
            {
                strSql1.Append("ID,");
                strSql2.Append("'" + model.ID + "',");
            }
            if (model.CustomerID != null)
            {
                strSql1.Append("CustomerID,");
                strSql2.Append("'" + model.CustomerID + "',");
            }
            if (model.ProductID != null)
            {
                strSql1.Append("ProductID,");
                strSql2.Append("'" + model.ProductID + "',");
            }

            strSql1.Append("UnitCost,");
            strSql2.Append(model.UnitCost + ",");

            strSql1.Append("Quantity,");
            strSql2.Append(model.Quantity + ",");

            if (model.OrderTime != null)
            {
                strSql1.Append("OrderTime,");
                strSql2.Append("'" + model.OrderTime + "',");
            }
            if (model.SiteCode != null)
            {
                strSql1.Append("SiteCode,");
                strSql2.Append("'" + model.SiteCode + "',");
            }
            strSql.Append("INSERT INTO SP_ShoppingCart(");
            strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
            strSql.Append(")");
            strSql.Append(" VALUES (");
            strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
            strSql.Append(");");

            strSql.Append(" UPDATE SP_ShoppingCart SET ID = '" + model.ID + "'");
            strSql.Append(" WHERE ID ='" + model.ID + "' ");
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

        #region 返回我的购物车中的某商品
        /// <summary>
        /// 返回购物车商品列表
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strCustomerID">用户ID</param>
        /// <param name="strProductID">商品ID</param>
        /// 
        public DataSet GetMyCartProduct(string strSiteCode, string strCustomerID, string strProductID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM SP_ShoppingCart ");
            strSql.Append(" WHERE SiteCode =@SiteCode ");
            strSql.Append(" AND [CustomerID] = @CustomerID ");
            strSql.Append(" AND [ProductID] = @ProductID");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                    new System.Data.SqlClient.SqlParameter("@CustomerID", strCustomerID),
                    new System.Data.SqlClient.SqlParameter("@ProductID", strProductID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 返回购物车商品列表
        /// <summary>
        /// 返回购物车商品列表
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strCustomerID">用户ID</param>
        /// 
        public DataSet GetCatList(string strSiteCode, string strCustomerID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM SP_ShoppingCart ");
            strSql.Append(" WHERE SiteCode = @SiteCode ");
            strSql.Append(" AND [CustomerID] = @CustomerID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                    new System.Data.SqlClient.SqlParameter("@CustomerID", strCustomerID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 返回我的购物车商品信息
        /// <summary>
        /// 返回我的购物车商品信息
        /// </summary>
        /// <param name="strMCProductID">购物车ID</param>
        /// 
        public DataSet GetMyCartProduct(string strMCProductID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM SP_ShoppingCart ");
            strSql.Append(" WHERE ID = @ID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strMCProductID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 删除我的购物车商品信息
        /// <summary>
        /// 返回我的购物车商品信息
        /// </summary>
        /// <param name="strMCProductID">购物车ID</param>
        /// 
        public bool DelMyCartProduct(string strMCProductID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" DELETE FROM SP_ShoppingCart ");
            strSql.Append(" WHERE ID = @ID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strMCProductID)
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

        #region 返回我的购物车商品列表
        /// <summary>
        /// 返回购物车商品列表
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strCustomerID">用户ID</param>
        /// 
        public DataSet GetMyCartList(string strSiteCode, string strCustomerID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT a.ID,a.ProductID,a.UnitCost,a.Quantity,b.Photo,b.Name,b.Unit ");
            strSql.Append(" FROM SP_ShoppingCart a ");
            strSql.Append(" LEFT JOIN SP_Product b ON (b.ID = a.ProductID) ");
            strSql.Append(" WHERE a.SiteCode = @SiteCode ");
            strSql.Append(" AND a.[CustomerID] =@CustomerID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                    new System.Data.SqlClient.SqlParameter("@CustomerID", strCustomerID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 返回我的订单信息列表
        /// <summary>
        /// 返回购物车商品列表
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strCustomerID">用户ID</param>
        /// 
        public DataSet GetMyOrderList(string strSiteCode, string strCustomerID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM SP_Orders a ");
            strSql.Append(" WHERE a.SiteCode = '" + strSiteCode + "' ");
            strSql.Append(" AND a.[CustomerID] = '" + strCustomerID + "' ");
            strSql.Append(" ORDER BY ShipDate DESC ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                    new System.Data.SqlClient.SqlParameter("@CustomerID", strCustomerID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 返回我的订单详细信息
        /// <summary>
        /// 返回我的订单详细信息
        /// </summary>
        /// <param name="strOrderID">订单ID</param>
        /// 
        public DataSet GetOrderList(string strOrderID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT b.ID,a.ProductID,b.Name,b.Photo,a.Quantity,a.UnitCost  ");
            strSql.Append(" FROM SP_OrderDetails a ");
            strSql.Append(" LEFT JOIN SP_Product b ON (b.ID = a.ProductID) ");
            strSql.Append(" WHERE OrderID = @OrderID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@OrderID", strOrderID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 修改购物车数量
        /// <summary>
        /// 修改购物车数量
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strCustomerID">用户ID</param>
        /// <param name="strProductID">商品ID</param>
        /// <param name="strFlag">是增加还是减少</param>
        /// 
        public string UpdateCatList(string strSiteCode, string strCustomerID, string strProductID, string strFlag)
        {
            string strReturn = string.Empty;
            StringBuilder strSql = new StringBuilder();
            if (strFlag == "+")
            {
                strSql.Append(" UPDATE SP_ShoppingCart SET Quantity = Quantity + 1 ");
            }
            else
            {
                strSql.Append(" UPDATE SP_ShoppingCart SET Quantity = Quantity - 1 ");
            }
            strSql.Append(" WHERE SiteCode = '" + strSiteCode + "' ");
            strSql.Append(" AND [CustomerID] = '" + strCustomerID + "' ");
            strSql.Append(" AND ProductID = '" + strProductID + "' ;");

            strSql.Append(" SELECT Quantity FROM SP_ShoppingCart ");
            strSql.Append(" WHERE SiteCode = '" + strSiteCode + "' ");
            strSql.Append(" AND [CustomerID] = '" + strCustomerID + "' ");
            strSql.Append(" AND ProductID = '" + strProductID + "' ");

            strSql.Append(" SELECT COUNT(ID) AS cc,SUM(Quantity) AS sq ,SUM(UnitCost * Quantity) AS sp ");
            strSql.Append(" FROM SP_ShoppingCart ");
            strSql.Append(" WHERE SiteCode = '" + strSiteCode + "' ");
            strSql.Append(" AND [CustomerID] = '" + strCustomerID + "' ");

            DataSet ds = DbHelperSQL.Query(strSql.ToString());

            if (null != ds && ds.Tables.Count > 1)
            {
                strReturn = ds.Tables[0].Rows[0]["Quantity"].ToString();
                strReturn = strReturn + "|" + ds.Tables[1].Rows[0]["cc"].ToString() + "|" + ds.Tables[1].Rows[0]["sq"].ToString() + "|" + ds.Tables[1].Rows[0]["sp"].ToString();
            }

            return strReturn;
        }
        #endregion

        #region 返回购物车汇总信息
        /// <summary>
        /// 返回购物车汇总信息
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strCustomerID">用户ID</param>
        /// 
        public string GetMyCatSum(string strSiteCode, string strCustomerID)
        {
            string strReturn = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(ID) AS cc,SUM(Quantity) AS sq ,SUM(UnitCost * Quantity) AS sp ");
            strSql.Append(" FROM SP_ShoppingCart ");
            strSql.Append(" WHERE SiteCode = @SiteCode ");
            strSql.Append(" AND [CustomerID] = @CustomerID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
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

        #region 提交订单处理
        public int CheckOutOrder(string strSiteCode, string strCustomerID, string strBuyName, string strBuyMobile, string strAddress)
        {
            int result = 0;

            StringBuilder strSql = new StringBuilder();
            strSql.Length = 0;
            strSql.Append(" DECLARE @Result AS INT; ");
            strSql.Append(" BEGIN TRAN; ");

            //删除购物车垃圾数据
            strSql.Append(" DELETE FROM SP_ShoppingCart WHERE Quantity < 1 AND CustomerID = '" + strCustomerID + "' AND SiteCode = '" + strSiteCode + "'; ");
            strSql.Append(" IF @@error <> 0 GOTO RB; ");

            //插入订单数据
            string strOrderID = Guid.NewGuid().ToString("N");
            strSql.Append(" INSERT INTO SP_Orders (ID,CustomerID,SiteCode,BuyName,BuyMobile,ShipDate,HasSend,HasReceive,PayWay,CarryWay,ReceiveAddress) ");
            strSql.Append(" VALUES ('" + strOrderID + "','" + strCustomerID + "','" + strSiteCode + "','" + strBuyName + "','" + strBuyMobile + "',");
            strSql.Append(" '" + DateTime.Now.ToString() + "',0,0,'','','" + strAddress + "'); ");
            strSql.Append(" IF @@error <> 0 GOTO RB; ");

            //插入订单详细信息
            strSql.Append(" INSERT INTO SP_OrderDetails(ID,OrderID,ProductID,Quantity,UnitCost) SELECT REPLACE(NEWID(),'-',''),'" + strOrderID + "',ProductID,Quantity,UnitCost FROM SP_ShoppingCart ");
            strSql.Append(" WHERE CustomerID = '" + strCustomerID + "' AND SiteCode = '" + strSiteCode + "'; ");
            strSql.Append(" IF (@@ROWCOUNT = 0 OR @@error <> 0) GOTO RB; ");

            //删除购物车数据
            strSql.Append(" DELETE FROM SP_ShoppingCart WHERE CustomerID = '" + strCustomerID + "' AND SiteCode = '" + strSiteCode + "'; ");
            strSql.Append(" IF @@error <> 0 GOTO RB; ");

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

            result = Convert.ToInt32(DbHelperSQL.GetSingle(strSql.ToString()));
            return result;
        }
        #endregion
    }
}
