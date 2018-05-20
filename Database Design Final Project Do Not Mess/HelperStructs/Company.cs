using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Design_Final_Project_Do_Not_Mess.HelperStructs
{
    public class Company
    {
        Int32 type;
        string typeName;
        Int32? catalogueNo;
        Int32 label_Id;
        string label;    
        public Company(BsonDocument input, ParentNode parentNode, IMongoDatabase discogs)
        {
            try
            {
                type = input["type"].AsInt32;
            }
            catch
            {
                type = Int32.Parse(input["type"].AsString);
            }
            
            typeName = input["type_name"].AsString;
            try
            {
                catalogueNo = input["catno"].AsInt32;
            }
            catch
            {
                try
                {
                    catalogueNo = Int32.Parse(input["catno"].AsString);
                }
                catch
                {
                    catalogueNo = null;
                }
            }
            try
            {
                label_Id = input["id"].AsInt32;
            }
            catch
            {
                label_Id = Int32.Parse(input["id"].AsString);
            }
            label = input["name"].AsString;
        }
        override
        public string ToString()
        {
            return typeName + "       " + label + "            " + catalogueNo;
        }
    }
}
