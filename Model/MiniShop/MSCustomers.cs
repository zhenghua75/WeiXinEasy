using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    /// <summary>
    /// 客户表
    /// </summary>
    public class MSCustomers
    {
        private string iD;
        private string openID;
        private string nickName;
        private string realName;
        private string iDnum;
        private string iDimg;
        private string phone;
        private string userPwd;
        private string email;
        private string qQnum;
        private string headImg;
        private int sex;
        private string pnote;
        private int isDel;
        private DateTime addTime;
        #region 编号
        /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        #endregion
        /// <summary>
        /// 微信标识
        /// </summary>
        public string OpenID
        {
            get { return openID; }
            set { openID = value; }
        }
        #region 用户昵称
        /// <summary>
        /// 用户名
        /// </summary>
        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }
        #endregion
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName
        {
            get { return realName; }
            set { realName = value; }
        }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDnum
        {
            get { return iDnum; }
            set { iDnum = value; }
        }
        /// <summary>
        /// 身份证照片
        /// </summary>
        public string IDimg
        {
            get { return iDimg; }
            set { iDimg = value; }
        }
        #region 电话
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        #endregion
        #region 登录密码
        /// <summary>
        /// 登录密码
        /// </summary>
        public string UserPwd
        {
            get { return userPwd; }
            set { userPwd = value; }
        }
        #endregion
        #region 电子邮件
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        #endregion
        #region QQ号码
        /// <summary>
        /// QQ号码
        /// </summary>
        public string QQnum
        {
            get { return qQnum; }
            set { qQnum = value; }
        }
        #endregion
        #region 头像
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImg
        {
            get { return headImg; }
            set { headImg = value; }
        }
        #endregion
        #region 性别 0表示男 1表示女
        /// <summary>
        /// 性别 0表示男 1表示女
        /// </summary>
        public int Sex
        {
            get { return sex; }
            set { sex = value; }
        }
        #endregion
        #region 个人说明
        /// <summary>
        /// 个人说明
        /// </summary>
        public string Pnote
        {
            get { return pnote; }
            set { pnote = value; }
        }
        #endregion
        #region 用户状态 0表示正常 1表示已删
        /// <summary>
        /// 用户状态 0表示正常 1表示已删
        /// </summary>
        public int IsDel
        {
            get { return isDel; }
            set { isDel = value; }
        }
        #endregion
        #region 添加时间
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }
        #endregion
    }
}
