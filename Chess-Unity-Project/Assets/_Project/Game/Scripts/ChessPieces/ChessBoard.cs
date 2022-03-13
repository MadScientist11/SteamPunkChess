using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace SteampunkChess
{
    public class ChessBoard : IInitializable
    {
        private TileSet _tileSet;
        private readonly string _gameFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        private PieceArrangement _pieceArrangement;
        
        protected ChessPiece ActivePiece;
        private ChessBoardInfoSO _chessBoardInfoSo;


        public ChessBoard(ChessBoardInfoSO chessBoardInfoSO, PiecesPrefabsSO piecesPrefabsSO)
        {
            _chessBoardInfoSo = chessBoardInfoSO;
            _tileSet = new TileSet(_chessBoardInfoSo);
            _pieceArrangement = new PieceArrangement(_gameFen, _chessBoardInfoSo, piecesPrefabsSO);
           
        }

        public void Initialize()
        {
            Object.Instantiate(_chessBoardInfoSo.boardPrefab);
            _tileSet.Initialize();
            _pieceArrangement.Initialize();
        }
        
        public void OnTileHover(GameObject tile)
        {
            Vector2Int hitPosition = _tileSet.LookupTileIndex(tile);
            
            ChessPiece cp = _pieceArrangement[hitPosition.x, hitPosition.y];

            bool onTIleSelected = Input.GetMouseButtonUp(0);
            if (onTIleSelected)
            {
                if (ActivePiece != null)
                {
                    if (cp != null)
                        OnSelectPieceAndShowAvailableMoves(hitPosition);
                    else 
                        OnMoveTo(hitPosition);
                }
                else
                {
                    if (cp != null)
                        OnSelectPieceAndShowAvailableMoves(hitPosition);
                }
            }
        }
        
        protected void OnSelectPieceAndShowAvailableMoves(Vector2Int hitPosition)
        {
            ActivePiece = _pieceArrangement[hitPosition.x, hitPosition.y];

        }
        
        protected void OnMoveTo(Vector2Int moveTo)
        {
            _pieceArrangement[ActivePiece.CurrentX, ActivePiece.CurrentY] = null;
            _pieceArrangement[moveTo.x, moveTo.y] = ActivePiece;
            ActivePiece.PositionPiece(moveTo.x, moveTo.y);
            ActivePiece = null;
        }
        

    }

    public class PieceArrangement : IInitializable
    {
        private ChessPiece[,] _chessPieces;

        private readonly string _gameFen;
        private ChessBoardInfoSO _chessBoardInfoSO;
        private PiecesPrefabsSO _piecesPrefabsSO;

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
            _chessPieces = ChessGameUtility.SpawnAllPieces(_gameFen, (_chessBoardInfoSO.boardSizeX, _chessBoardInfoSO.boardSizeY), _piecesPrefabsSO);
            PositionPieces();
            Debug.Log("Spawn");
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