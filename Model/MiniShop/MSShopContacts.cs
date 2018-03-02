using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    /// <summary>
    /// 店铺联系方式
    /// </summary>
    public class MSShopContacts
    {
        private string iD;
        private string sID;
        private string pID;
        private string nickName;
        private string phone;
        private int qQnum;
        private string email;
        private string address;
        private int isDefault;
        private int isDel;
        private DateTime addTime;

        /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
            get { return iD; }
            set { iD = value; }
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
        /// 产品编号
        /// </summary>
        public string PID
        {
            get { return pID; }
            set { pID = value; }
        }
        /// <summary>
        /// 称呼
        /// </summary>
        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }
        /// <summary>
        /// 电话、手机
        /// </summary>
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        /// <summary>
        /// QQ号
        /// </summary>
        public int QQnum
        {
            get { return qQnum; }
            set { qQnum = value; }
        }
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        /// <summary>
        /// 地址是否为默认
        /// 0表示否 1表示 是
        /// </summary>
        public int IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; }
        }
        /// <summary>
        /// 状态 0表示有效 1表示无效
        /// </summary>
        public int IsDel
        {
            get { return isDel; }
            set { isDel = value; }
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
