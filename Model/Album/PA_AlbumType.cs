//============================================================
// Producnt name:		Mozart
// Version: 			1.0
// Coded by:			Laoshimonk@qq.com
// Auto generated at: 	2014/4/9 17:03:13
//============================================================

using System;
namespace Model.PA
{
    public class PA_AlbumType
    {
        private string iD = String.Empty;
        private string siteCode = String.Empty;
        private string name = String.Empty;
        private string cover = String.Empty;
        private string content = String.Empty;
        private int isDel;
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
        public string Cover
        {
            get { return this.cover; }
            set { this.cover = value; }
        }
        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }
        public int IsDel
        {
            get { return isDel; }
            set { isDel = value; }
        }
    }
}
