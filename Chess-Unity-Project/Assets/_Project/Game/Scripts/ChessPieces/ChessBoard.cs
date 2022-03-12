using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteampunkChess
{
    public class ChessBoard
    {
        
        
        private TileSet _tileSet;
        
        private readonly string _gameFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
       

        

        public ChessBoard(ChessBoardInfoSO chessBoardInfoSO, PiecesPrefabsSO piecesPrefabsSO)
        {
            Object.Instantiate(chessBoardInfoSO.boardPrefab);
            _tileSet = new TileSet(chessBoardInfoSO);
            PieceArrangement pieceArrangement = new PieceArrangement(_gameFen, chessBoardInfoSO, piecesPrefabsSO);
            pieceArrangement.PositionPieces();
        }

        public void Initialize()
        {
            
        }

    }

    public class PieceArrangement
    {
        private readonly ChessPiece[,] _chessPieces;
  
        private ChessBoardInfoSO _chessBoardInfoSO;
        private PiecesPrefabsSO _piecesPrefabsSO;

        public PieceArrangement(string gameFen, ChessBoardInfoSO chessBoardInfoSO, PiecesPrefabsSO piecesPrefabsSO) //TODO: Class for chess notation, to interchange them
        {
            _chessBoardInfoSO = chessBoardInfoSO;
            _piecesPrefabsSO = piecesPrefabsSO;
            _chessPieces = ChessGameUtility.SpawnAllPieces(gameFen, (_chessBoardInfoSO.boardSizeX, _chessBoardInfoSO.boardSizeY), _piecesPrefabsSO);

        }
        

        public ChessPiece this[int x, int y]
        {
            get => _chessPieces[x, y];
            set => _chessPieces[x, y] = value;
        }
        
        public void PositionPieces()
        {
            for (int x = 0; x < _chessBoardInfoSO.boardSizeX; x++)
            for (int y = 0; y < _chessBoardInfoSO.boardSizeY; y++)
                if (this[x, y] != null)
                    this[x, y].PositionPiece(x, y, true);
        }

      
        
        
        
        
    }
    
    
}