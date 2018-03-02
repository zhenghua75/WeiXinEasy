using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Model.Album
{
    public class GreatUserPhoto
    {
        public GreatUserPhoto()
        {
            ID = Guid.NewGuid().ToString("N");
            AddTime = DateTime.Now;
        }

        public string ID { get; set; }
        public string SiteCode { get; set; }
        public string OpenId { get; set; }
        public string ThumbID { get; set; }
        public DateTime AddTime { get; set; }
    }
}
