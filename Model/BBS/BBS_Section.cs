//============================================================
// Producnt name:		Mozart
// Version: 			1.0
// Coded by:			Laoshimonk@qq.com
// Auto generated at: 	2014/4/8 18:26:38
//============================================================

using System;
namespace Model.BBS
{
    public class BBS_Section
    {
        private string iD = String.Empty;
        private string siteCode = String.Empty;
        private string name = String.Empty;
        private string manager = String.Empty;
        private string statement = String.Empty;
        private int clickCount;
        private int topicCount;
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
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public string Manager
        {
            get { return this.manager; }
            set { this.manager = value; }
        }
        public string Statement
        {
            get { return this.statement; }
            set { this.statement = value; }
        }
        public int ClickCount
        {
            get { return this.clickCount; }
            set { this.clickCount = value; }
        }
        public int TopicCount
        {
            get { return this.topicCount; }
            set { this.topicCount = value; }
        }
    }
}
