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
   

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
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