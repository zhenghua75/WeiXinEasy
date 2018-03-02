using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    /// <summary>
    /// 照片提交Model
    /// </summary>
   public class MSPhotoSubmit
    {
       /// <summary>
       /// 自编号
       /// </summary>
        private string iD;
       /// <summary>
       /// 用户编号
       /// </summary>
        private string uID;
       /// <summary>
       /// 订单号
       /// </summary>
        private string orderNum;
       /// <summary>
       /// 照片1
       /// </summary>
        private string img1;
       /// <summary>
       /// 照片1
       /// </summary>
        private string img2;
       /// <summary>
       /// 审核情况 默认0:未通过 1:通过
       /// </summary>
        private int reivew;
       /// <summary>
       /// 信息状态 默认0 正常 1删除
       /// </summary>
        private int pstate;
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
       /// 用户编号
       /// </summary>
        public string UID
        {
            get { return uID; }
            set { uID = value; }
        }
       /// <summary>
       /// 订单编号
       /// </summary>
        public string OrderNum
        {
            get { return orderNum; }
            set { orderNum = value; }
        }
       /// <summary>
       /// 照片1
       /// </summary>
        public string Img1
        {
            get { return img1; }
            set { img1 = value; }
        }
       /// <summary>
       /// 照片2
       /// </summary>
        public string Img2
        {
            get { return img2; }
            set { img2 = value; }
        }
       /// <summary>
        /// 审核情况 默认0:未通过 1:通过
       /// </summary>
        public int Reivew
        {
            get { return reivew; }
            set { reivew = value; }
        }

       /// <summary>
        /// 信息状态 默认0 正常 1删除
       /// </summary>
        public int Pstate
        {
            get { return pstate; }
            set { pstate = value; }
        }
        public DateTime AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }
    }
}
