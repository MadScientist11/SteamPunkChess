using Sirenix.OdinInspector;
using UnityEngine;

namespace SteamPunkChess
{
    [System.Serializable]
    public class GameData
    {
        [ShowInInspector]
        public PieceInfo[,] piecesInfo;
        public string parseFenError;
        public int whoseTurn;
        public bool canWhiteCastleKingSide;
        public bool canWhiteCastleQueenSide;
        public bool canBlackCastleKingSide;
        public bool canBlackCastleQueenSide;
        public Vector2Int enPassant;
        public int halfMoveClock;
        public int fullMoveNumber;
    }
}