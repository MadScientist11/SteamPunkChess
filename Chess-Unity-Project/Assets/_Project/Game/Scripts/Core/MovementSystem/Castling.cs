using System.Collections.Generic;
using UnityEngine;

namespace SteampunkChess
{
    public class Castling : ISpecialMove
    {
        private readonly List<Movement> _moveList;
        private readonly PieceArrangement _pieceArrangement;
        
        public Castling(List<Movement> moveList, PieceArrangement pieceArrangement)
        {
            _moveList = moveList;
            _pieceArrangement = pieceArrangement;
        }
        
        public void ProcessSpecialMove()
        {
            Movement lastMove = _moveList[_moveList.Count - 1];

            //Left rook
            if (lastMove.Destination.x == 2) // was king positioned?
            {
                if (lastMove.Destination.y == 0)
                {
                    ChessPiece rook = _pieceArrangement[0, 0];
                    _pieceArrangement[3, 0] = rook;
                    _pieceArrangement[3, 0].PositionPiece(3, 0, false, new ArcTweener(1,.65f));
                    _pieceArrangement[0, 0] = null;

                    
                }
                else if (lastMove.Destination.y == 7)
                {
                    ChessPiece rook = _pieceArrangement[0, 7];
                    _pieceArrangement[3, 7] = rook;
                    _pieceArrangement[3, 7].PositionPiece(3, 7, false, new ArcTweener(1,.65f));
                    _pieceArrangement[0, 7] = null;

                }
            }
            // Right rook
            else if (lastMove.Destination.x == 6)
            {
                if (lastMove.Destination.y == 0)
                {
                    ChessPiece rook = _pieceArrangement[7, 0];
                    _pieceArrangement[5, 0] = rook;
                    _pieceArrangement[5, 0].PositionPiece(5, 0, false, new ArcTweener(1,.65f));
                    _pieceArrangement[7, 0] = null;

                }
                else if (lastMove.Destination.y == 7)
                {
                    ChessPiece rook = _pieceArrangement[7, 7];
                    _pieceArrangement[5, 7] = rook;
                    _pieceArrangement[5, 7].PositionPiece(5, 7, false, new ArcTweener(1,.65f));
                    _pieceArrangement[7, 7] = null;
   
                }
            }
        }
    }
}