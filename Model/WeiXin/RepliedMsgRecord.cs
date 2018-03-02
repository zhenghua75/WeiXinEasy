/* ==============================================================================
 * 类名称：RepliedMsgRecord
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/3/15 16:42:35
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
    public class RepliedMsgRecord
    {
        public RepliedMsgRecord()
        {
            ID = Guid.NewGuid().ToString("N");
        }

        public string ID { get; set; }

        public string WXConfigID { get; set; }

        /// <summary>
        /// 同微信回复消息类型
        /// </summary>
        public string MsgType { get; set; }

        public string MsgBody { get; set; }

        public string ToUserName { get; set; }

        public string FromUserName { get; set; }

        public string CreateTime { get; set; }
    }
}
