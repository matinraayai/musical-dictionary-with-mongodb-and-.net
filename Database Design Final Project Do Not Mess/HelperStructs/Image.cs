using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Design_Final_Project_Do_Not_Mess.HelperStructs
{
    public class Image
    {
        string width;
        string uri150;
        string uri;
        string imageType;
        string height;

        public Image(BsonDocument input)
        {   
            width = input["width"].AsString;
            uri150 = input["uri150"].AsString;
            uri = input["uri"].AsString;
            imageType = input["imageType"].AsString;
            height = input["height"].AsString;
        }
        public BsonDocument toBson()
        {
            BsonDocument output = new BsonDocument();
            output.AddRange(new BsonDocument("width", width.ToString()));
            output.AddRange(new BsonDocument("uri150", uri150.ToString()));
            output.AddRange(new BsonDocument("uri", uri.ToString()));
            output.AddRange(new BsonDocument("ImageType", imageType.ToString()));
            output.AddRange(new BsonDocument("height", height.ToString()));
            return output;
        }
    }
}
