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
using Database_Design_Final_Project_Do_Not_Mess.Views;
using MongoDB.Bson;

namespace Database_Design_Final_Project_Do_Not_Mess
{
    /// <summary>
    /// Interaction logic for DiscogsMain.xaml
    /// </summary>
    public partial class DiscogsMain : Window
    {
        QuerryProcessor querryProcessor;
        //Artist Result variables
        long artistResultSize;
        long artistCurrentPage;
        string currentartistQuerry = "";
        //Label Result variables
        long labelResultSize;
        long labelCurrentPage;
        string currentlabelQuerry = "";
        //Master result varaibles
        long masterResultSize;
        long masterCurrentPage;
        string currentmasterQuerry = "";
        //Release result variables
        long releaseResultSize;
        long releaseCurrentPage;
        string currentreleaseQuerry = "";
        /// <summary>
        /// The constructor will create a new QuerryProcessor object upon creation. It writes the
        /// number of 4 database entities in the specified collections. It also querries all the 
        /// 4 collections to fill the stack panels.
        /// </summary>
        /// <param name="mongoClient">the client created in the log in page.</param>
        public DiscogsMain(MongoClient mongoClient)
        {
            this.querryProcessor = new QuerryProcessor(mongoClient);
            InitializeComponent();
            //Intitializing the artists tab
            artistsTab.Header = String.Format($"{artistsTab.Header} {querryProcessor.NumArtists}");
            List<Artist> artistOutput = querryProcessor.artistRead(0);
            for (int i = 0; i < artistOutput.Count; i++)
            {
                ArtistListView currentView = new ArtistListView(artistOutput[i], querryProcessor.DiscogsDatabase);
                artistPanel.Children.Add(currentView);
            }
            artistResultSize = querryProcessor.NumArtists;
            artistCurrentPage = 0;
            artistPanelStats.Text = String.Format($"Showing {artistCurrentPage + 1} - 20 of {artistResultSize}");
            //Initializing the labels tab
            labelsTab.Header = String.Format($"{labelsTab.Header} {querryProcessor.NumLabels}");
            List<Label> labelOutput = querryProcessor.labelRead(0);
            for (int i = 0; i < labelOutput.Count; i++)
            {
                LabelListView currentView = new LabelListView(labelOutput[i], querryProcessor.DiscogsDatabase);
                labelsPanel.Children.Add(currentView);
            }
            labelResultSize = querryProcessor.NumLabels;
            labelCurrentPage = 0;
            labelPanelStats.Text = String.Format($"Showing {labelCurrentPage + 1} - 20 of {labelResultSize}");
            //Intializing the masters tab
            mastersTab.Header = String.Format($"{mastersTab.Header} {querryProcessor.NumMasters}");
            
            List<Master> masterOutput = querryProcessor.masterRead(0);
            for (int i = 0; i < masterOutput.Count; i++)
            {
                MasterListView currentView = new MasterListView(masterOutput[i], querryProcessor.DiscogsDatabase);
                mastersPanel.Children.Add(currentView);
            }
            masterResultSize = querryProcessor.NumMasters;
            masterCurrentPage = 0;
            masterPanelStats.Text = String.Format($"Showing {masterCurrentPage + 1} - 20 of {masterResultSize}");
            //Initializing the releases tab
            releasesTab.Header = String.Format($"{releasesTab.Header} {querryProcessor.NumReleases}");
            releaseResultSize = querryProcessor.NumReleases;
            List<Release> releaseOutput = querryProcessor.releaseRead(0);
            for (int i = 0; i < releaseOutput.Count; i++)
            {
                ReleaseListView currentView = new ReleaseListView(releaseOutput[i], querryProcessor.DiscogsDatabase);
                releasesPanel.Children.Add(currentView);
            }
            releaseResultSize = querryProcessor.NumReleases;
            releaseCurrentPage = 0;
            releasePanelStats.Text = String.Format($"Showing {releaseCurrentPage + 1} - 20 of {releaseResultSize}");
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string temp = new TextRange(NameSearch.Document.ContentStart, NameSearch.Document.ContentEnd).Text;
            string text = new TextRange(NameSearch.Document.ContentStart, NameSearch.Document.ContentEnd).Text.Remove(temp.Length - 2, 2);
            if (text == null)
            {
                return;
            }
            if (artistsTab.IsSelected)
            {
                artistPanel.Children.RemoveRange(0, artistPanel.Children.Count);
                string artistName = new TextRange(NameSearch.Document.ContentStart, NameSearch.Document.ContentEnd).Text;
                currentartistQuerry = new TextRange (NameSearch.Document.ContentStart, NameSearch.Document.ContentEnd).Text.Remove(artistName.Length - 2, 2);
                List<Artist> output = querryProcessor.artistQuery(0, currentartistQuerry, Builders<BsonDocument>.Filter.Empty);
                artistPanel.Children.Clear();
                artistResultSize = querryProcessor.artistQuery(currentartistQuerry, Builders<BsonDocument>.Filter.Empty);
                artistCurrentPage = 0;
                for (int i = 0; i <output.Count; i++)
                {
                    ArtistListView currentView = new ArtistListView(output[i],querryProcessor.DiscogsDatabase);                   
                    artistPanel.Children.Add(currentView);
                }
                if (artistResultSize < 20)
                {
                    artistPanelStats.Text = String.Format($"Showing {artistCurrentPage + 1} - {artistResultSize} of {artistResultSize}");
                }
                else
                {
                    artistPanelStats.Text = String.Format($"Showing {artistCurrentPage + 1} - 20 of {artistResultSize}");
                }
                
            }

            if (labelsTab.IsSelected)
            {
                labelsPanel.Children.RemoveRange(0, labelsPanel.Children.Count);
                string labelName = new TextRange(NameSearch.Document.ContentStart, NameSearch.Document.ContentEnd).Text;
                currentlabelQuerry = new TextRange(NameSearch.Document.ContentStart, NameSearch.Document.ContentEnd).Text.Remove(labelName.Length - 2, 2);
                List<Label> output = querryProcessor.labelQuery(0, currentlabelQuerry, Builders<BsonDocument>.Filter.Empty);
                labelsPanel.Children.Clear();
                labelResultSize = querryProcessor.labelQuery(currentlabelQuerry, Builders<BsonDocument>.Filter.Empty);
                labelCurrentPage = 0;
                for (int i = 0; i < output.Count; i++)
                {
                    LabelListView currentView = new LabelListView(output[i], querryProcessor.DiscogsDatabase);
                    labelsPanel.Children.Add(currentView);
                }
                if (labelResultSize < 20)
                {
                    labelPanelStats.Text = String.Format($"Showing {labelCurrentPage + 1} - {labelResultSize} of {labelResultSize}");
                }
                else
                {
                    labelPanelStats.Text = String.Format($"Showing {labelCurrentPage + 1} - 20 of {labelResultSize}");
                }
            }

            if (mastersTab.IsSelected)
            {
                mastersPanel.Children.RemoveRange(0, mastersPanel.Children.Count);
                string masterName = new TextRange(NameSearch.Document.ContentStart, NameSearch.Document.ContentEnd).Text;
                currentmasterQuerry = new TextRange(NameSearch.Document.ContentStart, NameSearch.Document.ContentEnd).Text.Remove(masterName.Length - 2, 2);
                List<Master> output = querryProcessor.masterQuery(0, currentmasterQuerry, Builders<BsonDocument>.Filter.Empty);
                mastersPanel.Children.Clear();
                masterResultSize = querryProcessor.masterQuery(currentmasterQuerry, Builders<BsonDocument>.Filter.Empty);
                masterCurrentPage = 0;
                for (int i = 0; i < output.Count; i++)
                {
                    MasterListView currentView = new MasterListView(output[i], querryProcessor.DiscogsDatabase);
                    mastersPanel.Children.Add(currentView);
                }
                if (masterResultSize < 20)
                {
                    masterPanelStats.Text = String.Format($"Showing {masterCurrentPage + 1} - {masterResultSize} of {masterResultSize}");
                }
                else
                {
                    masterPanelStats.Text = String.Format($"Showing {masterCurrentPage + 1} - 20 of {masterResultSize}");
                }
            }

            if (releasesTab.IsSelected)
            {
                releasesPanel.Children.RemoveRange(0, releasesPanel.Children.Count);
                string releaseName = new TextRange(NameSearch.Document.ContentStart, NameSearch.Document.ContentEnd).Text;
                currentreleaseQuerry = new TextRange(NameSearch.Document.ContentStart, NameSearch.Document.ContentEnd).Text.Remove(releaseName.Length - 2, 2);
                List<Release> output = querryProcessor.releaseQuery(0, currentreleaseQuerry, Builders<BsonDocument>.Filter.Empty);
                releasesPanel.Children.Clear();
                releaseResultSize = querryProcessor.releaseQuery(currentreleaseQuerry, Builders<BsonDocument>.Filter.Empty);
                releaseCurrentPage = 0;
                for (int i = 0; i < output.Count; i++)
                {
                    ReleaseListView currentView = new ReleaseListView(output[i], querryProcessor.DiscogsDatabase);
                    releasesPanel.Children.Add(currentView);
                }
                if (releaseResultSize < 20)
                {
                    releasePanelStats.Text = String.Format($"Showing {releaseCurrentPage + 1} - {releaseResultSize} of {releaseResultSize}");
                }
                else
                {
                    releasePanelStats.Text = String.Format($"Showing {releaseCurrentPage + 1} - 20 of {releaseResultSize}");
                }
            }
        }

