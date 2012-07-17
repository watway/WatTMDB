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
            if (Timeout.HasValue)
                client.Timeout = Timeout.Value;

#if !WINDOWS_PHONE
            if (Proxy != null)
                client.Proxy = Proxy;
#endif

            Error = null;

            request.AddHeader("Accept", "application/json");
            request.AddParameter("api_key", ApiKey);

            var resp = client.Execute<T>(request);
            ResponseContent = resp.Content;
            if (resp.ResponseStatus != ResponseStatus.Completed)
            {
                if (resp.Content.Contains("status_message"))
                    Error = jsonDeserializer.Deserialize<TmdbError>(resp);
                else if (resp.ErrorException != null)
                    throw resp.ErrorException;
                else
                    Error = new TmdbError { status_message = resp.ErrorMessage };
            }

            return resp.Data;
        }

        #region Configuration
        /// <summary>
        /// Retrieve configuration data from TMDB
        /// (http://help.themoviedb.org/kb/api/configuration)
        /// </summary>
        /// <returns></returns>
        public TmdbConfiguration GetConfiguration()
        {
            var request = new RestRequest("configuration", Method.GET);

            return ProcessRequest<TmdbConfiguration>(request);
        }
        #endregion


        #region Authentication
        //public TmdbAuthToken GetAuthToken()
        //{
        //    var request = new RestRequest("authentication/token/new", Method.GET);
        //    return ProcessRequest<TmdbAuthToken>(request);
        //}

        //public TmdbAuthSession GetAuthSession(string RequestToken)
        //{
        //    var request = new RestRequest("authentication/session/new", Method.GET);
        //    request.AddParameter("request_token", RequestToken);
        //    return ProcessRequest<TmdbAuthSession>(request);
        //}
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
        /// <returns></returns>
        public TmdbMovieSearch SearchMovie(string query, int page, string language, bool? includeAdult = null, int? year = null)
        {
            if (string.IsNullOrEmpty(query))
            {
                Error = new TmdbError { status_message = "Search cannot be empty" };
                return null;
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

            return ProcessRequest<TmdbMovieSearch>(request);
        }

        /// <summary>
        /// Search for movies that are listed in TMDB
        /// (http://help.themoviedb.org/kb/api/search-movies)
        /// </summary>
        /// <param name="query">Is your search text.</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <returns></returns>
        public TmdbMovieSearch SearchMovie(string query, int page)
        {
            return SearchMovie(query, page, Language);
        }

        /// <summary>
        /// Search for people that are listed in TMDB.
        /// (http://help.themoviedb.org/kb/api/search-people)
        /// </summary>
        /// <param name="query">Is your search text.</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <returns></returns>
        public TmdbPersonSearch SearchPerson(string query, int page, string language)
        {
            if (string.IsNullOrEmpty(query))
            {
                Error = new TmdbError { status_message = "Search cannot be empty" };
                return null;
            }

            var request = new RestRequest("search/person", Method.GET);
            request.AddParameter("query", query.EscapeString());
            request.AddParameter("page", page);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);

            return ProcessRequest<TmdbPersonSearch>(request);
        }

        /// <summary>
        /// Search for people that are listed in TMDB.
        /// (http://help.themoviedb.org/kb/api/search-people)
        /// </summary>
        /// <param name="query">Is your search text.</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <returns></returns>
        public TmdbPersonSearch SearchPerson(string query, int page)
        {
            return SearchPerson(query, page, Language);
        }

        /// <summary>
        /// Search for production companies that are part of TMDB.
        /// (http://help.themoviedb.org/kb/api/search-companies)
        /// </summary>
        /// <param name="query">Is your search text.</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <returns></returns>
        public TmdbCompanySearch SearchCompany(string query, int page)
        {
            if (string.IsNullOrEmpty(query))
            {
                Error = new TmdbError { status_message = "Search cannot be empty" };
                return null;
            }

            var request = new RestRequest("search/company", Method.GET);
            request.AddParameter("query", query.EscapeString());
            request.AddParameter("page", page);

            return ProcessRequest<TmdbCompanySearch>(request);
        }
        #endregion


        #region Collections
        /// <summary>
        /// Get all of the basic information about a movie collection.
        /// (http://help.themoviedb.org/kb/api/collection-info)
        /// </summary>
        /// <param name="CollectionID">Collection ID, available in TmdbMovie::belongs_to_collection</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <returns></returns>
        public TmdbCollection GetCollectionInfo(int CollectionID, string language)
        {
            var request = new RestRequest("collection/{id}", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddUrlSegment("id", CollectionID.ToString());

            return ProcessRequest<TmdbCollection>(request);
        }

        /// <summary>
        /// Get all of the basic information about a movie collection.
        /// (http://help.themoviedb.org/kb/api/collection-info)
        /// </summary>
        /// <param name="CollectionID">Collection ID, available in TmdbMovie::belongs_to_collection</param>
        /// <returns></returns>
        public TmdbCollection GetCollectionInfo(int CollectionID)
        {
            return GetCollectionInfo(CollectionID, Language);
        }
        #endregion


        #region Movie Info
        /// <summary>
        /// Retrieve all the basic movie information for a particular movie by TMDB reference.
        /// (http://help.themoviedb.org/kb/api/movie-info)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <returns></returns>
        public TmdbMovie GetMovieInfo(int MovieID, string language)
        {
            var request = new RestRequest("movie/{id}", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovie>(request);
        }

        /// <summary>
        /// Retrieve all the basic movie information for a particular movie by TMDB reference.
        /// (http://help.themoviedb.org/kb/api/movie-info)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <returns></returns>
        public TmdbMovie GetMovieInfo(int MovieID)
        {
            return GetMovieInfo(MovieID, Language);
        }

        /// <summary>
        /// Retrieve all the basic movie information for a particular movie by IMDB reference.
        /// (http://help.themoviedb.org/kb/api/movie-info)
        /// </summary>
        /// <param name="IMDB_ID">IMDB movie id</param>
        /// <returns></returns>
        public TmdbMovie GetMovieByIMDB(string IMDB_ID)
        {
            if (string.IsNullOrEmpty(IMDB_ID))
            {
                Error = new TmdbError { status_message = "IMDB_ID cannot be empty" };
                return null;
            }

            var request = new RestRequest("movie/{id}", Method.GET);
            request.AddUrlSegment("id", IMDB_ID.EscapeString());

            return ProcessRequest<TmdbMovie>(request);
        }

        /// <summary>
        /// Get list of all the alternative titles for a particular movie.
        /// (http://help.themoviedb.org/kb/api/movie-alternative-titles)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <param name="Country">ISO 3166-1 country code (optional)</param>
        /// <returns></returns>
        public TmdbMovieAlternateTitles GetMovieAlternateTitles(int MovieID, string Country)
        {
            var request = new RestRequest("movie/{id}/alternative_titles", Method.GET);
            if (string.IsNullOrEmpty(Country) == false)
                request.AddParameter("country", Country);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovieAlternateTitles>(request);
        }

        /// <summary>
        /// Get list of all the cast information for a particular movie.
        /// (http://help.themoviedb.org/kb/api/movie-casts)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <returns></returns>
        public TmdbMovieCast GetMovieCast(int MovieID)
        {
            var request = new RestRequest("movie/{id}/casts", Method.GET);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovieCast>(request);
        }

        /// <summary>
        /// Get list of all the images for a particular movie.
        /// (http://help.themoviedb.org/kb/api/movie-images)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <returns></returns>
        public TmdbMovieImages GetMovieImages(int MovieID, string language)
        {
            var request = new RestRequest("movie/{id}/images", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovieImages>(request);
        }

        /// <summary>
        /// Get list of all the images for a particular movie.
        /// (http://help.themoviedb.org/kb/api/movie-images)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <returns></returns>
        public TmdbMovieImages GetMovieImages(int MovieID)
        {
            return GetMovieImages(MovieID, Language);
        }

        /// <summary>
        /// Get list of all the keywords that have been added to a particular movie.  Only English keywords exist currently.
        /// (http://help.themoviedb.org/kb/api/movie-keywords)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <returns></returns>
        public TmdbMovieKeywords GetMovieKeywords(int MovieID)
        {
            var request = new RestRequest("movie/{id}/keywords", Method.GET);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovieKeywords>(request);
        }

        /// <summary>
        /// Get all the release and certification data in TMDB for a particular movie
        /// (http://help.themoviedb.org/kb/api/movie-release-info)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <returns></returns>
        public TmdbMovieReleases GetMovieReleases(int MovieID)
        {
            var request = new RestRequest("movie/{id}/releases", Method.GET);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovieReleases>(request);
        }

        /// <summary>
        /// Get list of trailers for a particular movie.
        /// (http://help.themoviedb.org/kb/api/movie-trailers)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <returns></returns>
        public TmdbMovieTrailers GetMovieTrailers(int MovieID, string language)
        {
            var request = new RestRequest("movie/{id}/trailers", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbMovieTrailers>(request);
        }

        /// <summary>
        /// Get list of trailers for a particular movie.
        /// (http://help.themoviedb.org/kb/api/movie-trailers)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <returns></returns>
        public TmdbMovieTrailers GetMovieTrailers(int MovieID)
        {
            return GetMovieTrailers(MovieID, Language);
        }

        /// <summary>
        /// Get list of similar movies for a particular movie.
        /// (http://help.themoviedb.org/kb/api/movie-similar-movies)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <returns></returns>
        public TmdbSimilarMovies GetSimilarMovies(int MovieID, int page, string language)
        {
            var request = new RestRequest("movie/{id}/similar_movies", Method.GET);
            request.AddParameter("page", page);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbSimilarMovies>(request);
        }

        /// <summary>
        /// Get list of similar movies for a particular movie.
        /// (http://help.themoviedb.org/kb/api/movie-similar-movies)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <returns></returns>
        public TmdbSimilarMovies GetSimilarMovies(int MovieID, int page)
        {
            return GetSimilarMovies(MovieID, page, Language);
        }

        /// <summary>
        /// Get list of all available translations for a specific movie.
        /// (http://help.themoviedb.org/kb/api/movie-translations)
        /// </summary>
        /// <param name="MovieID">TMDB movie id</param>
        /// <returns></returns>
        public TmdbTranslations GetMovieTranslations(int MovieID)
        {
            var request = new RestRequest("movie/{id}/translations", Method.GET);
            request.AddUrlSegment("id", MovieID.ToString());

            return ProcessRequest<TmdbTranslations>(request);
        }
        #endregion


        #region Person Info
        /// <summary>
        /// Get all of the basic information for a person.
        /// (http://help.themoviedb.org/kb/api/person-info)
        /// </summary>
        /// <param name="PersonID">Person ID</param>
        /// <returns></returns>
        public TmdbPerson GetPersonInfo(int PersonID)
        {
            var request = new RestRequest("person/{id}", Method.GET);
            request.AddUrlSegment("id", PersonID.ToString());

            return ProcessRequest<TmdbPerson>(request);
        }

        /// <summary>
        /// Get list of cast and crew information for a person.
        /// (http://help.themoviedb.org/kb/api/person-credits)
        /// </summary>
        /// <param name="PersonID">Person ID</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <returns></returns>
        public TmdbPersonCredits GetPersonCredits(int PersonID, string language)
        {
            var request = new RestRequest("person/{id}/credits", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddUrlSegment("id", PersonID.ToString());

            return ProcessRequest<TmdbPersonCredits>(request);
        }

        /// <summary>
        /// Get list of cast and crew information for a person.
        /// (http://help.themoviedb.org/kb/api/person-credits)
        /// </summary>
        /// <param name="PersonID">Person ID</param>
        /// <returns></returns>
        public TmdbPersonCredits GetPersonCredits(int PersonID)
        {
            return GetPersonCredits(PersonID, Language);
        }

        /// <summary>
        /// Get list of images for a person.
        /// (http://help.themoviedb.org/kb/api/person-images)
        /// </summary>
        /// <param name="PersonID">Person ID</param>
        /// <returns></returns>
        public TmdbPersonImages GetPersonImages(int PersonID)
        {
            var request = new RestRequest("person/{id}/images", Method.GET);
            request.AddUrlSegment("id", PersonID.ToString());

            return ProcessRequest<TmdbPersonImages>(request);
        }
        #endregion


        #region Miscellaneous Movie
        /// <summary>
        /// Get the newest movie added to the TMDB.
        /// (http://help.themoviedb.org/kb/api/latest-movie)
        /// </summary>
        /// <returns></returns>
        //public TmdbLatestMovie GetLatestMovie()
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Get the list of movies currently in theatres.  Response will contain 20 movies per page.
        /// (http://help.themoviedb.org/kb/api/now-playing-movies)
        /// </summary>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <returns></returns>
        public TmdbNowPlaying GetNowPlayingMovies(int page, string language)
        {
            var request = new RestRequest("movie/now-playing", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddParameter("page", page);

            return ProcessRequest<TmdbNowPlaying>(request);
        }

        /// <summary>
        /// Get the list of movies currently in theatres.  Response will contain 20 movies per page.
        /// (http://help.themoviedb.org/kb/api/now-playing-movies)
        /// </summary>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <returns></returns>
        public TmdbNowPlaying GetNowPlayingMovies(int page)
        {
            return GetNowPlayingMovies(page, Language);
        }

        /// <summary>
        /// Get the daily popularity list of movies.  Response will contain 20 movies per page.
        /// (http://help.themoviedb.org/kb/api/popular-movie-list)
        /// </summary>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <returns></returns>
        public TmdbPopular GetPopularMovies(int page, string language)
        {
            var request = new RestRequest("movie/popular", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddParameter("page", page);

            return ProcessRequest<TmdbPopular>(request);
        }

        /// <summary>
        /// Get the daily popularity list of movies.  Response will contain 20 movies per page.
        /// (http://help.themoviedb.org/kb/api/popular-movie-list)
        /// </summary>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <returns></returns>
        public TmdbPopular GetPopularMovies(int page)
        {
            return GetPopularMovies(page, Language);
        }

        /// <summary>
        /// Get list of movies that have over 10 votes on TMDB.  Response will contain 20 movies per page.
        /// (http://help.themoviedb.org/kb/api/top-rated-movies)
        /// </summary>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <returns></returns>
        public TmdbTopRated GetTopRatedMovies(int page, string language)
        {
            var request = new RestRequest("movie/top-rated", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddParameter("page", page);

            return ProcessRequest<TmdbTopRated>(request);
        }

        /// <summary>
        /// Get list of movies that have over 10 votes on TMDB.  Response will contain 20 movies per page.
        /// (http://help.themoviedb.org/kb/api/top-rated-movies)
        /// </summary>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <returns></returns>
        public TmdbTopRated GetTopRatedMovies(int page)
        {
            return GetTopRatedMovies(page, Language);
        }

        /// <summary>
        /// Get list of movies that are arriving to theatres in the next few weeks.
        /// (http://help.themoviedb.org/kb/api/upcoming-movies)
        /// </summary>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <returns></returns>
        public TmdbUpcoming GetUpcomingMovies(int page, string language)
        {
            var request = new RestRequest("movie/upcoming", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddParameter("page", page);

            return ProcessRequest<TmdbUpcoming>(request);
        }

        /// <summary>
        /// Get list of movies that are arriving to theatres in the next few weeks.
        /// (http://help.themoviedb.org/kb/api/upcoming-movies)
        /// </summary>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <returns></returns>
        public TmdbUpcoming GetUpcomingMovies(int page)
        {
            return GetUpcomingMovies(page, Language);
        }
        #endregion


        #region Company Info
        /// <summary>
        /// Get basic information about a production company from TMDB.
        /// (http://help.themoviedb.org/kb/api/company-info)
        /// </summary>
        /// <param name="CompanyID">Company ID</param>
        /// <returns></returns>
        public TmdbCompany GetCompanyInfo(int CompanyID)
        {
            var request = new RestRequest("company/{id}", Method.GET);
            request.AddUrlSegment("id", CompanyID.ToString());

            return ProcessRequest<TmdbCompany>(request);
        }

        /// <summary>
        /// Get list of movies associated with a company.  Response will contain 20 movies per page.
        /// (http://help.themoviedb.org/kb/api/company-movies)
        /// </summary>
        /// <param name="CompanyID">Company ID</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <returns></returns>
        public TmdbCompanyMovies GetCompanyMovies(int CompanyID, int page, string language)
        {
            var request = new RestRequest("company/{id}/movies", Method.GET);
            request.AddUrlSegment("id", CompanyID.ToString());
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddParameter("page", page);

            return ProcessRequest<TmdbCompanyMovies>(request);
        }

        /// <summary>
        /// Get list of movies associated with a company.  Response will contain 20 movies per page.
        /// (http://help.themoviedb.org/kb/api/company-movies)
        /// </summary>
        /// <param name="CompanyID">Company ID</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <returns></returns>
        public TmdbCompanyMovies GetCompanyMovies(int CompanyID, int page)
        {
            return GetCompanyMovies(CompanyID, page, Language);
        }
        #endregion


        #region Genre Info
        /// <summary>
        /// Get list of genres used in TMDB.  The ids will correspond to those found in movie calls.
        /// (http://help.themoviedb.org/kb/api/genre-list)
        /// </summary>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <returns></returns>
        public TmdbGenre GetGenreList(string language)
        {
            var request = new RestRequest("genre/list", Method.GET);
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);

            return ProcessRequest<TmdbGenre>(request);
        }

        /// <summary>
        /// Get list of genres used in TMDB.  The ids will correspond to those found in movie calls.
        /// (http://help.themoviedb.org/kb/api/genre-list)
        /// </summary>
        /// <returns></returns>
        public TmdbGenre GetGenreList()
        {
            return GetGenreList(Language);
        }

        /// <summary>
        /// Get list of movies in a Genre.  Note that only movies with more than 10 votes get listed.
        /// (http://help.themoviedb.org/kb/api/genre-movies)
        /// </summary>
        /// <param name="GenreID">TMDB Genre ID</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <param name="language">optional - ISO 639-1 language code</param>
        /// <returns></returns>
        public TmdbGenreMovies GetGenreMovies(int GenreID, int page, string language)
        {
            var request = new RestRequest("genre/{id}/movies", Method.GET);
            request.AddUrlSegment("id", GenreID.ToString());
            if (string.IsNullOrEmpty(language) == false)
                request.AddParameter("language", language);
            request.AddParameter("page", page);

            return ProcessRequest<TmdbGenreMovies>(request);
        }

        /// <summary>
        /// Get list of movies in a Genre.  Note that only movies with more than 10 votes get listed.
        /// (http://help.themoviedb.org/kb/api/genre-movies)
        /// </summary>
        /// <param name="GenreID">TMDB Genre ID</param>
        /// <param name="page">Result page to retrieve (1 based)</param>
        /// <returns></returns>
        public TmdbGenreMovies GetGenreMovies(int GenreID, int page)
        {
            return GetGenreMovies(GenreID, page, Language);
        }
        #endregion
    }
}
