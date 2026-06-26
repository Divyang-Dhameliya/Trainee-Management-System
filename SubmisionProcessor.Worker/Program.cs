using Microsoft.EntityFrameworkCore;
using SubmisionProcessor.Worker;
using SubmissionProcessor.Worker;
using TraineeManagement.Api.Data;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddHostedService<SubmissionProcessingWorker>();
builder.Services.AddScoped<IFileStorageService,LocalFileStorageService>();
builder.Services.AddScoped<IProcessingJobService, ProcessingJobService>();
builder.Services.AddScoped<ISubmissionProcessingService,SubmissionProcessingService>();

// Register DBContext service
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(connectionString));

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

var host = builder.Build();
host.Run();
