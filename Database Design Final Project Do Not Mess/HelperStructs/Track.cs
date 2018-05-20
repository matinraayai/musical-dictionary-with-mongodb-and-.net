using Database_Design_Final_Project_Do_Not_Mess.Views;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Design_Final_Project_Do_Not_Mess.HelperStructs
{
    /// <summary>
    /// Represents a track object that is saved by a release object.
    /// </summary>
    public class Track
    {
        string position;
        string duration;
        List<ArtistJoin> artistJoins = new List<ArtistJoin>();
        List<ExtraArtist> extraArtists = new List<ExtraArtist>();
        string title;

        /// <summary>
        /// Public constructor of a track object. As far as checked, the type of the objects saved in the Bson
        /// documnet was consistant.
        /// </summary>
        /// <param name="input">Bson Document containing the track object.</param>
        /// <param name="parentNode"></param>
        /// <param name="discogs">the discogs database passed by the parent object.</param>
        public Track(BsonDocument input, ParentNode parentNode,  IMongoDatabase discogs)
        {
            try
            {
                duration = input["duration"].AsString;
                position = input["position"].AsString;
                title = input["title"].AsString;
                /////
                foreach (var element in input["artistJoins"].AsBsonArray)
                {
                    artistJoins.Add(new ArtistJoin(element.AsBsonDocument, parentNode, discogs));
                }
                /////
                foreach (var element in input["extraartists"].AsBsonArray)
                {
                    extraArtists.Add(new ExtraArtist(element.AsBsonDocument, parentNode, discogs));
                }
            }
            catch
            {
                throw new Exception("Invalid bson document input.");
            }
        }
        /// <summary>
        /// Returns the track object data as a string.
        /// </summary>
        /// <returns>the string to be displayed by master and release objects.</returns>
        override
        public string ToString()
        {
            if (artistJoins.Count == 0 && extraArtists.Count == 0)
            {
                return String.Format($"Position: {position}, Title: {title}, Duration: {duration}\n");
            }
            else
            {
                return String.Format($"Position: {position}, Title: {title}, Duration: {duration}\nCredits:" +
                    $" {helperMethods.listOutput<ArtistJoin>("", artistJoins)}\n{helperMethods.listOutput<ExtraArtist>("", extraArtists)}");
            }
            
        }
    }
}
