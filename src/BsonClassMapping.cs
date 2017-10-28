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
                cm.MapMember(m => m.FullName).SetElementName("full_name");
                cm.MapMember(m => m.UserName).SetElementName("user_name");
            });
            BsonClassMap.RegisterClassMap<Message>(cm =>
            {
                cm.AutoMap();
                cm.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                cm.MapMember(m => m.Timestamp).SetElementName("timestamp");
                cm.MapMember(m => m.SenderId).SetElementName("sender_id");
                cm.MapMember(m => m.Text).SetElementName("text");
                cm.MapMember(m => m.GifUri).SetElementName("gif_uri");
            });
        }
    }
}