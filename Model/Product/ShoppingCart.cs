//============================================================
// Producnt name:		Mozart
// Version: 			1.0
// Coded by:			Laoshimonk@qq.com
// Auto generated at: 	2014/3/31 12:50:10
//============================================================

using System;
namespace Model.SP
{
    public class SP_ShoppingCart
    {
        private string iD = String.Empty;
        private string customerID = String.Empty;
        private string siteCode = String.Empty;
        private string productID = String.Empty;
        private decimal unitCost;
        private int quantity;
        private DateTime orderTime;
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
        public string ProductID
        {
            get { return this.productID; }
            set { this.productID = value; }
        }
        public decimal UnitCost
        {
            get { return this.unitCost; }
            set { this.unitCost = value; }
        }
        public int Quantity
        {
            get { return this.quantity; }
            set { this.quantity = value; }
        }
        public DateTime OrderTime
        {
            get { return this.orderTime; }
            set { this.orderTime = value; }
        }
    }

    public class SP_MyCart
    {
        private string iD = String.Empty;
        private string photo = String.Empty;
        private string productID = String.Empty;
        private decimal unitCost;
        private int quantity;
        private string unit;
        private string name;
        public string ID
        {
            get { return this.iD; }
            set { this.iD = value; }
        }
        public string Photo
        {
            get { return this.photo; }
            set { this.photo = value; }
        }
        public string ProductID
        {
            get { return this.productID; }
            set { this.productID = value; }
        }
        public decimal UnitCost
        {
            get { return this.unitCost; }
            set { this.unitCost = value; }
        }
        public int Quantity
        {
            get { return this.quantity; }
            set { this.quantity = value; }
        }
        public string Unit
        {
            get { return this.unit; }
            set { this.unit = value; }
        }
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
    }
}
