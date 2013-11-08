using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatTmdb.V3
{
    public class TmdbTvCredits
    {
        public int id { get; set; }
        public List<TvCast> cast { get; set; }
        public List<TvCrew> crew { get; set; }
    }

    public class TvCast
    {
        public int id { get; set; }
        public string character_name { get; set; }
        public string credit_id { get; set; }
        public string name { get; set; }
        public string profile_path { get; set; }
        public int sort_order { get; set; }
    }

    public class TvCrew
    {
        public int id { get; set; }
        public string name { get; set; }
        public string profile_path { get; set; }
        public string job { get; set; }
        public string department { get; set; }
    }
}
