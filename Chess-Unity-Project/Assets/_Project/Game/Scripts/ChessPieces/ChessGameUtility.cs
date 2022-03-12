using System;
using System.Collections.Generic;
using SteamPunkChess;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SteampunkChess
{
    public static class ChessGameUtility
    {
        private const string PiecesParent = "PiecesParent";

        private static readonly Dictionary<ChessPieceType, Func<ChessPiece>> Pieces =
            new Dictionary<ChessPieceType, Func<ChessPiece>>()
            {
                { ChessPieceType.None, null },
                { ChessPieceType.Pawn, () => new Pawn() },
                { ChessPieceType.Rook, () => new Rook() },
                { ChessPieceType.Knight, () => new Knight() },
                { ChessPieceType.Bishop, () => new Bishop() },
                { ChessPieceType.Queen, () => new Queen() },
                { ChessPieceType.King, () => new King() },
            };
        
        public static ChessPiece[,] SpawnAllPieces(string gameFen, (int boardSizeX, int boardSizeY) chessBoardSize, PiecesPrefabsSO piecesPrefabsSO)
        {
            GameDataFen fenData = FenUtility.GameFenDataFromStringFen(gameFen);
            ChessPiece[,] chessPieces = new ChessPiece[chessBoardSize.boardSizeX, chessBoardSize.boardSizeY];
            GameObject piecesParent = new GameObject(PiecesParent);

            for (int x = 0; x < chessBoardSize.boardSizeX; x++)
            for (int y = 0; y < chessBoardSize.boardSizeY; y++)
                if (fenData.piecesInfo[x][y] != null)
                {
                    chessPieces[x, y] = SpawnSinglePiece(fenData.piecesInfo[x][y].type, fenData.piecesInfo[x][y].team,
                        piecesPrefabsSO, piecesParent.transform);
                    
                }

            return chessPieces;
        }
        
        private static ChessPiece SpawnSinglePiece(ChessPieceType type, Team team, PiecesPrefabsSO piecesPrefabsSO, Transform piecesParent)
        {
             GameObject pieceGO = Object.Instantiate(piecesPrefabsSO.piecesPrefabs[(int)type - 1], piecesParent);
             ChessPiece cp = Pieces[type].Invoke();
             cp.ChessType = type;
             cp.Team = team;
             cp.PieceTransform = pieceGO.transform;

        
            if (cp is Knight && team == Team.Black)
                cp.PieceTransform.rotation = Quaternion.AngleAxis(180f, cp.PieceTransform.up);
            
            return cp;
        }
    }
}