using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp.Deserializers;

namespace WatTmdb.V3
{
    public partial class Tmdb
    {
        private const string BASE_URL = "http://api.themoviedb.org/3";

        public string ApiKey { get; set; }
        public string Language { get; set; }
        private JsonDeserializer jsonDeserializer = null;

        public TmdbError Error { get; set; }

        public Tmdb(string apiKey, string language = null)
        {
            jsonDeserializer = new JsonDeserializer();
            Error = null;
            ApiKey = apiKey;
            Language = language ?? "en";
        }
    }
}
