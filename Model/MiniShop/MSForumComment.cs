using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    /// <summary>
    /// 评论 类属性构造器
    /// </summary>
   public class MSForumComment
    {
       /// <summary>
       /// 自编号
       /// </summary>
        private string iD;
       /// <summary>
       /// 回复编号
       /// </summary>
        private string upID;
       /// <summary>
       /// 帖子编号
       /// </summary>
        private string tID;
       /// <summary>
       /// 评论/回复 用户编号
       /// </summary>
        private string uID;
       /// <summary>
       /// 评论内容
       /// </summary>
        private string ctext;
       /// <summary>
       /// 审核 默认0 不通过   1通过
       /// </summary>
        private int review;
       /// <summary>
       /// 信息状态 默认0 正常 1 删除
       /// </summary>
        private int cstate;
       /// <summary>
       /// 评论时间
       /// </summary>
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
       /// 回复编号
       /// </summary>
        public string UpID
        {
            get { return upID; }
            set { upID = value; }
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
       /// 用户编号
       /// </summary>
        public string UID
        {
            get { return uID; }
            set { uID = value; }
        }
       /// <summary>
       /// 回复内容
       /// </summary>
        public string Ctext
        {
            get { return ctext; }
            set { ctext = value; }
        }
       /// <summary>
        /// 审核 默认0 不通过   1通过
       /// </summary>
        public int Review
        {
            get { return review; }
            set { review = value; }
        }
       /// <summary>
        /// 信息状态 默认0 正常 1 删除
       /// </summary>
        public int Cstate
        {
            get { return cstate; }
            set { cstate = value; }
        }
       /// <summary>
        /// 评论时间
       /// </summary>
        public DateTime AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }
    }
}
