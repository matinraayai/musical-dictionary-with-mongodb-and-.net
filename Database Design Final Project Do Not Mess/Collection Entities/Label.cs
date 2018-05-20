using Database_Design_Final_Project_Do_Not_Mess.HelperStructs;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Design_Final_Project_Do_Not_Mess
{
    /// <summary>
    /// Represents a label object in the label collection.
    /// </summary>
    public class Label
    {
        private string profile;
        private string parentLabel;
        private string name;
        private List<string> subLabels = new List<string>();
        private List<Uri> urls = new List<Uri>();
        private string lastUpdated;
        private string contactInfo;
        private Int32 id;
        private string dataQuality;
        private ParentNode parentNode;
        private bool isMain;

        /// <summary>
        /// creates a new label object from an input Bson Document.
        /// </summary>
        /// <param name="input">the input document</param>
        /// <param name="parentNode">The parent entity invoking the constructor.</param>
        /// <param name="isMain">Is this object the main interest of the invocation</param>
        /// <param name="discogs">the database instance</param>
        public Label(BsonDocument input, ParentNode parentNode, bool isMain, IMongoDatabase discogs)
        {
            ///
            profile = input["profile"].AsString;
            ///
            parentLabel = input["parentLabel"].AsString;
            ///
            name = input["name"].AsString;
            ///
            foreach (var element in input["sublabels"].AsBsonArray)
            {
                    subLabels.Add(element.AsString);
            }
            ///
            foreach (var element in input["urls"].AsBsonArray)
            {
                try
                {
                    urls.Add(new Uri(element.AsString));
                }
                catch
                {
                    urls.Add(new Uri("http://" + element.AsString));
                }
            }
            ///
            lastUpdated = input["updated_on"].AsString;
            ///
            contactInfo = input["contactinfo"].AsString;
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
            this.isMain = isMain;
        }

        /// <summary>
        /// Getters and setters of the class.
        /// </summary>
        public string Profile { get => profile; set => profile = value; }
        public string ParentLabel { get => parentLabel; set => parentLabel = value; }
        public string Name { get => name; set => name = value; }
        public List<string> SubLabels { get => subLabels; set => subLabels = value; }
        public List<Uri> Urls { get => urls; set => urls = value; }
        public string LastUpdated { get => lastUpdated; set => lastUpdated = value; }
        public string ContactInfo { get => contactInfo; set => contactInfo = value; }
        public int Id { get => id; set => id = value; }
        public string DataQuality { get => dataQuality; set => dataQuality = value; }
        public ParentNode ParentNode { get => parentNode; set => parentNode = value; }
        public bool IsMain { get => isMain; set => isMain = value; }

        /// <summary>
        /// A method that returns the bson document representation of the object. Will
        /// be implemented in the future.
        /// </summary>
        /// <returns>A new bson document</returns>
        public BsonDocument toBson()
        {
            return new BsonDocument();
        }
    }
}
