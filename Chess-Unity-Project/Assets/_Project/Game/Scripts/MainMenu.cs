using System;
using SteampunkChess.CloudService;
using SteampunkChess.NetworkService;
using SteampunkChess.PopUps;
using SteampunkChess.PopUpService;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace SteampunkChess
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private AssetReference _lobbyScene;
        private INetworkService _networkService;
        private ICloudService _cloudService;
        private IPopUpService _popUpService;
        private IInputSystem _inputSystem;

        [Inject]
        private void Construct(INetworkService networkService, ICloudService cloudService, IPopUpService popUpService, IInputSystem inputSystem)
        {
            _inputSystem = inputSystem;
            _popUpService = popUpService;
            _cloudService = cloudService;
            _networkService = networkService;
        }
        
        public void SwitchToLobby()
        {
            if (_networkService.OfflineMode)
            {
                _popUpService.ShowPopUp(GameConstants.PopUps.ErrorToast, "Cannot load lobby, check internet connection and restart the game");
                return;
                
            } 
            if (!_cloudService.IsLoggedIn)
            {
                _popUpService.ShowPopUp(GameConstants.PopUps.ErrorToast, "You are not logged in, progress will not be saved!");
            }

            _inputSystem.OnBackButtonPressed += BackFromLobby;

            _lobbyScene.LoadSceneAsync();
        }

        public void SwitchToSettings()
        {
            gameObject.SetActive(false);
            _popUpService.ShowPopUp(GameConstants.PopUps.SettingsPopUp, Array.Empty<int>());
            _inputSystem.OnBackButtonPressed += BackFromSettings;

        }

        private void BackFromLobby()
        {
            Addressables.LoadSceneAsync("MainMenu");
        }

        private void BackFromSettings()
        {
            _popUpService.HidePopUp(GameConstants.PopUps.SettingsPopUp, HideType.HideDestroyAndRelease);
            gameObject.SetActive(true);
        }
    }
}
