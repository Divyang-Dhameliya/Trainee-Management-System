using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.DTO.UserDTO;
using TraineeManagement.Api.Service.AuthInterface;
namespace TraineeManagement.Api.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequestModel userRequestModel)
    {
        _logger.LogInformation("HTTP Post received for Register User. Username: {Username}", userRequestModel.UserName); 

        RegisterUserResponseModel res = await _authService.RegisterUser(userRequestModel);
        _logger.LogInformation("Registration completed successfully. Username: {Username}", res.UserName);
        return Ok(res);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequestModel userRequestModel)
    {
        _logger.LogInformation("HTTP Post received for Login User. Username: {Username}", userRequestModel.UserName); 

        LoginUserResponseModel? res = await _authService.LoginUser(userRequestModel);
        _logger.LogInformation("Login completed successfully. Username: {Username}", userRequestModel.UserName);
        return Ok(res);
    }
}
