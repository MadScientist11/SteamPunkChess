using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteampunkChess
{
    public class EnPassant : ISpecialMove
    {
        private readonly List<Movement> _moveHistory;
        private readonly PieceArrangement _pieceArrangement;
        
       public EnPassant(List<Movement> moveHistory, PieceArrangement pieceArrangement)
       {
           _moveHistory = moveHistory;
           _pieceArrangement = pieceArrangement;
       }

        public Task ProcessSpecialMove()
        {
            Movement newMove = _moveHistory[_moveHistory.Count - 1];
            ChessPiece myPawn = _pieceArrangement[newMove.Destination.x, newMove.Destination.y];
            Movement targetPawnPos = _moveHistory[_moveHistory.Count - 2];
            ChessPiece targetPawn = _pieceArrangement[targetPawnPos.Destination.x, targetPawnPos.Destination.y];
            if (myPawn.CurrentX == targetPawn.CurrentX)
            {
                if (myPawn.CurrentY == targetPawn.CurrentY - 1 || myPawn.CurrentY == targetPawn.CurrentY + 1)
                {
                    _pieceArrangement[targetPawn.CurrentX, targetPawn.CurrentY] = null;
                    targetPawn.Dispose();
                }
            }
            return Task.CompletedTask;
        }
    }
}