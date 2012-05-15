using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatTmdb.V3
{
    public class MovieKeyword
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class TmdbMovieKeywords
    {
        public int id { get; set; }
        public List<MovieKeyword> keywords { get; set; }
    }
}
