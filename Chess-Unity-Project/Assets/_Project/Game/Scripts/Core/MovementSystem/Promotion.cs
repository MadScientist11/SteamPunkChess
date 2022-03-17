using System.Collections.Generic;
using UnityEngine;


namespace SteampunkChess
{
    public class Promotion : ISpecialMove
    {
        private readonly List<Movement> _moveList;
        private readonly PieceArrangement _pieceArrangement;

        //public SpecialMoveStringBuilder GetStringBuilder(MoveInfo moveInfo)
        //{
        //    return new EnPassantSpecialMoveStringBuilder(moveInfo);
        //}
        public Promotion(List<Movement> moveList, PieceArrangement pieceArrangement)
        {
            _moveList = moveList;
            _pieceArrangement = pieceArrangement;
        }
        
        //TODO: Choose promotion peace
        public void ProcessSpecialMove()
        {
            Movement lastMove = _moveList[_moveList.Count - 1];
            ChessPiece targetPawn = _pieceArrangement[lastMove.Destination.x, lastMove.Destination.y];
            Debug.Log("Promotion");

            if (targetPawn.Team == Team.White && lastMove.Destination.y == 7)
            {
                ChessPiece promQueen = _pieceArrangement.SpawnSinglePiece(ChessPieceType.Queen, Team.White);
                targetPawn.Dispose();
                _pieceArrangement[lastMove.Destination.x, lastMove.Destination.y] = promQueen;
                promQueen.PositionPiece(lastMove.Destination.x, lastMove.Destination.y, true);
            }
            else if (targetPawn.Team == Team.Black && lastMove.Destination.y == 0)
            {
                ChessPiece promQueen = _pieceArrangement.SpawnSinglePiece(ChessPieceType.Queen, Team.Black);
                targetPawn.Dispose();
                _pieceArrangement[lastMove.Destination.x, lastMove.Destination.y] = promQueen;
                promQueen.PositionPiece(lastMove.Destination.x, lastMove.Destination.y, true);
            }
        }
        //public SpecialMoveStringBuilder GetStringBuilder(MoveInfo moveInfo)
        //{
        //    return new PromotionSpecialMoveStringBuilder(moveInfo);
        //}

        public void ProcessSpecialMove(List<Movement> moveList, PieceArrangement pieceArrangement)
        {
            throw new System.NotImplementedException();
        }
    }
    
}