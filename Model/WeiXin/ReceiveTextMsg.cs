using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.WeiXin
{
    public class ReceiveTextMsg
    {
        public ReceiveTextMsg()
        {
            ID = Guid.NewGuid().ToString("N");
            AddTime = DateTime.Now;
        }

        public string ID { get; set; }

        public string WXConfigID { get; set; }

        public string ToUserName { get; set; }

        public string FromUserName { get; set; }

        public string CreateTime { get; set; }

        /// <summary>
        /// 同微信接收的消息类型
        /// </summary>
        public string MsgType { get; set; }

        public string Content { get; set; }

        public string MsgId { get; set; }

        public DateTime? AddTime { get; set; }

    }
}
