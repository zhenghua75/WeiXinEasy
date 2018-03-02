//============================================================
// Producnt name:		Mozart
// Version: 			1.0
// Coded by:			Laoshimonk@qq.com
// Auto generated at: 	2014/3/12 15:44:09
//============================================================

using System;
namespace Model.SYS
{
    public class SYS_Menu
    {
        private string no = String.Empty;
        private string name = String.Empty;
        private int order;
        private string parent = String.Empty;
        private int level;
        private string url = String.Empty;
        private string icon = String.Empty;
        private string target = String.Empty;
        public string No
        {
            get { return this.no; }
            set { this.no = value; }
        }
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public int Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
        public string Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }
        public int Level
        {
            get { return this.level; }
            set { this.level = value; }
        }
        public string Url
        {
            get { return this.url; }
            set { this.url = value; }
        }
        public string Icon
        {
            get { return this.icon; }
            set { this.icon = value; }
        }
        public string Target
        {
            get { return this.target; }
            set { this.target = value; }
        }
    }
}
