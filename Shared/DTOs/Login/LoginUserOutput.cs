namespace Shared.DTOs
{
    public class LoginUserOutput
    {

        public LoginUserOutput(string token)
        {
            Token = token;
        }

        public string Token { get; }
    }
}
