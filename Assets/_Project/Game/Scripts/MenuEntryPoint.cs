using PlayFab;
using PlayFab.ClientModels;
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

        [Inject]
        private void Construct(IPopUpService popUpService)
        {
            _popUpService = popUpService;

        }

        private void Start()
        {
            if (!Prefs.RememberMe)
            {
                _popUpService.ShowPopUp(GameConstants.PopUps.LogInWindow);
                return;
            }
            var request = new LoginWithPlayFabRequest()
            {
                Username = Prefs.Username,
                Password = Prefs.Password
            };

            PlayFabClientAPI.LoginWithPlayFab(request, OnLogInSuccess, OnLogInError);
        }

        private void OnLogInSuccess(LoginResult loginResult)
        {
            Logger.Debug("LogIn Success");
            Debug.Log("LogIn Success");
            _onLogInSignal?.Raise();

        }

        private void OnLogInError(PlayFabError error)
        {
            Logger.Debug(error.GenerateErrorReport());
            Debug.Log(error.GenerateErrorReport());
            _popUpService.ShowPopUp(GameConstants.PopUps.LogInWindow);
        }
    }
}