        /// <summary>
        /// don't touch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (artistsTab.IsSelected)
            {
                if (artistCurrentPage != artistResultSize/20 - 1)
                {
                    artistCurrentPage++;
                    List<Artist> output = querryProcessor.artistQuery((int)artistCurrentPage, currentartistQuerry, Builders<BsonDocument>.Filter.Empty);
                    artistPanel.Children.Clear();
                    artistResultSize = querryProcessor.artistQuery(currentartistQuerry, Builders<BsonDocument>.Filter.Empty);
                    for (int i = 0; i < output.Count; i++)
                    {
                        ArtistListView currentView = new ArtistListView(output[i], querryProcessor.DiscogsDatabase);
                        artistPanel.Children.Add(currentView);
                    }
                    if (artistCurrentPage == artistResultSize / 20 - 1)
                    {
                        artistPanelStats.Text = String.Format($"Showing {artistCurrentPage * 20 + 1} - {artistResultSize} of {artistResultSize}");
                    }
                    else
                    {
                        artistPanelStats.Text = String.Format($"Showing {artistCurrentPage * 20 + 1} - {artistCurrentPage * 20 + 20} of {artistResultSize}");
                    }
                }
            }

            if (labelsTab.IsSelected)
            {
                if (labelCurrentPage != labelResultSize / 20 - 1)
                {
                    labelCurrentPage++;
                    List<Label> output = querryProcessor.labelQuery((int)labelCurrentPage, currentlabelQuerry, Builders<BsonDocument>.Filter.Empty);
                    labelsPanel.Children.Clear();
                    labelResultSize = querryProcessor.labelQuery(currentlabelQuerry, Builders<BsonDocument>.Filter.Empty);
                    for (int i = 0; i < output.Count; i++)
                    {
                        LabelListView currentView = new LabelListView(output[i], querryProcessor.DiscogsDatabase);
                        labelsPanel.Children.Add(currentView);
                    }
                    if (labelCurrentPage == labelResultSize / 20 - 1)
                    {
                        labelPanelStats.Text = String.Format($"Showing {labelCurrentPage * 20 + 1} - {labelResultSize} of {labelResultSize}");
                    }
                    else
                    {
                        labelPanelStats.Text = String.Format($"Showing {labelCurrentPage * 20 + 1} - {labelCurrentPage * 20 + 20} of {labelResultSize}");
                    }
                }
            }
            if (mastersTab.IsSelected)
            {
                if (masterCurrentPage != masterResultSize / 20 - 1)
                {
                    masterCurrentPage++;
                    List<Master> output = querryProcessor.masterQuery((int)masterCurrentPage, currentmasterQuerry, Builders<BsonDocument>.Filter.Empty);
                    mastersPanel.Children.Clear();
                    masterResultSize = querryProcessor.masterQuery(currentmasterQuerry, Builders<BsonDocument>.Filter.Empty);
                    for (int i = 0; i < output.Count; i++)
                    {
                        MasterListView currentView = new MasterListView(output[i], querryProcessor.DiscogsDatabase);
                        mastersPanel.Children.Add(currentView);
                    }
                    if (masterCurrentPage == masterResultSize / 20 - 1)
                    {
                        masterPanelStats.Text = String.Format($"Showing {masterCurrentPage * 20 + 1} - {masterResultSize} of {masterResultSize}");
                    }
                    else
                    {
                        masterPanelStats.Text = String.Format($"Showing {masterCurrentPage * 20 + 1} - {masterCurrentPage * 20 + 20} of {masterResultSize}");
                    }
                }
            }
            if (releasesTab.IsSelected)
            {
                if (releaseCurrentPage != releaseResultSize / 20 - 1)
                {
                    releaseCurrentPage++;
                    List<Release> output = querryProcessor.releaseQuery((int)releaseCurrentPage, currentreleaseQuerry, Builders<BsonDocument>.Filter.Empty);
                    releasesPanel.Children.Clear();
                    releaseResultSize = querryProcessor.releaseQuery(currentreleaseQuerry, Builders<BsonDocument>.Filter.Empty);
                    for (int i = 0; i < output.Count; i++)
                    {
                        ReleaseListView currentView = new ReleaseListView(output[i], querryProcessor.DiscogsDatabase);
                        releasesPanel.Children.Add(currentView);
                    }
                    if (releaseCurrentPage == releaseResultSize / 20 - 1)
                    {
                        releasePanelStats.Text = String.Format($"Showing {releaseCurrentPage * 20 + 1} - {releaseResultSize} of {releaseResultSize}");
                    }
                    else
                    {
                        releasePanelStats.Text = String.Format($"Showing {releaseCurrentPage * 20 + 1} - {releaseCurrentPage * 20 + 20} of {releaseResultSize}");
                    }
                }
            }
        }
        /// <summary>
        /// Logic for navigating backwards.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            if (artistsTab.IsSelected)
            {
                if (artistCurrentPage != 0)
                {
                    artistCurrentPage--;
                    List<Artist> output = querryProcessor.artistQuery((int) artistCurrentPage, currentartistQuerry, Builders<BsonDocument>.Filter.Empty);
                    artistPanel.Children.Clear();
                    artistResultSize = querryProcessor.artistQuery(currentartistQuerry, Builders<BsonDocument>.Filter.Empty);
                    for (int i = 0; i < output.Count; i++)
                    {
                        ArtistListView currentView = new ArtistListView(output[i], querryProcessor.DiscogsDatabase);
                        artistPanel.Children.Add(currentView);
                    }
                    if (artistResultSize < 20)
                    {
                        artistPanelStats.Text = String.Format($"Showing {artistCurrentPage * 20 + 1} - {artistResultSize} of {artistResultSize}");
                    }
                    else
                    {
                        artistPanelStats.Text = String.Format($"Showing {artistCurrentPage * 20 + 1} - {artistCurrentPage * 20 + 20} of {artistResultSize}");
                    }
                }
            }

