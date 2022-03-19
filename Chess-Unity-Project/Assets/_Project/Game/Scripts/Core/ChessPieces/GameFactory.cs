using Zenject;

namespace SteampunkChess
{
    public class GameFactory : IGameFactory
    {
        private readonly IInstantiator _instantiator;

        public GameFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }
        
        public ChessGame Create()
        {
            ChessGame board = _instantiator.Instantiate<ChessGame>();
            return board;
        }
    }

    public interface IGameFactory
    {
        ChessGame Create();
    }
}