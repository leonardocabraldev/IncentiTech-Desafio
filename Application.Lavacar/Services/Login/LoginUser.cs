using Shared.DTOs;

namespace Application.Services.Login
{
    public class LoginUser
    {
        public LoginUserOutput Execute(LoginUserInput input)
        {
            return new LoginUserOutput(input.Email);
        }
    }
}
