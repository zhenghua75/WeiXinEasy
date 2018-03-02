//============================================================
// Producnt name:		Mozart
// Version: 			1.0
// Coded by:			Laoshimonk@qq.com
// Auto generated at: 	2014/3/20 15:50:54
//============================================================

using System;
namespace Model.CMS
{
    public class CMS_Article
    {
        private string iD = String.Empty;
        private string title = String.Empty;
        private string pic = String.Empty;
        private string summary = String.Empty;
        private string content = String.Empty;
        private string author = String.Empty;
        private string category = String.Empty;
        private int clickNum;
        private bool isTop;
        private bool isDel;
        private DateTime createTime;
        private string createUser = String.Empty;
        private string siteCode = String.Empty;
        private int order;
        public string ID
        {
            get { return this.iD; }
            set { this.iD = value; }
        }
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }
        public string Pic
        {
            get { return this.pic; }
            set { this.pic = value; }
        }
        public string Summary
        {
            get { return this.summary; }
            set { this.summary = value; }
        }
        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }
        public string Author
        {
            get { return this.author; }
            set { this.author = value; }
        }
        public string Category
        {
            get { return category; }
            set { category = value; }
        }
        public int ClickNum
        {
            get { return this.clickNum; }
            set { this.clickNum = value; }
        }
        public bool IsTop
        {
            get { return this.isTop; }
            set { this.isTop = value; }
        }
        public bool IsDel
        {
            get { return this.isDel; }
            set { this.isDel = value; }
        }
        public DateTime CreateTime
        {
            get { return this.createTime; }
            set { this.createTime = value; }
        }
        public string CreateUser
        {
            get { return this.createUser; }
            set { this.createUser = value; }
        }
        public string SiteCode
        {
            get { return this.siteCode; }
            set { this.siteCode = value; }
        }
        public int Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
    }
}
