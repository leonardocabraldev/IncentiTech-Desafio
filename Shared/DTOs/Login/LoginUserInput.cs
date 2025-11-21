namespace Shared.DTOs
{
    public class LoginUserInput
    {
        public LoginUserInput(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; }
        public string Password { get; }

    }
}
