using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.RC
{
   public class RC_Users
    {
        private string iD;
        private string raceID;
        private string openID;
        private string speed;
        private int isDel;
        private int isWin;
        private DateTime addTime;

        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        public string RaceID
        {
            get { return raceID; }
            set { raceID = value; }
        }
        public string OpenID
        {
            get { return openID; }
            set { openID = value; }
        }
        public string Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public int IsDel
        {
            get { return isDel; }
            set { isDel = value; }
        }
        public int IsWin
        {
            get { return isWin; }
            set { isWin = value; }
        }
        public DateTime AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }
    }
}
