using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Album
{
    public class UserPhoto
    {
        public UserPhoto()
        {
            ID = Guid.NewGuid().ToString("N");
            AddTime = DateTime.Now;
            State = 0;
        }

        public string ID { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string SiteCode { get; set; }

        public string OpenId { get; set; }

        public string FilePath { get; set; }

        public string Remark { get; set; }

        public DateTime? AddTime { get; set; }

        public int? State { get; set; }

        public int GreatCount { get; set; }
    }
}
