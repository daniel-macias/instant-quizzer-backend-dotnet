using MongoDB.Bson.Serialization.Attributes;

namespace InstantQuizzerBackend.Models
{
    public class Result
    {
        [BsonElement("personName")]
        public string PersonName { get; set; }

        [BsonElement("responses")]
        public List<bool> Responses { get; set; }
    }
}
