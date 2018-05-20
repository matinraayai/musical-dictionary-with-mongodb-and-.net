using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Database_Design_Final_Project_Do_Not_Mess
{
    /// <summary>
    /// This object was supposed to be the model of the MVC model, but as the
    /// project went on, this object became less important.
    /// </summary>
    public class QuerryProcessor
    {
        //User connection configuration
        MongoClient userClient;
        //Main database instance
        IMongoDatabase discogsDatabase;
        //List of collections
        IMongoCollection<BsonDocument> artists;
        IMongoCollection<BsonDocument> releases;
        IMongoCollection<BsonDocument> masters;
        IMongoCollection<BsonDocument> labels;
        //Statistics numbers related to each collection
        long numArtists;
        long numReleases;
        long numMasters;
        long numLabels;

        /// <summary>
        /// Getters.
        /// </summary>
        public IMongoDatabase DiscogsDatabase { get => discogsDatabase; }
        public long NumArtists { get => numArtists; }
        public long NumReleases { get => numReleases; }
        public long NumMasters { get => numMasters;}
        public long NumLabels { get => numLabels;}


        public QuerryProcessor(MongoClient userClient)
        {
            this.userClient = userClient;
            discogsDatabase = userClient.GetDatabase("discogs_copy");
            artists = discogsDatabase.GetCollection<BsonDocument>("artists");
            releases = discogsDatabase.GetCollection<BsonDocument>("releases");
            masters = discogsDatabase.GetCollection<BsonDocument>("masters");
            labels = discogsDatabase.GetCollection<BsonDocument>("labels");
            numArtists = artists.Count(new BsonDocument());
            numReleases = releases.Count(new BsonDocument());
            numMasters = masters.Count(new BsonDocument());
            numLabels = labels.Count(new BsonDocument());
        }

        //Artist Collection methods for CRUD Operations

        /// <summary>
        /// Implements the search function for an artist.
        /// </summary>
        /// <param name="skip">page number</param>
        /// <param name="searchQuerry">the string to be searched in the database</param>
        /// <param name="filter">Any kind of tags that was set by the user.</param>
        /// <returns></returns>
        public List<Artist> artistQuery(int skip, string searchQuerry, FilterDefinition<BsonDocument> filter)
        {
            var stringFilter = Builders<BsonDocument>.Filter.Regex("l_name", new BsonRegularExpression(searchQuerry.ToLower()));
            var searchFilter = filter & stringFilter;
            using (var cursor = artists.Find(searchFilter).Skip(skip * 20).Limit(20).ToCursor())
            {
                List<Artist> output = new List<Artist>();
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        output.Add(new Artist(doc,HelperStructs.ParentNode.Artist, true, discogsDatabase));
                    }
                }
                return output;
            } 
        }

        /// <summary>
        /// Returns the size of the search querry
        /// </summary>
        /// <param name="searchQuerry">search string</param>
        /// <param name="filter">filter passed on by the user</param>
        /// <returns></returns>
        public long artistQuery(string searchQuerry, FilterDefinition<BsonDocument> filter)
        {
            var stringFilter = Builders<BsonDocument>.Filter.Regex("l_name", new BsonRegularExpression(searchQuerry.ToLower()));
            var searchFilter = filter & stringFilter;
            return artists.Find(searchFilter).Count();
        }





        /// <summary>
        /// Finds 20 artists from the artists collections and skipping a number of items.
        /// </summary>
        /// <param name="skip">number of artists wanted to be skipped, multiplied by 20</param>
        /// <returns>List of the desired Artists</returns>
        public List<Artist> artistRead(int skip)
        {
            using (var cursor = artists.Find(new BsonDocument()).Skip(20 * skip).Limit(20).ToCursor())
            {
                List<Artist> output = new List<Artist>();
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        output.Add(new Artist(doc, HelperStructs.ParentNode.Artist, true, discogsDatabase));
                    }
                }
                return output;
            }
        }

        /// <summary>
        /// Enables the user to create a new artist object in the database.
        /// </summary>
        /// <param name="artist">the artist object to be inserted into the database</param>
        /// <returns>true if operation succeeds</returns>
        public bool artistCreate(Artist artist)
        {
            try
            {
                var sort = Builders<BsonDocument>.Sort.Descending("id");
                var cursor = artists.Find(Builders<BsonDocument>.Filter.Empty).Sort(sort).First();
                artist.Id = cursor["id"].AsInt32 + 1;
                artists.InsertOne(artist.toBson());
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// Updates an artist object in the database based on its ID. The ID cannot be changed.
        /// </summary>
        /// <param name="artist">The artist object to be updated in the database.</param>
        /// <returns>true if operation succeeds.</returns>
        public bool artistUpdate(Artist artist)
        {
            try
            {
                artists.UpdateOne(Builders<BsonDocument>.Filter.Eq("id", artist.Id), artist.toBson());
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// This method removes an artist based on its id. However, it won't delete any other objects in
        /// other collections related to the deleted object.
        /// </summary>
        /// <param name="artist">The artist object wanted to be deleted.</param>
        /// <returns>true if the operation is a success.</returns>
        public bool artistDelete(Artist artist)
        {
            try
            {
                artists.DeleteOne(Builders<BsonDocument>.Filter.Eq("id", artist.Id));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Labels Collection Methods for CRUD Operations

        /// <summary>
        /// Method for finding a particular label
        /// </summary>
        /// <param name="skip">page number</param>
        /// <param name="searchQuerry">search string</param>
        /// <param name="filter">Any filter passed on by the user.</param>
        /// <returns></returns>
        public List<Label> labelQuery(int skip, string searchQuerry, FilterDefinition<BsonDocument> filter)
        {
            var stringFilter = Builders<BsonDocument>.Filter.Regex("l_name", new BsonRegularExpression(searchQuerry.ToLower()));
            var searchFilter = filter & stringFilter;
            using (var cursor = labels.Find(searchFilter).Skip(skip * 20).Limit(20).ToCursor())
            {
                List<Label> output = new List<Label>();
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        output.Add(new Label(doc, HelperStructs.ParentNode.Label, true, discogsDatabase));
                    }
                }
                return output;
            }
        }



        /// <summary>
        /// Returns the size of the search querry
        /// </summary>
        /// <param name="searchQuerry">search string</param>
        /// <param name="filter">filter passed on by the user</param>
        /// <returns></returns>
        public long labelQuery(string searchQuerry, FilterDefinition<BsonDocument> filter)
        {
            var stringFilter = Builders<BsonDocument>.Filter.Regex("l_name", new BsonRegularExpression(searchQuerry.ToLower()));
            var searchFilter = filter & stringFilter;
            return labels.Find(searchFilter).Count();
        }
        /// <summary>
        /// Method for finding 20 labels out of the whole labels collection.
        /// </summary>
        /// <param name="skip">number of labels wanted to be skipped, multiplied by 20</param>
        /// <returns>List of the desired labels</returns>
        public List<Label> labelRead(int skip)
        {
            using (var cursor = labels.Find(new BsonDocument()).Skip(20 * skip).Limit(20).ToCursor())
            {
                List<Label> output = new List<Label>();
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        output.Add(new Label(doc, HelperStructs.ParentNode.Label, true, discogsDatabase));
                    }
                }
                return output;
            }
        }

        /// <summary>
        /// This method enables the user to create a new label object in the database.
        /// </summary>
        /// <param name="label">the label object to be inserted into the database</param>
        /// <returns>true if operation succeeds</returns>
        public bool labelCreate(Label label)
        {
            try
            {
                var sort = Builders<BsonDocument>.Sort.Descending("id");
                var cursor = labels.Find(Builders<BsonDocument>.Filter.Empty).Sort(sort).First();
                label.Id = cursor["id"].AsInt32 + 1;
                labels.InsertOne(label.toBson());
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Updates an label object in the database based on its ID. The ID cannot be changed.
        /// </summary>
        /// <param name="label">The label object to be updated in the database.</param>
        /// <returns>true if operation succeeds.</returns>
        public bool labelUpdate(Label label)
        {
            try
            {
                labels.UpdateOne(Builders<BsonDocument>.Filter.Eq("id", label.Id), label.toBson());
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// This method removes an label based on its id. However, it won't delete any other objects in
        /// other collections related to the deleted object.
        /// </summary>
        /// <param name="label">The label object wanted to be deleted.</param>
        /// <returns>true if the operation is a success.</returns>
        public bool LabelDelete(Label label)
        {
            try
            {
                labels.DeleteOne(Builders<BsonDocument>.Filter.Eq("id", label.Id));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        //Masters Collection Methods for CRUD Operations

        public List<Master> masterQuery(int skip, string searchQuerry, FilterDefinition<BsonDocument> filter)
        {
            var stringFilter = Builders<BsonDocument>.Filter.Regex("l_title", new BsonRegularExpression(searchQuerry.ToLower()));
            var searchFilter = filter & stringFilter;
            using (var cursor = masters.Find(searchFilter).Skip(skip * 20).Limit(20).ToCursor())
            {
                List<Master> output = new List<Master>();
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        output.Add(new Master(doc, HelperStructs.ParentNode.Master, discogsDatabase));
                    }
                }
                return output;
            }
        }




        /// <summary>
        /// Returns the size of the search querry
        /// </summary>
        /// <param name="searchQuerry">search string</param>
        /// <param name="filter">filter passed on by the user</param>
        /// <returns></returns>
        public long masterQuery(string searchQuerry, FilterDefinition<BsonDocument> filter)
        {
            var stringFilter = Builders<BsonDocument>.Filter.Regex("l_title", new BsonRegularExpression(searchQuerry.ToLower()));
            var searchFilter = filter & stringFilter;
            return masters.Find(searchFilter).Count();
        }
        /// <summary>
        /// Finds 20 masters from the masters collections and skipping a number of items.
        /// </summary>
        /// <param name="skip">number of masters wanted to be skipped, multiplied by 20</param>
        /// <returns>List of the desired masters</returns>
        public List<Master> masterRead(int skip)
        {
            using (var cursor = masters.Find(new BsonDocument()).Skip(20 * skip).Limit(20).ToCursor())
            {
                List<Master> output = new List<Master>();
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        output.Add(new Master(doc, HelperStructs.ParentNode.Master, discogsDatabase));
                    }
                }
                return output;
            }
        }

        /// <summary>
        /// Enables the user to create a new master object in the database.
        /// </summary>
        /// <param name="master">the master object to be inserted into the database</param>
        /// <returns>true if operation succeeds</returns>
        public bool masterCreate(Master master)
        {
            try
            {
                var sort = Builders<BsonDocument>.Sort.Descending("id");
                var cursor = masters.Find(Builders<BsonDocument>.Filter.Empty).Sort(sort).First();
                master.Id = cursor["id"].AsInt32 + 1;
                masters.InsertOne(master.toBson());
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Updates an master object in the database based on its ID. The ID cannot be changed.
        /// </summary>
        /// <param name="master">The master object to be updated in the database.</param>
        /// <returns>true if operation succeeds.</returns>
        public bool masterUpdate(Master master)
        {
            try
            {
                masters.UpdateOne(Builders<BsonDocument>.Filter.Eq("id", master.Id), master.toBson());
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// This method removes an master based on its id. However, it won't delete any other objects in
        /// other collections related to the deleted object.
        /// </summary>
        /// <param name="master">The master object wanted to be deleted.</param>
        /// <returns>true if the operation is a success.</returns>
        public bool masterDelete(Master master)
        {
            try
            {
                masters.DeleteOne(Builders<BsonDocument>.Filter.Eq("id", master.Id));
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Releases Collection Methods for CRUD Operations

        public List<Release> releaseQuery(int skip, string searchQuerry, FilterDefinition<BsonDocument> filter)
        {
            var stringFilter = Builders<BsonDocument>.Filter.Regex("l_title", new BsonRegularExpression(searchQuerry.ToLower()));
            var searchFilter = filter & stringFilter;
            using (var cursor = releases.Find(searchFilter).Skip(skip * 20).Limit(20).ToCursor())
            {
                List<Release> output = new List<Release>();
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        output.Add(new Release(doc, HelperStructs.ParentNode.Release, discogsDatabase));
                    }
                }
                return output;
            }
        }

        /// <summary>
        /// Returns the size of the search querry
        /// </summary>
        /// <param name="searchQuerry">search string</param>
        /// <param name="filter">filter passed on by the user</param>
        /// <returns></returns>
        public long releaseQuery(string searchQuerry, FilterDefinition<BsonDocument> filter)
        {
            var stringFilter = Builders<BsonDocument>.Filter.Regex("l_title", new BsonRegularExpression(searchQuerry.ToLower()));
            var searchFilter = filter & stringFilter;
            return releases.Find(searchFilter).Count();
        }
        /// <summary>
        /// Finds 20 releases from the releases collections and skipping a number of items.
        /// </summary>
        /// <param name="skip">number of releases wanted to be skipped, multiplied by 20</param>
        /// <returns>List of the desired releases</returns>
        public List<Release> releaseRead(int skip)
        {
            using (var cursor = releases.Find(new BsonDocument()).Skip(20 * skip).Limit(20).ToCursor())
            {
                List<Release> output = new List<Release>();
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        output.Add(new Release(doc, HelperStructs.ParentNode.Release, discogsDatabase));
                    }
                }
                return output;
            }
        }

        /// <summary>
        /// Enables the user to create a new release object in the database.
        /// </summary>
        /// <param name="release">the release object to be inserted into the database</param>
        /// <returns>true if operation succeeds</returns>
        public bool releaseCreate(Release release)
        {
            try
            {
                var sort = Builders<BsonDocument>.Sort.Descending("id");
                var cursor = releases.Find(Builders<BsonDocument>.Filter.Empty).Sort(sort).First();
                release.Id = cursor["id"].AsInt32 + 1;
                releases.InsertOne(release.toBson());
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Updates an release object in the database based on its ID. The ID cannot be changed.
        /// </summary>
        /// <param name="release">The release object to be updated in the database.</param>
        /// <returns>true if operation succeeds.</returns>
        public bool releaseUpdate(Release release)
        {
            try
            {
                releases.UpdateOne(Builders<BsonDocument>.Filter.Eq("id", release.Id), release.toBson());
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// This method removes an release based on its id. However, it won't delete any other objects in
        /// other collections related to the deleted object.
        /// </summary>
        /// <param name="release">The release object wanted to be deleted.</param>
        /// <returns>true if the operation is a success.</returns>
        public bool releaseDelete(Release release)
        {
            try
            {
                releases.DeleteOne(Builders<BsonDocument>.Filter.Eq("id", release.Id));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
