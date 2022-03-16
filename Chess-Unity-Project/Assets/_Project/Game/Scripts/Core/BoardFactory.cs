using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class BoardFactory : MonoBehaviour
    {
        public static DiContainer DIContainer;
  

        public static ChessBoard CreateBoard()
        {
            ChessBoard board = DIContainer.Instantiate<ChessBoard>();
            return board;
        }

        public static void CreateBoardPrefab(GameObject boardPrefab)
        {
            DIContainer.InstantiatePrefab(boardPrefab);
        }
    }
}

