using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class BoardFactory : IBoardFactory
    {
        private readonly DiContainer _diContainer;
        private readonly ChessBoardInfoSO _boardInfoSO;
        
        public BoardFactory(DiContainer diContainer, ChessBoardInfoSO boardInfoSO)
        {
            _diContainer = diContainer;
            _boardInfoSO = boardInfoSO;
        }
        
        public ChessBoard Create()
        {
            ChessBoard board = _diContainer.Instantiate<ChessBoard>();
            GameObject boardGO = Object.Instantiate(_boardInfoSO.boardPrefab);
            boardGO.GetComponent<BoardInputHandler>().Initialize(board);
            return board;
        }
    }

    public interface IBoardFactory
    {
        ChessBoard Create();
    }
}

