using PlayFab;
using PlayFab.ClientModels;
using SteampunkChess.CloudService.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace SteampunkChess.CloudService
{
    public static class TaskEx
    {
        /// <summary>
        /// Blocks while condition is true or timeout occurs.
        /// </summary>
        /// <param name="condition">The condition that will perpetuate the block.</param>
        /// <param name="frequency">The frequency at which the condition will be check, in milliseconds.</param>
        /// <param name="timeout">Timeout in milliseconds.</param>
        /// <exception cref="TimeoutException"></exception>
        /// <returns></returns>
        public static async Task WaitWhile(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (condition()) await Task.Delay(frequency);
            });

            if(waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)))
                throw new TimeoutException();
        }

        /// <summary>
        /// Blocks until condition is true or timeout occurs.
        /// </summary>
        /// <param name="condition">The break condition.</param>
        /// <param name="frequency">The frequency at which the condition will be checked.</param>
        /// <param name="timeout">The timeout in milliseconds.</param>
        /// <returns></returns>
        public static async Task WaitUntil(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (!condition()) await Task.Delay(frequency);
            });

            if (waitTask != await Task.WhenAny(waitTask, 
                    Task.Delay(timeout))) 
                throw new TimeoutException();
        }
    }
    [CreateAssetMenu(fileName = "PlayFabServiceSO", menuName = "Services/PlayFabServiceSO")]
    public class PlayFabServiceSO : ScriptableObject, ICloudService
    {
        private const string TitleId = "832CB";
        private Action _onSuccess;
        private Action<string> _onError;
        
        private PlayerData _playerData;

        public bool IsLoggedIn =>  PlayFabClientAPI.IsClientLoggedIn();
        public string InitializationMessage => "Initializing cloud service";
        
        public async Task Initialize()
        {
            await Task.Delay(2000);

        }
        
        [Inject]
        private void Construct(ServiceContainer serviceContainer, PlayerData playerData)
        {
            _playerData = playerData;
            serviceContainer.ServiceList.Add(this);
        }

        public void RegisterUser(RegisterUserParams userParams, Action onSuccess, Action<string> onError)
        {
            var request = new RegisterPlayFabUserRequest()
            {
                Email = userParams.Email,
                Password = userParams.Password,
                Username = userParams.Username,
                RequireBothUsernameAndEmail = true
            };
            _onSuccess = onSuccess;
            _onError = onError;
            
            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
        }

        public void LogInUser(LogInUserParams userParams, Action onSuccess, Action<string> onError)
        {
            var request = new LoginWithPlayFabRequest()
            {
                Username = userParams.Username,
                Password = userParams.Password,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
                {
                    GetPlayerProfile = true
                }
            };
            _onSuccess = onSuccess;
            _onError = onError;
           

            PlayFabClientAPI.LoginWithPlayFab(request, OnLogInSuccess, OnError);

        }

        public void UpdateUserData(Dictionary<string, string> data)
        {
            var request = new UpdateUserDataRequest()
            {
                Data = data,
            };
            PlayFabClientAPI.UpdateUserData(request,
                result => Debug.Log("Successfully updated user data"),
                error => {
                    Debug.Log("Got error setting user data Ancestor to Arthur");
                    Debug.Log(error.GenerateErrorReport());
                });
        }

        public void SendAccountRecoveryEmail(ResetPasswordUserParams userParams, Action onSuccess, Action<string> onError)
        {
            var request = new SendAccountRecoveryEmailRequest()
            {
                Email = userParams.Email,
                TitleId = TitleId
            };
            _onSuccess = onSuccess;
            _onError = onError;

            PlayFabClientAPI.SendAccountRecoveryEmail(request, OnSendAccountRecoveryEmailSuccess, OnError);

        }

        private void OnSendAccountRecoveryEmailSuccess(SendAccountRecoveryEmailResult recoveryEmailResult)
        {
            _onSuccess?.Invoke();
            _onSuccess = null;
            Logger.Debug("Recovery email successfully sent!");
        }

        private void OnRegisterSuccess(RegisterPlayFabUserResult result)
        {
            _onSuccess?.Invoke();
            _onSuccess = null;
            UpdateUserData(new Dictionary<string, string>()
            {
                [GameConstants.PlayerDataKeys.PlayerScoreKey] = "100",
            });
            GetPlayerData(result.Username);
            Logger.Debug("Register success");
       
        }

        private void GetPlayerData(string username)
        {
            _playerData.PlayerName = username;
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnFailed);
        }

        private void OnFailed(PlayFabError error)
        {
            
        }

        private void OnDataReceived(GetUserDataResult result)
        {
            if (result.Data.TryGetValue(GameConstants.PlayerDataKeys.PlayerScoreKey, out var dataRecord))
            {
                _playerData.PlayerScore = Convert.ToInt32(dataRecord.Value);
            }
        }

        private void OnLogInSuccess(LoginResult loginResult)
        {
            _onSuccess?.Invoke();
            _onSuccess = null;
            
            Logger.Debug("LogIn success");
            
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
            {
                GetPlayerData(result.AccountInfo.Username);
                Logger.DebugError("Completion");
            }, error =>
            {
            } );
            
            
        }

        private void OnError(PlayFabError error)
        {
            _onError?.Invoke(error.ErrorMessage);
            _onError = null;
            Logger.Debug(error.GenerateErrorReport());
        }
    }
}
