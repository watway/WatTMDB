﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WatTmdb.WP7Sample
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
