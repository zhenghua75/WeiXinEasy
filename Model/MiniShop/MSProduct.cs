using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    /// <summary>
    /// 店铺产品
    /// </summary>
   public class MSProduct
    {
        private string iD;
        private string customerID;
        private string siteCode;
        private string sID;
        private string cid;
        private string ptitle;
        private string pcontent;
        private decimal price;
        private int isSecHand;
        private int pstate;
        private int review;
        private string zipCode;
        private DateTime addTime;

        /// <summary>
        /// 产品编号
        /// </summary>
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
       /// <summary>
       /// 客户编号
       /// </summary>
        public string CustomerID
        {
            get { return customerID; }
            set { customerID = value; }
        }
        /// <summary>
        /// 登录代码
        /// </summary>
        public string SiteCode
        {
            get { return siteCode; }
            set { siteCode = value; }
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
        /// 产品类别编号
        /// </summary>
        public string Cid
        {
            get { return cid; }
            set { cid = value; }
        }
        /// <summary>
        /// 产品标题
        /// </summary>
        public string Ptitle
        {
            get { return ptitle; }
            set { ptitle = value; }
        }
        /// <summary>
        /// 内容描述
        /// </summary>
        public string Pcontent
        {
            get { return pcontent; }
            set { pcontent = value; }
        }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }
       /// <summary>
       /// 产品是否二手 0表示否 1表示是
       /// </summary>
        public int IsSecHand
        {
            get { return isSecHand; }
            set { isSecHand = value; }
        }
        /// <summary>
        /// 产品状态 0表示有效 1表示无效
        /// </summary>
        public int Pstate
        {
            get { return pstate; }
            set { pstate = value; }
        }
       /// <summary>
       /// 审核情况  0为未通过 1为通过
       /// </summary>
        public int Review
        {
            get { return review; }
            set { review = value; }
        }
       /// <summary>
       /// 邮费 空表示免邮
       /// </summary>
        public string ZipCode
        {
            get { return zipCode; }
            set { zipCode = value; }
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

   public class MSVProduct
   {
       public string ID { get; set; }
       public string Ptitle { get; set; }
       public decimal Price { get; set; }
       public string PimgUrl { get; set; }

  }
}
