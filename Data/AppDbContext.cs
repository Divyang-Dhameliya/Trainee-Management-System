
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Models;

namespace TraineeManagement.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<TraineeModel> Trainees { get; set; }

    public DbSet<UserModel> Users { get; set; }
}
