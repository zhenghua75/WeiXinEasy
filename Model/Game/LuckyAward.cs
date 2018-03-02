using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Game
{
   public class LuckyAward
    {
        private string iD;
        private string actID;
        private string award;
        private string awardContent;
        private int awardNum;
        private int awardPro;
        private int awardSort;
        private DateTime addTime;
        private int isDel;

       /// <summary>
       /// 奖项编号
       /// </summary>
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
       /// <summary>
       /// 活动编码
       /// </summary>
        public string ActID
        {
            get { return actID; }
            set { actID = value; }
        }
       /// <summary>
       /// 奖项
       /// </summary>
        public string Award
        {
            get { return award; }
            set { award = value; }
        }
       /// <summary>
       /// 奖项内容
       /// </summary>
        public string AwardContent
        {
            get { return awardContent; }
            set { awardContent = value; }
        }
       /// <summary>
       /// 奖项数量
       /// </summary>
        public int AwardNum
        {
            get { return awardNum; }
            set { awardNum = value; }
        }
       /// <summary>
       /// 几率
       /// </summary>
        public int AwardPro
        {
            get { return awardPro; }
            set { awardPro = value; }
        }
       /// <summary>
       /// 奖项类别 0:大转盘  1：刮刮卡
       /// </summary>
        public int AwardSort
        {
            get { return awardSort; }
            set { awardSort = value; }
        }
       /// <summary>
       /// 添加时间
       /// </summary>
        public DateTime AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }
       /// <summary>
        /// 奖项状态  1为删除 0为正常
       /// </summary>
        public int IsDel
        {
            get { return isDel; }
            set { isDel = value; }
        }
    }
}
