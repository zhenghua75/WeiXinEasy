using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    /// <summary>
    /// 论坛帖子
    /// </summary>
   public class MSForumTopic
    {
        private string iD;
        private string fID;
        private string uID;
        private string topicTitle;
        private string topicDesc;
        private int topicState;
        private int treview;
        private int isTop;
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
       /// 论坛编号
       /// </summary>
        public string FID
        {
            get { return fID; }
            set { fID = value; }
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
       /// 标题
       /// </summary>
        public string TopicTitle
        {
            get { return topicTitle; }
            set { topicTitle = value; }
        }
       /// <summary>
       /// 描述
       /// </summary>
        public string TopicDesc
        {
            get { return topicDesc; }
            set { topicDesc = value; }
        }
       /// <summary>
       /// 状态 默认0：正常  1：删除
       /// </summary>
        public int TopicState
        {
            get { return topicState; }
            set { topicState = value; }
        }
       /// <summary>
       /// 审核情况 默认0：未审核   1:审核
       /// </summary>
        public int Treview
        {
            get { return treview; }
            set { treview = value; }
        }
        /// <summary>
        /// 是否置顶  0:是  1:否   默认1
        /// </summary>
        public int IsTop
        {
            get { return isTop; }
            set { isTop = value; }
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
