using System.Collections.Generic;
using System.Linq;

namespace SteampunkChess
{
    public class ChessPlayer : IInitializable
    {
        public Team Team { get; }
        private readonly ChessBoard _board;
        private readonly List<ChessPiece> _activePieces;
        public IReadOnlyList<ChessPiece> ActivePieces => _activePieces;
        
        public bool CanLeftSideCastle { get; set; }
        public bool CanRightSideCastle { get; set; }

        public float PlayerRemainingTime { get; set; }

        

        public ChessPlayer(Team team, ChessBoard board, float matchTime)
        {
            _activePieces = new List<ChessPiece>(32);
            Team = team;
            _board = board;
            PlayerRemainingTime = matchTime;
        }

        public void Initialize()
        {
            //GetPlayerPieces();
            ChessPiece.OnPieceSpawned += AddPiece;
            ChessPiece.OnPieceDestroyed += RemovePiece;
        }

       
        private void GetPlayerPieces()
        {
            for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
                if (_board[x, y] != null && _board[x, y].Team == Team)
                    AddPiece(_board[x, y]);
        }

        private void AddPiece(ChessPiece piece)
        {
            if (!_activePieces.Contains(piece) && piece.Team == Team)
            {
                _activePieces.Add(piece);
            }
                
        }

        public IEnumerable<ChessPiece> GetPiecesOfType<T>() where T : ChessPiece
        {
            return ActivePieces.Where(p => p is T);
        }
        
        
        private void RemovePiece(ChessPiece piece)
        {
            if (_activePieces.Contains(piece))
                _activePieces.Remove(piece);
        }

       
    }
}