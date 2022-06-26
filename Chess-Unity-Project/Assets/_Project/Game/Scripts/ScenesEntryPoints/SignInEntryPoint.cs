using System;
using SteampunkChess.CloudService;
using SteampunkChess.CloudService.Models;
using SteampunkChess.PopUpService;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class SignInEntryPoint : MonoBehaviour
    {
        [SerializeField] private SignInMenu _signInMenu;
        private IPopUpService _popUpService;
        private ICloudService _cloudService;

        [Inject]
        private void Construct(IPopUpService popUpService, ICloudService cloudService)
        {
            _cloudService = cloudService;
            _popUpService = popUpService;
        }
        private void Awake()
        {
            ProcessUserValidation();
        }

        private void ProcessUserValidation()
        {
            if (Prefs.RememberMe)
            {
                var userParams = new LogInUserParams(Prefs.Username, Prefs.Password);
                _cloudService.LogInUser(userParams, _signInMenu.LoadMainMenu, error =>
                {
                    _popUpService.ShowPopUp(GameConstants.PopUps.ErrorToast, error);
                    _popUpService.ShowPopUp(GameConstants.PopUps.LogInWindow,Array.Empty<int>());
                });
            }
            else
            {
                _popUpService.ShowPopUp(GameConstants.PopUps.LogInWindow,Array.Empty<int>());
            }
        }

        
        
        
    }
}
