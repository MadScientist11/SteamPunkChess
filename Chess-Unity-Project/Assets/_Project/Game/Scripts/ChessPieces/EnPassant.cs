using System.Collections.Generic;

namespace SteampunkChess
{
    public class EnPassant : ISpecialMove
    {
        private readonly List<Movement> _moveList;
        private readonly PieceArrangement _pieceArrangement;
        
       public EnPassant(List<Movement> moveList, PieceArrangement pieceArrangement)
       {
           _moveList = moveList;
           _pieceArrangement = pieceArrangement;
       }

        public void ProcessSpecialMove()
        {
            Movement newMove = _moveList[_moveList.Count - 1];
            ChessPiece myPawn = _pieceArrangement[newMove.Destination.x, newMove.Destination.y];
            Movement targetPawnPos = _moveList[_moveList.Count - 2];
            ChessPiece targetPawn = _pieceArrangement[targetPawnPos.Destination.x, targetPawnPos.Destination.y];
            if (myPawn.CurrentX == targetPawn.CurrentX)
            {
                if (myPawn.CurrentY == targetPawn.CurrentY - 1 || myPawn.CurrentY == targetPawn.CurrentY + 1)
                {
                    _pieceArrangement[targetPawn.CurrentX, targetPawn.CurrentY] = null;
                    targetPawn.Dispose();
                }
            }
        }
    }
}