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

        /// <summary>
        /// String representation of response content
        /// </summary>
        public string ResponseContent { get; set; }

#if !WINDOWS_PHONE
        /// <summary>
        /// Proxy to use for requests made.  Passed on to underying WebRequest if set.
        /// </summary>
        public IWebProxy Proxy { get; set; }
#endif
        /// <summary>
        /// Timeout in milliseconds to use for requests made.
        /// </summary>
        public int? Timeout { get; set; }

        public Tmdb(string apiKey)
        {
            Error = null;
            ApiKey = apiKey;
            Language = null;
            Timeout = null;
        }

        public Tmdb(string apiKey, string language)
        {
            Error = null;
            ApiKey = apiKey;
            Language = language;
            Timeout = null;
        }
    }
}
