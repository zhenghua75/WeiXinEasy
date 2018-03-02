using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    /// <summary>
    /// 订单物流
    /// </summary>
   public class MSOrderLogistics
    {
       /// <summary>
       /// 物流编号
       /// </summary>
        private string iD;
       /// <summary>
       /// 产品订单
       /// </summary>
        private string oID;
       /// <summary>
       /// 物流公司
       /// </summary>
        private string cName;
        private DateTime addTime;

       /// <summary>
       /// 物流编号
       /// </summary>
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
       /// <summary>
       /// 产品订单
       /// </summary>
        public string OID
        {
            get { return oID; }
            set { oID = value; }
        }
       /// <summary>
       /// 物流公司
       /// </summary>
        public string CName
        {
            get { return cName; }
            set { cName = value; }
        }
        public DateTime AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }
    }
}
