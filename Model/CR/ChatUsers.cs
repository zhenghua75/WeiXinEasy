using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.CR
{
   public class ChatUsers
    {
        private string iD;
        private string nickName;
        private string openID;
        private int roomID;
        private DateTime addTime;
        private int isDel;
        private int isWin;

        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }
        public string OpenID
        {
            get { return openID; }
            set { openID = value; }
        }
        public int RoomID
        {
            get { return roomID; }
            set { roomID = value; }
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
        public int IsWin
        {
            get { return isWin; }
            set { isWin = value; }
        }
    }
}
