using Sirenix.OdinInspector;
using UnityEngine;

namespace SteampunkChess
{
    [System.Serializable]
    public class PieceArrangementData
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


        public FenNotationString ToFen()
        {
            return new FenNotationString(FenUtility.FenStringFromGameData(this));
        }
    }
}