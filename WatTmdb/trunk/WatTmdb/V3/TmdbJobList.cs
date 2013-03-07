using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatTmdb.V3
{
    public class TmdbJobList
    {
        public List<JobListItem> jobs { get; set; }
    }

    public class JobListItem
    {
        public string department { get; set; }
        public List<string> job_list { get; set; }

        public override string ToString()
        {
            return department;
        }
    }
}
