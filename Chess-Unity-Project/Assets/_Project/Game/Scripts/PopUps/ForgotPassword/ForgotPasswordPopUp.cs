using SteampunkChess.CloudService;
using SteampunkChess.PopUpService;
using TMPro;
using UnityEngine;
using Zenject;

namespace SteampunkChess.PopUps
{
    public class ForgotPasswordPopUp : PopUp
    {

        [Header("ForgotPasswordPopUp")]
        [SerializeField] private TMP_InputField _emailInputField;

        private IPopUpService _popUpService;
        private ICloudService _cloudService;

        [Inject]
        private void Construct(IPopUpService popUpService, ICloudService cloudService)
        {
            _popUpService = popUpService;
            _cloudService = cloudService;
        }

        public void SendAccountRecoveryEmail()
        {
            _cloudService.SendAccountRecoveryEmail(new CloudService.Models.ResetPasswordUserParams(_emailInputField.text),
                OnSuccessAccountRecoveryEmail, OnErrorAccountRecoveryEmail);

        }

        private void OnSuccessAccountRecoveryEmail()
        {
            _popUpService.HidePopUp(GameConstants.PopUps.ForgotPasswordWindow, HideType.HideDestroyAndRelease);
            _popUpService.ShowPopUp(GameConstants.PopUps.LogInWindow);

        }

        private void OnErrorAccountRecoveryEmail(string error)
        {

        }
    }
}