using System;
using ChatApi.Models;
using MongoDB.Driver;

namespace ChatApi
{
    internal static class CollectionManager
    {
        private static string connString = Environment.GetEnvironmentVariable("MONGODB_CONN_STR");

        public static IMongoCollection<User> GetUserCollection()
        {
            var client = new MongoClient(connString);
            var db = client.GetDatabase("chat-demo");
            var collection = db.GetCollection<User>("users");
            return collection;
        }

        public static IMongoCollection<Message> GetMessageCollection()
        {
            var client = new MongoClient(connString);
            var db = client.GetDatabase("chat-demo");
            var collection = db.GetCollection<Message>("messages");
            return collection;
        }
    }
}