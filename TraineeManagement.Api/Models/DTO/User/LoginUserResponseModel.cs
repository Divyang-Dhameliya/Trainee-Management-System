using Org.BouncyCastle.Bcpg.Sig;
using TraineeManagement.Api.Enum.User;

namespace TraineeManagement.Api.DTO.UserDTO;

public class LoginUserResponseModel
{    
    public string? token { get; set; }
    public int expiresIn { get; set; }

    public UserModelDTO? user { get; set; }

    public LoginUserResponseModel(string? jwttoken, int expiresin, UserModelDTO userDTO)
    {
        token = jwttoken;
        user = userDTO;
        expiresIn = expiresin;
    }
}