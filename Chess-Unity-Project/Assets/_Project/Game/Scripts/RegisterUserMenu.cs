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
        private INetworkService _networkService;
        private readonly System.Random _random = new System.Random();
        
        [Inject]
        private void Construct(IPopUpService popUpService, INetworkService networkService)
        {
            _networkService = networkService;
            _popUpService = popUpService;
        }

        public void ContinueAsGuest()
        {
            _popUpService.HideAll(HideType.HideDestroyAndRelease);
            Prefs.Username = $"Guest{GenerateGuestIndex()}";
            _networkService.LocalPlayer.PlayerName = Prefs.Username;
            
            SwitchToMainMenu();
        }

        public void SwitchToMainMenu()
        {
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
