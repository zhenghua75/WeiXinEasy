//============================================================
// Producnt name:		Mozart
// Version: 			1.0
// Coded by:			Laoshimonk@qq.com
// Auto generated at: 	2014/4/6 15:11:52
//============================================================

using System;
namespace Model.SP
{
    public class SP_OrderDetail
    {
        private string iD = String.Empty;
        private string orderID = String.Empty;
        private string productID = String.Empty;
        private int quantity;
        private decimal unitCost;
        public string ID
        {
            get { return this.iD; }
            set { this.iD = value; }
        }
        public string OrderID
        {
            get { return this.orderID; }
            set { this.orderID = value; }
        }
        public string ProductID
        {
            get { return this.productID; }
            set { this.productID = value; }
        }
        public int Quantity
        {
            get { return this.quantity; }
            set { this.quantity = value; }
        }
        public decimal UnitCost
        {
            get { return this.unitCost; }
            set { this.unitCost = value; }
        }
    }

    public class SP_MyOrderDetail
    {
        private string iD = String.Empty;
        private string name = String.Empty;
        private string photo = String.Empty;
        private int quantity;
        private decimal unitCost;
        public string ID
        {
            get { return this.iD; }
            set { this.iD = value; }
        }
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public string Photo
        {
            get { return this.photo; }
            set { this.photo = value; }
        }
        public int Quantity
        {
            get { return this.quantity; }
            set { this.quantity = value; }
        }
        public decimal UnitCost
        {
            get { return this.unitCost; }
            set { this.unitCost = value; }
        }
    }
}
