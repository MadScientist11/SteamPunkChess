using System;
using SteampunkChess.NetworkService;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace SteampunkChess
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingUI;
        private IInputSystem _inputSystem;
        private INetworkService _networkService;

        [Inject]
        private void Construct(IInputSystem inputSystem, INetworkService networkService)
        {
            _networkService = networkService;
            _inputSystem = inputSystem;
        }
        public void Disable()
        {
            gameObject.SetActive(false);
            _loadingUI.SetActive(false);
        }

        public void SwitchToLoading()
        {
            Disable();
            _loadingUI.SetActive(true);
        }
        
        public void SwitchToLobby()
        {
            Disable();
            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            _inputSystem.OnBackButtonPressed = () =>
            {
                Addressables.LoadSceneAsync("MainMenu");
                _networkService.LeaveRoom();
            };
        }
    }
}
