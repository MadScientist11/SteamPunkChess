using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace SteampunkChess
{
    //public class MockChessBoard : ChessBoard
    //{
    //    public MockChessBoard(ChessBoardInfoSO chessBoardInfoSO, PiecesPrefabsSO piecesPrefabsSO) : base(chessBoardInfoSO, piecesPrefabsSO)
    //    {
    //    }
//
    //    public MockChessBoard(TileSet tileSet, PieceArrangement pieceArrangement) : base(tileSet, pieceArrangement)
    //    {
    //    }
//
    //    public MockChessBoard(ChessBoard chessBoard) : base(chessBoard)
    //    {
    //    }
    //}
//
    public class ChessBoard : IInitializable
    {
        private TileSet _tileSet;
        private readonly string _gameFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        private PieceArrangement _pieceArrangement;

        protected ChessPiece ActivePiece;
        private ChessBoardInfoSO _chessBoardInfoSO;
        private List<Movement> _availableMoves;
        private List<Movement> _moveHistory;
        private TileSelection _tileSelection;
        private bool _canMove;


        public ChessBoard(ChessBoardInfoSO chessBoardInfoSO, PiecesPrefabsSO piecesPrefabsSO,
            TileSelectionInfoSO tileSelectionInfoSO)
        {
            _chessBoardInfoSO = chessBoardInfoSO;
            _moveHistory = new List<Movement>();
            _tileSelection = new TileSelection(tileSelectionInfoSO);
            _tileSet = new TileSet(_chessBoardInfoSO);
            _pieceArrangement = new PieceArrangement(_gameFen, _chessBoardInfoSO, piecesPrefabsSO);
        }


        private void DeepCopy()
        {
        }

        public void Initialize()
        {
            Object.Instantiate(_chessBoardInfoSO.boardPrefab);
            _canMove = true;
            _tileSet.Initialize();
            _pieceArrangement.Initialize();
            _tileSelection.Initialize();
        }

        public void OnTileHover(GameObject tile)
        {
            Vector2Int hitPosition = _tileSet.LookupTileIndex(tile);
            ChessPiece cp = _pieceArrangement[hitPosition.x, hitPosition.y];

            bool onTileSelected = Input.GetMouseButtonUp(0);
            if (onTileSelected)
            {
                if (ActivePiece != null && _canMove)
                {
                    if (cp != null && ActivePiece.IsFromSameTeam(cp))
                    {
                        OnSelectPieceAndShowAvailableMoves(hitPosition);
                    }
                    else if (SearchForMove(_availableMoves, hitPosition, out Movement move))
                    {
                        OnMoveTo(move);
                    }
                }
                else
                {
                    if (cp != null)
                        OnSelectPieceAndShowAvailableMoves(hitPosition);
                }
            }
        }

        private bool SearchForMove(List<Movement> moves, Vector2Int hitPosition, out Movement move)
        {
            for (int i = 0; i < moves.Count; i++)
            {
                if (moves[i].Destination.x == hitPosition.x && moves[i].Destination.y == hitPosition.y)
                {
                    move = moves[i];
                    return true;
                }
            }

            move = null;
            return false;
        }


        protected void OnSelectPieceAndShowAvailableMoves(Vector2Int hitPosition)
        {
            ActivePiece = _pieceArrangement[hitPosition.x, hitPosition.y];
            _availableMoves = ActivePiece.GetAvailableMoves(_pieceArrangement, _chessBoardInfoSO.boardSizeX,
                _chessBoardInfoSO.boardSizeY, _moveHistory, _availableMoves);
            ShowAvailableMoves();
        }

        private void ShowAvailableMoves()
        {
            List<(Vector3, bool)> tileData = new List<(Vector3, bool)>();
            for (int i = 0; i < _availableMoves.Count; i++)
            {
                Vector3 tileCenter =
                    TileSet.GetTileCenter(_availableMoves[i].Destination.x, _availableMoves[i].Destination.y);
                tileData.Add((tileCenter, _availableMoves[i].IsAttackMove));
            }

            _tileSelection.ShowSelection(tileData);
        }


        protected async void OnMoveTo(Movement move)
        {
            _canMove = false;
            _tileSelection.ClearSelection();
            _moveHistory.Add(move);
            await move.Process();
            ActivePiece = null;
            _canMove = true;
        }
    }

    public class ChessPlayer
    {
        public ChessPiece ActivePiece;
    }

    public class ChessGame
    {
        public ChessPlayer CurrentPlayer;
    }
}