# Using the Library
Add a reference to WatTmdb.dll to your project.

The main api class is _Tmdb_ in the _WatTmdb.V3_ namespace.  The constructer takes your API key, and also an option language string, for example "**en**", or you can pass **null**.  Instructions to obtain an API key can be found [here](http://help.themoviedb.org/kb/general/how-do-i-register-for-an-api-key).

{{
WatTmdb.V3.Tmdb api = new Tmdb("apikey", "en");
or
WatTmdb.V3.Tmdb api = new Tmdb("apikey");
}}
The api class contains most of the methods currently available by The Movie Db service, except those that are used for updating information back into The Movie Db.

## Available Methods
### Configuration
_[http://help.themoviedb.org/kb/api/configuration](http://help.themoviedb.org/kb/api/configuration)_
{{
TmdbConfiguration GetConfiguration()
}}

### Searching
_[http://help.themoviedb.org/kb/api/search](http://help.themoviedb.org/kb/api/search)_
{{
TmdbMovieSearch SearchMovie(string query, int page)
TmdbPersonSearch SearchPerson(string query, int page)
TmdbCompanySearch SearchCompany(string query, int page)
}}

### Movie Collections
_[http://help.themoviedb.org/kb/api/collections](http://help.themoviedb.org/kb/api/collections)_
{{
TmdbCollection GetCollectionInfo(int CollectionID)
}}

### Movie Info
_[http://help.themoviedb.org/kb/api/movie-info-2](http://help.themoviedb.org/kb/api/movie-info-2)_
{{
TmdbMovie GetMovieInfo(int MovieID)
TmdbMovie GetMovieByIMDB(string IMDB_ID)
TmdbMovieAlternateTitles GetMovieAlternateTitles(int MovieID, string Country)
TmdbMovieCast GetMovieCast(int MovieID)
TmdbMovieImages GetMovieImages(int MovieID)
TmdbMovieKeywords GetMovieKeywords(int MovieID)
TmdbMovieReleases GetMovieReleases(int MovieID)
TmdbMovieTrailers GetMovieTrailers(int MovieID)
TmdbSimilarMovies GetSimilarMovies(int MovieID, int page)
TmdbTranslations GetMovieTranslations(int MovieID)
}}

### Person Info
_[http://help.themoviedb.org/kb/api/person-info-2](http://help.themoviedb.org/kb/api/person-info-2)_
{{
TmdbPerson GetPersonInfo(int PersonID)
TmdbPersonCredits GetPersonCredits(int PersonID)
TmdbPersonImages GetPersonImages(int PersonID)
}}

### Miscellaneous Movie
_[http://help.themoviedb.org/kb/api/misc-movie](http://help.themoviedb.org/kb/api/misc-movie)_
{{
TmdbNowPlaying GetNowPlayingMovies(int page)
TmdbPopular GetPopularMovies(int page)
TmdbTopRated GetTopRatedMovies(int page)
TmdbUpcoming GetUpcomingMovies(int page)
}}

### Company Info
_[http://help.themoviedb.org/kb/api/company-info-2](http://help.themoviedb.org/kb/api/company-info-2)_
{{
TmdbCompany GetCompanyInfo(int CompanyID)
TmdbCompanyMovies GetCompanyMovies(int CompanyID, int page)
}}

### Genre Info
_[http://help.themoviedb.org/kb/api/genre-info](http://help.themoviedb.org/kb/api/genre-info)_
{{
TmdbGenre GetGenreList()
TmdbGenreMovies GetGenreMovies (int GenreID)
}}