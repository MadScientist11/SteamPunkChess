using SteampunkChess.CloudService;
using SteampunkChess.CloudService.Models;
using SteampunkChess.PopUpService;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class MenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private RegisterUserMenu _registerUserMenu;
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
                _cloudService.LogInUser(userParams, null, null);
            }
            
            if (!_cloudService.IsLoggedIn)
            {
                _popUpService.ShowPopUp(GameConstants.PopUps.LogInWindow);
            }
            else
            {
                Debug.LogError("Logged in");
                _registerUserMenu.SwitchToMainMenu();
            }
        }
       
    }
}