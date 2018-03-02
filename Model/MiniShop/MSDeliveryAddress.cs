using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    /// <summary>
    /// 客户收货地址
    /// </summary>
   public class MSDeliveryAddress
    {
        private string iD;
        private string uID;
        private string daName;
        private string daPhone;
        private string daAddress;
        private string addressDetail;
        private string daZipCode;
        private int isDefault;
        private int isDel;
       /// <summary>
       /// 收货地址编号
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
       /// 收货人称呼
       /// </summary>
        public string DaName
        {
            get { return daName; }
            set { daName = value; }
        }
       /// <summary>
       /// 收货人电话
       /// </summary>
        public string DaPhone
        {
            get { return daPhone; }
            set { daPhone = value; }
        }
       /// <summary>
       /// 收货区域
       /// </summary>
        public string DaAddress
        {
            get { return daAddress; }
            set { daAddress = value; }
        }
       /// <summary>
       /// 收货地址详细
       /// </summary>
        public string AddressDetail
        {
            get { return addressDetail; }
            set { addressDetail = value; }
        }
       /// <summary>
       /// 邮编
       /// </summary>
        public string DaZipCode
        {
            get { return daZipCode; }
            set { daZipCode = value; }
        }
       /// <summary>
       /// 是否默认 1为默认，0为正常
       /// </summary>
        public int IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; }
        }
       /// <summary>
       /// 收货地址状态：0为正常  1为删除
       /// </summary>
        public int IsDel
        {
            get { return isDel; }
            set { isDel = value; }
        }
    }
}
