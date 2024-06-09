using InstantQuizzerBackend.Data;
using InstantQuizzerBackend.Services;
using InstantQuizzerBackend.Models;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<MongoDbContext>(sp => new MongoDbContext(builder.Configuration));
builder.Services.AddScoped<QuizService>();

// Register the Swagger generator, defining one or more Swagger documents
builder.Services.AddEndpointsApiExplorer(); // Enables the API explorer which Swagger uses
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Quiz API", Version = "v1" });
});

// Add CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy",
        builder => builder.WithOrigins("https://instant-quizzer-frontend.vercel.app") // Allow specific origin
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials()); // This is necessary only if your front end needs to send credentials
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quiz API V1"));
}

app.UseRouting();

// Enable CORS using the named policy
app.UseCors("MyCorsPolicy");


// Define your endpoints here

// Create a new Quiz
app.MapPost("/api/quizzes", async (Quiz quiz, QuizService quizService) => 
{
    await quizService.CreateQuizAsync(quiz);
    return Results.Created($"/api/quizzes/{quiz.Id}", quiz);
});

app.MapGet("/api/quizzes", async (QuizService quizService, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Getting all quizzes");
        var quizzes = await quizService.GetAllQuizzesAsync();
        return Results.Ok(quizzes);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error getting all quizzes");
        return Results.BadRequest("Internal Server Error");
    }
});

// Get a Quiz by ID
app.MapGet("/api/quizzes/{id}", async (string id, QuizService quizService) => 
{
    var quiz = await quizService.GetQuizByIdAsync(id);
    return quiz != null ? Results.Ok(quiz) : Results.NotFound();
});

// Update a Quiz
app.MapPut("/api/quizzes/{id}", async (string id, Quiz quiz, QuizService quizService) => 
{
    var existingQuiz = await quizService.GetQuizByIdAsync(id);
    if (existingQuiz == null) return Results.NotFound();
    await quizService.UpdateQuizAsync(id, quiz);
    return Results.NoContent();
});

// Delete a Quiz
app.MapDelete("/api/quizzes/{id}", async (string id, QuizService quizService) => 
{
    await quizService.DeleteQuizAsync(id);
    return Results.Ok($"Deleted quiz {id}");
});

app.MapPost("/api/quizzes/{id}/results", async (string id, Result newResult, MongoDbContext dbContext) => 
{
    var quiz = await dbContext.Quizzes.Find(q => q.Id == id).FirstOrDefaultAsync();
    if (quiz == null)
    {
        return Results.NotFound("Quiz not found");
    }

    // Check if the length of responses matches the number of questions in the quiz
    if (newResult.Responses.Count != quiz.Questions.Count)
    {
        return Results.BadRequest(new { message = "The number of responses does not match the number of questions in the quiz." });
    }

    quiz.Results.Add(newResult);
    await dbContext.Quizzes.ReplaceOneAsync(q => q.Id == id, quiz);
    return Results.Ok(new { message = "Results added successfully", quiz });
});


app.Run();
