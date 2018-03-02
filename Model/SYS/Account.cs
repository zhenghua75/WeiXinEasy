//============================================================
// Producnt name:		Mozart
// Version: 			1.0
// Coded by:			Laoshimonk@qq.com
// Auto generated at: 	2014/3/12 15:44:09
//============================================================

using System;
namespace Model.SYS
{
    public class SYS_Account
    {
        private string iD = String.Empty;
        private string loginName = String.Empty;
        private string password = String.Empty;
        private string agentID = String.Empty;
        private string name = String.Empty;
        private string roleID = String.Empty;
        private string email = String.Empty;
        private string address = String.Empty;
        private string mobile = String.Empty;
        private string telphone = String.Empty;
        private string status = String.Empty;
        private DateTime createTime;
        private string siteCode = String.Empty;
        public string ID
        {
            get { return this.iD; }
            set { this.iD = value; }
        }
        public string LoginName
        {
            get { return this.loginName; }
            set { this.loginName = value; }
        }
        public string Password
        {
            get { return this.password; }
            set { this.password = value; }
        }
        public string AgentID
        {
            get { return this.agentID; }
            set { this.agentID = value; }
        }
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public string RoleID
        {
            get { return this.roleID; }
            set { this.roleID = value; }
        }
        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }
        public string Address
        {
            get { return this.address; }
            set { this.address = value; }
        }
        public string Mobile
        {
            get { return this.mobile; }
            set { this.mobile = value; }
        }
        public string Telphone
        {
            get { return this.telphone; }
            set { this.telphone = value; }
        }
        public string Status
        {
            get { return this.status; }
            set { this.status = value; }
        }
        public DateTime CreateTime
        {
            get { return this.createTime; }
            set { this.createTime = value; }
        }
        public string SiteCode
        {
            get { return this.siteCode; }
            set { this.siteCode = value; }
        }
    }
}
