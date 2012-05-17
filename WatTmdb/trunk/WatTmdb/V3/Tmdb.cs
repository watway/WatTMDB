using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp.Deserializers;
using System.Net;

namespace WatTmdb.V3
{
    public partial class Tmdb
    {
        private const string BASE_URL = "http://api.themoviedb.org/3";

        public string ApiKey { get; set; }
        public string Language { get; set; }
        private JsonDeserializer jsonDeserializer = new JsonDeserializer();

        public TmdbError Error { get; set; }

#if !WINDOWS_PHONE
        public IWebProxy Proxy { get; set; }
#endif

        public Tmdb(string apiKey)
        {
            Error = null;
            ApiKey = apiKey;
            Language = "en";
        }

        public Tmdb(string apiKey, string language)
        {
            Error = null;
            ApiKey = apiKey;
            Language = language ?? "en";
        }
    }
}
