using TraineeManagement.Api.Models;
using TraineeManagement.Api.DTO.UserDTO;
using Microsoft.AspNetCore.Identity;

namespace TraineeManagement.Api.Service.PasswordServiceInterface;

public interface IPasswordService
{   
    string GetHashedPassword(string plainTextPassword);
    Task<bool> VerifyPassword(UserModel user, string providedPassword);
}