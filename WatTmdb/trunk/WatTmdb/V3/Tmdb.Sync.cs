using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace WatTmdb.V3
{
    public partial class Tmdb
    {
        private T ProcessRequest<T>(RestClient client, RestRequest request)
            where T : new()
        {
            Error = null;
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
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("configuration", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);

            return ProcessRequest<TmdbConfiguration>(client, request);
        }
        #endregion

        #region Search
        public TmdbMovieSearch SearchMovie(string query, int page)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("search/movie", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("query", query.EscapeString());
            request.AddParameter("page", page);
            request.AddParameter("language", "en");

            return ProcessRequest<TmdbMovieSearch>(client, request);
        }

        public TmdbPersonSearch SearchPerson(string query, int page)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("search/person", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("query", query.EscapeString());
            request.AddParameter("page", page);
            request.AddParameter("language", "en");

            return ProcessRequest<TmdbPersonSearch>(client, request);
        }
        #endregion


        #region Collections
        public TmdbCollection GetCollectionInfo(int CollectionID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("collection/{id}", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("language", Language);
            request.AddUrlSegment("id", CollectionID.ToString());

            return ProcessRequest<TmdbCollection>(client, request);
        }
        #endregion


        #region Movie Info
        public TmdbMovie GetMovieInfo(int MovieID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/{id}", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("language", Language);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovie>(client, request);
        }

        public TmdbMovie GetMovieByIMDB(string IMDB_ID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/{id}", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("id", IMDB_ID.EscapeString());

            return ProcessRequest<TmdbMovie>(client, request);
        }

        public TmdbMovieAlternateTitles GetMovieAlternateTitles(int MovieID, string Country)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/{id}/alternative_titles", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            if (string.IsNullOrEmpty(Country) == false)
                request.AddParameter("country", Country);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovieAlternateTitles>(client, request);
        }

        public TmdbMovieCast GetMovieCast(int MovieID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/{id}/casts", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovieCast>(client, request);
        }

        public TmdbMovieImages GetMovieImages(int MovieID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/{id}/images", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("language", Language);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovieImages>(client, request);
        }

        public TmdbMovieKeywords GetMovieKeywords(int MovieID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/{id}/keywords", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovieKeywords>(client, request);
        }

        public TmdbMovieReleases GetMovieReleases(int MovieID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/{id}/releases", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovieReleases>(client, request);
        }

        public TmdbMovieTrailers GetMovieTrailers(int MovieID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/{id}/trailers", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("language", Language);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovieTrailers>(client, request);
        }

        public TmdbSimilarMovies GetSimilarMovies(int MovieID, int page)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/{id}/similar_movies", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("page", page);
            request.AddParameter("language", "en");
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbSimilarMovies>(client, request);
        }
        #endregion

        #region Person Info
        public TmdbPerson GetPersonInfo(int PersonID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("person/{id}", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("id", PersonID.ToString());

            return ProcessRequest<TmdbPerson>(client, request);
        }

        public TmdbPersonCredits GetPersonCredits(int PersonID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("person/{id}/credits", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("language", "en");
            request.AddUrlSegment("id", PersonID.ToString());

            return ProcessRequest<TmdbPersonCredits>(client, request);
        }

        public TmdbPersonImages GetPersonImages(int PersonID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("person/{id}/images", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("id", PersonID.ToString());

            return ProcessRequest<TmdbPersonImages>(client, request);
        }
        #endregion

        #region Miscellaneous Movie
        public TmdbNowPlaying GetNowPlayingMovies(int page)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/now-playing", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("language", "en");
            request.AddParameter("page", page);

            return ProcessRequest<TmdbNowPlaying>(client, request);
        }

        public TmdbPopular GetPopularMovies(int page)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/popular", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("language", "en");
            request.AddParameter("page", page);

            return ProcessRequest<TmdbPopular>(client, request);
        }

        public TmdbTopRated GetTopRatedMovies(int page)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/top-rated", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("language", "en");
            request.AddParameter("page", page);

            return ProcessRequest<TmdbTopRated>(client, request);
        }
        #endregion

        #region Company Info
        public TmdbCompany GetCompanyInfo(int CompanyID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("company/{id}", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("id", CompanyID.ToString());

            return ProcessRequest<TmdbCompany>(client, request);
        }

        public TmdbCompanyMovies GetCompanyMovies(int CompanyID, int page)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("company/{id}/movies", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("id", CompanyID.ToString());
            request.AddParameter("language", "en");
            request.AddParameter("page", page);

            return ProcessRequest<TmdbCompanyMovies>(client, request);
        }
        #endregion
    }
}
