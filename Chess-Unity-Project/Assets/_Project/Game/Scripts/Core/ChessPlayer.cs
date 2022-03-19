﻿using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace SteampunkChess
{
    public class ChessPlayer : IInitializable
    {
        public Team Team { get; }
        private readonly ChessBoard _board;
        public List<ChessPiece> ActivePieces { get; }

        public bool CanLeftSideCastle { get; set; }
        public bool CanRightSideCastle { get; set; }

        public ChessPlayer(Team team, ChessBoard board)
        {
            ActivePieces = new List<ChessPiece>(16);
            _board = board;
            Team = team;
        }

        public void Initialize()
        {
            GetPlayerPieces();
        }

        private void GetPlayerPieces()
        {
            for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
                if (_board[x, y] != null && _board[x, y].Team == Team)
                    AddPiece(_board[x, y]);
        }

        public void AddPiece(ChessPiece piece)
        {
            if (!ActivePieces.Contains(piece))
                ActivePieces.Add(piece);
        }

        public void RemovePiece(ChessPiece piece)
        {
            if (ActivePieces.Contains(piece))
                ActivePieces.Remove(piece);
        }

        public ChessPiece[] GetPiecesOfType<T>() where T : ChessPiece
        {
            return ActivePieces.Where(p => p is T).ToArray();
        }

        public List<ChessPiece> GetActivePieces()
        {
            return ActivePieces;
        }
    }
}