using System.Linq;
using SteampunkChess.CloudService;
using SteampunkChess.CloudService.Models;
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

        [Inject]
        private void Construct(IPopUpService popUpService, ICloudService cloudService)
        {
            _cloudService = cloudService;
            _popUpService = popUpService;
        }

        private void Start()
        {
            ProcessUserValidation();
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
            if (_onLogInSignal != null) _onLogInSignal.Raise();
            Logger.Debug("LogIn Success");
        }

        private void OnLogInError(string error)
        {
            _popUpService.ShowPopUp(GameConstants.PopUps.LogInWindow);
        }
    }
}