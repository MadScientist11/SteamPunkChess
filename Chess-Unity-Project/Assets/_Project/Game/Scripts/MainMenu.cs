using System.Collections.Generic;
using SteampunkChess.CloudService;
using SteampunkChess.NetworkService;
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

        [Inject]
        private void Construct(INetworkService networkService, ICloudService cloudService, IPopUpService popUpService)
        {
            _popUpService = popUpService;
            _cloudService = cloudService;
            _networkService = networkService;
        }
        
        public void OnPlay()
        {
            if (_networkService.OfflineMode)
            {
                _popUpService.ShowPopUp(GameConstants.PopUps.ErrorToast, "Cannot load lobby, check internet connection and restart the game");
                return;
                
            }
            else if (!_cloudService.IsLoggedIn)
            {
                _popUpService.ShowPopUp(GameConstants.PopUps.ErrorToast, "You are not logged in, progress will not be saved!");
            }
            
            _lobbyScene.LoadSceneAsync();
        }

        public void IncreasePlayerScore()
        {
            _cloudService.UpdateUserData(new Dictionary<string, string>()
            {
                [GameConstants.PlayerDataKeys.PlayerScoreKey]= "51",
            });
        }
        
    }
}
