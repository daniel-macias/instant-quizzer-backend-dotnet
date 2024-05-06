using InstantQuizzerBackend.Data;
using InstantQuizzerBackend.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstantQuizzerBackend.Services
{
    public class QuizService
    {
        private readonly IMongoCollection<Quiz> _quizzes;

        public QuizService(MongoDbContext dbContext)
        {
            _quizzes = dbContext.Quizzes;
        }

        // Create a Quiz
        public async Task CreateQuizAsync(Quiz quiz)
        {
            await _quizzes.InsertOneAsync(quiz);
        }

        // Get all Quizzes
        public async Task<List<Quiz>> GetAllQuizzesAsync()
        {
            return await _quizzes.Find(_ => true).ToListAsync();
        }

        // Get a Quiz by ID
        public async Task<Quiz> GetQuizByIdAsync(string id)
        {
            return await _quizzes.Find(quiz => quiz.Id == id).FirstOrDefaultAsync();
        }

        // Update a Quiz
        public async Task UpdateQuizAsync(string id, Quiz updatedQuiz)
        {
            await _quizzes.ReplaceOneAsync(quiz => quiz.Id == id, updatedQuiz);
        }

        // Delete a Quiz
        public async Task DeleteQuizAsync(string id)
        {
            await _quizzes.DeleteOneAsync(quiz => quiz.Id == id);
        }
    }
}
