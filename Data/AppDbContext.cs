
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Models;

namespace TraineeManagement.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<TraineeModel> Trainees { get; set; }

    public DbSet<UserModel> Users { get; set; }

    public DbSet<MentorModel> Mentors { get; set; }

    public DbSet<LearningTaskModel> LearningTasks { get; set;}

    public DbSet<TaskAssignmentModel> TaskAssignments { get; set; }

    public DbSet<SubmissionModel> Submissions { get; set; }

    public DbSet<ReviewModel> Reviews { get; set; }
}
