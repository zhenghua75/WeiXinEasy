using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Album
{
    public class PhotoList
    {
        public string ID { get; set; }
        public string SiteCode { get; set; }
        public string Name { get; set; }
        public string Sign { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int IsDel { get; set; }
    }
}
