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
using System.Collections.ObjectModel;

namespace WatTmdb.WP7Sample.ViewModels
{
    public class MoviesViewModel : ViewModelBase
    {
        public TmdbConfiguration tmdbConfig { get; set; }

        public MoviesViewModel()
        {
            this.NowPlayingItems = new ObservableCollection<NowPlayingViewModel>();
        }

        /// <summary>
        /// NOTE: method contains calls that will block the current thread, use within a worker thread, not the UI thread.
        /// </summary>
        /// <param name="page"></param>
        public void LoadNowPlaying(int page)
        {
            var facade = new TmdbFacade();

            if (tmdbConfig == null)
                tmdbConfig = facade.GetConfiguration();

            var result = facade.GetNowPlayingMovies(page);
            var baseUrl = string.Format("{0}{1}", tmdbConfig.images.base_url, tmdbConfig.images.poster_sizes[0]);

            foreach (var item in result.results)
            {
                AddNowPlayingItem(new NowPlayingViewModel(item, baseUrl));
            }
        }

        #region Binding Properties

        public ObservableCollection<NowPlayingViewModel> NowPlayingItems { get; private set; }
        public void AddNowPlayingItem(NowPlayingViewModel item)
        {
            NowPlayingItems.Add(item);
            NotifyPropertyChanged("NowPlayingAdded");
        }

        #endregion
    }
}
