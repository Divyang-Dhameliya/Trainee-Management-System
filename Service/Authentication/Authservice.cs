using TraineeManagement.Api.Models;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.Service.AuthInterface;
using TraineeManagement.Api.Service.PasswordServiceInterface;
using TraineeManagement.Api.DTO.UserDTO;
namespace TraineeManagement.Api.Service.AuthService;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IPasswordService _passwordService;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IPasswordService passwordService, IConfiguration config) 
    { 
        _context = context; 
        _passwordService = passwordService;
        _config = config;
    }

    public async Task<RegisterUserResponseModel> RegisterUser(RegisterUserRequestModel userRequestModel)
    {   
        UserModel? user = _context.Users.FirstOrDefault(
            user => user.UserName == userRequestModel.UserName
        );

        if(user != null)
        {
            throw new Exception("User Already Exists");
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

        if(user == null || userRequestModel.Password == null || user.UserName == null || user.Role == null) return null;

        bool isValidPassword = await _passwordService.VerifyPassword(user, userRequestModel.Password);

        if(!isValidPassword) return null;

        string token = JwtTokenHelper.GenerateToken(_config, user.Id, user.UserName, user.Role.ToString());

        UserModelDTO userDto = new UserModelDTO(
            user.Id,
            user.UserName,
            user.Role
        );

        var jwtSettings = _config.GetSection("JwtSettings");

        LoginUserResponseModel res = new LoginUserResponseModel(
            token,
            int.Parse(jwtSettings["ExpiryMinutes"]),
            userDto        
        );

        return res;
    }


}