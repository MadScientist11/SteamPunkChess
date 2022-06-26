using System;
using SteampunkChess.NetworkService;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace SteampunkChess
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private Volume _postProcessingVolume;
        private IGameFactory _gameFactory;
        private INetworkService _networkService;
        private IInputSystem _inputSystem;


        [Inject]
        public void Construct(IGameFactory gameFactory, INetworkService networkService, IInputSystem inputSystem)
        {
            _inputSystem = inputSystem;
            _networkService = networkService;
            _gameFactory = gameFactory;
        }

        private void Awake()
        {
            _postProcessingVolume.enabled = Prefs.Settings.PostProcessing;
        }

        private void Start()
        {
            ChessGame game = _gameFactory.Create();
           game.Initialize();
           Debug.Log("Entry");
        }

        private void OnDisable()
        {
            _networkService.RoomCallbacksDispatcher.ClearRoomCallbackEvents();
            _networkService.LobbyCallbacksDispatcher.ClearLobbyCallbacksEvents();
            _inputSystem.OnCameraViewChanged = null;
        }
    }

    public interface IInitializable
    {
        void Initialize();
    }
    
    public interface IInitializable<T>
    {
        void Initialize(T param);
    }
}