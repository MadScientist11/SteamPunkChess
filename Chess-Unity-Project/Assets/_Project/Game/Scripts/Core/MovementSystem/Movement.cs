using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SteampunkChess
{
    [Serializable]
    public class Movement
    {
        [field: SerializeField] public Vector2Int Start { get; }
        
        [field: SerializeField] public Vector2Int Destination { get; }
        
        public ChessPiece MovePiece { get; }

        public bool IsAttackMove { get; }

        public PieceArrangement _pieceArrangement;
        private readonly ISpecialMove _specialMove;
        

        public Movement(Vector2Int start, Vector2Int destination, PieceArrangement pieceArrangement)
        {
            Start = start;
            Destination = destination;
            _pieceArrangement = pieceArrangement;
            MovePiece = pieceArrangement[Start.x, Start.y];
            IsAttackMove = _pieceArrangement[Destination.x, Destination.y] != null || _specialMove is EnPassant;
        }
        
        public Movement(Vector2Int start, Vector2Int destination, PieceArrangement pieceArrangement, ISpecialMove specialMove)
        {
            Start = start;
            Destination = destination;
            _specialMove = specialMove;
            _pieceArrangement = pieceArrangement;
            MovePiece = pieceArrangement[Start.x, Start.y];
            IsAttackMove = _pieceArrangement[Destination.x, Destination.y] != null || _specialMove is EnPassant;
        }
     

        public async Task Process()
        {
            ChessPiece pieceToMove = MovePiece;
            ChessPiece enemy = _pieceArrangement[Destination.x, Destination.y];
            _pieceArrangement[Start.x, Start.y] = null;
            _pieceArrangement[Destination.x, Destination.y] = pieceToMove;
            await pieceToMove.PositionPiece(Destination.x, Destination.y);
            _specialMove?.ProcessSpecialMove();
            enemy?.Dispose();
        }

        public string GetMovePGN()
        {
            return MoveStringBuilder.Init(this).WithSpecialMove(_specialMove).Build();
        }
        
        
        public async void ProcessBackwards()
        {
            ChessPiece pieceToMove = _pieceArrangement[Start.x, Start.y];
            _pieceArrangement[Start.x, Start.y] = null;
            _pieceArrangement[Destination.x, Destination.y] = pieceToMove;
            await pieceToMove.PositionPiece(Destination.x, Destination.y);
        }
        
        protected bool Equals(Movement other)
        {
            return Start.Equals(other.Start) && Destination.Equals(other.Destination);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Movement) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Start.GetHashCode() * 397) ^ Destination.GetHashCode();
            }
        }
    }
    
    public class MoveStringBuilder
    {
        private readonly string[,] _pieces = 
        {
            {"", "", "\u265C", "\u265E", "\u265D", "\u265B", "\u265A"},

            {"", "", "\u2656", "\u2658", "\u2657", "\u2655", "\u2654"}
        };
        private readonly char[] _boardChars =
        {
            'a','b','c','d','e','f','g','h'
        };
        private readonly Movement _move;
       
        private ISpecialMove _specialMove;

        public MoveStringBuilder(Movement move)
        {
            _move = move;
        }

        public static MoveStringBuilder Init(Movement move)
        {
            return new MoveStringBuilder(move);
        }

        public MoveStringBuilder WithSpecialMove(ISpecialMove specialMove)
        {
            _specialMove = specialMove;
            return this;
        }
        
        public string Build()
        {
            if (_specialMove is Castling)
            {
                if (_move.Destination.x == 2)
                    return "O-O-O ";
                if (_move.Destination.x == 6)
                    return "O-O";
            }
            else if (_specialMove is Promotion)
            {
                return BuildPromotionString();
            }
            

            return BuildGenericString();
        }
        
        private string BuildGenericString()
        {
            return $"<size=30>{GetUnicodeOfPiece(_move.MovePiece.ChessType, _move.MovePiece.Team)}</size>" +
                   $"{IsAnyPieceEaten(_move.MovePiece, _move.Start, _move.IsAttackMove)}" +
                   $"{GetBoardMoveNamingFromPosition(_move.Destination.x, _move.Destination.y)} ";
        }
        private string BuildPromotionString()
        {
            return $"{IsAnyPieceEaten(_move.MovePiece, _move.Start, _move.IsAttackMove)}" +
                   $"{GetBoardMoveNamingFromPosition(_move.Destination.x, _move.Destination.y)} = <size=30>" +
                   $"{GetUnicodeOfPiece(ChessPieceType.Queen, _move.MovePiece.Team)}</size> ";
        }
        
        private string GetBoardMoveNamingFromPosition(int x, int y)
        {
            return $"{GetXBoardChar(x)}{y + 1}";
        }
        private string GetXBoardChar(int x)
        {
            return _boardChars[x].ToString();
        }

        private string GetUnicodeOfPiece(ChessPieceType type, Team team)
        {
            return _pieces[(int)team, (int)type];
        }

        private string IsAnyPieceEaten(ChessPiece piece, Vector2Int previousPosition, bool isEaten)
        {
            if(!isEaten) return "";

            if (piece.ChessType == ChessPieceType.Pawn)
            {
                return GetXBoardChar(previousPosition.x) + "x";
            }

            return "x";
        }

    }
}