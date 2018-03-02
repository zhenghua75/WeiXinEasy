//============================================================
// Producnt name:		Mozart
// Version: 			1.0
// Coded by:			Laoshimonk@qq.com
// Auto generated at: 	2014/4/6 12:51:13
//============================================================

using System;
namespace Model.SP
{
    public class SP_Order
    {
        private string iD = String.Empty;
        private string customerID = String.Empty;
        private string siteCode = String.Empty;
        private string buyName = String.Empty;
        private string buyMobile = String.Empty;
        private DateTime shipDate;
        private int hasSend;
        private int hasReceive;
        private string payWay = String.Empty;
        private string carryWay = String.Empty;
        private string receiveAddress = String.Empty;
        public string ID
        {
            get { return this.iD; }
            set { this.iD = value; }
        }
        public string CustomerID
        {
            get { return this.customerID; }
            set { this.customerID = value; }
        }
        public string SiteCode
        {
            get { return this.siteCode; }
            set { this.siteCode = value; }
        }
        public string BuyName
        {
            get { return this.buyName; }
            set { this.buyName = value; }
        }
        public string BuyMobile
        {
            get { return this.buyMobile; }
            set { this.buyMobile = value; }
        }
        public DateTime ShipDate
        {
            get { return this.shipDate; }
            set { this.shipDate = value; }
        }
        public int HasSend
        {
            get { return this.hasSend; }
            set { this.hasSend = value; }
        }
        public int HasReceive
        {
            get { return this.hasReceive; }
            set { this.hasReceive = value; }
        }
        public string PayWay
        {
            get { return this.payWay; }
            set { this.payWay = value; }
        }
        public string CarryWay
        {
            get { return this.carryWay; }
            set { this.carryWay = value; }
        }
        public string ReceiveAddress
        {
            get { return this.receiveAddress; }
            set { this.receiveAddress = value; }
        }
    }
}
