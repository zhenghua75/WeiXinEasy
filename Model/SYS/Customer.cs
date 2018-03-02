//============================================================
// Producnt name:		Mozart
// Version: 			1.0
// Coded by:			Laoshimonk@qq.com
// Auto generated at: 	2014/3/27 17:44:02
//============================================================

using System;
namespace Model.SYS
{
    public class SYS_Customer
    {
        private string iD = String.Empty;
        private string siteCode = String.Empty;
        private string mobile = String.Empty;
        private string name = String.Empty;
        private string passWord = String.Empty;
        private string openID = String.Empty;
        private string memberShipNo = string.Empty;
        public string ID
        {
            get { return this.iD; }
            set { this.iD = value; }
        }
        public string SiteCode
        {
            get { return this.siteCode; }
            set { this.siteCode = value; }
        }
        public string Mobile
        {
            get { return this.mobile; }
            set { this.mobile = value; }
        }
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public string PassWord
        {
            get { return this.passWord; }
            set { this.passWord = value; }
        }
        public string OpenID
        {
            get { return this.openID; }
            set { this.openID = value; }
        }
        public string MemberShipNo
        {
            get { return this.memberShipNo; }
            set { this.memberShipNo = value; }
        }
    }
}
