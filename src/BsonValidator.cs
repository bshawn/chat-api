using MongoDB.Bson;

namespace ChatApi
{
    internal static class BsonValidator
    {
        public static bool IsValidObjectId(string id)
        {
            ObjectId objId;
            return ObjectId.TryParse(id, out objId);
        }
    }
}