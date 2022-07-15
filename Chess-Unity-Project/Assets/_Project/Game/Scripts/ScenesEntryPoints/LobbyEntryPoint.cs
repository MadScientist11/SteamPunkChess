using System;
using SteampunkChess.NetworkService;
using SteampunkChess.PopUps;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace SteampunkChess
{
    public class LobbyEntryPoint : MonoBehaviour
    {
        private Lobby _lobby;
        private INetworkService _networkService;
        private IInputSystem _inputSystem;

        [Inject]
        private void Construct(Lobby lobby, INetworkService networkService, IInputSystem inputSystem)
        {
            _inputSystem = inputSystem;
            _networkService = networkService;
            _lobby = lobby;
        }

        private void Awake()
        {
            _lobby.Initialize();
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
