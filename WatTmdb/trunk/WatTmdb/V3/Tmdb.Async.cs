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
            if (Timeout.HasValue)
                client.Timeout = Timeout.Value;

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

                    ResponseContent = resp.Content;
                    if (resp.ResponseStatus != ResponseStatus.Completed)
                    {
                        if (resp.Content.Contains("status_message"))
                            result.Error = jsonDeserializer.Deserialize<TmdbError>(resp);
                        else if (resp.ErrorException != null)
                            throw resp.ErrorException;
                        else
                            result.Error = new TmdbError { status_message = resp.Content };
                    }

                    Error = result.Error;

                    callback(result);
                });
        }

        #region Configuration
        /// <summary>
        /// Retrieve configuration data from TMDB
        /// (http://help.themoviedb.org/kb/api/configuration)
        /// </summary>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetConfiguration(object UserState, Action<TmdbAsyncResult<TmdbConfiguration>> callback)
        {
            var request = new RestRequest("configuration", Method.GET);
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbConfiguration>(request, callback);
        }
        #endregion


        #region Search
        /// <summary>
        /// Search for movies that are listed in TMDB
        /// (http://help.themoviedb.org/kb/api/search-movies)
        /// </summary>
        /// <param name="query">Is your search text.</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <param name="includeAdult">optional - include adult items in your search, (Default=false)</param>
        /// <param name="year">optional - to get a closer result</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void SearchMovie(string query, int page, string language, bool? includeAdult, int? year, object UserState, Action<TmdbAsyncResult<TmdbMovieSearch>> callback)
        {
            if (string.IsNullOrEmpty(query))
            {
                callback(new TmdbAsyncResult<TmdbMovieSearch>
                {
                    Data = null,
                    Error = new TmdbError { status_message = "Search cannot be empty" },
                    UserState = UserState
                });
                return;
            }

            var request = new RestRequest("search/movie", Method.GET);
            request.AddParameter("query", query.EscapeString());
            request.AddParameter("page", page);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            if (includeAdult.HasValue)
                request.AddParameter("include_adult", includeAdult.Value ? "true" : "false");
            if (year.HasValue)
                request.AddParameter("year", year.Value);
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbMovieSearch>(request, callback);
        }

        /// <summary>
        /// Search for movies that are listed in TMDB
        /// (http://help.themoviedb.org/kb/api/search-movies)
        /// </summary>
        /// <param name="query">Is your search text.</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void SearchMovie(string query, int page, object UserState, Action<TmdbAsyncResult<TmdbMovieSearch>> callback)
        {
            SearchMovie(query, page, Language, null, null, UserState, callback);
        }

        /// <summary>
        /// Search for people that are listed in TMDB.
        /// (http://help.themoviedb.org/kb/api/search-people)
        /// </summary>
        /// <param name="query">Is your search text.</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void SearchPerson(string query, int page, string language, object UserState, Action<TmdbAsyncResult<TmdbPersonSearch>> callback)
        {
            if (string.IsNullOrEmpty(query))
            {
                callback(new TmdbAsyncResult<TmdbPersonSearch>
                {
                    Data = null,
                    Error = new TmdbError { status_message = "Search cannot be empty" },
                    UserState = UserState
                });
                return;
            }

            var request = new RestRequest("search/person", Method.GET);
            request.AddParameter("query", query.EscapeString());
            request.AddParameter("page", page);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbPersonSearch>(request, callback);
        }

        /// <summary>
        /// Search for people that are listed in TMDB.
        /// (http://help.themoviedb.org/kb/api/search-people)
        /// </summary>
        /// <param name="query">Is your search text.</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void SearchPerson(string query, int page, object UserState, Action<TmdbAsyncResult<TmdbPersonSearch>> callback)
        {
            SearchPerson(query, page, Language, UserState, callback);
        }

        /// <summary>
        /// Search for production companies that are part of TMDB.
        /// (http://help.themoviedb.org/kb/api/search-companies)
        /// </summary>
        /// <param name="query">Is your search text.</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void SearchCompany(string query, int page, object UserState, Action<TmdbAsyncResult<TmdbCompanySearch>> callback)
        {
            if (string.IsNullOrEmpty(query))
            {
                callback(new TmdbAsyncResult<TmdbCompanySearch>
                {
                    Data = null,
                    Error = new TmdbError { status_message = "Search cannot be empty" },
                    UserState = UserState
                });
                return;
            }

            var request = new RestRequest("search/company", Method.GET);
            request.AddParameter("query", query.EscapeString());
            request.AddParameter("page", page);
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbCompanySearch>(request, callback);
        }
        #endregion


        #region Collections
        /// <summary>
        /// Get all of the basic information about a movie collection.
        /// (http://help.themoviedb.org/kb/api/collection-info)
        /// </summary>
        /// <param name="CollectionID">Collection ID, available in TmdbMovie::belongs_to_collection</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetCollectionInfo(int CollectionID, string language, object UserState, Action<TmdbAsyncResult<TmdbCollection>> callback)
        {
            var request = new RestRequest("collection/{id}", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddUrlSegment("id", CollectionID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbCollection>(request, callback);
        }

        /// <summary>
        /// Get all of the basic information about a movie collection.
        /// (http://help.themoviedb.org/kb/api/collection-info)
        /// </summary>
        /// <param name="CollectionID">Collection ID, available in TmdbMovie::belongs_to_collection</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetCollectionInfo(int CollectionID, object UserState, Action<TmdbAsyncResult<TmdbCollection>> callback)
        {
            GetCollectionInfo(CollectionID, Language, UserState, callback);
        }
        #endregion


        #region Movie Info
        /// <summary>
        /// Retrieve all the basic movie information for a particular movie by TMDB reference.
        /// (http://help.themoviedb.org/kb/api/movie-info)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetMovieInfo(int MovieID, string language, object UserState, Action<TmdbAsyncResult<TmdbMovie>> callback)
        {
            var request = new RestRequest("movie/{id}", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddUrlSegment("id", MovieID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbMovie>(request, callback);
        }

        /// <summary>
        /// Retrieve all the basic movie information for a particular movie by TMDB reference.
        /// (http://help.themoviedb.org/kb/api/movie-info)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetMovieInfo(int MovieID, object UserState, Action<TmdbAsyncResult<TmdbMovie>> callback)
        {
            GetMovieInfo(MovieID, Language, UserState, callback);
        }

        /// <summary>
        /// Retrieve all the basic movie information for a particular movie by IMDB reference.
        /// (http://help.themoviedb.org/kb/api/movie-info)
        /// </summary>
        /// <param name="IMDB_ID">IMDB movie id</param>
        /// <param name="callback"></param>
        public void GetMovieByIMDB(string IMDB_ID, object UserState, Action<TmdbAsyncResult<TmdbMovie>> callback)
        {
            if (string.IsNullOrEmpty(IMDB_ID))
            {
                callback(new TmdbAsyncResult<TmdbMovie>
                {
                    Data = null,
                    Error = new TmdbError { status_message = "Search cannot be empty" },
                    UserState = UserState
                });
                return;
            }

            var request = new RestRequest("movie/{id}", Method.GET);
            request.AddUrlSegment("id", IMDB_ID.EscapeString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbMovie>(request, callback);
        }

        /// <summary>
        /// Get list of all the alternative titles for a particular movie.
        /// (http://help.themoviedb.org/kb/api/movie-alternative-titles)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <param name="Country">ISO 3166-1 country code (optional)</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetMovieAlternateTitles(int MovieID, string Country, object UserState, Action<TmdbAsyncResult<TmdbMovieAlternateTitles>> callback)
        {
            var request = new RestRequest("movie/{id}/alternative_titles", Method.GET);
            if (string.IsNullOrEmpty(Country) == false)
                request.AddParameter("country", Country);
            request.AddUrlSegment("id", MovieID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbMovieAlternateTitles>(request, callback);
        }

        /// <summary>
        /// Get list of all the cast information for a particular movie.
        /// (http://help.themoviedb.org/kb/api/movie-casts)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetMovieCast(int MovieID, object UserState, Action<TmdbAsyncResult<TmdbMovieCast>> callback)
        {
            var request = new RestRequest("movie/{id}/casts", Method.GET);
            request.AddUrlSegment("id", MovieID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbMovieCast>(request, callback);
        }

        /// <summary>
        /// Get list of all the images for a particular movie.
        /// (http://help.themoviedb.org/kb/api/movie-images)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetMovieImages(int MovieID, string language, object UserState, Action<TmdbAsyncResult<TmdbMovieImages>> callback)
        {
            var request = new RestRequest("movie/{id}/images", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddUrlSegment("id", MovieID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbMovieImages>(request, callback);
        }

        /// <summary>
        /// Get list of all the images for a particular movie.
        /// (http://help.themoviedb.org/kb/api/movie-images)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetMovieImages(int MovieID, object UserState, Action<TmdbAsyncResult<TmdbMovieImages>> callback)
        {
            GetMovieImages(MovieID, Language, UserState, callback);
        }

        /// <summary>
        /// Get list of all the keywords that have been added to a particular movie.  Only English keywords exist currently.
        /// (http://help.themoviedb.org/kb/api/movie-keywords)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetMovieKeywords(int MovieID, object UserState, Action<TmdbAsyncResult<TmdbMovieKeywords>> callback)
        {
            var request = new RestRequest("movie/{id}/keywords", Method.GET);
            request.AddUrlSegment("id", MovieID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbMovieKeywords>(request, callback);
        }

        /// <summary>
        /// Get all the release and certification data in TMDB for a particular movie
        /// (http://help.themoviedb.org/kb/api/movie-release-info)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetMovieReleases(int MovieID, object UserState, Action<TmdbAsyncResult<TmdbMovieReleases>> callback)
        {
            var request = new RestRequest("movie/{id}/releases", Method.GET);
            request.AddUrlSegment("id", MovieID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbMovieReleases>(request, callback);
        }

        /// <summary>
        /// Get list of trailers for a particular movie.
        /// (http://help.themoviedb.org/kb/api/movie-trailers)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetMovieTrailers(int MovieID, string language, object UserState, Action<TmdbAsyncResult<TmdbMovieTrailers>> callback)
        {
            var request = new RestRequest("movie/{id}/trailers", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddUrlSegment("id", MovieID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbMovieTrailers>(request, callback);
        }

        /// <summary>
        /// Get list of trailers for a particular movie.
        /// (http://help.themoviedb.org/kb/api/movie-trailers)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetMovieTrailers(int MovieID, object UserState, Action<TmdbAsyncResult<TmdbMovieTrailers>> callback)
        {
            GetMovieTrailers(MovieID, Language, UserState, callback);
        }

        /// <summary>
        /// Get list of all available translations for a specific movie.
        /// (http://help.themoviedb.org/kb/api/movie-translations)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <returns></returns>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetMovieTranslations(int MovieID, object UserState, Action<TmdbAsyncResult<TmdbTranslations>> callback)
        {
            var request = new RestRequest("movie/{id}/translations", Method.GET);
            request.AddUrlSegment("id", MovieID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbTranslations>(request, callback);
        }

        /// <summary>
        /// Get list of similar movies for a particular movie.
        /// (http://help.themoviedb.org/kb/api/movie-similar-movies)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetSimilarMovies(int MovieID, int page, string language, object UserState, Action<TmdbAsyncResult<TmdbSimilarMovies>> callback)
        {
            var request = new RestRequest("movie/{id}/similar_movies", Method.GET);
            request.AddParameter("page", page);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddUrlSegment("id", MovieID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbSimilarMovies>(request, callback);
        }

        /// <summary>
        /// Get list of similar movies for a particular movie.
        /// (http://help.themoviedb.org/kb/api/movie-similar-movies)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetSimilarMovies(int MovieID, int page, object UserState, Action<TmdbAsyncResult<TmdbSimilarMovies>> callback)
        {
            GetSimilarMovies(MovieID, page, Language, UserState, callback);
        }

        /// <summary>
        /// Get list of movies that are arriving to theatres in the next few weeks.
        /// (http://help.themoviedb.org/kb/api/upcoming-movies)
        /// </summary>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetUpcomingMovies(int page, string language, object UserState, Action<TmdbAsyncResult<TmdbUpcoming>> callback)
        {
            var request = new RestRequest("movie/upcoming", Method.GET);
            request.AddParameter("page", page);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbUpcoming>(request, callback);
        }

        /// <summary>
        /// Get list of movies that are arriving to theatres in the next few weeks.
        /// (http://help.themoviedb.org/kb/api/upcoming-movies)
        /// </summary>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetUpcomingMovies(int page, object UserState, Action<TmdbAsyncResult<TmdbUpcoming>> callback)
        {
            GetUpcomingMovies(page, Language, UserState, callback);
        }

        #endregion


        #region Person Info
        /// <summary>
        /// Get all of the basic information for a person.
        /// (http://help.themoviedb.org/kb/api/person-info)
        /// </summary>
        /// <param name="PersonID">Person ID</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetPersonInfo(int PersonID, object UserState, Action<TmdbAsyncResult<TmdbPerson>> callback)
        {
            var request = new RestRequest("person/{id}", Method.GET);
            request.AddUrlSegment("id", PersonID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbPerson>(request, callback);
        }

        /// <summary>
        /// Get list of cast and crew information for a person.
        /// (http://help.themoviedb.org/kb/api/person-credits)
        /// </summary>
        /// <param name="PersonID">Person ID</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetPersonCredits(int PersonID, string language, object UserState, Action<TmdbAsyncResult<TmdbPersonCredits>> callback)
        {
            var request = new RestRequest("person/{id}/credits", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddUrlSegment("id", PersonID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbPersonCredits>(request, callback);
        }

        /// <summary>
        /// Get list of cast and crew information for a person.
        /// (http://help.themoviedb.org/kb/api/person-credits)
        /// </summary>
        /// <param name="PersonID">Person ID</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetPersonCredits(int PersonID, object UserState, Action<TmdbAsyncResult<TmdbPersonCredits>> callback)
        {
            GetPersonCredits(PersonID, Language, UserState, callback);
        }

        /// <summary>
        /// Get list of images for a person.
        /// (http://help.themoviedb.org/kb/api/person-images)
        /// </summary>
        /// <param name="PersonID">Person ID</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetPersonImages(int PersonID, object UserState, Action<TmdbAsyncResult<TmdbPersonImages>> callback)
        {
            var request = new RestRequest("person/{id}/images", Method.GET);
            request.AddUrlSegment("id", PersonID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbPersonImages>(request, callback);
        }
        #endregion


        #region Miscellaneous Movie
        /// <summary>
        /// Get the newest movie added to the TMDB.
        /// (http://help.themoviedb.org/kb/api/latest-movie)
        /// </summary>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        //public void GetLatestMovie(object UserState, Action<TmdbAsyncResult<TmdbLatestMovie>> callback)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Get the list of movies currently in theatres.  Response will contain 20 movies per page.
        /// (http://help.themoviedb.org/kb/api/now-playing-movies)
        /// </summary>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetNowPlayingMovies(int page, string language, object UserState, Action<TmdbAsyncResult<TmdbNowPlaying>> callback)
        {
            var request = new RestRequest("movie/now-playing", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddParameter("page", page);
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbNowPlaying>(request, callback);
        }

        /// <summary>
        /// Get the list of movies currently in theatres.  Response will contain 20 movies per page.
        /// (http://help.themoviedb.org/kb/api/now-playing-movies)
        /// </summary>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetNowPlayingMovies(int page, object UserState, Action<TmdbAsyncResult<TmdbNowPlaying>> callback)
        {
            GetNowPlayingMovies(page, Language, UserState, callback);
        }

        /// <summary>
        /// Get the daily popularity list of movies.  Response will contain 20 movies per page.
        /// (http://help.themoviedb.org/kb/api/popular-movie-list)
        /// </summary>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetPopularMovies(int page, string language, object UserState, Action<TmdbAsyncResult<TmdbPopular>> callback)
        {
            var request = new RestRequest("movie/popular", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddParameter("page", page);
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbPopular>(request, callback);
        }

        /// <summary>
        /// Get the daily popularity list of movies.  Response will contain 20 movies per page.
        /// (http://help.themoviedb.org/kb/api/popular-movie-list)
        /// </summary>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetPopularMovies(int page, object UserState, Action<TmdbAsyncResult<TmdbPopular>> callback)
        {
            GetPopularMovies(page, Language, UserState, callback);
        }

        /// <summary>
        /// Get list of movies that have over 10 votes on TMDB.  Response will contain 20 movies per page.
        /// (http://help.themoviedb.org/kb/api/top-rated-movies)
        /// </summary>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetTopRatedMovies(int page, string language, object UserState, Action<TmdbAsyncResult<TmdbTopRated>> callback)
        {
            var request = new RestRequest("movie/top-rated", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddParameter("page", page);
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbTopRated>(request, callback);
        }

        /// <summary>
        /// Get list of movies that have over 10 votes on TMDB.  Response will contain 20 movies per page.
        /// (http://help.themoviedb.org/kb/api/top-rated-movies)
        /// </summary>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetTopRatedMovies(int page, object UserState, Action<TmdbAsyncResult<TmdbTopRated>> callback)
        {
            GetTopRatedMovies(page, Language, UserState, callback);
        }
        #endregion


        #region Company Info
        /// <summary>
        /// Get basic information about a production company from TMDB.
        /// (http://help.themoviedb.org/kb/api/company-info)
        /// </summary>
        /// <param name="CompanyID">Company ID</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetCompanyInfo(int CompanyID, object UserState, Action<TmdbAsyncResult<TmdbCompany>> callback)
        {
            var request = new RestRequest("company/{id}", Method.GET);
            request.AddUrlSegment("id", CompanyID.ToString());
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbCompany>(request, callback);
        }

        /// <summary>
        /// Get list of movies associated with a company.  Response will contain 20 movies per page.
        /// (http://help.themoviedb.org/kb/api/company-movies)
        /// </summary>
        /// <param name="CompanyID">Company ID</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetCompanyMovies(int CompanyID, int page, string language, object UserState, Action<TmdbAsyncResult<TmdbCompanyMovies>> callback)
        {
            var request = new RestRequest("company/{id}/movies", Method.GET);
            request.AddUrlSegment("id", CompanyID.ToString());
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddParameter("page", page);
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbCompanyMovies>(request, callback);
        }

        /// <summary>
        /// Get list of movies associated with a company.  Response will contain 20 movies per page.
        /// (http://help.themoviedb.org/kb/api/company-movies)
        /// </summary>
        /// <param name="CompanyID">Company ID</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetCompanyMovies(int CompanyID, int page, object UserState, Action<TmdbAsyncResult<TmdbCompanyMovies>> callback)
        {
            GetCompanyMovies(CompanyID, page, Language, UserState, callback);
        }
        #endregion


        #region Genre Info
        /// <summary>
        /// Get list of genres used in TMDB.  The ids will correspond to those found in movie calls.
        /// (http://help.themoviedb.org/kb/api/genre-list)
        /// </summary>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetGenreList(string language, object UserState, Action<TmdbAsyncResult<TmdbGenre>> callback)
        {
            var request = new RestRequest("genre/list", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbGenre>(request, callback);
        }

        /// <summary>
        /// Get list of genres used in TMDB.  The ids will correspond to those found in movie calls.
        /// (http://help.themoviedb.org/kb/api/genre-list)
        /// </summary>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetGenreList(object UserState, Action<TmdbAsyncResult<TmdbGenre>> callback)
        {
            GetGenreList(Language, UserState, callback);
        }

        /// <summary>
        /// Get list of movies in a Genre.  Note that only movies with more than 10 votes get listed.
        /// (http://help.themoviedb.org/kb/api/genre-movies)
        /// </summary>
        /// <param name="GenreID">TMDB Genre ID</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetGenreMovies(int GenreID, int page, string language, object UserState, Action<TmdbAsyncResult<TmdbGenreMovies>> callback)
        {
            var request = new RestRequest("genre/{id}/movies", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddParameter("page", page);
            request.UserState = UserState;

            ProcessAsyncRequest<TmdbGenreMovies>(request, callback);
        }

        /// <summary>
        /// Get list of movies in a Genre.  Note that only movies with more than 10 votes get listed.
        /// (http://help.themoviedb.org/kb/api/genre-movies)
        /// </summary>
        /// <param name="GenreID">TMDB Genre ID</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetGenreMovies(int GenreID, int page, object UserState, Action<TmdbAsyncResult<TmdbGenreMovies>> callback)
        {
            GetGenreMovies(GenreID, page, Language, UserState, callback);
        }
        #endregion
    }
}
