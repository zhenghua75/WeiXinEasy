using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.WeiXin
{
    public class User
    {
        public User()
        {
            ID = Guid.NewGuid().ToString("N");
            Subscribe = 1;
            Sex = 0;
            AddTime = DateTime.Now;
        }

        public string ID { get; set; }

        public string OpenID { get; set; }

        public string WXConfigID { get; set; }

        public int Subscribe { get; set; }

        public string NickName { get; set; }

        public int Sex { get; set; }

        public string Language { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string Country { get; set; }

        public string HeadImgUrl { get; set; }

        public string SubscribeTime { get; set; }

        public string GroupID { get; set; }

        public DateTime? AddTime { get; set; }
    }
}
