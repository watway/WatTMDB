﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatTmdb.V3
{
    public class TmdbTvEpisode
    {
        public string air_date { get; set; }
        public int episode_number { get; set; }
        public string name { get; set; }
        public string overview { get; set; }
        public int id { get; set; }
        public string production_code { get; set; }
        public int season_number { get; set; }
        public string still_path { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }
    }
}
