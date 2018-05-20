using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Design_Final_Project_Do_Not_Mess.HelperStructs
{
    public class ExtraArtist
    {
        private string artist;
        private Int32 artist_id;
        private List<string> roles = new List<string>();
        private string anv;
        public ExtraArtist(BsonDocument input,ParentNode parentNode, IMongoDatabase discogs)
        {
            artist = input["artist_name"].AsString;
            try
            {
                artist_id = input["artist_id"].AsInt32;
            }
            catch (Exception ex)
            {
                artist_id = Int32.Parse(input["artist_id"].AsString);
            }
            foreach (var element in input["roles"].AsBsonArray)
            {
                try
                {
                    roles.Add(element.AsString);
                }
                catch (Exception ex)
                {
                    foreach (var smallelement in element.AsBsonArray)
                    {
                        roles.Add(smallelement.AsString);
                    }
                }
            }
            try
            {
                anv = input["anv"].AsString;
            }
            catch
            {
                anv = "";
            }
        }

        public string Artist { get => artist; set => artist = value; }
        public int Artist_id { get => artist_id; set => artist_id = value; }
        public List<string> Roles { get => roles; set => roles = value; }
        public string Anv { get => anv; set => anv = value; }

        override
        public string ToString()
        {
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < roles.Count; i++)
            {
                output.Append(roles[i]);
                if (i != roles.Count - 1)
                {
                    output.Append(", ");
                }
            }
            output.Append(" " + artist);
            output.Append(" ");
            output.Append(anv);
            return output.ToString();
        }
        public BsonDocument toBson()
        {
            BsonArray rolesArray = new BsonArray();
            for (int i = 0; i < roles.Count; i++)
            {
                rolesArray.AddRange(new BsonDocument(i.ToString(), roles[i]));
            }
            BsonDocument outputArray = new BsonDocument();
            outputArray.AddRange(new BsonDocument("roles", rolesArray));
            outputArray.AddRange(new BsonDocument("artist_id", artist_id));
            outputArray.AddRange(new BsonDocument("anv", anv));
            outputArray.AddRange(new BsonDocument("artist_name", artist));
            return outputArray;
        }
    }
}
