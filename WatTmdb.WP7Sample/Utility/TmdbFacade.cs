using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using WatTmdb.V3;

namespace WatTmdb.WP7Sample
{
    public class TmdbFacade
    {
        private AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        /// <summary>
        /// Get the Configuration, method will block the current thread
        /// </summary>
        public TmdbConfiguration GetConfiguration()
        {
            var api = new Tmdb(App.ApiKey, null);
            TmdbConfiguration result = null;

            api.GetConfiguration(null, tmdbResult =>
                {
                    result = tmdbResult.Data;
                    _autoResetEvent.Set(); // signal event to continue
                });

            _autoResetEvent.WaitOne(); // block the current thread here until response has been received

            return result;
        }

        public TmdbNowPlaying GetNowPlayingMovies(int page)
        {
            var api = new Tmdb(App.ApiKey, null);
            TmdbNowPlaying result = null;

            api.GetNowPlayingMovies(page, null, tmdbResult =>
                {
                    result = tmdbResult.Data;
                    _autoResetEvent.Set();
                });

            _autoResetEvent.WaitOne();

            return result;
        }
    }
}
