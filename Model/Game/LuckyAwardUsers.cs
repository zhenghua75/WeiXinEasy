using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Game
{
    public class LuckyAwardUsers
    {
        private string iD;
        private string actID;
        private string aID;
        private string openID;
        private string nickName;
        private string phone;
        private string sNCode;
        private int sendAward;
        private DateTime addTime;
        private int isDel;

        /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        /// <summary>
        /// 活动编号
        /// </summary>
        public string ActID
        {
            get { return actID; }
            set { actID = value; }
        }
        /// <summary>
        /// 奖项编号
        /// </summary>
        public string AID
        {
            get { return aID; }
            set { aID = value; }
        }
        /// <summary>
        /// 用户OpenID
        /// </summary>
        public string OpenID
        {
            get { return openID; }
            set { openID = value; }
        }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }
        /// <summary>
        /// 用户电话
        /// </summary>
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        /// <summary>
        /// 用户中奖SN码
        /// </summary>
        public string SNCode
        {
            get { return sNCode; }
            set { sNCode = value; }
        }
        /// <summary>
        /// 是否发送奖品 0为未发放 1为发放
        /// </summary>
        public int SendAward
        {
            get { return sendAward; }
            set { sendAward = value; }
        }
        /// <summary>
        /// 中奖时间
        /// </summary>
        public DateTime AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }
        /// <summary>
        /// 用户状态  1为删除 0为正常
        /// </summary>
        public int IsDel
        {
            get { return isDel; }
            set { isDel = value; }
        }
    }
}
