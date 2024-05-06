using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace InstantQuizzerBackend.Models
{
    public class Quiz
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("quizTitle")]
        public string QuizTitle { get; set; }

        [BsonElement("questions")]
        public List<Question> Questions { get; set; }

        [BsonElement("results")]
        public List<Result> Results { get; set; }
    }
}
