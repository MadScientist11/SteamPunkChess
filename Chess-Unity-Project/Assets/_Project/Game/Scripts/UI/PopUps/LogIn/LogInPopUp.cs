using System;
using SteampunkChess.CloudService;
using SteampunkChess.CloudService.Models;
using SteampunkChess.LocalizationSystem;
using SteampunkChess.PopUpService;
using SteampunkChess.SignalSystem;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

namespace SteampunkChess.PopUps
{
    public class LogInPopUp : PopUp
    {
        [Header("LogInPopUp")] [SerializeField]
        private TMP_InputField _usernameInputField;

        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private Toggle _rememberMeToggle;


        private IPopUpService _popUpService;
        private ICloudService _cloudService;
        private ILocalizationSystem _localizationSystem;

        public override string PopUpKey { get; set; } = GameConstants.PopUps.LogInWindow;

        [Inject]
        private void Construct(IPopUpService popUpService, ICloudService cloudService,
            ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;
            _popUpService = popUpService;
            _cloudService = cloudService;
        }

        public void LogIn()
        {
            _cloudService.LogInUser(new LogInUserParams(_usernameInputField.text, _passwordInputField.text),
                OnLogInSuccess,
                OnLogInError);
        }

        private void OnLogInSuccess()
        {
            if (_rememberMeToggle.isOn)
            {
                Prefs.RememberMe = true;
                Prefs.Username = _usernameInputField.text;
                Prefs.Password = _passwordInputField.text;
            }

            _popUpService.HidePopUp(GameConstants.PopUps.LogInWindow, HideType.HideDestroyAndRelease);
            _popUpService.ShowPopUp(GameConstants.PopUps.SuccessToast,
                _localizationSystem.GetLocalizedString("UI Text", "youhavesuccessfullylogged_text"));

            Addressables.LoadSceneAsync("MainMenu");
        }

        private void OnLogInError(string error)
        {
            _popUpService.ShowPopUp(GameConstants.PopUps.ErrorToast, $"{error}");
        }

        public void SwitchToLogInPopUp()
        {
            _popUpService.HidePopUp(GameConstants.PopUps.LogInWindow, HideType.HideDestroyAndRelease);
            _popUpService.ShowPopUp(GameConstants.PopUps.SignUpWindow, Array.Empty<int>());
        }

        public void SwitchToForgotPasswordPopUp()
        {
            _popUpService.HidePopUp(GameConstants.PopUps.LogInWindow, HideType.HideDestroyAndRelease);
            _popUpService.ShowPopUp(GameConstants.PopUps.ForgotPasswordWindow, Array.Empty<int>());
        }
    }
}