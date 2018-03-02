using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    /// <summary>
    /// 产品类别
    /// </summary>
    public class MSProductCategory
    {
        private string iD;
        private string upID;
        private string sID;
        private string cname;
        private int csecHand;
        private int cstate;
        private int sortin;
        private DateTime addTime;

        /// <summary>
        /// 类别编号
        /// </summary>
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        /// <summary>
        /// 一级导航类别编号
        /// </summary>
        public string UpID
        {
            get { return upID; }
            set { upID = value; }
        }
        /// <summary>
        /// 店铺编号
        /// </summary>
        public string SID
        {
            get { return sID; }
            set { sID = value; }
        }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string Cname
        {
            get { return cname; }
            set { cname = value; }
        }
        /// <summary>
        /// 是否二手类别 默认0：否  1：是
        /// </summary>
        public int CsecHand
        {
            get { return csecHand; }
            set { csecHand = value; }
        }
        /// <summary>
        /// 类别状态 0表示有效 1表示无效
        /// </summary>
        public int Cstate
        {
            get { return cstate; }
            set { cstate = value; }
        }
        /// <summary>
        /// 导航排序
        /// </summary>
        public int Sortin
        {
            get { return sortin; }
            set { sortin = value; }
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
