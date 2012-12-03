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
using WatTmdb.V3;

namespace WatTmdb.WP7Sample.ViewModels
{
    public class NowPlayingViewModel : ViewModelBase
    {
        public NowPlaying NowPlayingItem;
        public string BaseUrl;

        public NowPlayingViewModel(NowPlaying item, string baseUrl)
        {
            NowPlayingItem = item;
            BaseUrl = baseUrl;
        }

        #region Binding Properties
        public int ID
        {
            get { return NowPlayingItem.id; }
        }

        public string Title
        {
            get { return NowPlayingItem.title; }
        }

        public string ThumbnailUrl
        {
            //get { return NowPlayingItem.poster_path != null ? BaseUrl + NowPlayingItem.poster_path : null; }
            get
            {
                var url = NowPlayingItem.poster_path != null ? BaseUrl + NowPlayingItem.poster_path : null;
                return url;
            }
        }
        #endregion
    }
}
