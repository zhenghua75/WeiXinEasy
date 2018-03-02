using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    /// <summary>
    /// 产品参数
    /// </summary>
    public class MSProductPara
    {
        private string iD;
        private string pID;
        private string parName;
        private decimal price;
        private int stock;
        private int parState;
        private DateTime addTime;

        /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        /// <summary>
        /// 产品编号
        /// </summary>
        public string PID
        {
            get { return pID; }
            set { pID = value; }
        }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParName
        {
            get { return parName; }
            set { parName = value; }
        }
        /// <summary>
        /// 产品价格
        /// </summary>
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }
        /// <summary>
        /// 库存
        /// </summary>
        public int Stock
        {
            get { return stock; }
            set { stock = value; }
        }
        /// <summary>
        /// 参数状态
        /// </summary>
        public int ParState
        {
            get { return parState; }
            set { parState = value; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }
    }
}
