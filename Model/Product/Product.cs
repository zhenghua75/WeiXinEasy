//============================================================
// Producnt name:		Mozart
// Version: 			1.0
// Coded by:			Laoshimonk@qq.com
// Auto generated at: 	2014/3/12 15:44:09
//============================================================

using System;
namespace Model.SP
{
    public class SP_Product
    {
        private string iD = String.Empty;
        private string name = String.Empty;
        private string photo = String.Empty;
        private string unit = String.Empty;
        private decimal normalPrice;
        private decimal memberPrice;
        private DateTime pdate;
        private string catID = String.Empty;
        private string desc = String.Empty;
        private int state;
        private string siteCode = String.Empty;
        private DateTime startTime;
        private DateTime endTime;
        private int isTop;
        private int credits;
        private int order;
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
        public string Unit
        {
            get { return this.unit; }
            set { this.unit = value; }
        }
        public decimal NormalPrice
        {
            get { return this.normalPrice; }
            set { this.normalPrice = value; }
        }
        public decimal MemberPrice
        {
            get { return this.memberPrice; }
            set { this.memberPrice = value; }
        }
        public DateTime Pdate
        {
            get { return this.pdate; }
            set { this.pdate = value; }
        }
        public string CatID
        {
            get { return this.catID; }
            set { this.catID = value; }
        }
        public string Desc
        {
            get { return this.desc; }
            set { this.desc = value; }
        }
        public int State
        {
            get { return this.state; }
            set { this.state = value; }
        }
        public string SiteCode
        {
            get { return this.siteCode; }
            set { this.siteCode = value; }
        }
        public DateTime StartTime
        {
            get { return this.startTime; }
            set { this.startTime = value; }
        }
        public DateTime EndTime
        {
            get { return this.endTime; }
            set { this.endTime = value; }
        }
        public int IsTop
        {
            get { return this.isTop; }
            set { this.isTop = value; }
        }
        public int Credits
        {
            get { return this.credits; }
            set { this.credits = value; }
        }
        public int Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
    }
}
