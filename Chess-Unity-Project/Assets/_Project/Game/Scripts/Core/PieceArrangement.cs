using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SteampunkChess
{
    public class PieceArrangement : IInitializable
    {
        private ChessPiece[,] _chessPieces;

        private readonly NotationString _notationString;
        private readonly ChessBoardInfoSO _chessBoardInfoSO;
        private readonly PiecesPrefabsSO _piecesPrefabsSO;
        private readonly ChessPieceFactory _chessPieceFactory;

        private const string PiecesParentName = "Pieces";
        private Transform _piecesParent;

        public PieceArrangement(NotationString notationString, ChessBoardInfoSO chessBoardInfoSO, PiecesPrefabsSO piecesPrefabsSO, ChessPieceFactory chessPieceFactory) 
        {
            _notationString = notationString;
            _chessBoardInfoSO = chessBoardInfoSO;
            _piecesPrefabsSO = piecesPrefabsSO;
            _chessPieceFactory = chessPieceFactory;
            _chessPieces = new ChessPiece[chessBoardInfoSO.boardSizeX, chessBoardInfoSO.boardSizeY];
        }
        

        public ChessPiece this[int x, int y]
        {
            get => _chessPieces[x, y];
            set => _chessPieces[x, y] = value;
        }

        public PieceArrangement DeepCopy()
        {
            PieceArrangement pieceArrangement = new PieceArrangement(_notationString, _chessBoardInfoSO, _piecesPrefabsSO, _chessPieceFactory);
  
            for (int x = 0; x < _chessBoardInfoSO.boardSizeX; x++)
            {
                for (int y = 0; y < _chessBoardInfoSO.boardSizeY; y++)
                {
                    if (this[x, y] != null)
                    {
                        pieceArrangement[x, y] = this[x, y];
                    }
                }
            }

            return pieceArrangement;
        }
        
        public void Initialize()
        {
            _chessPieces = SpawnAllPieces(_notationString, (_chessBoardInfoSO.boardSizeX, _chessBoardInfoSO.boardSizeY));
            PositionPieces();
        }
        
        private ChessPiece[,] SpawnAllPieces(NotationString notationString, (int boardSizeX, int boardSizeY) chessBoardSize)
        {
            PieceArrangementData data = notationString.GameDataFromNotationString();
            ChessPiece[,] chessPieces = new ChessPiece[chessBoardSize.boardSizeX, chessBoardSize.boardSizeY];
            _piecesParent = new GameObject(PiecesParentName).transform;

            for (int x = 0; x < chessBoardSize.boardSizeX; x++)
            for (int y = 0; y < chessBoardSize.boardSizeY; y++)
                if (data.piecesInfo[x, y] != null)
                    chessPieces[x, y] = SpawnSinglePiece(data.piecesInfo[x, y].type, data.piecesInfo[x, y].team);
                    
                

            return chessPieces;
        }
        
        public ChessPiece SpawnSinglePiece(ChessPieceType pieceType, Team team)
        {
            var prefabIndex = ((int)pieceType - 1) + ((int)team * 6);
            var pieceGO = Object.Instantiate(_piecesPrefabsSO.piecesPrefabs[prefabIndex], _piecesParent);

            ChessPiece cp = _chessPieceFactory.CreatePiece(pieceType);
            
            cp.ChessType = pieceType;
            cp.Team = team;
            cp.PieceTransform = pieceGO.transform;
            cp.Initialize();
            
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