using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.SYS
{
   public class SysThemes
    {
        private string iD;
        private string name;
        private string value;
        private int isState;
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        public int IsState
        {
            get { return isState; }
            set { isState = value; }
        }
    }
}
