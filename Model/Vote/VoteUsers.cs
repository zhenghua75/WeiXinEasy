using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Vote
{
   public class VoteUsers
    {
        private string iD;
        private string voteID;
        private string subjectID;
        private string openID;
        private string userName;
        private DateTime addTime;
        private string userIP;
        private int isDel;

        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        public string VoteID
        {
            get { return voteID; }
            set { voteID = value; }
        }
        public string SubjectID
        {
            get { return subjectID; }
            set { subjectID = value; }
        }
        public string OpenID
        {
            get { return openID; }
            set { openID = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public DateTime AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }
        public string UserIP
        {
            get { return userIP; }
            set { userIP = value; }
        }
        public int IsDel
        {
            get { return isDel; }
            set { isDel = value; }
        }

    }
}
