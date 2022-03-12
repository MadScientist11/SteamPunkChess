using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteampunkChess
{
    public class ChessBoard
    {
        private ChessBoardInfoSO _chessBoardInfoSO;
        private PiecesPrefabsSO _piecesPrefabsSO;
        
        private readonly ChessPiece[,] _chessPieces;
        private TileSet _tileSet;
        
        
        public ChessPiece this[int x, int y]
        {
            get => _chessPieces[x, y];
            set => _chessPieces[x, y] = value;
        }

        private readonly string _gameFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        public ChessBoard(ChessBoardInfoSO chessBoardInfoSO)
        {
            Debug.Log(chessBoardInfoSO);
            Object.Instantiate(chessBoardInfoSO.boardPrefab);
            _tileSet = new TileSet(chessBoardInfoSO);
            //_chessPieces = ChessGameUtility.SpawnAllPieces(_gameFen, (_chessBoardInfoSO.boardSizeX, _chessBoardInfoSO.boardSizeY), _piecesPrefabsSO);
        }

        public void Initialize()
        {
            
        }

    }
}