using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.RC
{
   public class RC_Race
    {
        private string iD;
        private string siteCode;
        private string rtitle;
        private string raceDesc;
        private string appID;
        private string codeImg;
        private int moveNum;
        private string startTime;
        private string endTime;
        private int isDel;
        private DateTime addTime;

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
        public string Rtitle
        {
            get { return rtitle; }
            set { rtitle = value; }
        }
        public string RaceDesc
        {
            get { return raceDesc; }
            set { raceDesc = value; }
        }
        public string AppID
        {
            get { return appID; }
            set { appID = value; }
        }
        public string CodeImg
        {
            get { return codeImg; }
            set { codeImg = value; }
        }
        public int MoveNum
        {
            get { return moveNum; }
            set { moveNum = value; }
        }
        public string StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        public string EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
        public int IsDel
        {
            get { return isDel; }
            set { isDel = value; }
        }
        public DateTime AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }
    }
}
