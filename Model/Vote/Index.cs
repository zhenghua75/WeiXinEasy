using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Vote
{
    public class Index
    {
        private string iD;
        private string subjectID;

        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }

        public string SubjectID
        {
            get { return subjectID; }
            set { subjectID = value; }
        }
    }
}
