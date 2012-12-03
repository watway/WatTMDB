using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using WatTmdb.V3;

namespace WatTmdb.WP7Sample
{
    public partial class MainPage : PhoneApplicationPage
    {
        private TmdbConfiguration tmdbConfig;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(MainPage_Loaded);

        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // load the config for the image paths
            var api = new Tmdb(App.ApiKey, null);
            api.GetConfiguration(null, result => tmdbConfig = result.Data);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (tbSearch.Text != "")
            {
                Tmdb api = new Tmdb(App.ApiKey, null);
                api.SearchMovie(tbSearch.Text, 1, null, result =>
                    {
                        var baseUrl = "";
                        if (tmdbConfig != null)
                            baseUrl = string.Format("{0}{1}", tmdbConfig.images.base_url, tmdbConfig.images.poster_sizes[0]);

                        if (result.Data != null)
                        {
                            listbox.ItemsSource = (from r in result.Data.results
                                                   select new Movie
                                                   {
                                                       ID = r.id,
                                                       Title = r.title,
                                                       ThumbnailUrl = r.poster_path != null ? baseUrl + r.poster_path : null
                                                   }).ToList();
                        }
                        else
                            MessageBox.Show(result.Error.status_message, "Error", MessageBoxButton.OK);
                    });
            }
        }

        private void Movies_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/MoviesView.xaml", UriKind.Relative));
        }
    }
}