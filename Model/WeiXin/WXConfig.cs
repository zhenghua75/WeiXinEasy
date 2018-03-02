/* ==============================================================================
 * 类名称：WXConfig
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/3/15 10:29:37
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
    public class WXConfig
    {
        public WXConfig()
        {
        }

        public string ID { get; set; }

        public string SiteCode { get; set; }

        public string WXID { get; set; }

        public string WXName { get; set; }

        public string WXToken { get; set; }

        public string WXAppID { get; set; }

        public string WXAppSecret { get; set; }

        /// <summary>
        /// 状态，1表示有效
        /// </summary>
        public int? State { get; set; }

        /// <summary>
        /// 消息加密方式，0：明文模式，1：兼容模式，2：安全模式
        /// </summary>
        public int? EncryptMode { get; set; }

        public string EncodingAESKey { get; set; }
    }
}
