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
using System.Windows.Shapes;

namespace Database_Design_Final_Project_Do_Not_Mess.Views
{
    /// <summary>
    /// Interaction logic for ReleaseWindowView.xaml
    /// </summary>
    public partial class ReleaseWindowView : Window
    {
        Release release;
        IMongoDatabase discogsDatabase;
        public ReleaseWindowView(Release release, IMongoDatabase discogsDatabase)
        {
            InitializeComponent();
            this.release = release;
            this.discogsDatabase = discogsDatabase;
            Title.Text = release.Title;

            status.Text = "Status: " + release.Status;
            styles.Text = helperMethods.listOutput<string>("Styles:", release.Styles);
            genres.Text = helperMethods.listOutput<string>("Genres:", release.Genres);
            Artist.Text = release.L_artist + helperMethods.listOutput<HelperStructs.ArtistJoin>("Artists", release.ArtistJoins);
            Credits.Text = helperMethods.listOutput<HelperStructs.ExtraArtist>("", release.ExtraArtists);
            labels.Text = helperMethods.listOutput<HelperStructs.LabelCat>("", release.Labels);
            companies.Text = helperMethods.listOutput<HelperStructs.Company>("", release.Companies);
            formats.Text = helperMethods.listOutput<HelperStructs.Format>("", release.Formats);
            tracks.Text = helperMethods.listOutput<HelperStructs.Track>("", release.TrackList);
            barcode.Text = "Barcode: " + release.Barcode;
            Title.Text = release.Title;
            country.Text = "Country: " + release.Country;
            notes.Text = "Notes: " + release.Notes;
            dataQuality.Text = "Data Quality: " + release.Data_quality;
            lastUpdated.Text = "Last Updated: " + release.LastUpdated1;
            dateReleased.Text = "Date Released: " + release.DateReleased;
        }
    }
}
