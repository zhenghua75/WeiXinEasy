using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Model.MiniShop
{
    /// <summary>
    /// 帖子点赞和喜欢  实体类
    /// </summary>
   public class MSForumTopicLove
    {
        private string iD;
        private string tID;
        private string uID;
        private int tlike;
        private int tlove;
        private DateTime addTime;

       /// <summary>
       /// 自编号
       /// </summary>
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
       /// <summary>
       /// 帖子编号
       /// </summary>
        public string TID
        {
            get { return tID; }
            set { tID = value; }
        }
       /// <summary>
       /// 户编号
       /// </summary>
        public string UID
        {
            get { return uID; }
            set { uID = value; }
        }
       /// <summary>
       /// 喜欢
       /// </summary>
        public int Tlike
        {
            get { return tlike; }
            set { tlike = value; }
        }
       /// <summary>
       /// 点赞
       /// </summary>
        public int Tlove
        {
            get { return tlove; }
            set { tlove = value; }
        }
       /// <summary>
       /// 添加时间
       /// </summary>
        public DateTime AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }
    }
}
