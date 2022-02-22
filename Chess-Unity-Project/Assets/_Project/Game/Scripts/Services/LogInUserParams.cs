namespace SteampunkChess.CloudService.Models
{
    public class LogInUserParams
    {
        public string Username { get; }
        public string Password { get; }

        public LogInUserParams(string username, string password)
        {
            Username = username;
            Password = password;
        }

    }
}
