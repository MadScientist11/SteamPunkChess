using Zenject;

namespace SteampunkChess
{
    public class GameFactory : IGameFactory
    {
        private readonly IInstantiator _instantiator;

        public ChessGame CachedGame { get; private set; }

        public GameFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }
        
        public ChessGame Create()
        {
            if (CachedGame == null)
            {
                ChessGame chessGame = _instantiator.Instantiate<ChessGame>();
                CachedGame = chessGame;
                
            }
             
            return CachedGame;
        }
    }

    public interface IGameFactory
    {
        public ChessGame CachedGame { get;  }
        ChessGame Create();
    }
}