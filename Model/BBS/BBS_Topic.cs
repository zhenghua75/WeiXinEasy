//============================================================
// Producnt name:		Mozart
// Version: 			1.0
// Coded by:			Laoshimonk@qq.com
// Auto generated at: 	2014/4/8 18:27:39
//============================================================

using System;
namespace Model.BBS
{
    public class BBS_Topic
    {
        private string iD = String.Empty;
        private string sID = String.Empty;
        private string uID = String.Empty;
        private string topic = String.Empty;
        private string content = String.Empty;
        private int clickCount;
        private int flag;
        private int isTop;
        private int isElite;
        private DateTime lastDate;
        private int replyCount;
        public string ID
        {
            get { return this.iD; }
            set { this.iD = value; }
        }
        public string SID
        {
            get { return this.sID; }
            set { this.sID = value; }
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
        public int ClickCount
        {
            get { return this.clickCount; }
            set { this.clickCount = value; }
        }
        public int Flag
        {
            get { return this.flag; }
            set { this.flag = value; }
        }
        public int IsTop
        {
            get { return this.isTop; }
            set { this.isTop = value; }
        }
        public int IsElite
        {
            get { return this.isElite; }
            set { this.isElite = value; }
        }
        public DateTime LastDate
        {
            get { return this.lastDate; }
            set { this.lastDate = value; }
        }
        public int ReplyCount
        {
            get { return this.replyCount; }
            set { this.replyCount = value; }
        }
    }

    public class BBS_TopicInfo
    {
        private string sName = String.Empty;
        private string iD = String.Empty;
        private string sID = String.Empty;
        private string uID = String.Empty;
        private string topic = String.Empty;
        private string content = String.Empty;
        private int clickCount;
        private int flag;
        private int isTop;
        private int isElite;
        private DateTime lastDate;
        private int replyCount;

        public string SName
        {
            get { return this.sName; }
            set { this.sName = value; }
        }
        public string ID
        {
            get { return this.iD; }
            set { this.iD = value; }
        }
        public string SID
        {
            get { return this.sID; }
            set { this.sID = value; }
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
        public int ClickCount
        {
            get { return this.clickCount; }
            set { this.clickCount = value; }
        }
        public int Flag
        {
            get { return this.flag; }
            set { this.flag = value; }
        }
        public int IsTop
        {
            get { return this.isTop; }
            set { this.isTop = value; }
        }
        public int IsElite
        {
            get { return this.isElite; }
            set { this.isElite = value; }
        }
        public DateTime LastDate
        {
            get { return this.lastDate; }
            set { this.lastDate = value; }
        }
        public int ReplyCount
        {
            get { return this.replyCount; }
            set { this.replyCount = value; }
        }
    }
}
