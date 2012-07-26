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
            client.AddHandler("application/json", new WatJsonDeserializer());
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
                    ResponseHeaders = resp.Headers.ToDictionary(k => k.Name, v => v.Value);

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

        private void ProcessAsyncRequestETag(RestRequest request, Action<TmdbAsyncETagResult> callback)
        {
            var client = new RestClient(BASE_URL);
            if (Timeout.HasValue)
                client.Timeout = Timeout.Value;

#if !WINDOWS_PHONE
            if (Proxy!=null)
                client.Proxy = Proxy;
#endif

            Error = null;

            request.Method = Method.HEAD;
            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);

            var asyncHandle = client.ExecuteAsync(request, resp =>
                {
                    ResponseContent = resp.Content;
                    ResponseHeaders = resp.Headers.ToDictionary(k => k.Name, v => v.Value);

                    var result = new TmdbAsyncETagResult
                    {
                        ETag = ResponseETag,
                        UserState = request.UserState
                    };

                    if (resp.ResponseStatus != ResponseStatus.Completed && resp.ErrorException != null)
                        throw resp.ErrorException;

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
            ProcessAsyncRequest<TmdbConfiguration>(BuildGetConfigurationRequest(UserState), callback);
        }

        public void GetConfigurationETag(object UserState, Action<TmdbAsyncETagResult> callback)
        {
            ProcessAsyncRequestETag(BuildGetConfigurationRequest(UserState), callback);
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

            ProcessAsyncRequest<TmdbMovieSearch>(BuildSearchMovieRequest(query, page, language, includeAdult, year, UserState), callback);
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

            ProcessAsyncRequest<TmdbPersonSearch>(BuildSearchPersonRequest(query, page, language, UserState), callback);
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

            ProcessAsyncRequest<TmdbCompanySearch>(BuildSearchCompanyRequest(query, page, UserState), callback);
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
            ProcessAsyncRequest<TmdbCollection>(BuildGetCollectionInfoRequest(CollectionID, language, UserState), callback);
        }

        public void GetCollectionInfoETag(int CollectionID, string language, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            ProcessAsyncRequestETag(BuildGetCollectionInfoRequest(CollectionID, language, UserState), callback);
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

        public void GetCollectionInfoETag(int CollectionID, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            GetCollectionInfoETag(CollectionID, Language, UserState, callback);
        }

        /// <summary>
        /// Get all the images for a movie collection
        /// http://help.themoviedb.org/kb/api/collection-images
        /// </summary>
        /// <param name="CollectionID">Collection ID, available in TmdbMovie::belongs_to_collection</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetCollectionImages(int CollectionID, string language, object UserState, Action<TmdbAsyncResult<TmdbCollectionImages>> callback)
        {
            ProcessAsyncRequest<TmdbCollectionImages>(BuildGetCollectionImagesRequest(CollectionID, language, UserState), callback);
        }

        public void GetCollectionImagesETag(int CollectionID, string language, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            ProcessAsyncRequestETag(BuildGetCollectionImagesRequest(CollectionID, language, UserState), callback);
        }

        /// <summary>
        /// Get all the images for a movie collection
        /// http://help.themoviedb.org/kb/api/collection-images
        /// </summary>
        /// <param name="CollectionID">Collection ID, available in TmdbMovie::belongs_to_collection</param>
        /// <param name="UserState">User object to include in callback</param>
        /// <param name="callback"></param>
        public void GetCollectionImages(int CollectionID, object UserState, Action<TmdbAsyncResult<TmdbCollectionImages>> callback)
        {
            GetCollectionImages(CollectionID, Language, UserState, callback);
        }

        public void GetCollectionImagesETag(int CollectionID, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            GetCollectionImagesETag(CollectionID, Language, UserState, callback);
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
            ProcessAsyncRequest<TmdbMovie>(BuildGetMovieInfoRequest(MovieID, language, UserState), callback);
        }

        public void GetMovieInfoETag(int MovieID, string language, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            ProcessAsyncRequestETag(BuildGetMovieInfoRequest(MovieID, language, UserState), callback);
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

        public void GetMovieInfoETag(int MovieID, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            GetMovieInfoETag(MovieID, Language, UserState, callback);
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

            ProcessAsyncRequest<TmdbMovie>(BuildGetMovieByIMDBRequest(IMDB_ID, UserState), callback);
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
            ProcessAsyncRequest<TmdbMovieAlternateTitles>(BuildGetMovieAlternateTitlesRequest(MovieID, Country, UserState), callback);
        }

        public void GetMovieAlternateTitlesETag(int MovieID, string Country, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            ProcessAsyncRequestETag(BuildGetMovieAlternateTitlesRequest(MovieID, Country, UserState), callback);
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
            ProcessAsyncRequest<TmdbMovieCast>(BuildGetMovieCastRequest(MovieID, UserState), callback);
        }

        public void GetMovieCastETag(int MovieID, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            ProcessAsyncRequestETag(BuildGetMovieCastRequest(MovieID, UserState), callback);
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
            ProcessAsyncRequest<TmdbMovieImages>(BuildGetMovieImagesRequest(MovieID, language, UserState), callback);
        }

        public void GetMovieImagesETag(int MovieID, string language, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            ProcessAsyncRequestETag(BuildGetMovieImagesRequest(MovieID, language, UserState), callback);
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

        public void GetMovieImagesETag(int MovieID, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            GetMovieImagesETag(MovieID, Language, UserState, callback);
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
            ProcessAsyncRequest<TmdbMovieKeywords>(BuildGetMovieKeywordsRequest(MovieID, UserState), callback);
        }

        public void GetMovieKeywordsETag(int MovieID, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            ProcessAsyncRequestETag(BuildGetMovieKeywordsRequest(MovieID, UserState), callback);
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
            ProcessAsyncRequest<TmdbMovieReleases>(BuildGetMovieReleasesRequest(MovieID, UserState), callback);
        }

        public void GetMovieReleasesETag(int MovieID, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            ProcessAsyncRequestETag(BuildGetMovieReleasesRequest(MovieID, UserState), callback);
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
            ProcessAsyncRequest<TmdbMovieTrailers>(BuildGetMovieTrailersRequest(MovieID, language, UserState), callback);
        }

        public void GetMovieTrailersETag(int MovieID, string language, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            ProcessAsyncRequestETag(BuildGetMovieTrailersRequest(MovieID, language, UserState), callback);
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

        public void GetMovieTrailersETag(int MovieID, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            GetMovieTrailersETag(MovieID, Language, UserState, callback);
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
            ProcessAsyncRequest<TmdbTranslations>(BuildGetMovieTranslationsRequest(MovieID, UserState), callback);
        }

        public void GetMovieTranslationsETag(int MovieID, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            ProcessAsyncRequestETag(BuildGetMovieTranslationsRequest(MovieID, UserState), callback);
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
            ProcessAsyncRequest<TmdbSimilarMovies>(BuildGetSimilarMoviesRequest(MovieID, page, language, UserState), callback);
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
            ProcessAsyncRequest<TmdbUpcoming>(BuildGetUpcomingMoviesRequest(page, language, UserState), callback);
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
            ProcessAsyncRequest<TmdbPerson>(BuildGetPersonInfoRequest(PersonID, UserState), callback);
        }

        public void GetPersonInfoETag(int PersonID, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            ProcessAsyncRequestETag(BuildGetPersonInfoRequest(PersonID, UserState), callback);
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
            ProcessAsyncRequest<TmdbPersonCredits>(BuildGetPersonCreditsRequest(PersonID, language, UserState), callback);
        }

        public void GetPersonCreditsETag(int PersonID, string language, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            ProcessAsyncRequestETag(BuildGetPersonCreditsRequest(PersonID, language, UserState), callback);
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

        public void GetPersonCreditsETag(int PersonID, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            GetPersonCreditsETag(PersonID, Language, UserState, callback);
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
            ProcessAsyncRequest<TmdbPersonImages>(BuildGetPersonImagesRequest(PersonID, UserState), callback);
        }

        public void GetPersonImagesETag(int PersonID, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            ProcessAsyncRequestETag(BuildGetPersonImagesRequest(PersonID, UserState), callback);
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
        public void GetLatestMovie(object UserState, Action<TmdbAsyncResult<TmdbLatestMovie>> callback)
        {
            ProcessAsyncRequest<TmdbLatestMovie>(BuildGetLatestMovieRequest(UserState), callback);
        }

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
            ProcessAsyncRequest<TmdbNowPlaying>(BuildGetNowPlayingMoviesRequest(page, language, UserState), callback);
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
            ProcessAsyncRequest<TmdbPopular>(BuildGetPopularMoviesRequest(page, language, UserState), callback);
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
            ProcessAsyncRequest<TmdbTopRated>(BuildGetTopRatedMoviesRequest(page, language, UserState), callback);
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
            ProcessAsyncRequest<TmdbCompany>(BuildGetCompanyInfoRequest(CompanyID, UserState), callback);
        }

        public void GetCompanyInfoETag(int CompanyID, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            ProcessAsyncRequestETag(BuildGetCompanyInfoRequest(CompanyID, UserState), callback);
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
            ProcessAsyncRequest<TmdbCompanyMovies>(BuildGetCompanyMoviesRequest(CompanyID, page, language, UserState), callback);
        }

        public void GetCompanyMoviesETag(int CompanyID, int page, string language, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            ProcessAsyncRequestETag(BuildGetCompanyMoviesRequest(CompanyID, page, language, UserState), callback);
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

        public void GetCompanyMoviesETag(int CompanyID, int page, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            GetCompanyMoviesETag(CompanyID, page, Language, UserState, callback);
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
            ProcessAsyncRequest<TmdbGenre>(BuildGetGenreListRequest(language, UserState), callback);
        }

        public void GetGenreListETag(string language, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            ProcessAsyncRequestETag(BuildGetGenreListRequest(language, UserState), callback);
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

        public void GetGenreListETag(object UserState, Action<TmdbAsyncETagResult> callback)
        {
            GetGenreListETag(Language, UserState, callback);
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
            ProcessAsyncRequest<TmdbGenreMovies>(BuildGetGenreMoviesRequest(GenreID, page, language, UserState), callback);
        }

        public void GetGenreMoviesETag(int GenreID, int page, string language, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            ProcessAsyncRequestETag(BuildGetGenreMoviesRequest(GenreID, page, language, UserState), callback);
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

        public void GetGenreMoviesETag(int GenreID, int page, object UserState, Action<TmdbAsyncETagResult> callback)
        {
            GetGenreMoviesETag(GenreID, page, Language, UserState, callback);
        }
        #endregion
    }
}
