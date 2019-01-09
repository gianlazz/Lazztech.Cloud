using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using MongoDB.Driver;
using System;

namespace Lazztech.MongoDB
{
    internal class MongoDBLogger : ILogger
    {
        private IMongoDatabase _db;

        public MongoDBLogger(IMongoDatabase mongoDatabase)
        {
            _db = mongoDatabase;
        }

        public void Log(Exception exception)
        {
            _db.GetCollection<Log>(typeof(Log).Name).InsertOne(new Log() { Details = exception.ToString() });
        }

        public void Log(string s)
        {
            _db.GetCollection<Log>(typeof(Log).Name).InsertOne(new Log() { Details = s });
        }
    }
}