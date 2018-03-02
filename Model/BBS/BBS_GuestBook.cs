using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Model.BBS
{
   public class BBS_GuestBook
    {
        private string iD;
        private string userName;
        private string userMobile;
        private string content;
        private string replay;
        private int state;
        private DateTime createTime;
        private string siteCode;

        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string UserMobile
        {
            get { return userMobile; }
            set { userMobile = value; }
        }
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        public string Replay
        {
            get { return replay; }
            set { replay = value; }
        }
        public int State
        {
            get { return state; }
            set { state = value; }
        }
        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }
        public string SiteCode
        {
            get { return siteCode; }
            set { siteCode = value; }
        }
    }
}
