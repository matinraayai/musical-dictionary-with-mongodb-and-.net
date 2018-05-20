using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database_Design_Final_Project_Do_Not_Mess.HelperStructs;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Database_Design_Final_Project_Do_Not_Mess
{
    public class Master
    {
        private List<string> styles = new List<string>();
        private List<string> genres = new List<string>();
        private List<ExtraArtist> extraArtists = new List<ExtraArtist>();
        private string title;
        private string mainRelease;
        private string notes;
        private List<ArtistJoin> artistjoins = new List<ArtistJoin>();
        private Int32 year;
        private string anv;
        private List<string> artists = new List<string>();
        private List<Image> images = new List<Image>();
        private string LastUpdated;
        private string artist;
        private Int32 id;
        private string dataQuality;
        //Fields needed for the relation tree
        private ParentNode parentNode;
        /// <summary>
        /// Creates a master object from an input Bson Doucment.
        /// </summary>
        /// <param name="input">the input document</param>
        /// <param name="parentNode">Parent object that invoked the constructor</param>
        /// <param name="discogs">an instance of the database</param>
        public Master(BsonDocument input,ParentNode parentNode, IMongoDatabase discogs)
        {
            foreach (var element in input["styles"].AsBsonArray)
            {
                styles.Add(element.AsString);
            }
            foreach (var element in input["genres"].AsBsonArray)
            {
                genres.Add(element.AsString);
            }
            foreach (var element in input["extraartists"].AsBsonArray)
            {
                extraArtists.Add(new ExtraArtist(element.AsBsonDocument, parentNode, discogs));
            }
            title = input["title"].AsString;
            mainRelease = input["main_release"].AsString;
            string notes = input["notes"].AsString;
            foreach (var element in input["artistJoins"].AsBsonArray)
            {
                artistjoins.Add(new ArtistJoin(element.AsBsonDocument, parentNode, discogs));
            }
            try
            {
                year = input["year"].AsInt32;
            }
            catch
            {
                year = Convert.ToInt32(input["year"].AsDouble);
            }
            anv = input["anv"].AsString;
            foreach (var element in input["artists"].AsBsonArray)
            {
                artists.Add(element.AsString);
            }
            foreach (var element in input["images"].AsBsonArray)
            {
                images.Add(new Image(element.AsBsonDocument));
            }
            LastUpdated = input["updated_on"].AsString;
            artist = input["artist"].AsString;
            try
            {
                id = input["id"].AsInt32;
            }
            catch
            {
                id = Convert.ToInt32(input["id"].AsDouble);
            }
            dataQuality = input["data_quality"].AsString;
            this.parentNode = parentNode;
        }
        /// <summary>
        /// Getters and setters.
        /// </summary>
        public string Title { get => title;}
        public string LastUpdated1 { get => LastUpdated; }
        public string Artist { get => artist; }
        public List<string> Styles { get => styles; set => styles = value; }
        public List<string> Genres { get => genres; set => genres = value; }
        public List<ExtraArtist> ExtraArtists { get => extraArtists; set => extraArtists = value; }
        public string Title1 { get => title; set => title = value; }
        public string MainRelease { get => mainRelease; set => mainRelease = value; }
        public string Notes { get => notes; set => notes = value; }
        public List<ArtistJoin> Artistjoins { get => artistjoins; set => artistjoins = value; }
        public int Year { get => year; set => year = value; }
        public string Anv { get => anv; set => anv = value; }
        public List<string> Artists { get => artists; set => artists = value; }
        public List<Image> Images { get => images; set => images = value; }
        public string LastUpdated2 { get => LastUpdated; set => LastUpdated = value; }
        public string Artist1 { get => artist; set => artist = value; }
        public int Id { get => id; set => id = value; }
        public string DataQuality { get => dataQuality; set => dataQuality = value; }

        public BsonDocument toBson()
        {
            return new BsonDocument();
        }
    }
}
