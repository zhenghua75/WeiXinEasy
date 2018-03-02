using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Model.SYS
{
   public class Account_Ext
    {
        private string accountID;
        private string photo;
        private string summary;
        private string remark;
        private string siteCategory;
        private string themes;
        private string codeImg;
        private string printImg1;
        private string printImg2;
        private string printImg3;
        private string printImg4;

        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }
        public string Photo
        {
            get { return photo; }
            set { photo = value; }
        }
        public string Summary
        {
            get { return summary; }
            set { summary = value; }
        }
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        public string SiteCategory
        {
            get { return siteCategory; }
            set { siteCategory = value; }
        }
        public string Themes
        {
            get { return themes; }
            set { themes = value; }
        }
        public string CodeImg
        {
            get { return codeImg; }
            set { codeImg = value; }
        }
        public string PrintImg1
        {
            get { return printImg1; }
            set { printImg1 = value; }
        }
        public string PrintImg2
        {
            get { return printImg2; }
            set { printImg2 = value; }
        }
        public string PrintImg3
        {
            get { return printImg3; }
            set { printImg3 = value; }
        }
        public string PrintImg4
        {
            get { return printImg4; }
            set { printImg4 = value; }
        }
    }
}
