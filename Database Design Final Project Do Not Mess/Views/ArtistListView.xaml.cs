﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Database_Design_Final_Project_Do_Not_Mess.Views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ArtistListView : UserControl
    {
        Artist artist;
        IMongoDatabase discogsDatabase;

        public ArtistListView (Artist artist, IMongoDatabase discogsDatabase)
        {
            InitializeComponent();
            this.artist = artist;
            this.discogsDatabase = discogsDatabase;
            Name.Text = artist.Name;
            Profile.Text = artist.Profile;
            
        }
        //Mouse events for the artist name.
        private void Name_MouseEnter(object sender, MouseEventArgs e)
        {
            Name.Foreground = Brushes.Blue;
            Name.Cursor = Cursors.Hand;
        }

        private void Name_MouseLeave(object sender, MouseEventArgs e)
        {
            Name.Foreground = Brushes.Black;
            Name.Cursor = Cursors.Arrow;
        }

        private void Name_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ArtistWindowView windowView = new ArtistWindowView(artist, discogsDatabase);
            windowView.ShowDialog();
        }
    }
}
