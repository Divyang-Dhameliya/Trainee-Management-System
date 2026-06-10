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

builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddScoped<IPasswordHasher<UserModel>, PasswordHasher<UserModel>>();
    
// Get connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register DBContext service
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(connectionString));
    
var app = builder.Build();

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
