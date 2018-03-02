using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.CMS
{
    public class CMS_Category
    {
        private string iD = String.Empty;
        private string siteCode = String.Empty;
        private string name = String.Empty;
        private string pic = String.Empty;
        private string summary = string.Empty;
        private string content = String.Empty;
        private string link = String.Empty;
        private bool isDel;
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
        public string Pic
        {
            get { return pic; }
            set { pic = value; }
        }
        public string Summary
        {
            get { return this.summary; }
            set { this.summary = value; }
        }
        public bool IsDel
        {
            get { return this.isDel; }
            set { this.isDel = value; }
        }
        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }
        public string Link
        {
            get { return this.link; }
            set { this.link = value; }
        }
        public int Order { get; set; }
    }
}
