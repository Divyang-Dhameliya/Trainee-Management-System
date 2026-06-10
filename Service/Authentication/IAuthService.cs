using TraineeManagement.Api.DTO.UserDTO;

namespace TraineeManagement.Api.Service.AuthInterface;

public interface IAuthService
{
    Task<RegisterUserResponseModel> RegisterUser(RegisterUserRequestModel userRequestModel);

    Task<LoginUserResponseModel?> LoginUser(LoginUserRequestModel userRequestModel);
}