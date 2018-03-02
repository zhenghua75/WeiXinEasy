/* ==============================================================================
 * 类名称：CouponNews
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/4/5 11:07:31
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
    public class CouponNews
    {
        public string ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string PicUrl { get; set; }

        public string Url { get; set; }

        public DateTime? AddTime { get; set; }

        public string Remark { get; set; }

        public string WXConfigID { get; set; }
    }
}
