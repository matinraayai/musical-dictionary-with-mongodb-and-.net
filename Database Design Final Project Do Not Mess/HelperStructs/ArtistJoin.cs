using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Design_Final_Project_Do_Not_Mess.HelperStructs

{
    public class ArtistJoin
    {
        private Int32 artist_id;
        private string artist;
        private string relation;
        private string anv;

        public ArtistJoin(BsonDocument input, ParentNode parentNode, IMongoDatabase discogs)
        {
            try
            {
                artist_id = input["artist_id"].AsInt32;
            }
            catch {
                try
                {
                   artist_id = Int32.Parse(input["artist_id"].AsString);
                }
                catch
                {
                   artist_id = Convert.ToInt32(input["artist_id"].AsDouble);
                }
            }
            artist = input["artist_name"].AsString;
            try
            {
                relation = input["join_relation"].AsString;
            }
            catch
            {
                relation = "";
            }
            if (!input["anv"].IsBsonNull)
            {
                anv = input["anv"].AsString;
            }
            else
            {
                anv = "";
            }
        }
        /// <summary>
        /// Properties used to get information out of the object.
        /// </summary>
        public string Artist { get => artist;}
        public string Relation { get => relation;}
        public string Anv { get => anv; }

        override
        public string ToString()
        {
            return artist + relation + "  " + anv;
        }
        public BsonDocument toBson()
        {
            BsonDocument outputArray = new BsonDocument();
            outputArray.AddRange(new BsonDocument("artist_id", artist_id));
            outputArray.AddRange(new BsonDocument("join_relation", relation));
            outputArray.AddRange(new BsonDocument("anv", anv));
            outputArray.AddRange(new BsonDocument("artist_name", artist));
            return outputArray;
        }
    }
}
