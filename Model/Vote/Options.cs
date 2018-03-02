//============================================================
// Producnt name:		Mozart
// Version: 			1.0
// Coded by:			Laoshimonk@qq.com
// Auto generated at: 	2014/4/16 17:32:16
//============================================================

using System;
namespace Model.Vote
{
    public class VOTE_Option
    {
        private string iD = String.Empty;
        private string subjectID = String.Empty;
        private string title = String.Empty;
        private int order;
        private string ico;
        private string contentdesc;
        private int amount;
        private int isDel;


        public string ID
        {
            get { return this.iD; }
            set { this.iD = value; }
        }
        public string SubjectID
        {
            get { return this.subjectID; }
            set { this.subjectID = value; }
        }
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }
        public int Order
        {
            get { return order; }
            set { order = value; }
        }
        public string Ico
        {
            get { return ico; }
            set { ico = value; }
        }
        public string Contentdesc
        {
            get { return contentdesc; }
            set { contentdesc = value; }
        }
        public int Amount
        {
            get { return this.amount; }
            set { this.amount = value; }
        }
        public int IsDel
        {
            get { return isDel; }
            set { isDel = value; }
        }
    }
}
