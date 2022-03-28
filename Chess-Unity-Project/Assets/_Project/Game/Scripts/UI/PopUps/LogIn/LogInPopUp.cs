using SteampunkChess.CloudService;
using SteampunkChess.CloudService.Models;
using SteampunkChess.PopUpService;
using SteampunkChess.SignalSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SteampunkChess.PopUps
{
    public class LogInPopUp : PopUp
    {
        [Header("LogInPopUp")]
        [SerializeField] private TMP_InputField _usernameInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private Toggle _rememberMeToggle;

        [SerializeField] private Signal _onLogInSignal;

        private IPopUpService _popUpService;
        private ICloudService _cloudService;

        [Inject]
        private void Construct(IPopUpService popUpService, ICloudService cloudService)
        {
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
            _onLogInSignal?.Raise();

        }

        private void OnLogInError(string error)
        {

        }

        public void SwitchToLogInPopUp()
        {
            _popUpService.HidePopUp(GameConstants.PopUps.LogInWindow, HideType.HideDestroyAndRelease);
            _popUpService.ShowPopUp(GameConstants.PopUps.SignUpWindow);
        }

        public void SwitchToForgotPasswordPopUp()
        {
            _popUpService.HidePopUp(GameConstants.PopUps.LogInWindow, HideType.HideDestroyAndRelease);
            _popUpService.ShowPopUp(GameConstants.PopUps.ForgotPasswordWindow);
        }
    }
}