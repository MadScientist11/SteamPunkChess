﻿using PlayFab;
using PlayFab.ClientModels;
using SteampunkChess.CloudService.Models;
using System;
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
        
        
        public string InitializationMessage => "Initialize cloud service";

        public async Task Initialize()
        {
            await Task.Delay(000);
        }
        
        [Inject]
        private void Construct(ServiceContainer serviceContainer)
        {
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
                Password = userParams.Password
            };
            _onSuccess = onSuccess;
            _onError = onError;

            PlayFabClientAPI.LoginWithPlayFab(request, OnLogInSuccess, OnError);

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
            Logger.Debug("Register success");
            
        }

        private void OnLogInSuccess(LoginResult loginResult)
        {
            _onSuccess?.Invoke();
            _onSuccess = null;
            Logger.Debug("LogIn success");
 
        }

        private void OnError(PlayFabError error)
        {
            _onError?.Invoke(error.ErrorMessage);
            _onError = null;
            Logger.Debug(error.GenerateErrorReport());
        }
    }
}
