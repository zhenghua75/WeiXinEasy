using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    public class MSShoppingCart
    {
        private string iD;
        private string customerID;
        private string pid;
        private int quantity;
        private decimal unitCost;
        private DateTime orderTime;

        /// <summary>
        /// 购物车编号
        /// </summary>
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        /// <summary>
        /// 客户编号
        /// </summary>
        public string CustomerID
        {
            get { return customerID; }
            set { customerID = value; }
        }
        /// <summary>
        /// 产品编号
        /// </summary>
        public string Pid
        {
            get { return pid; }
            set { pid = value; }
        }
        /// <summary>
        /// 产品数量
        /// </summary>
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal UnitCost
        {
            get { return unitCost; }
            set { unitCost = value; }
        }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime OrderTime
        {
            get { return orderTime; }
            set { orderTime = value; }
        }
    }
}
