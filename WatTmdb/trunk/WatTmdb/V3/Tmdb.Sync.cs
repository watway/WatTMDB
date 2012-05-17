using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using System.Net;

namespace WatTmdb.V3
{
    public partial class Tmdb
    {
        private T ProcessRequest<T>(RestRequest request)
            where T : new()
        {
            var client = new RestClient(BASE_URL);

#if !WINDOWS_PHONE
            if (Proxy != null)
                client.Proxy = Proxy;
#endif

            Error = null;

            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);

            var resp = client.Execute<T>(request);
            if (resp.ResponseStatus != ResponseStatus.Completed)
            {
                if (resp.Content.Contains("status_message"))
                    Error = jsonDeserializer.Deserialize<TmdbError>(resp);
                else
                    Error = new TmdbError { status_message = resp.Content };
            }

            return resp.Data;
        }

        #region Configuration
        public TmdbConfiguration GetConfiguration()
        {
            var request = new RestRequest("configuration", Method.GET);

            return ProcessRequest<TmdbConfiguration>(request);
        }
        #endregion

        #region Search
        public TmdbMovieSearch SearchMovie(string query, int page)
        {
            var request = new RestRequest("search/movie", Method.GET);
            request.AddParameter("query", query.EscapeString());
            request.AddParameter("page", page);
            request.AddParameter("language", "en");

            return ProcessRequest<TmdbMovieSearch>(request);
        }

        public TmdbPersonSearch SearchPerson(string query, int page)
        {
            var request = new RestRequest("search/person", Method.GET);
            request.AddParameter("query", query.EscapeString());
            request.AddParameter("page", page);
            request.AddParameter("language", "en");

            return ProcessRequest<TmdbPersonSearch>(request);
        }
        #endregion


        #region Collections
        public TmdbCollection GetCollectionInfo(int CollectionID)
        {
            var request = new RestRequest("collection/{id}", Method.GET);
            request.AddParameter("language", Language);
            request.AddUrlSegment("id", CollectionID.ToString());

            return ProcessRequest<TmdbCollection>(request);
        }
        #endregion


        #region Movie Info
        public TmdbMovie GetMovieInfo(int MovieID)
        {
            var request = new RestRequest("movie/{id}", Method.GET);
            request.AddParameter("language", Language);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovie>(request);
        }

        public TmdbMovie GetMovieByIMDB(string IMDB_ID)
        {
            var request = new RestRequest("movie/{id}", Method.GET);
            request.AddUrlSegment("id", IMDB_ID.EscapeString());

            return ProcessRequest<TmdbMovie>(request);
        }

        public TmdbMovieAlternateTitles GetMovieAlternateTitles(int MovieID, string Country)
        {
            var request = new RestRequest("movie/{id}/alternative_titles", Method.GET);
            if (string.IsNullOrEmpty(Country) == false)
                request.AddParameter("country", Country);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovieAlternateTitles>(request);
        }

        public TmdbMovieCast GetMovieCast(int MovieID)
        {
            var request = new RestRequest("movie/{id}/casts", Method.GET);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovieCast>(request);
        }

        public TmdbMovieImages GetMovieImages(int MovieID)
        {
            var request = new RestRequest("movie/{id}/images", Method.GET);
            request.AddParameter("language", Language);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovieImages>(request);
        }

        public TmdbMovieKeywords GetMovieKeywords(int MovieID)
        {
            var request = new RestRequest("movie/{id}/keywords", Method.GET);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovieKeywords>(request);
        }

        public TmdbMovieReleases GetMovieReleases(int MovieID)
        {
            var request = new RestRequest("movie/{id}/releases", Method.GET);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovieReleases>(request);
        }

        public TmdbMovieTrailers GetMovieTrailers(int MovieID)
        {
            var request = new RestRequest("movie/{id}/trailers", Method.GET);
            request.AddParameter("language", Language);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovieTrailers>(request);
        }

        public TmdbSimilarMovies GetSimilarMovies(int MovieID, int page)
        {
            var request = new RestRequest("movie/{id}/similar_movies", Method.GET);
            request.AddParameter("page", page);
            request.AddParameter("language", "en");
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbSimilarMovies>(request);
        }
        #endregion

        #region Person Info
        public TmdbPerson GetPersonInfo(int PersonID)
        {
            var request = new RestRequest("person/{id}", Method.GET);
            request.AddUrlSegment("id", PersonID.ToString());

            return ProcessRequest<TmdbPerson>(request);
        }

        public TmdbPersonCredits GetPersonCredits(int PersonID)
        {
            var request = new RestRequest("person/{id}/credits", Method.GET);
            request.AddParameter("language", "en");
            request.AddUrlSegment("id", PersonID.ToString());

            return ProcessRequest<TmdbPersonCredits>(request);
        }

        public TmdbPersonImages GetPersonImages(int PersonID)
        {
            var request = new RestRequest("person/{id}/images", Method.GET);
            request.AddUrlSegment("id", PersonID.ToString());

            return ProcessRequest<TmdbPersonImages>(request);
        }
        #endregion

        #region Miscellaneous Movie
        public TmdbNowPlaying GetNowPlayingMovies(int page)
        {
            var request = new RestRequest("movie/now-playing", Method.GET);
            request.AddParameter("language", "en");
            request.AddParameter("page", page);

            return ProcessRequest<TmdbNowPlaying>(request);
        }

        public TmdbPopular GetPopularMovies(int page)
        {
            var request = new RestRequest("movie/popular", Method.GET);
            request.AddParameter("language", "en");
            request.AddParameter("page", page);

            return ProcessRequest<TmdbPopular>(request);
        }

        public TmdbTopRated GetTopRatedMovies(int page)
        {
            var request = new RestRequest("movie/top-rated", Method.GET);
            request.AddParameter("language", "en");
            request.AddParameter("page", page);

            return ProcessRequest<TmdbTopRated>(request);
        }
        #endregion

        #region Company Info
        public TmdbCompany GetCompanyInfo(int CompanyID)
        {
            var request = new RestRequest("company/{id}", Method.GET);
            request.AddUrlSegment("id", CompanyID.ToString());

            return ProcessRequest<TmdbCompany>(request);
        }

        public TmdbCompanyMovies GetCompanyMovies(int CompanyID, int page)
        {
            var request = new RestRequest("company/{id}/movies", Method.GET);
            request.AddUrlSegment("id", CompanyID.ToString());
            request.AddParameter("language", "en");
            request.AddParameter("page", page);

            return ProcessRequest<TmdbCompanyMovies>(request);
        }
        #endregion
    }
}
