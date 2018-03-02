//============================================================
// Producnt name:		Mozart
// Version: 			1.0
// Coded by:			Laoshimonk@qq.com
// Auto generated at: 	2014/4/8 18:28:22
//============================================================

using System;
namespace Model.BBS
{
    public class BBS_Reply
    {
        private string iD = String.Empty;
        private string rID = String.Empty;
        private string uID = String.Empty;
        private string topic = String.Empty;
        private string content = String.Empty;
        private DateTime rDate;
        private int flag;
        public string ID
        {
            get { return this.iD; }
            set { this.iD = value; }
        }
        public string RID
        {
            get { return this.rID; }
            set { this.rID = value; }
        }
        public string UID
        {
            get { return this.uID; }
            set { this.uID = value; }
        }
        public string Topic
        {
            get { return this.topic; }
            set { this.topic = value; }
        }
        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }
        public DateTime RDate
        {
            get { return this.rDate; }
            set { this.rDate = value; }
        }
        public int Flag
        {
            get { return this.flag; }
            set { this.flag = value; }
        }
    }
}
