using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace InstantQuizzerBackend.Models
{
    public class Question
    {
        [BsonElement("questionTitle")]
        public string QuestionTitle { get; set; }

        [BsonElement("possibleAnswers")]
        public List<string> PossibleAnswers { get; set; }

        [BsonElement("correctAnswers")]
        public List<int> CorrectAnswers { get; set; }
    }
}
