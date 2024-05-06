using MongoDB.Bson.Serialization.Attributes;

namespace InstantQuizzerBackend.Models
{
    public class Result
    {
        [BsonElement("personName")]
        public string PersonName { get; set; }

        [BsonElement("percentageEarned")]
        public int PercentageEarned { get; set; }
    }
}
