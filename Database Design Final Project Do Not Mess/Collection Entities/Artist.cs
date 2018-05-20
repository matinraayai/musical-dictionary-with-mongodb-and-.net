using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database_Design_Final_Project_Do_Not_Mess.HelperStructs;
using MongoDB.Driver;

namespace Database_Design_Final_Project_Do_Not_Mess
{
    //This class represents an Artist object. It implements the discogsElement interface.
    public class Artist
    {
        //Attributes that needs to be saved for object integrity
        private string profile;
        private List<Uri> urls = new List<Uri>();
        private List<string> nameVariations = new List<string>();     
        private string name;
        private List<string> aliases = new List<string>();
        private List<string> members = new List<string>();
        private string lastUpdated;
        private List<string> groups = new List<string>();
        private Int32 id;
        private string dataQuality;
        private string realName;
        private List<Image> images = new List<Image>();
        //Attribute to check whether this artist is the main artist of interest.
        private bool isMainArtist;
        private ParentNode parentNode;




        //Constructs an artist object out of a single BsonDocument
        public Artist(BsonDocument input, ParentNode parentNode, bool isMain, IMongoDatabase discogs)
        {
            //Part of the constructor that will be done no matter what.
            profile = input["profile"].AsString;
            ///
            foreach (var element in input["urls"].AsBsonArray)
            {
                urls.Add(new Uri(element.AsString));
            }
            ///
            foreach (var element in input["namevariations"].AsBsonArray)
            {
                nameVariations.Add(element.AsString);
            }
            ///
            name = input["name"].AsString;
            ///
            foreach (var element in input["aliases"].AsBsonArray)
            {
                aliases.Add(element.AsString);
            }
            ///
            foreach (var element in input["members"].AsBsonArray)
            {
                members.Add(element.AsString);
            }
            ///
            foreach (var element in input["images"].AsBsonArray)
            {
                images.Add(new Image(element.AsBsonDocument));
            }
            ///
            lastUpdated = input["updated_on"].AsString;
            ///
            foreach (var element in input["groups"].AsBsonArray)
            {
                groups.Add(element.AsString);
            }
            ///
            try
            {
                id = input["id"].AsInt32;
            }
            catch
            {
                id = Convert.ToInt32(input["id"].AsDouble);
            }
            ///
            dataQuality = input["data_quality"].AsString;
            ///
            realName = input["realname"].AsString;
            ///
            //Part of the constructor for saving relations
            this.isMainArtist = isMain;
            this.parentNode = parentNode;
            
        }
        /// <summary>
        /// Getters and Setters
        /// </summary>
        public int Id { get => id; set => id = value; }
        public string Name { get => name; }
        public string Profile { get => profile; }
        public List<Uri> Urls { get => urls;}
        public List<string> NameVariations { get => nameVariations; }
        public string Name1 { get => name;}
        public List<string> Aliases { get => aliases; }
        public List<string> Members { get => members;}
        public string LastUpdated { get => lastUpdated; }
        public List<string> Groups { get => groups; }
        public string DataQuality { get => dataQuality;}
        public string RealName { get => realName;}
        public List<Image> Images { get => images;}
        public bool IsMainArtist { get => isMainArtist; }
        public ParentNode ParentNode { get => parentNode; }
        /// <summary>
        /// Returns a Bson document version of the object.
        /// </summary>
        /// <returns></returns>
        public BsonDocument toBson()
        {
            BsonDocument outputArray = new BsonDocument();
            outputArray.AddRange(new BsonDocument("profile", profile));
            BsonArray urlsArray = new BsonArray();
            for (int i = 0; i < urls.Count; i++)
            {
                urlsArray.AddRange(new BsonDocument(i.ToString(), urls[i].ToString()));
            }
            outputArray.AddRange(new BsonDocument("urls", urlsArray));
            BsonArray namevariationsArray = new BsonArray();
            for (int i = 0; i < urls.Count; i++)
            {
                namevariationsArray.AddRange(new BsonDocument(i.ToString(), nameVariations[i]));
            }
            outputArray.AddRange(new BsonDocument("namevariations", namevariationsArray));
            outputArray.AddRange(new BsonDocument("l_name", name.ToLower()));
            outputArray.AddRange(new BsonDocument("name", name));
            BsonArray aliasesArray = new BsonArray();
            for (int i = 0; i < urls.Count; i++)
            {
                aliasesArray.AddRange(new BsonDocument(i.ToString(), aliases[i]));
            }
            outputArray.AddRange(new BsonDocument("aliases", aliasesArray));
            BsonArray membersArray = new BsonArray();
            for (int i = 0; i < urls.Count; i++)
            {
                membersArray.AddRange(new BsonDocument(i.ToString(), members[i]));
            }
            outputArray.AddRange(new BsonDocument("members", membersArray));
            BsonArray imagesArray = new BsonArray();
            for (int i = 0; i < urls.Count; i++)
            {
                imagesArray.AddRange(new BsonDocument(i.ToString(), this.images[i].toBson()));
            }
            outputArray.AddRange(new BsonDocument("images", imagesArray));
            outputArray.AddRange(new BsonDocument("updated_on", lastUpdated));
            BsonArray groupsArray = new BsonArray();
            for (int i = 0; i < urls.Count; i++)
            {
                imagesArray.AddRange(new BsonDocument(i.ToString(), groups[i]));
            }
            outputArray.AddRange(new BsonDocument("groups", groupsArray));
            outputArray.AddRange(new BsonDocument("id", id));
            outputArray.AddRange(new BsonDocument("data_quality", dataQuality));
            outputArray.AddRange(new BsonDocument("realname", realName));
            return outputArray;
        }
        /// <summary>
        /// Provides a string representation of an object.
        /// </summary>
        /// <returns>the string containing info about the object.</returns>
        override
        public string ToString()
        {
            return String.Format($"{profile}, {urls}, {nameVariations},{name}, {aliases}, {members}, {lastUpdated}, {groups}, {id}, {dataQuality}, {realName}");
        }
        
    }
}
