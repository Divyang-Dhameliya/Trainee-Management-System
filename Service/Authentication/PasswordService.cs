using TraineeManagement.Api.Models;
using TraineeManagement.Api.Service.TraineeeInterface;
using TraineeManagement.Api.DTO.TraineeDTO;
using Microsoft.EntityFrameworkCore.Storage.Json;
using TraineeManagement.Api.Data;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Constants;
using TraineeManagement.Api.Service.PasswordServiceInterface;
using Microsoft.AspNetCore.Identity;
namespace TraineeManagement.Api.Service.TraineeService;

public class PasswordService : IPasswordService
{
    private readonly IPasswordHasher<UserModel> _passwordHasher;
    private readonly AppDbContext _context;

    public PasswordService(AppDbContext context, IPasswordHasher<UserModel> passwordHasher)
    {
        _passwordHasher = passwordHasher;
        _context = context;
    }

    public string GetHashedPassword(string plainTextPassword)
    {
        return _passwordHasher.HashPassword(null!, plainTextPassword);
    }

    public async Task<bool> VerifyPassword(UserModel user, string providedPassword)
    {
        if (user.PasswordHash == null) return false;

        PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(
            null!,
            user.PasswordHash,
            providedPassword
        );

        switch (result)
        {
            case PasswordVerificationResult.Success:
                return true;

            case PasswordVerificationResult.SuccessRehashNeeded:
                user.PasswordHash = _passwordHasher.HashPassword(user, providedPassword);
                await _context.SaveChangesAsync();
                return true;

            case PasswordVerificationResult.Failed:
            default:
                return false;
        }

    }

}