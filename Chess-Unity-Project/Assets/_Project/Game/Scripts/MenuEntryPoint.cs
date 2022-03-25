using System.Linq;
using SteampunkChess.CloudService;
using SteampunkChess.CloudService.Models;
using SteampunkChess.NetworkService;
using SteampunkChess.PopUps;
using SteampunkChess.PopUpService;
using SteampunkChess.SignalSystem;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class MenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private Signal _onLogInSignal;
        
        private IPopUpService _popUpService;
        private ICloudService _cloudService;
        private INetworkService _networkService;

        [Inject]
        private void Construct(IPopUpService popUpService, ICloudService cloudService, INetworkService networkService)
        {
            _networkService = networkService;
            _cloudService = cloudService;
            _popUpService = popUpService;
        }

        private void Awake()
        {
            ProcessUserValidation();
            if (_networkService.OfflineMode)
            {
                _popUpService.ShowPopUp(GameConstants.PopUps.ErrorToast, "Failed to connect. Starting in offline mode!");
            }
        }
        
        private void ProcessUserValidation()
        {
            if (GameCommandLineArgs.GameArgs.Contains(GameConstants.GameCLIArgs.SkipUserValidation))
                return;

            if (!Prefs.RememberMe)
            {
                _popUpService.ShowPopUp(GameConstants.PopUps.LogInWindow);
                return;
            }

            var userParams = new LogInUserParams(Prefs.Username, Prefs.Password);
            _cloudService.LogInUser(userParams, OnLogInSuccess, OnLogInError);
        }
        
        private void OnLogInSuccess()
        {
            OnLogin();
            if (_onLogInSignal != null) _onLogInSignal.Raise();
            Logger.Debug("LogIn Success");
            
            void OnLogin()
            {
                _popUpService.HideAll(HideType.HideDestroyAndRelease);
                _networkService.Username = Prefs.Username;
            }
        }

        private void OnLogInError(string error)
        {
            _popUpService.ShowPopUp(GameConstants.PopUps.LogInWindow);
        }
    }
}