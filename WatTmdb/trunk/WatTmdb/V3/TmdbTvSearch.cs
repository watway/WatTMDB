using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatTmdb.V3
{
    public class TvResult
    {
        public string backdrop_path { get; set; }
        public string id { get; set; }
        public string original_name { get; set; }
        public string first_air_date { get; set; }
        public string poster_path { get; set; }
        public double popularity { get; set; }
        public string name { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }
    }

    public class TmdbTvSearch : TmdbSearchResultBase<TvResult> { }
}
