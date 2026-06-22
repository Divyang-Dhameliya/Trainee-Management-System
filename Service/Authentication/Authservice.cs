using TraineeManagement.Api.Models;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.Service.AuthInterface;
using TraineeManagement.Api.Service.PasswordServiceInterface;
using TraineeManagement.Api.DTO.UserDTO;
using System.Net;
using TraineeManagement.Api.Helpers;
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
            throw new HttpStatusException(HttpStatusCode.BadRequest,"Username and Password is Required");
        }

        UserModel? user = _context.Users.FirstOrDefault(
            user => user.UserName == userRequestModel.UserName
        );

        if(user != null)
        {
            throw new HttpStatusException(HttpStatusCode.BadRequest,"User Already Exists, Proceed with Login.");
        }

        UserModel newUser = new UserModel(
            userRequestModel.UserName,
            userRequestModel.Email,
            _passwordService.GetHashedPassword(userRequestModel.Password),
            userRequestModel.Role
        );

        _context.Users.Add(newUser);

        await _context.SaveChangesAsync();

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
            throw new HttpStatusException(HttpStatusCode.BadRequest,"Invalid Credentials");
        }

        bool isValidPassword = await _passwordService.VerifyPassword(user, userRequestModel.Password);

        if(!isValidPassword)
        {
            throw new HttpStatusException(HttpStatusCode.Unauthorized,"Invalid Credentials");
        }

        string token = JwtTokenHelper.GenerateToken(_config, user.Id, user.UserName, user.Role.ToString());

        UserModelDTO userDto = new UserModelDTO(
            user.Id,
            user.UserName,
            user.Role
        );

        var jwtSettings = _config.GetSection("JwtSettings");

        if(jwtSettings == null || jwtSettings["ExpiryMinutes"] == null)
        {
            _logger.LogCritical("JWT configuration Not Found");
        }

        LoginUserResponseModel res = new LoginUserResponseModel(
            token,
            int.Parse(jwtSettings["ExpiryMinutes"]),
            userDto        
        );

        return res;
    }
}