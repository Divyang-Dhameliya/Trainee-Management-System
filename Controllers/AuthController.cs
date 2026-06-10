using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.DTO.UserDTO;
using TraineeManagement.Api.Service.AuthInterface;
namespace TraineeManagement.Api.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequestModel userRequestModel)
    {
        try{
            RegisterUserResponseModel res = await _authService.RegisterUser(userRequestModel);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequestModel userRequestModel)
    {
        LoginUserResponseModel? res = await _authService.LoginUser(userRequestModel);

        if(res == null)
        {
            return Unauthorized("Invalid Credentitals.");
        }

        return Ok(res);

    }
}
