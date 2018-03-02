using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.HoliDay
{
   public class HD_HoliDayUsers
    {
        private string iD;
        private string hID;
        private string openID;
        private string phone;
        private string nickName;
        private string married;
        private int age;
        private DateTime addTime;
        private int isDel;

        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        public string HID
        {
            get { return hID; }
            set { hID = value; }
        }
        public string OpenID
        {
            get { return openID; }
            set { openID = value; }
        }
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }
        public string Married
        {
            get { return married; }
            set { married = value; }
        }
        public int Age
        {
            get { return age; }
            set { age = value; }
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