            if (labelsTab.IsSelected)
            {
                if (labelCurrentPage != 0)
                {
                    labelCurrentPage--;
                    List<Label> output = querryProcessor.labelQuery((int)labelCurrentPage, currentlabelQuerry, Builders<BsonDocument>.Filter.Empty);
                    labelsPanel.Children.Clear();
                    labelResultSize = querryProcessor.labelQuery(currentlabelQuerry, Builders<BsonDocument>.Filter.Empty);
                    for (int i = 0; i < output.Count; i++)
                    {
                        LabelListView currentView = new LabelListView(output[i], querryProcessor.DiscogsDatabase);
                        labelsPanel.Children.Add(currentView);
                    }
                    if (labelResultSize < 20)
                    {
                        labelPanelStats.Text = String.Format($"Showing {labelCurrentPage * 20 + 1} - {labelResultSize} of {labelResultSize}");
                    }
                    else
                    {
                        labelPanelStats.Text = String.Format($"Showing {labelCurrentPage * 20 + 1} - {labelCurrentPage * 20 + 20} of {labelResultSize}");
                    }
                }
            }

            if (mastersTab.IsSelected)
            {
                if (masterCurrentPage != 0)
                {
                    masterCurrentPage--;
                    List<Master> output = querryProcessor.masterQuery((int)masterCurrentPage, currentmasterQuerry, Builders<BsonDocument>.Filter.Empty);
                    mastersPanel.Children.Clear();
                    masterResultSize = querryProcessor.masterQuery(currentmasterQuerry, Builders<BsonDocument>.Filter.Empty);
                    for (int i = 0; i < output.Count; i++)
                    {
                        MasterListView currentView = new MasterListView(output[i], querryProcessor.DiscogsDatabase);
                        mastersPanel.Children.Add(currentView);
                    }
                    if (masterResultSize < 20)
                    {
                        masterPanelStats.Text = String.Format($"Showing {masterCurrentPage * 20 + 1} - {masterResultSize} of {masterResultSize}");
                    }
                    else
                    {
                        masterPanelStats.Text = String.Format($"Showing {masterCurrentPage * 20 + 1} - {masterCurrentPage * 20 + 20} of {masterResultSize}");
                    }
                }
            }

            if (releasesTab.IsSelected)
            {
                if (releaseCurrentPage != 0)
                {
                    releaseCurrentPage--;
                    List<Release> output = querryProcessor.releaseQuery((int)releaseCurrentPage, currentreleaseQuerry, Builders<BsonDocument>.Filter.Empty);
                    releasesPanel.Children.Clear();
                    releaseResultSize = querryProcessor.releaseQuery(currentreleaseQuerry, Builders<BsonDocument>.Filter.Empty);
                    for (int i = 0; i < output.Count; i++)
                    {
                        ReleaseListView currentView = new ReleaseListView(output[i], querryProcessor.DiscogsDatabase);
                        releasesPanel.Children.Add(currentView);
                    }
                    if (releaseResultSize < 20)
                    {
                        releasePanelStats.Text = String.Format($"Showing {releaseCurrentPage * 20 + 1} - {releaseResultSize} of {releaseResultSize}");
                    }
                    else
                    {
                        releasePanelStats.Text = String.Format($"Showing {releaseCurrentPage * 20 + 1} - {releaseCurrentPage * 20 + 20} of {releaseResultSize}");
                    }
                }
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {

        }
    }
}
