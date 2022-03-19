using UnityEngine;

namespace SteampunkChess
{
    public class BoardInputHandler : MonoBehaviour, IInputHandler<GameObject>, IInitializable<ChessBoard>
    {
        private ChessBoard _chessBoard;
        
        
        public void Initialize(ChessBoard chessBoard)
        {
            _chessBoard = chessBoard;
        }

        public void ProcessInput(GameObject obj)
        {
           _chessBoard.OnTileHover(obj);
        }

    }
}