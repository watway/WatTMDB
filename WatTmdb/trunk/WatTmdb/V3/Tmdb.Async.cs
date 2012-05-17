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
        private void ProcessAsyncRequest<T>(RestRequest request, Action<TmdbAsyncResult<T>> callback)
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

            var asyncHandle = client.ExecuteAsync<T>(request, resp =>
                {
                    var result = new TmdbAsyncResult<T>
                    {
                        Data = resp.Data,
                        UserState = request.UserState
                    };

                    if (resp.ResponseStatus != ResponseStatus.Completed)
                    {
                        if (resp.Content.Contains("status_message"))
                            result.Error = jsonDeserializer.Deserialize<TmdbError>(resp);
                        else
                            result.Error = new TmdbError { status_message = resp.Content };
                    }

                    Error = result.Error;

                    callback(result);
                });
        }

        #region Configuration
        public void GetConfiguration(object UserState, Action<TmdbAsyncResult<TmdbConfiguration>> callback)
        {
            var request = new RestRequest("configuration", Method.GET);
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbConfiguration>(request, callback);
        }
        #endregion

        #region Search
        public void SearchMovie(string query, int page, object UserState, Action<TmdbAsyncResult<TmdbMovieSearch>> callback)
        {
            var request = new RestRequest("search/movie", Method.GET);
            request.AddParameter("query", query.EscapeString());
            request.AddParameter("page", page);
            request.AddParameter("language", "en");
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbMovieSearch>(request, callback);
        }

        public void SearchPerson(string query, int page, object UserState, Action<TmdbAsyncResult<TmdbPersonSearch>> callback)
        {
            var request = new RestRequest("search/person", Method.GET);
            request.AddParameter("query", query.EscapeString());
            request.AddParameter("page", page);
            request.AddParameter("language", "en");
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbPersonSearch>(request, callback);
        }
        #endregion

        #region Collections
        public void GetCollectionInfo(int CollectionID, object UserState, Action<TmdbAsyncResult<TmdbCollection>> callback)
        {
            var request = new RestRequest("collection/{id}", Method.GET);
            request.AddParameter("language", Language);
            request.AddUrlSegment("id", CollectionID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbCollection>(request, callback);
        }
        #endregion


        #region Movie Info
        public void GetMovieInfo(int MovieID, object UserState, Action<TmdbAsyncResult<TmdbMovie>> callback)
        {
            var request = new RestRequest("movie/{id}", Method.GET);
            request.AddParameter("language", Language);
            request.AddUrlSegment("id", MovieID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbMovie>(request, callback);
        }

        public void GetMovieByIMDB(string IMDB_ID, object UserState, Action<TmdbAsyncResult<TmdbMovie>> callback)
        {
            var request = new RestRequest("movie/{id}", Method.GET);
            request.AddUrlSegment("id", IMDB_ID.EscapeString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbMovie>(request, callback);
        }

        public void GetMovieAlternateTitles(int MovieID, string Country, object UserState, Action<TmdbAsyncResult<TmdbMovieAlternateTitles>> callback)
        {
            var request = new RestRequest("movie/{id}/alternative_titles", Method.GET);
            if (string.IsNullOrEmpty(Country) == false)
                request.AddParameter("country", Country);
            request.AddUrlSegment("id", MovieID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbMovieAlternateTitles>(request, callback);
        }

        public void GetMovieCast(int MovieID, object UserState, Action<TmdbAsyncResult<TmdbMovieCast>> callback)
        {
            var request = new RestRequest("movie/{id}/casts", Method.GET);
            request.AddUrlSegment("id", MovieID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbMovieCast>(request, callback);
        }

        public void GetMovieImages(int MovieID, object UserState, Action<TmdbAsyncResult<TmdbMovieImages>> callback)
        {
            var request = new RestRequest("movie/{id}/images", Method.GET);
            request.AddParameter("language", Language);
            request.AddUrlSegment("id", MovieID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbMovieImages>(request, callback);
        }

        public void GetMovieKeywords(int MovieID, object UserState, Action<TmdbAsyncResult<TmdbMovieKeywords>> callback)
        {
            var request = new RestRequest("movie/{id}/keywords", Method.GET);
            request.AddUrlSegment("id", MovieID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbMovieKeywords>(request, callback);
        }

        public void GetMovieReleases(int MovieID, object UserState, Action<TmdbAsyncResult<TmdbMovieReleases>> callback)
        {
            var request = new RestRequest("movie/{id}/releases", Method.GET);
            request.AddUrlSegment("id", MovieID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbMovieReleases>(request, callback);
        }

        public void GetMovieTrailers(int MovieID, object UserState, Action<TmdbAsyncResult<TmdbMovieTrailers>> callback)
        {
            var request = new RestRequest("movie/{id}/trailers", Method.GET);
            request.AddParameter("language", Language);
            request.AddUrlSegment("id", MovieID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbMovieTrailers>(request, callback);
        }

        public void GetSimilarMovies(int MovieID, int page, object UserState, Action<TmdbAsyncResult<TmdbSimilarMovies>> callback)
        {
            var request = new RestRequest("movie/{id}/similar_movies", Method.GET);
            request.AddParameter("page", page);
            request.AddParameter("language", "en");
            request.AddUrlSegment("id", MovieID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbSimilarMovies>(request, callback);
        }
        #endregion

        #region Person Info
        public void GetPersonInfo(int PersonID, object UserState, Action<TmdbAsyncResult<TmdbPerson>> callback)
        {
            var request = new RestRequest("person/{id}", Method.GET);
            request.AddUrlSegment("id", PersonID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbPerson>(request, callback);
        }

        public void GetPersonCredits(int PersonID, object UserState, Action<TmdbAsyncResult<TmdbPersonCredits>> callback)
        {
            var request = new RestRequest("person/{id}/credits", Method.GET);
            request.AddParameter("language", "en");
            request.AddUrlSegment("id", PersonID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbPersonCredits>(request, callback);
        }

        public void GetPersonImages(int PersonID, object UserState, Action<TmdbAsyncResult<TmdbPersonImages>> callback)
        {
            var request = new RestRequest("person/{id}/images", Method.GET);
            request.AddUrlSegment("id", PersonID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbPersonImages>(request, callback);
        }
        #endregion

        #region Miscellaneous Movie
        public void GetNowPlayingMovies(int page, object UserState, Action<TmdbAsyncResult<TmdbNowPlaying>> callback)
        {
            var request = new RestRequest("movie/now-playing", Method.GET);
            request.AddParameter("language", "en");
            request.AddParameter("page", page);
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbNowPlaying>(request, callback);
        }

        public void GetPopularMovies(int page, object UserState, Action<TmdbAsyncResult<TmdbPopular>> callback)
        {
            var request = new RestRequest("movie/popular", Method.GET);
            request.AddParameter("language", "en");
            request.AddParameter("page", page);
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbPopular>(request, callback);
        }

        public void GetTopRatedMovies(int page, object UserState, Action<TmdbAsyncResult<TmdbTopRated>> callback)
        {
            var request = new RestRequest("movie/top-rated", Method.GET);
            request.AddParameter("language", "en");
            request.AddParameter("page", page);
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbTopRated>(request, callback);
        }
        #endregion

        #region Company Info
        public void GetCompanyInfo(int CompanyID, object UserState, Action<TmdbAsyncResult<TmdbCompany>> callback)
        {
            var request = new RestRequest("company/{id}", Method.GET);
            request.AddUrlSegment("id", CompanyID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbCompany>(request, callback);
        }

        public void GetCompanyMovies(int CompanyID, int page, object UserState, Action<TmdbAsyncResult<TmdbCompanyMovies>> callback)
        {
            var request = new RestRequest("company/{id}/movies", Method.GET);
            request.AddUrlSegment("id", CompanyID.ToString());
            request.AddParameter("language", "en");
            request.AddParameter("page", page);
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbCompanyMovies>(request, callback);
        }
        #endregion
    }
}
