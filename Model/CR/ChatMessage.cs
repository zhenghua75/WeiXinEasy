using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.CR
{
   public class ChatMessage
    {
        private string iD;
        private string userID;
        private int roomID;
        private string msgType;
        private string msgText;
        private int msgState;
        private int isDel;
        private DateTime addTime;

        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        public int RoomID
        {
            get { return roomID; }
            set { roomID = value; }
        }
        public string MsgType
        {
            get { return msgType; }
            set { msgType = value; }
        }
        public string MsgText
        {
            get { return msgText; }
            set { msgText = value; }
        }
        public int MsgState
        {
            get { return msgState; }
            set { msgState = value; }
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
