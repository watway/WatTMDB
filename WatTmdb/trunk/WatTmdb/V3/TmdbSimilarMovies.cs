using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatTmdb.V3
{
    public class SimilarMovie
    {
        public string backdrop_path { get; set; }
        public int id { get; set; }
        public string original_title { get; set; }
        public string release_date { get; set; }
        public string poster_path { get; set; }
        public string title { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }
    }

    public class TmdbSimilarMovies
    {
        public int page { get; set; }
        public List<SimilarMovie> results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }
}
