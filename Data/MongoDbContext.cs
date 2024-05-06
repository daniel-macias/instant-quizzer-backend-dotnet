using MongoDB.Driver;
using InstantQuizzerBackend.Models;
using Microsoft.Extensions.Configuration;

namespace InstantQuizzerBackend.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDB:ConnectionString"];
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("instant_quizzer");
        }

        public IMongoCollection<Quiz> Quizzes => _database.GetCollection<Quiz>("Quizzes");
    }
}
