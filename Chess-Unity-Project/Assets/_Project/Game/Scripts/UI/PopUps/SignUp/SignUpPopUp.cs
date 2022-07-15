using System;
using SteampunkChess.CloudService;
using SteampunkChess.CloudService.Models;
using SteampunkChess.PopUpService;
using SteampunkChess.SignalSystem;
using System.Text.RegularExpressions;
using SteampunkChess.LocalizationSystem;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace SteampunkChess.PopUps
{
    public class SignUpPopUp : PopUp
    {

        [Header("SignUpPopUp")]
        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private TMP_InputField _usernameInputField;

        [SerializeField] private Signal _onLogInSignal;

        private IPopUpService _popUpService;
        private ICloudService _cloudService;
        private ILocalizationSystem _localizationSystem;

        private const string EmailPattern = 
            @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + 
            "@" + 
            @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";

        public override string PopUpKey { get; set; } = GameConstants.PopUps.SignUpWindow;

        [Inject]
        private void Construct(IPopUpService popUpService, ICloudService cloudService, ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;
            _popUpService = popUpService;
            _cloudService = cloudService;
        }

        public void SignUp()
        {
          
            if(_passwordInputField.text.Length < 6)
            {
                Logger.Debug("Password too short");
                return;
            }

            if(!Regex.IsMatch(_emailInputField.text, EmailPattern))
            {
                Logger.Debug("Invalid email");
                return;
            }



            _cloudService.RegisterUser(
                new RegisterUserParams(_usernameInputField.text, _emailInputField.text, _passwordInputField.text),
                OnRegisterSuccess, OnRegisterError);


        }

        private void OnRegisterSuccess()
        {
            _popUpService.HidePopUp(GameConstants.PopUps.SignUpWindow, HideType.HideDestroyAndRelease);
            _popUpService.ShowPopUp(GameConstants.PopUps.SuccessToast, _localizationSystem.GetLocalizedString("UI Text","youhavesuccessfullysigned_text"));
            Addressables.LoadSceneAsync("MainMenu");
        }
        
        private void OnRegisterError(string error)
        {
            _popUpService.ShowPopUp(GameConstants.PopUps.ErrorToast, $"{error}");
        }

        public void SwitchToLogInPopUp()
        {
            _popUpService.HidePopUp(GameConstants.PopUps.SignUpWindow, HideType.HideDestroyAndRelease);
            _popUpService.ShowPopUp(GameConstants.PopUps.LogInWindow, Array.Empty<int>());

        }
    }
}