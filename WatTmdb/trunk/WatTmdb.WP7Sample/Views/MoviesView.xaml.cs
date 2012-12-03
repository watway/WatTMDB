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
using System.Threading;
using WatTmdb.WP7Sample.ViewModels;

namespace WatTmdb.WP7Sample.Views
{
    public partial class MoviesView : PhoneApplicationPage
    {
        private ViewModels.MoviesViewModel Model { get; set; }

        public MoviesView()
        {
            InitializeComponent();

            Model = App.MoviesViewModel;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (Model == null)
            {
                Model = new MoviesViewModel();
                DataContext = Model;
                App.MoviesViewModel = Model;

                ThreadPool.QueueUserWorkItem(delegate(object o)
                {
                    try
                    {
                        // create new model not currently bound to the view to work with
                        var newModel = new MoviesViewModel();
                        newModel.LoadNowPlaying(1);

                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                App.MoviesViewModel = newModel;
                                DataContext = newModel;
                            });
                    }
                    catch (Exception ex)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                MessageBox.Show(ex.Message, "WP7 Sample", MessageBoxButton.OK);
                            });
                    }
                });
            }
            else
            {
                DataContext = Model;
            }

            base.OnNavigatedTo(e);
        }
    }
}