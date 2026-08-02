using TraineeManagement.Api.Models;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.Service.AuthInterface;
using TraineeManagement.Api.Service.PasswordServiceInterface;
using TraineeManagement.Api.DTO.UserDTO;
using System.Net;
using TraineeManagement.Api.Helpers;
using TraineeManagement.Api.Enum.User;
using TraineeManagement.Api.Enum.Mentor;
using TraineeManagement.Api.Enum.Trainee;

namespace TraineeManagement.Api.Service.AuthService;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IPasswordService _passwordService;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthService> _logger;

    public AuthService(AppDbContext context, IPasswordService passwordService, IConfiguration config, ILogger<AuthService> logger) 
    { 
        _context = context; 
        _passwordService = passwordService;
        _config = config;
        _logger = logger;
    }

    public async Task<RegisterUserResponseModel> RegisterUser(RegisterUserRequestModel userRequestModel)
    {   
        
        if(userRequestModel.Password == null || userRequestModel.UserName == null)
        {
            _logger.LogInformation("Username & Password is required.");
            throw new HttpStatusException(HttpStatusCode.BadRequest,"Username and Password is Required");
        }

        UserModel? user = _context.Users.FirstOrDefault(
            user => user.UserName == userRequestModel.UserName
        );

        if(user != null)
        {
            _logger.LogInformation("User already Exists.");
            throw new HttpStatusException(HttpStatusCode.BadRequest,"User Already Exists, Proceed with Login.");
        }

        // allow-list only. Admin (or any other value) can never
        // come from this endpoint — it always collapses to Trainee.
        UserRole requestedRole = userRequestModel.Role == UserRole.Mentor
            ? UserRole.Mentor
            : UserRole.Trainee;

        if (userRequestModel.FirstName == null || userRequestModel.LastName == null)
        {
            throw new HttpStatusException(HttpStatusCode.BadRequest, "FirstName and LastName are required.");
        }

        if (requestedRole == UserRole.Trainee && userRequestModel.TechStack == null)
        {
            throw new HttpStatusException(HttpStatusCode.BadRequest, "TechStack is required to register as a Trainee.");
        }

        if (requestedRole == UserRole.Mentor && userRequestModel.Expertise == null)
        {
            throw new HttpStatusException(HttpStatusCode.BadRequest, "Expertise is required to register as a Mentor.");
        }

        UserModel newUser = new UserModel(
            userRequestModel.UserName,
            userRequestModel.Email,
            _passwordService.GetHashedPassword(userRequestModel.Password),
            requestedRole
        );

        await using var transaction = await _context.Database.BeginTransactionAsync();

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync(); 

        if (requestedRole == UserRole.Trainee)
        {
            TraineeModel trainee = new TraineeModel(
                userRequestModel.FirstName,
                userRequestModel.LastName,
                userRequestModel.Email,
                userRequestModel.TechStack,
                TraineeStatus.Active
            )
            {
                UserId = newUser.Id
            };

            _context.Trainees.Add(trainee);
            await _context.SaveChangesAsync();
        }
        else
        {
            MentorModel mentor = new MentorModel(
                userRequestModel.FirstName,
                userRequestModel.LastName,
                userRequestModel.Email,
                userRequestModel.Expertise,
                MentorStatus.Active
            )
            {
                UserId = newUser.Id
            };

            _context.Mentors.Add(mentor);
            await _context.SaveChangesAsync();
        }

        await transaction.CommitAsync();

        RegisterUserResponseModel response = new RegisterUserResponseModel(
            newUser.UserName,
            newUser.Email,
            newUser.Role,   
            newUser.CreatedDate,
            newUser.UpdatedDate
        );

        return response;
    }

    public async Task<LoginUserResponseModel?> LoginUser(LoginUserRequestModel userRequestModel)
    {
        UserModel? user = _context.Users.FirstOrDefault(
            user => user.UserName == userRequestModel.UserName
        );

        if(user == null || userRequestModel.Password == null || user.UserName == null)
        {
            _logger.LogError("Invalid Credentials.");
            throw new HttpStatusException(HttpStatusCode.BadRequest,"Invalid Credentials");
        }

        bool isValidPassword = await _passwordService.VerifyPassword(user, userRequestModel.Password);

        if(!isValidPassword)
        {
            _logger.LogInformation("Invalid Credentials.");
            throw new HttpStatusException(HttpStatusCode.Unauthorized,"Invalid Credentials");
        }

        string token = JwtTokenHelper.GenerateToken(_config, user.Id, user.UserName, user.Role.ToString());

        UserModelDTO userDto = new UserModelDTO(
            user.Id,
            user.UserName,
            user.Role
        );

        var jwtSettings = _config.GetSection("JwtSettings");

        if(jwtSettings == null || !int.TryParse(jwtSettings["ExpiryMinutes"], out int expiryMinutes))
        {
           _logger.LogCritical("JWT configuration (JwtSettings:ExpiryMinutes) is missing or invalid.");
           throw new HttpStatusException(HttpStatusCode.InternalServerError, "Authentication is temporarily unavailable. Please try again later.");
        }

        LoginUserResponseModel res = new LoginUserResponseModel(
            token,
            expiryMinutes
            userDto        
        );

        return res;
    }
}