using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.SYS
{
    public class ExceptionLog
    {
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
    }
}
