using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Design_Final_Project_Do_Not_Mess.HelperStructs
{
    public class LabelCat
    {
        string catNo;
        Label label;

        public LabelCat(BsonDocument input, ParentNode parentNode, IMongoDatabase discogs)
        {
            catNo = input["catno"].AsString;
            var filter = Builders<BsonDocument>.Filter.Eq("l_name", input["name"].AsString.ToLower());
            label = new Label(discogs.GetCollection<BsonDocument>("labels").Find(filter).First(), parentNode, false, discogs);
        }

        public string CatNo { get => catNo; }
        public Label Label { get => label;}
        override
            public string ToString()
        {
            return String.Format($"{label.Name},(Cat#:{catNo})");
        }
    }
        
}
