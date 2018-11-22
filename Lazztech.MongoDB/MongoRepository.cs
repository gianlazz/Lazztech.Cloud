using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HackathonManager.RepositoryPattern;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace HackathonManager.MongoDB
{
    public class MongoRepository : IRepository
    {
        private readonly IMongoDatabase _db;

        /// <summary>
        /// Gets the HackathonManager Database from the localhost
        /// </summary>
        public MongoRepository()
        {
            var client = new MongoClient();
            _db = client.GetDatabase("hackathonmanager");

            var pack = new ConventionPack();
            pack.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register("My Solution Conventions", pack, t => true);
        }

        /// <summary>
        /// Gets the HackathonManager Database from the supplied connection string
        /// </summary>
        /// <param name="connectionString"></param>
        public MongoRepository(string connectionString)
        {
            var client = new MongoClient(connectionString);
            _db = client.GetDatabase("hackathonmanager");

            var pack = new ConventionPack();
            pack.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register("My Solution Conventions", pack, t => true);
        }

        public void Add<T>(T item) where T : class, new()
        {
            _db.GetCollection<T>(typeof(T).Name).InsertOne(item);
        }

        public void Add<T>(IEnumerable<T> items) where T : class, new()
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        public IQueryable<T> All<T>() where T : class, new()
        {
            return _db.GetCollection<T>(typeof(T).Name).AsQueryable();
        }

        public void Delete<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            var items = All<T>().Where(expression).ToList();
            foreach (T item in items)
            {
                Delete(item);
            }
        }

        public void Delete<T>(T item) where T : class, new()
        {
            //WorkAround for DeleteOne parameter
            ObjectFilterDefinition<T> filter = new ObjectFilterDefinition<T>(item);
            // Remove the object.
            _db.GetCollection<T>(typeof(T).Name).FindOneAndDelete(filter);
            //// Remove the object.
            //_db.GetCollection<T>(typeof(T).Name).DeleteOneAsync(typeof(T).Name);
            //_db.GetCollection<T>(typeof(T).Name).FindOneAndDelete((typeof(T).Name) => )
        }

        public void DeleteAll<T>() where T : class, new()
        {
            _db.DropCollection(typeof(T).Name);
        }

        public T Single<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            //return All<T>().Where(expression).SingleOrDefault();
            //return All<T>().Where(expression).Single();
            return All<T>().Where(expression).FirstOrDefault();
        }
    }
}
