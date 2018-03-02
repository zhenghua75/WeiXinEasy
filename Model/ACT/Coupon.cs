/* ==============================================================================
 * 类名称：Coupon
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/3/22 14:51:23
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

namespace Model.ACT
{
    public class Coupon
    {
        public Coupon()
        {
            ID = Guid.NewGuid().ToString("N");
            Random random = new Random(Guid.NewGuid().GetHashCode());
            CouponCode = string.Format("{0}{1}",DateTime.Now.ToString("ddHHmm"),random.Next(10,99));
        }

        public string ID{ get; set; }             
        public string SiteCode { get; set; }      
        public string SiteActivityID { get; set; }
        public string OpenID { get; set; }
        public string CouponCode{ get; set; }     
        public string LimitTime { get; set; }
        public DateTime? CheckTime { get; set; }
        public int CouponStatus { get; set; }  
        public DateTime? AddTime { get; set; }
        public string Remark { get; set; }        
    } 

    public class MyCouponInfo
    {
        public string ID { get; set; }
        public string ActTitle { get; set; }
        public string Photo { get; set; }
        public string AddTime { get; set; }
        public string CheckTime { get; set; }
        public string RemainDay { get; set; }
        public string ActContent { get; set; }
        public string CouponStatus { get; set; }
        public string CouponCode { get; set; }
        public string Remark { get; set; }
        public string SiteCode { get; set; }
    }
}
