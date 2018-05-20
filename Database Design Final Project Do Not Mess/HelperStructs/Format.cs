using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Design_Final_Project_Do_Not_Mess.HelperStructs
{
    public class Format
    {
        string text;
        List<string> descriptions = new List<string>();
        string name;
        Int32 quantity;

        public Format(BsonDocument input)
        {
            text = input["text"].AsString;
            foreach (var element in input["descriptions"].AsBsonArray)
            {
                descriptions.Add(element.AsString);
            }
            name = input["name"].AsString;
            try
            {
                quantity = input["qty"].AsInt32;
            }
            catch
            {
                quantity = Int32.Parse(input["qty"].AsString);
            }
        }

        public string Text { get => text; set => text = value; }
        public List<string> Descriptions { get => descriptions; set => descriptions = value; }
        public string Name { get => name; set => name = value; }
        public int Quantity { get => quantity; set => quantity = value; }

        override
        public string ToString()
        {
            StringBuilder temp = new StringBuilder();
            temp.Append(text);
            temp.Append("   ");
            for (int i = 0; i < descriptions.Count; i++)
            {
                temp.Append(descriptions[i]);
                if (i != descriptions.Count - 1)
                {
                    temp.Append(", ");
                }
            }
            temp.Append(",qty: ");
            temp.Append(quantity);
            return temp.ToString();
        }
        public BsonDocument toBson()
        {
            BsonDocument outputArray = new BsonDocument();
            outputArray.AddRange(new BsonDocument("text", this.text));

            BsonArray descriptionsArray = new BsonArray();
            for (int i = 0; i < descriptions.Count; i++)
            {
              descriptionsArray.AddRange(new BsonDocument(i.ToString(), descriptions[i]));
            }
            outputArray.AddRange(new BsonDocument("descriptions", descriptionsArray));
            outputArray.AddRange(new BsonDocument("name", name));
            outputArray.AddRange(new BsonDocument("qty", quantity));
            return outputArray;
        }
    }
}
