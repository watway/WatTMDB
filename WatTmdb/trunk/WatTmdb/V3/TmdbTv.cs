using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatTmdb.V3
{
    public class TmdbTv
    {
        public string backdrop_path { get; set; }
        public List<TvCreatedBy> created_by { get; set; }
        public List<int> episode_run_time { get; set; }
        public string first_air_date { get; set; }
        public List<MovieGenre> genres { get; set; }
        public string homepage { get; set; }
        public int id { get; set; }
        public bool in_production { get; set; }
        public List<string> languages { get; set; }
        public string last_air_date { get; set; }
        public string name { get; set; }
        public List<TvNetwork> networks { get; set; }
        public int number_of_episodes { get; set; }
        public int number_of_seasons { get; set; }
        public string original_name { get; set; }
        public List<string> origin_country { get; set; }
        public string overview { get; set; }
        public double popularity { get; set; }
        public string poster_path { get; set; }
        public List<TvSeason> seasons { get; set; }
        public string status { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }
    }

    public class TvCreatedBy
    {
        public int id { get; set; }
        public string name { get; set; }
        public string profile_path { get; set; }
    }

    public class TvNetwork
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class TvSeason
    {
        public int season_number { get; set; }
        public string air_date { get; set; }
        public string poster_path { get; set; }
    }
}
