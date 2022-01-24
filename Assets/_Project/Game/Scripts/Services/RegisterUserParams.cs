namespace SteampunkChess.CloudService.Models
{
    public class RegisterUserParams
    {
        public string Username { get; }
        public string Email { get; }
        public string Password { get; }

        public RegisterUserParams(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        }

    }
}
