using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Model.CR
{
   public class ChatRoom
    {
        private int iD;
        private string siteCode;
        private string roomName;
        private int phoneNum;
        private string roomDesc;
        private string roomImg;
        private string appID;
        private string webCodeImg;
        private DateTime addTime;
        private int isDel;

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
        public string SiteCode
        {
            get { return siteCode; }
            set { siteCode = value; }
        }
        public string RoomName
        {
            get { return roomName; }
            set { roomName = value; }
        }
        public int PhoneNum
        {
            get { return phoneNum; }
            set { phoneNum = value; }
        }
        public string RoomDesc
        {
            get { return roomDesc; }
            set { roomDesc = value; }
        }
        public string RoomImg
        {
            get { return roomImg; }
            set { roomImg = value; }
        }
        public string AppID
        {
            get { return appID; }
            set { appID = value; }
        }
        public string WebCodeImg
        {
            get { return webCodeImg; }
            set { webCodeImg = value; }
        }
        public DateTime AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }
        public int IsDel
        {
            get { return isDel; }
            set { isDel = value; }
        }
    }
}
