using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace WatTmdb.V3
{
    public class Tmdb
    {
        private const string BASE_URL = "http://api.themoviedb.org/3";

        public string ApiKey { get; set; }
        public string Language { get; set; }

        public Tmdb(string apiKey, string language)
        {
            ApiKey = apiKey;
            Language = language ?? "en";
        }

        #region Configuration
        public TmdbConfiguration GetConfiguration()
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("configuration", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);

            IRestResponse<TmdbConfiguration> resp = client.Execute<TmdbConfiguration>(request);
            return resp.Data;
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

            IRestResponse<TmdbMovieSearch> resp = client.Execute<TmdbMovieSearch>(request);
            return resp.Data;
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

            IRestResponse<TmdbPersonSearch> resp = client.Execute<TmdbPersonSearch>(request);
            return resp.Data;
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

            IRestResponse<TmdbCollection> resp = client.Execute<TmdbCollection>(request);

            return resp.Data;
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

            //var response = client.Execute(request);
            //var content = response.Content; // RAW response
            IRestResponse<TmdbMovie> resp = client.Execute<TmdbMovie>(request);

            return resp.Data;
        }

        public TmdbMovie GetMovieByIMDB(string IMDB_ID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/{id}", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("id", IMDB_ID.EscapeString());

            IRestResponse<TmdbMovie> resp = client.Execute<TmdbMovie>(request);

            return resp.Data;
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

            IRestResponse<TmdbMovieAlternateTitles> resp = client.Execute<TmdbMovieAlternateTitles>(request);

            return resp.Data;
        }

        public TmdbMovieCast GetMovieCast(int MovieID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/{id}/casts", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("id", MovieID.ToString());

            IRestResponse<TmdbMovieCast> resp = client.Execute<TmdbMovieCast>(request);

            return resp.Data;
        }

        public TmdbMovieImages GetMovieImages(int MovieID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/{id}/images", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("language", Language);
            request.AddUrlSegment("id", MovieID.ToString());

            IRestResponse<TmdbMovieImages> resp = client.Execute<TmdbMovieImages>(request);

            return resp.Data;
        }

        public TmdbMovieKeywords GetMovieKeywords(int MovieID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/{id}/keywords", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("id", MovieID.ToString());

            IRestResponse<TmdbMovieKeywords> resp = client.Execute<TmdbMovieKeywords>(request);

            return resp.Data;
        }

        public TmdbMovieReleases GetMovieReleases(int MovieID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/{id}/releases", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("id", MovieID.ToString());

            IRestResponse<TmdbMovieReleases> resp = client.Execute<TmdbMovieReleases>(request);

            return resp.Data;
        }

        public TmdbMovieTrailers GetMovieTrailers(int MovieID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/{id}/trailers", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("language", Language);
            request.AddUrlSegment("id", MovieID.ToString());

            IRestResponse<TmdbMovieTrailers> resp = client.Execute<TmdbMovieTrailers>(request);

            return resp.Data;
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

            IRestResponse<TmdbSimilarMovies> resp = client.Execute<TmdbSimilarMovies>(request);
            return resp.Data;
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

            IRestResponse<TmdbPerson> resp = client.Execute<TmdbPerson>(request);
            return resp.Data;
        }

        public TmdbPersonCredits GetPersonCredits(int PersonID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("person/{id}/credits", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("language", "en");
            request.AddUrlSegment("id", PersonID.ToString());

            IRestResponse<TmdbPersonCredits> resp = client.Execute<TmdbPersonCredits>(request);
            return resp.Data;
        }

        public TmdbPersonImages GetPersonImages(int PersonID)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("person/{id}/images", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddUrlSegment("id", PersonID.ToString());

            IRestResponse<TmdbPersonImages> resp = client.Execute<TmdbPersonImages>(request);
            return resp.Data;
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

            IRestResponse<TmdbNowPlaying> resp = client.Execute<TmdbNowPlaying>(request);
            return resp.Data;
        }

        public TmdbPopular GetPopularMovies(int page)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/popular", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("language", "en");
            request.AddParameter("page", page);

            IRestResponse<TmdbPopular> resp = client.Execute<TmdbPopular>(request);
            return resp.Data;
        }

        public TmdbTopRated GetTopRatedMovies(int page)
        {
            var client = new RestClient(BASE_URL);

            var request = new RestRequest("movie/top-rated", Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("language", "en");
            request.AddParameter("page", page);

            IRestResponse<TmdbTopRated> resp = client.Execute<TmdbTopRated>(request);
            return resp.Data;
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

            IRestResponse<TmdbCompany> resp = client.Execute<TmdbCompany>(request);
            return resp.Data;
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

            IRestResponse<TmdbCompanyMovies> resp = client.Execute<TmdbCompanyMovies>(request);
            return resp.Data;
        }
        #endregion
    }
}
