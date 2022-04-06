using SteampunkChess.NetworkService;
using SteampunkChess.PopUps;
using SteampunkChess.PopUpService;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class RegisterUserMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenu;
        
        private IPopUpService _popUpService;
        
        private readonly System.Random _random = new System.Random();
        private PlayFabPlayerData _playFabPlayerData;
        private INetworkService _networkService;

        [Inject]
        private void Construct(IPopUpService popUpService, PlayFabPlayerData playFabPlayerData, INetworkService networkService)
        {
            _networkService = networkService;
            _playFabPlayerData = playFabPlayerData;
            _popUpService = popUpService;
        }

        public void ContinueAsGuest()
        {
            _popUpService.HideAll(HideType.HideDestroyAndRelease);
            
            int index = GenerateGuestIndex();
            _playFabPlayerData.PlayerName = $"Guest{index}";
            _playFabPlayerData.PlayerScore = 0;
            
            SwitchToMainMenu();
        }

        public void SwitchToMainMenu()
        {
            Debug.LogError("Switch to menu");
            gameObject.SetActive(false);
            _mainMenu.SetActive(true);
        }

        private int GenerateGuestIndex()
        {
            int _min = 1000;
            int _max = 9999;
            return _random.Next(_min, _max);
        }
    }
}
