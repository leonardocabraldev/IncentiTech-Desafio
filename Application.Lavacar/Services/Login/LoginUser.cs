using Shared.LavaCar.DTOs;

namespace Application.Lavacar.Services.Login
{
    public class LoginUser
    {
       public LoginOutputDTO Execute(LoginInputDTO input)
       {
            var newToken = $"{input.Email}";
            return new LoginOutputDTO(newToken);
       }
    }
}
