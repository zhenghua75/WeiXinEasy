using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
   public class MSOrderLog
    {
        private DateTime logdate;

        public DateTime Logdate
        {
            get { return logdate; }
            set { logdate = value; }
        }
        private string textmsg;

        public string Textmsg
        {
            get { return textmsg; }
            set { textmsg = value; }
        }
    }
}
