using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatTmdb.V3
{
    public class TmdbMovieReview : TmdbSearchResultBase<MovieReview>
    {
        public string id { get; set; }
    }

    public class MovieReview
    {
        public string id { get; set; }
        public string author { get; set; }
        public string content { get; set; }
        public string url { get; set; }
    }
}
