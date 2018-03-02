using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.SYS
{
    public class PayConfig
    {
        public PayConfig()
        {
            ID = Guid.NewGuid().ToString("N");
            Charset = "utf-8";
        }

        public string ID { get; set; }

        public string SiteCode { get; set; }

        public string PayMode { get; set; }

        public bool EncryptParams { get; set; }

        public string Partner { get; set; }

        public string SignType { get; set; }

        public string SignKey { get; set; }

        public string PrivateKey { get; set; }

        public string PublicKey { get; set; }

        public string Charset { get; set; }

        public string AppID { get; set; }

        public string MoreParams { get; set; }

    }
}
