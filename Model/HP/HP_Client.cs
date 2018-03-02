using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HP
{
    public class HP_Client
    {
        public HP_Client()
        {
            ID = Guid.NewGuid().ToString("N");

        }

        public string ID { get; set; }

        public string SiteCode { get; set; }

        public string ClientCode { get; set; }

        public int IsDel { get; set; }
    }
}
