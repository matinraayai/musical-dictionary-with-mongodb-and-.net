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
    /// <summary>
    /// Represents the release entity stored in the release collection of the database.
    /// </summary>
    public class Release
    {
        
        private string status;
        private List<string> styles = new List<string>();
        private List<string> genres = new List<string>();
        private List<ArtistJoin> artistJoins = new List<ArtistJoin>();
        private List<ExtraArtist> extraArtists = new List<ExtraArtist>();
        private string barcode;
        private string title;
        private string country;
        private string notes;
        private List<LabelCat> labels = new List<LabelCat>();
        private List<Company> companies = new List<Company>();
        private string dateReleased;
        private string data_quality;
        private string l_artist;
        private List<Format> formats = new List<Format>();
        private List<Image> images = new List<Image>();
        private string LastUpdated;
        private Int32 masterId;
        private Int32 id;
        private List<Track> trackList = new List<Track>();
        /// <summary>
        /// Public constructor of the class. Turns an input Bson document into a release object.
        /// </summary>
        /// <param name="input">Bson Document</param>
        /// <param name="parentNode">Parent node invoking the constructor</param>
        /// <param name="discogs">the database instance</param>
        public Release(BsonDocument input, ParentNode parentNode, IMongoDatabase discogs)
        {
            try
            {
                //Part of the constructor that will be done no matter what.
                status = input["status"].AsString;
                ///
                foreach (var element in input["styles"].AsBsonArray)
                {
                    styles.Add(element.AsString);
                }
                ///
                foreach (var element in input["genres"].AsBsonArray)
                {
                    genres.Add(element.AsString);
                }
                ///
                foreach (var element in input["artistJoins"].AsBsonArray)
                {
                    artistJoins.Add(new ArtistJoin(element.AsBsonDocument, parentNode, discogs));
                }
                ///
                foreach (var element in input["extraartists"].AsBsonArray)
                {
                    extraArtists.Add(new ExtraArtist(element.AsBsonDocument, parentNode, discogs));
                }
                ///
                try
                {
                    barcode = input["barcode"].AsString;
                }
                catch
                {
                    barcode = "";
                }
                ///
                title = input["title"].AsString;
                ///
                country = input["country"].AsString;
                ///
                notes = input["notes"].AsString;
                ///
                foreach (var element in input["labels"].AsBsonArray)
                {
                    labels.Add(new LabelCat(element.AsBsonDocument, parentNode, discogs));
                }
                ///
                foreach (var element in input["companies"].AsBsonArray)
                {
                    companies.Add(new Company(element.AsBsonDocument, parentNode, discogs));
                }
                ///
                dateReleased = input["released"].AsString;
                ///
                data_quality = input["data_quality"].AsString;
                ///
                try
                {
                    l_artist = input["l_artist"].AsString;
                }
                catch
                {
                    l_artist = "";
                }
                ///
                foreach (var element in input["formats"].AsBsonArray)
                {
                    formats.Add(new Format(element.AsBsonDocument));
                }
                ///
                foreach (var element in input["images"].AsBsonArray)
                {
                    images.Add(new Image(element.AsBsonDocument));
                }
                ///
                LastUpdated = input["updated_on"].AsString;
                ///
                try
                {
                    masterId = input["master_id"].AsInt32;
                }
                catch
                {
                    masterId = Convert.ToInt32(input["master_id"].AsDouble);
                }
                ///
                country = input["country"].AsString;
                try
                {
                    id = input["id"].AsInt32;
                }
                catch
                {
                    id = Convert.ToInt32(input["id"].AsDouble);
                }
                ///
                foreach (var element in input["tracklist"].AsBsonArray)
                {
                    trackList.Add(new Track(element.AsBsonDocument, parentNode, discogs));
                }
            }
            catch
            {
                throw new Exception("Invalid input file.");
            }
        }

        /// <summary>
        /// Getters and Setters.
        /// </summary>
        public string Status { get => status; set => status = value; }
        public List<string> Styles { get => styles; set => styles = value; }
        public List<string> Genres { get => genres; set => genres = value; }
        public List<ArtistJoin> ArtistJoins { get => artistJoins; set => artistJoins = value; }
        public List<ExtraArtist> ExtraArtists { get => extraArtists; set => extraArtists = value; }
        public string Barcode { get => barcode; set => barcode = value; }
        public string Title { get => title; set => title = value; }
        public string Country { get => country; set => country = value; }
        public string Notes { get => notes; set => notes = value; }
        public List<LabelCat> Labels { get => labels; set => labels = value; }
        public List<Company> Companies { get => companies; set => companies = value; }
        public string DateReleased { get => dateReleased; set => dateReleased = value; }
        public string Data_quality { get => data_quality; set => data_quality = value; }
        public string L_artist { get => l_artist; set => l_artist = value; }
        public List<Format> Formats { get => formats; set => formats = value; }
        public List<Image> Images { get => images; set => images = value; }
        public string LastUpdated1 { get => LastUpdated; set => LastUpdated = value; }
        public int MasterId { get => masterId; set => masterId = value; }
        public int Id { get => id; set => id = value; }
        public List<Track> TrackList { get => trackList; set => trackList = value; }

        /// <summary>
        /// An unimplemented method for returning the bson presentation of this element.
        /// This method is intended for writing into the database.
        /// </summary>
        /// <returns>a blank Bson document.</returns>
        public BsonDocument toBson()
        {
            return new BsonDocument();
        }
    }
}
