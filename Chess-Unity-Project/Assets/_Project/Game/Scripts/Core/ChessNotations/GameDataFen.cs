using Sirenix.OdinInspector;
using UnityEngine;

namespace SteamPunkChess
{
    [System.Serializable]
    public class GameDataFen
    {
        [ShowInInspector]
        public PieceInfo[][] piecesInfo;
        public string parseFenError;
        public int WhoseTurn;
        public bool CanWhiteCastleKingSide;
        public bool CanWhiteCastleQueenSide;
        public bool CanBlackCastleKingSide;
        public bool CanBlackCastleQueenSide;
        public Vector2Int EnPassant;
        public int HalfMoveClock;
        public int FullMoveNumber;
    }
}