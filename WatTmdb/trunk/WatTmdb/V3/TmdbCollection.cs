using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatTmdb.V3
{
    public class CollectionPart
    {
        public string title { get; set; }
        public int id { get; set; }
        public string release_date { get; set; }
        public string poster_path { get; set; }
        public string backdrop_path { get; set; }
    }

    public class TmdbCollection
    {
        public int id { get; set; }
        public string name { get; set; }
        public string poster_path { get; set; }
        public string backdrop_path { get; set; }
        public List<CollectionPart> parts { get; set; }
    }
}
