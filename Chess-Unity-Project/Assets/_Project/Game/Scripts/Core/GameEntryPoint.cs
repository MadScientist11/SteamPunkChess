using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class GameEntryPoint : MonoBehaviour
    {
        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            
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