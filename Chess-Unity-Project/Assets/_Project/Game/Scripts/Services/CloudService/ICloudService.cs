using SteampunkChess.CloudService.Models;
using System;
using System.Collections.Generic;
using SteampunkChess.NetworkService;

namespace SteampunkChess.CloudService
{
    public interface ICloudService : IService
    {
        bool IsLoggedIn { get; }
        void UpdateUserData(Dictionary<string, string> data);
        void RegisterUser(RegisterUserParams userParams, Action onSuccess, Action<string> onError);
        void LogInUser(LogInUserParams userParams, Action onSuccess, Action<string> onError);
        void SendAccountRecoveryEmail(ResetPasswordUserParams userParams, Action onSuccess, Action<string> onError);
    }
}
