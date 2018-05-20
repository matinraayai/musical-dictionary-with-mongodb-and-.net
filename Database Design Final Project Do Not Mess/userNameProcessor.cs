using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Clusters;
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
namespace Database_Design_Final_Project_Do_Not_Mess
{
    public class userNameProcessor
    {
        private MongoClient mongoClient;
        private MongoCredential userCredential;
        public userNameProcessor()
        {
        }

        public bool checkConection()
        {
            var checkForConnection = new MongoClient("mongodb://localhost:27017");
            Thread.Sleep(2000);
            bool output = checkForConnection.Cluster.Description.State == ClusterState.Connected;
            return output;
        }

        public static string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException("securePassword");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        //This method checks if the entered username and password combination exists in the database.
        //It returns a Tuple as its output, which contains whether the client has successfully conntcted
        //to the database and and error logs that occured during execution.
        public Tuple<bool, string> checkProfile(string username, SecureString password)
        {
            userCredential = MongoCredential.CreateMongoCRCredential("discogs", username, password);
            var settings = new MongoClientSettings
            {
                Server = new MongoServerAddress("localhost", 27017),
                Credentials = new[] { userCredential }
            };
            try
            {
                mongoClient = new MongoClient(settings);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, ex.Message);

            };
            //Enough time to confirm that the client is connected to the database.
            Thread.Sleep(1000);
            if (mongoClient.Cluster.Description.State == ClusterState.Connected)
            {

                return new Tuple<bool, string>(true, "Successfully logged in.");
            }
            else
            {
                return new Tuple<bool, string>(false, "Failed to connect to the database. Please check your connection.");
            }

        }


        public Tuple<bool,string> insertProfile(string username, SecureString password)
        {
            if (username.IndexOf('\\') != -1 || username.IndexOf('/') != -1)
            {
                return new Tuple<bool, string>(false, "Username contains illegal character.");
            }
            var checkForConnection = new MongoClient("mongodb://localhost:27017");
            var database = checkForConnection.GetDatabase("discogs");
            try
            {
                var command = new BsonDocument { { "createUser", username }, { "pwd", ConvertToUnsecureString(password) }, { "roles", new BsonArray { new BsonDocument { { "role", "readWrite" }, { "db", "discogs" } } } } };
                database.RunCommand<BsonDocument>(command);
            }
            catch(Exception ex)
            {
                return new Tuple<bool, string>(false, "This user already exists.");
            }
            
            return new Tuple<bool,string> (true,"");
        }

        public bool deleteProfile(string username, string password)
        {
            return false;
        }

        public MongoClient getClient()
        {
            return mongoClient;
        }
    }
}
