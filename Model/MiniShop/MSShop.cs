using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    /// <summary>
    /// 店铺
    /// </summary>
    public class MSShop
    {
        private string iD;
        private string uID;
        private string shopName;
        private string wXName;
        private string wXNum;
        private string shopLogo;
        private string shopBackImg;
        private string shopDesc;
        private int shopState;
        private DateTime addTime;

        /// <summary>
        /// 店铺编号
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
        /// 店铺名称
        /// </summary>
        public string ShopName
        {
            get { return shopName; }
            set { shopName = value; }
        }
        /// <summary>
        /// 微信名称
        /// </summary>
        public string WXName
        {
            get { return wXName; }
            set { wXName = value; }
        }
        /// <summary>
        /// 微信号
        /// </summary>
        public string WXNum
        {
            get { return wXNum; }
            set { wXNum = value; }
        }
        
        /// <summary>
        /// 店铺LOGO
        /// </summary>
        public string ShopLogo
        {
            get { return shopLogo; }
            set { shopLogo = value; }
        }
        /// <summary>
        /// 店铺描述
        /// </summary>
        public string ShopDesc
        {
            get { return shopDesc; }
            set { shopDesc = value; }
        }
        /// <summary>
        /// 店铺背景
        /// </summary>
        public string ShopBackImg
        {
            get { return shopBackImg; }
            set { shopBackImg = value; }
        }
        /// <summary>
        /// 店铺状态 0表示有效 1表示无效
        /// </summary>
        public int ShopState
        {
            get { return shopState; }
            set { shopState = value; }
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
