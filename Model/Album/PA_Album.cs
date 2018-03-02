//============================================================
// Producnt name:		Mozart
// Version: 			1.0
// Coded by:			Laoshimonk@qq.com
// Auto generated at: 	2014/4/9 17:03:46
//============================================================

using System;
namespace Model.PA
{
    public class PA_Album
    {
        private string iD = String.Empty;
        private string type = String.Empty;
        private string name = String.Empty;
        private string photo = String.Empty;
        private string note = String.Empty;
        private int isDel;
        public string ID
        {
            get { return this.iD; }
            set { this.iD = value; }
        }
        public string Type
        {
            get { return this.type; }
            set { this.type = value; }
        }
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public string Photo
        {
            get { return this.photo; }
            set { this.photo = value; }
        }
        public string Note
        {
            get { return this.note; }
            set { this.note = value; }
        }
        public int IsDel
        {
            get { return isDel; }
            set { isDel = value; }
        }
    }
}
