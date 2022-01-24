namespace SteampunkChess.CloudService.Models
{
    public class ResetPasswordUserParams
    {
        public string Email { get; }

        public ResetPasswordUserParams(string email)
        {
            Email = email;
        }
    }
}
