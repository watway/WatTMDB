using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatTmdb.V3
{
    public class TmdbTvImages : TmdbMovieImages { }

    public class TmdbTvSeasonImages
    {
        public int id { get; set; }
        public List<Poster> posters { get; set; }
    }

    public class TmdbTvEpisodeImages
    {
        public int id { get; set; }
        public List<Still> stills { get; set; }
    }

    public class Still
    {
        public string file_path { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string iso_639_1 { get; set; }
        public double aspect_ratio { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }
    }
}
