using MongoDB.Driver;
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
    /// Interaction logic for ReleaseListView.xaml
    /// </summary>
    public partial class ReleaseListView : UserControl
    {
        Release release;
        IMongoDatabase discogsDatabase;

        public ReleaseListView(Release release, IMongoDatabase discogsDatabase)
        {
            InitializeComponent();
            this.release = release;
            this.discogsDatabase = discogsDatabase;
            Title.Text = release.Title;
            StringBuilder temp = new StringBuilder();
            for (int i = 0; i < release.ExtraArtists.Count; i++)
            {
                temp.Append(release.ExtraArtists[i].Artist);
                if (i != release.ExtraArtists.Count - 1)
                {
                    temp.Append(", ");
                }
            }
            for (int i = 1; i < release.ArtistJoins.Count; i++)
            {
                temp.Append(release.ArtistJoins[i].Artist);
                if (i != release.ArtistJoins.Count - 1)
                {
                    temp.Append(", ");
                }
            }
            Artist.Text = release.L_artist + temp.ToString(); 
            temp.Clear();
            for (int i = 0; i < release.Labels.Count; i++)
            {
                temp.Append(release.Labels[i].Label.Name);
                if (i != release.Labels.Count - 1)
                {
                    temp.Append(", ");
                }
            }
            Label.Text = temp.ToString();
            Country.Text = release.Country;
            temp.Clear();
            for (int i = 0; i < release.Formats.Count; i++)
            {
                temp.Append(release.Formats[i].Name);
                if (i != release.Formats.Count - 1)
                {
                    temp.Append(", ");
                }
            }
            Format.Text = temp.ToString();
            Year.Text = release.DateReleased;

        }
        //Mouse events for the release name.
        private void Name_MouseEnter(object sender, MouseEventArgs e)
        {
            Title.Foreground = Brushes.Blue;
            Title.Cursor = Cursors.Hand;
        }

        private void Name_MouseLeave(object sender, MouseEventArgs e)
        {
            Title.Foreground = Brushes.Black;
            Title.Cursor = Cursors.Arrow;
        }

        private void Name_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ReleaseWindowView windowView = new ReleaseWindowView(release, discogsDatabase);
            windowView.ShowDialog();
        }
    }
}
