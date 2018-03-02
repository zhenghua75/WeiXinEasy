using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.HoliDay
{
   public class HD_HoliDay
    {
        private string iD;
        private string siteCode;
        private string htitle;
        private string himg;
        private string hcontent;
        private DateTime hstartTime;
        private DateTime hendTime;
        private DateTime haddTime;
        private int hisdel;

        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        public string SiteCode
        {
            get { return siteCode; }
            set { siteCode = value; }
        }
        public string Htitle
        {
            get { return htitle; }
            set { htitle = value; }
        }
        public string Himg
        {
            get { return himg; }
            set { himg = value; }
        }
        public string Hcontent
        {
            get { return hcontent; }
            set { hcontent = value; }
        }
        public DateTime HstartTime
        {
            get { return hstartTime; }
            set { hstartTime = value; }
        }
        public DateTime HendTime
        {
            get { return hendTime; }
            set { hendTime = value; }
        }
        public DateTime HaddTime
        {
            get { return haddTime; }
            set { haddTime = value; }
        }
        public int Hisdel
        {
            get { return hisdel; }
            set { hisdel = value; }
        }
    }
}
