//============================================================
// Producnt name:		Mozart
// Version: 			1.0
// Coded by:			Laoshimonk@qq.com
// Auto generated at: 	2014/4/16 17:30:58
//============================================================

using System;
namespace Model.Vote
{
    public class VOTE_Subject
    {
        private string iD = String.Empty;
        private string subject = String.Empty;
        private string content = String.Empty;
        private string type = String.Empty;
        private string siteCode = String.Empty;
        private DateTime beginTime;
        private DateTime endTime;
        private DateTime createTime;
        private int isValid;
        public string ID
        {
            get { return this.iD; }
            set { this.iD = value; }
        }
        public string Subject
        {
            get { return this.subject; }
            set { this.subject = value; }
        }
        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }
        public string Type
        {
            get { return this.type; }
            set { this.type = value; }
        }
        public string SiteCode
        {
            get { return this.siteCode; }
            set { this.siteCode = value; }
        }
        public DateTime BeginTime
        {
            get { return this.beginTime; }
            set { this.beginTime = value; }
        }
        public DateTime EndTime
        {
            get { return this.endTime; }
            set { this.endTime = value; }
        }
        public DateTime CreateTime
        {
            get { return this.createTime; }
            set { this.createTime = value; }
        }
        public int IsValid
        {
            get { return this.isValid; }
            set { this.isValid = value; }
        }
    }
}
