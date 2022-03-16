using System.Threading.Tasks;
using UnityEngine;

namespace SteampunkChess
{
    public class Movement
    {
        public Vector2Int Start { get; }
        public Vector2Int Destination { get; }

        public bool IsAttackMove => 
            _pieceArrangement[Destination.x, Destination.y] != null || _specialMove is EnPassant;


        private readonly PieceArrangement _pieceArrangement;

        private readonly ISpecialMove _specialMove;

        public Movement(Vector2Int start, Vector2Int destination, PieceArrangement pieceArrangement)
        {
            Start = start;
            Destination = destination;
            _pieceArrangement = pieceArrangement;
        }
        
        public Movement(Vector2Int start, Vector2Int destination, PieceArrangement pieceArrangement, ISpecialMove specialMove)
        {
            Start = start;
            Destination = destination;
            _specialMove = specialMove;
            _pieceArrangement = pieceArrangement;
        }
     

        public async Task Process()
        {
           
            ChessPiece pieceToMove = _pieceArrangement[Start.x, Start.y];
            ChessPiece enemy = _pieceArrangement[Destination.x, Destination.y];
           
            _pieceArrangement[Start.x, Start.y] = null;
            _pieceArrangement[Destination.x, Destination.y] = pieceToMove;
            await pieceToMove.PositionPiece(Destination.x, Destination.y);
            _specialMove?.ProcessSpecialMove();
            enemy?.Dispose();
        }
        
        public async void ProcessBackwards()
        {
            ChessPiece pieceToMove = _pieceArrangement[Start.x, Start.y];
            _pieceArrangement[Start.x, Start.y] = null;
            _pieceArrangement[Destination.x, Destination.y] = pieceToMove;
            await pieceToMove.PositionPiece(Destination.x, Destination.y);
        }
    }
}