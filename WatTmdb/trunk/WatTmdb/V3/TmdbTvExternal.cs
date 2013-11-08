using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatTmdb.V3
{
    public class TmdbTvExternal
    {
        public string imdb_id { get; set; }
        public string freebase_id { get; set; }
        public string freebase_mid { get; set; }
        public int id { get; set; }
        public string tvdb_id { get; set; }
        public string tvrage_id { get; set; }
    }

    public class TmdbTvSeasonExternal
    {
        public string freebase_id { get; set; }
        public string freebase_mid { get; set; }
        public int id { get; set; }
        public string tvdb_id { get; set; }
        public string tvrate_id { get; set; }
    }
}
