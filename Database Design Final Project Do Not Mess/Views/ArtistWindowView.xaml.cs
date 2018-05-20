using MongoDB.Bson;
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
    /// Interaction logic for ArtistWindowView.xaml
    /// </summary>
    public partial class ArtistWindowView : Window
    {
        Artist artist;
        IMongoDatabase discogsDatabase;
        int releaseCurrentPage = 0;
        long releaseResultSize;
        long masterResultSize;
        FilterDefinition<BsonDocument> masterSearchFilter;
        FilterDefinition<BsonDocument> releaseSearchFilter;

        /// <summary>
        /// Constructs a view for an individual artist.
        /// </summary>
        /// <param name="artist"></param>
        /// <param name="discogsDatabase"></param>
        public ArtistWindowView(Artist artist, IMongoDatabase discogsDatabase)
        {
            InitializeComponent();
            this.Title = artist.Name;
            this.artist = artist;
            this.discogsDatabase = discogsDatabase;
            /////////
            var artistJoinFilter = Builders<BsonDocument>.Filter.Eq("artistJoins.artist_id", artist.Id.ToString());
            var extraArtistFilter = Builders<BsonDocument>.Filter.Eq("extraartists.artist_id", artist.Id.ToString());
            var ArtistsFilter = Builders<BsonDocument>.Filter.Eq("artists", artist.Name);
            var ArtistFilter = Builders<BsonDocument>.Filter.Eq("l_artist", artist.Name.ToLower());
            masterSearchFilter = artistJoinFilter | extraArtistFilter | ArtistsFilter | ArtistFilter;
            /////////
            var releaseSearch = Builders<BsonDocument>.Filter.Eq("master_id", 0);
            var releaseSearch2 = Builders<BsonDocument>.Filter.Eq("master_id", "0");
            releaseSearchFilter = (artistJoinFilter | extraArtistFilter) & (releaseSearch | releaseSearch2);
            //////////
            artistName.Text = artist.Name;
            profile.Text = "Profile: " + artist.Profile;
            ///////
            nameVariations.Text = helperMethods.listOutput<string>("Name Variations:", artist.NameVariations);
            ///////////
            Uris.Text = helperMethods.listOutput<Uri>("URLs:", artist.Urls);
            ///////////
            aliases.Text = helperMethods.listOutput<string>("Aliases:", artist.Aliases);
            ////////////
            if (artist.Members.Count == 0)
            {
                members.Text = helperMethods.listOutput<string>("Groups:", artist.Groups);
            }
            else
            {
                members.Text = helperMethods.listOutput<string>("Members:", artist.Members);
            }
            ///////////
            dataQuality.Text = "Data Quality: " + artist.DataQuality;
            ///////////
            lastUpdated.Text = "Last Updated: " + artist.LastUpdated;
            ///////////
            realName.Text = "Real Name: " + artist.RealName;
            //Search for Masters and Releases:
            //Search for all Masters released by this artist:

            int masterSearchCount = 0;
            using (var cursor = discogsDatabase.GetCollection<BsonDocument>("masters").Find(masterSearchFilter).Limit(20).ToCursor())
            {
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        Master currentMaster = new Master(doc, HelperStructs.ParentNode.Master, discogsDatabase);
                        var list = new MasterListView(currentMaster, discogsDatabase);
                        releaseStack.Children.Add(list);
                        masterSearchCount++;
                    }
                }
            }
            //Search for Releases released by this artist:
            if (masterSearchCount < 20)
            {

                using (var cursor = discogsDatabase.GetCollection<BsonDocument>("releases").Find(releaseSearchFilter).Limit(20 - masterSearchCount).ToCursor())
                {
                    while (cursor.MoveNext())
                    {
                        foreach (var doc in cursor.Current)
                        {
                            Release currentMaster = new Release(doc, HelperStructs.ParentNode.Release, discogsDatabase);
                            var list = new ReleaseListView(currentMaster, discogsDatabase);
                            releaseStack.Children.Add(list);
                        }
                    }
                }
            }
            masterResultSize = discogsDatabase.GetCollection<BsonDocument>("masters").Find(masterSearchFilter).Count();
            releaseResultSize = discogsDatabase.GetCollection<BsonDocument>("releases").Find(releaseSearchFilter).Count();
            if (releaseResultSize + masterResultSize < 20)
            {
                PageStatus.Text = String.Format($"Showing {releaseCurrentPage * 20 + 1} - {releaseResultSize + masterResultSize} of {releaseResultSize + masterResultSize}");
            }
            else
            {
                PageStatus.Text = String.Format($"Showing {releaseCurrentPage * 20 + 1} - {releaseCurrentPage * 20 + 20} of {releaseResultSize + masterResultSize}");
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (releaseCurrentPage != (releaseResultSize + masterResultSize) / 20)
            {
                releaseCurrentPage++;
                releaseStack.Children.Clear();
                int masterSearchCount = 0;
                if (releaseCurrentPage < (masterResultSize) / 20 - 1)
                {

                    using (var cursor = discogsDatabase.GetCollection<BsonDocument>("masters").Find(masterSearchFilter).Skip(releaseCurrentPage * 20).Limit(20).ToCursor())
                    {
                        while (cursor.MoveNext())
                        {
                            foreach (var doc in cursor.Current)
                            {
                                Master currentMaster = new Master(doc, HelperStructs.ParentNode.Master, discogsDatabase);
                                var list = new MasterListView(currentMaster, discogsDatabase);
                                releaseStack.Children.Add(list);
                                masterSearchCount++;
                            }
                        }
                    }
                }

                //Search for Releases released by this artist:
                if (masterSearchCount < 20)
                {
                    using (var cursor = discogsDatabase.GetCollection<BsonDocument>("releases").Find(releaseSearchFilter).Skip(Convert.ToInt32(releaseCurrentPage - (masterResultSize) / 20 + 1) * 20).Limit(20 - masterSearchCount).ToCursor())
                    {
                        while (cursor.MoveNext())
                        {
                            foreach (var doc in cursor.Current)
                            {
                                Release currentMaster = new Release(doc, HelperStructs.ParentNode.Release, discogsDatabase);
                                var list = new ReleaseListView(currentMaster, discogsDatabase);
                                releaseStack.Children.Add(list);
                            }
                        }
                    }
                }
                if (releaseCurrentPage == (releaseResultSize + masterResultSize) / 20)
                {
                    PageStatus.Text = String.Format($"Showing {releaseCurrentPage * 20 + 1} - {releaseResultSize + masterResultSize} of {releaseResultSize + masterResultSize}");
                }
                else
                {
                    PageStatus.Text = String.Format($"Showing {releaseCurrentPage * 20 + 1} - {releaseCurrentPage * 20 + 20} of {releaseResultSize + masterResultSize}");
                }
            }
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (releaseCurrentPage != 0)
            {
                releaseCurrentPage--;
                releaseStack.Children.Clear();
                int masterSearchCount = 0;
                if (releaseCurrentPage < (masterResultSize) / 20 - 1)
                {

                    using (var cursor = discogsDatabase.GetCollection<BsonDocument>("masters").Find(masterSearchFilter).Skip(releaseCurrentPage * 20).Limit(20).ToCursor())
                    {
                        while (cursor.MoveNext())
                        {
                            foreach (var doc in cursor.Current)
                            {
                                Master currentMaster = new Master(doc, HelperStructs.ParentNode.Master, discogsDatabase);
                                var list = new MasterListView(currentMaster, discogsDatabase);
                                releaseStack.Children.Add(list);
                                masterSearchCount++;
                            }
                        }
                    }
                }

                //Search for Releases released by this artist:
                if (masterSearchCount < 20)
                {
                    using (var cursor = discogsDatabase.GetCollection<BsonDocument>("releases").Find(releaseSearchFilter).
                        Skip(Convert.ToInt32(releaseCurrentPage - (masterResultSize) / 20 + 1) * 20).Limit(20 - masterSearchCount).ToCursor())
                    {
                        while (cursor.MoveNext())
                        {
                            foreach (var doc in cursor.Current)
                            {
                                Release currentMaster = new Release(doc, HelperStructs.ParentNode.Release, discogsDatabase);
                                var list = new ReleaseListView(currentMaster, discogsDatabase);
                                releaseStack.Children.Add(list);
                            }
                        }
                    }
                }
                if (releaseResultSize + masterResultSize < 20)
                {
                    PageStatus.Text = String.Format($"Showing {releaseCurrentPage * 20 + 1} - {releaseResultSize + masterResultSize} of {releaseResultSize + masterResultSize}");
                }
                else
                {
                    PageStatus.Text = String.Format($"Showing {releaseCurrentPage * 20 + 1} - {releaseCurrentPage * 20 + 20} of {releaseResultSize + masterResultSize}");
                }
            }
        }

        private void deleteArtist_Click(object sender, RoutedEventArgs e)
        {
            var artistFilter = Builders<BsonDocument>.Filter.Eq("id", artist.Id);
            discogsDatabase.GetCollection<BsonDocument>("artists").DeleteOne(artistFilter);
            ////Release join
            var updateRelease = Builders<BsonDocument>.Update.Set("artistJoins.$.artist_id", "0").Set("updated_on", String.Format("{0:yyyy-MM-dd}", DateTime.Today));
            var artistjoinFilter = Builders<BsonDocument>.Filter.Eq("artistJoins.artist_id", artist.Id.ToString());
            discogsDatabase.GetCollection<BsonDocument>("releases").UpdateMany(artistjoinFilter, updateRelease);
            /////Release Extra
            updateRelease = Builders<BsonDocument>.Update.Set("extraartists.$.artist_id", "0").Set("updated_on", String.Format("{0:yyyy-MM-dd}", DateTime.Today));           
            var extraArtistFilter = Builders<BsonDocument>.Filter.Eq("extraartists.artist_id", artist.Id.ToString());
            discogsDatabase.GetCollection<BsonDocument>("releases").UpdateMany(extraArtistFilter, updateRelease);
            /////////Master join
            var updateMaster = Builders<BsonDocument>.Update.Set("artistJoins.$.artist_id", "0").Set("updated_on", String.Format("{0:yyyy-MM-dd}", DateTime.Today));
            discogsDatabase.GetCollection<BsonDocument>("masters").UpdateMany(artistjoinFilter, updateMaster);
            /////Master Extra
            updateMaster = Builders<BsonDocument>.Update.Set("extraartists.$.artist_id", "0").Set("updated_on", String.Format("{0:yyyy-MM-dd}", DateTime.Today));
            discogsDatabase.GetCollection<BsonDocument>("masters").UpdateMany(extraArtistFilter, updateMaster);
            this.Close();
            
            /////////
        }

        private void editProfile_Click(object sender, RoutedEventArgs e)
        {
            profile.IsEnabled = true;
            enterEdit.IsEnabled = true;
        }

        private void enterEdit_Click(object sender, RoutedEventArgs e)
        {
            var Filter = Builders<BsonDocument>.Filter.Eq("id", artist.Id);
            var update = Builders<BsonDocument>.Update.Set("profile",profile.Text).Set("updated_on", String.Format("{0:yyyy-MM-dd}", DateTime.Today)).Set("data_quality", "Needs Vote");
            discogsDatabase.GetCollection<BsonDocument>("artists").UpdateOne(Filter, update);
            this.artist = new Artist(discogsDatabase.GetCollection<BsonDocument>("artists").Find(Filter).First(), HelperStructs.ParentNode.Artist, true, discogsDatabase);
            lastUpdated.Text = "Last Updated: " + artist.LastUpdated;
            dataQuality.Text = "Data Quality: " + artist.DataQuality;
            profile.Text = "Profile: " + artist.Profile;
            profile.IsEnabled = false;
            enterEdit.IsEnabled = false;
        }
    }
}

