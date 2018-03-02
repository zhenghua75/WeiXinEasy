using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    /// <summary>
    /// 产品订单
    /// </summary>
   public class MSProductOrder
    {
        private string iD;
        private string cartID;
        private string customerID;
        private string buyName;
        private string phone;
        private string leaveMsg;
        private string payWay;
        private string carryWay;
        private string reveiveAddress;
        private string zipCode;
        private int isSend;
        private int isReceive;
        private int orderState;
        private int payState;
        private DateTime payTime;
        private DateTime addTime;

        #region 订单编号
        /// <summary>
       /// 订单编号
       /// </summary>
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        #endregion 
        #region 购物车编号
        /// <summary>
       /// 购物车编号
       /// </summary>
        public string CartID
        {
            get { return cartID; }
            set { cartID = value; }
        }
        #endregion
        #region 客户编号
        /// <summary>
       /// 客户编号
       /// </summary>
        public string CustomerID
        {
            get { return customerID; }
            set { customerID = value; }
        }
        #endregion
        #region 收件人姓名
        /// <summary>
       /// 收件人姓名
       /// </summary>
        public string BuyName
        {
            get { return buyName; }
            set { buyName = value; }
        }
        #endregion
        #region 收件人电话
        /// <summary>
       /// 收件人电话
       /// </summary>
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        #endregion
        #region 留言信息
        /// <summary>
       /// 留言信息
       /// </summary>
        public string LeaveMsg
        {
            get { return leaveMsg; }
            set { leaveMsg = value; }
        }
        #endregion
        #region 支付方式
        /// <summary>
       /// 支付方式
       /// </summary>
        public string PayWay
        {
            get { return payWay; }
            set { payWay = value; }
        }
        #endregion 支付方式
        #region 运输方式
        /// <summary>
       /// 运输方式
       /// </summary>
        public string CarryWay
        {
            get { return carryWay; }
            set { carryWay = value; }
        }
        #endregion
        #region 收件人地址
        /// <summary>
       /// 收件人地址
       /// </summary>
        public string ReveiveAddress
        {
            get { return reveiveAddress; }
            set { reveiveAddress = value; }
        }
        #endregion
        #region 收件人邮编
        /// <summary>
       /// 收件人邮编
       /// </summary>
        public string ZipCode
        {
            get { return zipCode; }
            set { zipCode = value; }
        }
        #endregion
        #region 是否已发货 0表示未发货，1表示发货
        /// <summary>
       /// 是否已发货 0表示未发货，1表示发货
       /// </summary>
        public int IsSend
        {
            get { return isSend; }
            set { isSend = value; }
        }
        #endregion
        #region 是否签收 0表示未签收，1表示签收
        /// <summary>
       /// 是否签收 0表示未签收，1表示签收
       /// </summary>
        public int IsReceive
        {
            get { return isReceive; }
            set { isReceive = value; }
        }
        #endregion
        #region 订单状态 0表示正常 1表示已删
        /// <summary>
       /// 订单状态 0表示正常 1表示已删
       /// </summary>
        public int OrderState
        {
            get { return orderState; }
            set { orderState = value; }
        }
        #endregion
        #region 支付状态 0表示未支付，1表示已支付
        /// <summary>
       /// 支付状态 0表示未支付，1表示已支付
       /// </summary>
        public int PayState
        {
            get { return payState; }
            set { payState = value; }
        }
        #endregion
        #region 支付时间
        /// <summary>
       /// 支付时间
       /// </summary>
        public DateTime PayTime
        {
            get { return payTime; }
            set { payTime = value; }
        }
        #endregion
        #region 添加时间
        /// <summary>
       /// 添加时间
       /// </summary>
        public DateTime AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }
        #endregion
    }

   public class MSChargeOrder
   {
       public string AddTime { get; set; }
       public string PayState { get; set; }
       public string PID { get; set; }
       public string Pnum { get; set; }
       public string UnitCost { get; set; }
   }
}
