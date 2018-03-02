/* ==============================================================================
 * 类名称：Photo
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/4/5 16:38:43
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

namespace Model.HP
{
    public class Photo
    {
        public Photo()
        {
            ID = Guid.NewGuid().ToString("N");
            State = 0;
            Pdate = DateTime.Now;
        }

        public string ID { get; set; }

        public string SiteCode { get; set; }

        public string OpenId { get; set; }

        public string ClientID { get; set; }

        public string PrintCode { get; set; }

        public string Img { get; set; }

        public int? State { get; set; }

        public DateTime Pdate { get; set; } 

        /// <summary>
        /// 附加的文字
        /// </summary>
        public string AttachText { get; set; }
    }
}
