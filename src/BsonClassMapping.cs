using ChatApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ChatApi
{
    internal static class BsonClassMapping
    {
        public static void RegisterBsonClassMaps()
        {
            BsonClassMap.RegisterClassMap<User>(cm =>
            {
                cm.AutoMap();
                cm.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                cm.MapMember(m => m.FullName).SetElementName("fullName");
                cm.MapMember(m => m.UserName).SetElementName("userName");
            });
        }
    }
}