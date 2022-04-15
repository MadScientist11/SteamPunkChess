using SteampunkChess.PopUps;
using SteampunkChess.PopUpService;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace SteampunkChess
{
    public class SignInMenu : MonoBehaviour
    {
        private IPopUpService _popUpService;
        private PlayFabPlayerData _playFabPlayerData;
        
        private readonly System.Random _random = new System.Random();

        [Inject]
        private void Construct(IPopUpService popUpService, PlayFabPlayerData playFabPlayerData)
        {
            _playFabPlayerData = playFabPlayerData;
            _popUpService = popUpService;
        }

        public void ContinueAsGuest()
        {
            _popUpService.HideAll(HideType.HideDestroyAndRelease);
            
            int index = GenerateGuestIndex();
            _playFabPlayerData.PlayerName = $"Guest{index}";
            _playFabPlayerData.PlayerScore = 0;
            
            LoadMainMenu();
        }
        
        public void LoadMainMenu()
        {
            Addressables.LoadSceneAsync("MainMenu");
        }

        private int GenerateGuestIndex()
        {
            int _min = 1000;
            int _max = 9999;
            return _random.Next(_min, _max);
        }
    }
}
