/* ==============================================================================
 * 类名称：MsgAutoRule
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/3/15 14:30:15
 * 修改人：
 * 修改时间：
 * 修改备注：
 * @version 1.0
 * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.WeiXin
{
    public class MsgAutoRule
    {
        //ID,WXConfigID,MatchType,MatchPattern,MsgType,MsgValue,Handle,AddTime,AddUser,LastModTime,LastModUser,Order,Enabled
        //public string ID { get; set; }
        //public string WXConfigID { get; set; }
        ///// <summary>
        ///// default：默认自动回复
        ///// subscribe：订阅自动回复
        ///// keywords：关键字自动回复 
        ///// regexp：正则表达式自动回复
        ///// </summary>
        //public string MatchType { get; set; }
        //public string MatchPattern { get; set; }
        ///// <summary>
        ///// 同消息消息类型
        ///// </summary>
        //public string MsgType { get; set; }
        //public string MsgValue { get; set; }
        //public string Handle { get; set; }
        //public DateTime? AddTime { get; set; }
        //public string AddUser { get; set; }
        //public DateTime? LastModTime { get; set; }
        //public string LastModUser { get; set; }
        private string iD;
        private string wXConfigID;
        private string matchType;
        private string matchPattern;
        private string msgType;
        private string msgValue;
        private string handle;
        private DateTime addTime;
        private string addUser;
        private DateTime lastModTime;
        private string lastModUser;
        private int order;
        private int enabled;
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        public string WXConfigID
        {
            get { return wXConfigID; }
            set { wXConfigID = value; }
        }
        public string MatchType
        {
            get { return matchType; }
            set { matchType = value; }
        }
        public string MsgType
        {
            get { return msgType; }
            set { msgType = value; }
        }
        public string MatchPattern
        {
            get { return matchPattern; }
            set { matchPattern = value; }
        }
        public string MsgValue
        {
            get { return msgValue; }
            set { msgValue = value; }
        }
        public string Handle
        {
            get { return handle; }
            set { handle = value; }
        }
        public DateTime AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }
        public string AddUser
        {
            get { return addUser; }
            set { addUser = value; }
        }
        public DateTime LastModTime
        {
            get { return lastModTime; }
            set { lastModTime = value; }
        }
        public string LastModUser
        {
            get { return lastModUser; }
            set { lastModUser = value; }
        }
        public int Order
        {
            get { return order; }
            set { order = value; }
        }
        public int Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

    }
}
