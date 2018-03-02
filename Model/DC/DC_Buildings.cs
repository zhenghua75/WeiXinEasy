//============================================================
// Producnt name:		Mozart
// Version: 			1.0
// Coded by:			Laoshimonk@qq.com
// Auto generated at: 	2014/5/10 11:01:10
//============================================================

using System;
namespace Model.DC
{
    public class DC_Building
    {
        private string iD = String.Empty;
        private string siteCode = String.Empty;
        private string name = String.Empty;
        private string photo = String.Empty;
        private decimal aVEPrice;
        private decimal greenRate;
        private decimal volumeRate;
        private string school = String.Empty;
        private string parkingSpaces = String.Empty;
        private string propertyDevelopers = String.Empty;
        private string propertyCompany = String.Empty;
        private string regions = String.Empty;
        private string busLine = String.Empty;
        private string address = String.Empty;
        private string content = String.Empty;
        private DateTime createTime;
        private int isDel;
        public string ID
        {
            get { return this.iD; }
            set { this.iD = value; }
        }
        public string SiteCode
        {
            get { return siteCode; }
            set { siteCode = value; }
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
        public decimal AVEPrice
        {
            get { return this.aVEPrice; }
            set { this.aVEPrice = value; }
        }
        public decimal GreenRate
        {
            get { return this.greenRate; }
            set { this.greenRate = value; }
        }
        public decimal VolumeRate
        {
            get { return this.volumeRate; }
            set { this.volumeRate = value; }
        }
        public string School
        {
            get { return this.school; }
            set { this.school = value; }
        }
        public string ParkingSpaces
        {
            get { return this.parkingSpaces; }
            set { this.parkingSpaces = value; }
        }
        public string PropertyDevelopers
        {
            get { return this.propertyDevelopers; }
            set { this.propertyDevelopers = value; }
        }
        public string PropertyCompany
        {
            get { return this.propertyCompany; }
            set { this.propertyCompany = value; }
        }
        public string Regions
        {
            get { return this.regions; }
            set { this.regions = value; }
        }
        public string BusLine
        {
            get { return this.busLine; }
            set { this.busLine = value; }
        }
        public string Address
        {
            get { return this.address; }
            set { this.address = value; }
        }
        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }
        public DateTime CreateTime
        {
            get { return this.createTime; }
            set { this.createTime = value; }
        }
        public int IsDel
        {
            get { return this.isDel; }
            set { this.isDel = value; }
        }
    }
}
