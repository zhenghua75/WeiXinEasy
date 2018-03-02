using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.WeiXin
{
    public class Media
    {
        public Media()
        {
            ID = Guid.NewGuid().ToString("N");
            UploadTime = DateTime.Now;
            IsAutoSyn = true;
        }

        public string ID { get; set; }

        public string MediaName { get; set; }

        public string MediaFile { get; set; }

        public DateTime? UploadTime { get; set; }

        public bool IsAutoSyn { get; set; }

        public DateTime? LastSynTime { get; set; }

        public string MediaID { get; set; }

        public string MediaType { get; set; }
    }
}
