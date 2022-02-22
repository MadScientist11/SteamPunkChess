using SteampunkChess.CloudService.Models;
using System;

namespace SteampunkChess.CloudService
{
    public interface ICloudService
    {
        void RegisterUser(RegisterUserParams userParams, Action onSuccess, Action<string> onError);
        void LogInUser(LogInUserParams userParams, Action onSuccess, Action<string> onError);
        void SendAccountRecoveryEmail(ResetPasswordUserParams userParams, Action onSuccess, Action<string> onError);
    }
}
