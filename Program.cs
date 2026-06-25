using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Service.AuthInterface;
using TraineeManagement.Api.Service.AuthService;
using TraineeManagement.Api.Service.PasswordServiceInterface;
using TraineeManagement.Api.Service.TraineeeInterface;
using TraineeManagement.Api.Service.TraineeService;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Google.Protobuf.WellKnownTypes;
using TraineeManagement.Api.Service.LearningTaskInterface;
using TraineeManagement.Api.Service.MentorInterface;
using TraineeManagement.Api.Service.MentorService;
using TraineeManagement.Api.Service.LearningTaskService;
using TraineeManagement.Api.Service.PasswordService;
using TraineeManagement.Api.Service.TaskAssignmentInterface;
using TraineeManagement.Api.Service.SubmissionInterface;
using TraineeManagement.Api.Service.ReviewInterface;
using Polly;
using System.Net;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ClockSkew = TimeSpan.Zero 
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddValidation();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddScoped<ITraineeService, TraineeService>(); 
builder.Services.AddScoped<IAuthService, AuthService>(); 
builder.Services.AddScoped<IMentorService, MentorService>(); 
builder.Services.AddScoped<ILearningTaskService, LearningTaskService>(); 
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<ITaskAssignmentService, TaskAssignmentService>();
builder.Services.AddScoped<ISubmissionService, SubmissionService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IFileStorageService,LocalFileStorageService>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
builder.Services.AddScoped<IProcessingJobService, ProcessingJobService>();
builder.Services.AddScoped<IPasswordHasher<UserModel>, PasswordHasher<UserModel>>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// File Storage Configuration Setup
builder.Services.Configure<FileStorageOptions> (
    builder.Configuration.GetSection("FileStorage")
);

// RabbitMq Configuration Setup
builder.Services.Configure<RabbitMqOptions> (
    builder.Configuration.GetSection(
        RabbitMqOptions.SectionName
    )
);

// Register DBContext service
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(connectionString));

// Cors Configuration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000","http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Redis Configuration
builder.Services.AddStackExchangeRedisCache(options =>
{ 
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
});

// Inter-Service Communication Setup
builder.Services.AddHttpClient<TraineeProfileClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5002");              
})
.AddStandardResilienceHandler(options =>
{
    // Finite Timeouts Config
    options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(10); 
    options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(3);       

    // Safe Conditional Retries (Only transient errors)
    options.Retry.MaxRetryAttempts = 3;
    options.Retry.BackoffType = DelayBackoffType.Exponential;
    options.Retry.UseJitter = true; 
    options.Retry.ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
        .HandleResult(response => 
            response.StatusCode >= HttpStatusCode.InternalServerError || 
            response.StatusCode == HttpStatusCode.RequestTimeout ||     
            response.StatusCode == HttpStatusCode.TooManyRequests);      

    // Circuit Breaker Configuration
    options.CircuitBreaker.FailureRatio = 0.5; 
    options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
    options.CircuitBreaker.MinimumThroughput = 8;
    options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(15); 
});
    
var app = builder.Build();

app.UseExceptionHandler(); 
app.UseAuthentication(); 
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
