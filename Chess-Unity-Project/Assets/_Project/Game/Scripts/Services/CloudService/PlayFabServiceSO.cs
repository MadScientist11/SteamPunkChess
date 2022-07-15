using PlayFab;
using PlayFab.ClientModels;
using SteampunkChess.CloudService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace SteampunkChess.CloudService
{
    [CreateAssetMenu(fileName = "PlayFabServiceSO", menuName = "Services/PlayFabServiceSO")]
    public class PlayFabServiceSO : ScriptableObject, ICloudService
    {
        private const string TitleId = "832CB";
        private Action _onSuccess;
        private Action<string> _onError;
        
        private PlayFabPlayerData _playFabPlayerData;

        public bool IsLoggedIn =>  PlayFabClientAPI.IsClientLoggedIn();
        public string InitializationMessage => "Establishing connection to the cloud...";
        
        public async Task Initialize()
        {
            await Task.Delay(2000);
        }
        
        [Inject]
        private void Construct(ServiceContainer serviceContainer, PlayFabPlayerData playFabPlayerData)
        {
            _playFabPlayerData = playFabPlayerData;
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
            foreach (var dataKey in data.Keys)
            {
                switch (dataKey)
                {
                    case GameConstants.PlayerDataKeys.PlayerScoreKey:
                    {
                        _playFabPlayerData.PlayerScore = Convert.ToInt32(data[dataKey]);
                        break;
                    }
                }
            }
            
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

        public void Logout()
        {
            PlayFabClientAPI.ForgetAllCredentials();
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
            UpdateUserData(new Dictionary<string, string>
            {
                [GameConstants.PlayerDataKeys.PlayerScoreKey] = "100",
            });
            GetPlayerData(result.Username);
            Logger.Debug("Register success");
       
        }

        private void GetPlayerData(string username)
        {
            _playFabPlayerData.PlayerName = username;
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnFailed);
        }

        private void OnFailed(PlayFabError error)
        {
            
        }

        private void OnDataReceived(GetUserDataResult result)
        {
            if (result.Data.TryGetValue(GameConstants.PlayerDataKeys.PlayerScoreKey, out var dataRecord))
            {
                _playFabPlayerData.PlayerScore = Convert.ToInt32(dataRecord.Value);
            }
        }

        private void OnLogInSuccess(LoginResult loginResult)
        {
            _onSuccess?.Invoke();
            _onSuccess = null;
            
            Logger.DebugError("LogIn success");
            
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
