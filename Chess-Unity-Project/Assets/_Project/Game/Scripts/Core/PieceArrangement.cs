using System;
using System.Collections.Generic;
using SteamPunkChess;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace SteampunkChess
{
    public class PieceArrangement : IInitializable
    {
        private ChessPiece[,] _chessPieces;

        private readonly string _gameFen;
        private ChessBoardInfoSO _chessBoardInfoSO;
        private PiecesPrefabsSO _piecesPrefabsSO;
        
        private const string PiecesParent = "Pieces";
        private GameObject _piecesParentGO;

        private readonly Dictionary<ChessPieceType, Func<ChessPiece>> Pieces =
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

        public PieceArrangement(string gameFen, ChessBoardInfoSO chessBoardInfoSO, PiecesPrefabsSO piecesPrefabsSO) //TODO: Class for chess notation, to interchange them
        {
            _gameFen = gameFen;
            _chessBoardInfoSO = chessBoardInfoSO;
            _piecesPrefabsSO = piecesPrefabsSO;
        }
        

        public ChessPiece this[int x, int y]
        {
            get => _chessPieces[x, y];
            set => _chessPieces[x, y] = value;
        }
        
        public void Initialize()
        {
            _chessPieces = SpawnAllPieces(_gameFen, (_chessBoardInfoSO.boardSizeX, _chessBoardInfoSO.boardSizeY), _piecesPrefabsSO);
            PositionPieces();
            Debug.Log("Spawn");
        }
        
        private ChessPiece[,] SpawnAllPieces(string gameFen, (int boardSizeX, int boardSizeY) chessBoardSize, PiecesPrefabsSO piecesPrefabsSO)
        {
            GameDataFen fenData = FenUtility.GameFenDataFromStringFen(gameFen);
            ChessPiece[,] chessPieces = new ChessPiece[chessBoardSize.boardSizeX, chessBoardSize.boardSizeY];
            _piecesParentGO = new GameObject(PiecesParent);

            for (int x = 0; x < chessBoardSize.boardSizeX; x++)
            for (int y = 0; y < chessBoardSize.boardSizeY; y++)
                if (fenData.piecesInfo[x][y] != null)
                    chessPieces[x, y] = SpawnSinglePiece(fenData.piecesInfo[x][y].type, fenData.piecesInfo[x][y].team);
                    
                

            return chessPieces;
        }
        
        public ChessPiece SpawnSinglePiece(ChessPieceType type, Team team)
        {
            GameObject pieceGO = Object.Instantiate(_piecesPrefabsSO.piecesPrefabs[(int)type - 1], _piecesParentGO.transform);
            ChessPiece cp = Pieces[type].Invoke();
            cp.ChessType = type;
            cp.Team = team;
            cp.PieceTransform = pieceGO.transform;

        
            if (cp is Knight && team == Team.Black)
                cp.PieceTransform.rotation = Quaternion.AngleAxis(180f, cp.PieceTransform.up);
            
            return cp;
        }
        
        private void PositionPieces()
        {
            for (int x = 0; x < _chessBoardInfoSO.boardSizeX; x++)
            for (int y = 0; y < _chessBoardInfoSO.boardSizeY; y++)
                if (this[x, y] != null)
                    this[x, y].PositionPiece(x, y, true);
        }
    }
}