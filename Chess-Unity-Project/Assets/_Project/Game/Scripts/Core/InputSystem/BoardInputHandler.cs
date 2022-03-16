using SteampunkChess;
using UnityEngine;
using Zenject;

namespace SteamPunkChess
{
    public class BoardInputHandler : MonoBehaviour, IInputHandler<GameObject>
    {
        private ChessBoard _chessBoard;
        
        [Inject]
        private void Initialize(ChessBoard chessBoard)
        {
            _chessBoard = chessBoard;
        }

        public void ProcessInput(GameObject obj)
        {
           _chessBoard.OnTileHover(obj);
        }
    }
}