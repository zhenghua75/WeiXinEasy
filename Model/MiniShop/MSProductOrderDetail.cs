using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    /// <summary>
    /// 订单详细
    /// </summary>
    public class MSProductOrderDetail
    {
        private string iD;
        private string oID;
        private string pID;
        private string mID;
        private int quantity;
        private decimal unitCost;

        #region 订单详细编号
        /// <summary>
        /// 订单详细编号
        /// </summary>
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        #endregion
        #region 订单编号
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OID
        {
            get { return oID; }
            set { oID = value; }
        }
        #endregion
        #region 产品编号
        /// <summary>
        /// 产品编号
        /// </summary>
        public string PID
        {
            get { return pID; }
            set { pID = value; }
        }
        #endregion
        /// <summary>
        /// 型号编号
        /// </summary>
        public string MID
        {
            get { return mID; }
            set { mID = value; }
        }
        #region 产品数量
        /// <summary>
        /// 产品数量
        /// </summary>
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        #endregion
        #region 总价
        /// <summary>
        /// 总价
        /// </summary>
        public decimal UnitCost
        {
            get { return unitCost; }
            set { unitCost = value; }
        }
        #endregion
    }
}
