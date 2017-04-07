# WatTMDB
.NET Library for use with Version 3 API available from The Movie Db

Website: http://www.themoviedb.org
API: http://docs.themoviedb.apiary.io

**Note: you will need to request an API key from TMDb to be able to use this library.**  Instructions to obtain an API key can be found [here](http://help.themoviedb.org/kb/general/how-do-i-register-for-an-api-key).

The library takes advantage of [RestSharp](http://restsharp.org/) to simplify the requests and JSON responses.

## Using the Library
Add a reference to WatTmdb.dll to your project.

The main api class is _Tmdb_ in the _WatTmdb.V3_ namespace.  The constructer takes your API key, and also an option language string, for example "*en*", or you can pass *null*.

```
WatTmdb.V3.Tmdb api = new Tmdb("apikey", "en");
or
WatTmdb.V3.Tmdb api = new Tmdb("apikey");
```
The api class contains most of the methods currently available by The Movie Db service, except those that are used for updating information back into The Movie Db.

### Available Methods
#### Configuration
_http://help.themoviedb.org/kb/api/configuration_
```
TmdbConfiguration GetConfiguration()
```

#### Searching
_http://help.themoviedb.org/kb/api/search_
```
TmdbMovieSearch SearchMovie(string query, int page)
TmdbPersonSearch SearchPerson(string query, int page)
TmdbCompanySearch SearchCompany(string query, int page)
```

#### Movie Collections
_http://help.themoviedb.org/kb/api/collections_
```
TmdbCollection GetCollectionInfo(int CollectionID)
```

#### Movie Info
_http://help.themoviedb.org/kb/api/movie-info-2_
```
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
```

#### Person Info
_http://help.themoviedb.org/kb/api/person-info-2_
```
TmdbPerson GetPersonInfo(int PersonID)
TmdbPersonCredits GetPersonCredits(int PersonID)
TmdbPersonImages GetPersonImages(int PersonID)
```

#### Miscellaneous Movie
_http://help.themoviedb.org/kb/api/misc-movie_
```
TmdbNowPlaying GetNowPlayingMovies(int page)
TmdbPopular GetPopularMovies(int page)
TmdbTopRated GetTopRatedMovies(int page)
TmdbUpcoming GetUpcomingMovies(int page)
```

#### Company Info
_http://help.themoviedb.org/kb/api/company-info-2_
```
TmdbCompany GetCompanyInfo(int CompanyID)
TmdbCompanyMovies GetCompanyMovies(int CompanyID, int page)
```

#### Genre Info
_http://help.themoviedb.org/kb/api/genre-info_
```
TmdbGenre GetGenreList()
TmdbGenreMovies GetGenreMovies (int GenreID)
```

#### Authentication Methods
1. GetAuthToken - generate a valid request token for user based authentication. [TMDB api](http://docs.themoviedb.apiary.io/#get-%2F3%2Fauthentication%2Ftoken%2Fnew)
2. GetAuthSession - generate a session id for user based authentication. [TMDB api](http://docs.themoviedb.apiary.io/#get-%2F3%2Fauthentication%2Fsession%2Fnew)
3. GetGuestSession - generate a guest session id. [TMDB api](http://docs.themoviedb.apiary.io/#get-%2F3%2Fauthentication%2Fguest_session%2Fnew)

#### Account Methods
1. GetAccountInfo - get basic information for an account [TMDB api](http://docs.themoviedb.apiary.io/#get-%2F3%2Faccount)
2. GetAccountLists - get lists that have been created and movies marked as a favourite [TMDB api](http://docs.themoviedb.apiary.io/#get-%2F3%2Faccount%2F%7Bid%7D%2Ffavorite_movies)
3. GetAccountRatedMovies - get list of rated movies for an account [TMDB api](http://docs.themoviedb.apiary.io/#get-%2F3%2Faccount%2F%7Bid%7D%2Frated_movies)
4. GetAccountWatchlistMovies - get list of movies on an accounts watchlist [TMDB api](http://docs.themoviedb.apiary.io/#get-%2F3%2Faccount%2F%7Bid%7D%2Fmovie_watchlist)

#### Changes Methods
1. GetChangesByMovie - Get list of movie ides that have been edited.  Defaults to changes in the last 24 hours.  Then use the method GetMovieChanges to get the data that has changed. [TMDB api](http://docs.themoviedb.apiary.io/#get-%2F3%2Fmovie%2Fchanges)
2. GetChangesByPerson - Get list of person ids that have been edited.  Defaults to changes in the last 24 hours.  Then use the method GetPersonChanges to get the data that has changed. [TMDB api](http://docs.themoviedb.apiary.io/#get-%2F3%2Fperson%2Fchanges)

#### Keyword Methods
1. GetKeyword - get the basic information for a specific keyword id. [TMDB api](http://docs.themoviedb.apiary.io/#get-%2F3%2Fkeyword%2F%7Bid%7D)
2. GetKeywordMovies - get the list of movies for a particular keyword by id. [TMDB api](http://docs.themoviedb.apiary.io/#get-%2F3%2Fkeyword%2F%7Bid%7D%2Fmovies)

#### Movie Methods
1. GetMovieLists - get the lists that the movie belongs to. [TMDB api](http://docs.themoviedb.apiary.io/#get-%2F3%2Fmovie%2F%7Bid%7D%2Flists)
2. GetMovieChanges - get list of changes for a specific movie. [TMDB api](http://docs.themoviedb.apiary.io/#get-%2F3%2Fmovie%2F%7Bid%7D%2Fchanges)

#### Person Methods
1. GetPersonChanges - get the list of changes for a specific person. [TMDB api](http://docs.themoviedb.apiary.io/#get-%2F3%2Fperson%2F%7Bid%7D%2Fchanges)

If you're after a list of the Genres available, you can use the GetGenreList method.
