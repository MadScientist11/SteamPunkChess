using System;
using System.Collections.Generic;
using SteamPunkChess;
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
    public class Movement
    {
        public Vector2Int _from;
        public Vector2Int _to;
        private readonly PieceArrangement _pieceArrangement;

        public Movement(Vector2Int from, Vector2Int to, PieceArrangement pieceArrangement)
        {
            _from = from;
            _to = to;
            _pieceArrangement = pieceArrangement;
        }

        public async void Process()
        {
            //_pieceToMove.GetSpec
            ChessPiece pieceToMove = _pieceArrangement[_from.x, _from.y];
            _pieceArrangement[_from.x, _from.y] = null;
            _pieceArrangement[_to.x, _to.y] = pieceToMove;
            await pieceToMove.PositionPiece(_to.x, _to.y);
            Debug.Log("Move is done");
        }
        
        public async void ProcessBackwards()
        {
            //_pieceToMove.GetSpec
            ChessPiece pieceToMove = _pieceArrangement[_from.x, _from.y];
            _pieceArrangement[_from.x, _from.y] = null;
            _pieceArrangement[_to.x, _to.y] = pieceToMove;
            await pieceToMove.PositionPiece(_to.x, _to.y);
        }
    }
    public class ChessBoard : IInitializable
    {
        private TileSet _tileSet;
        private readonly string _gameFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        private PieceArrangement _pieceArrangement;
        
        protected ChessPiece ActivePiece;
        private ChessBoardInfoSO _chessBoardInfoSO;
        private List<Movement> _availableMoves;
        private List<Movement[]> _moveHistory;


        public ChessBoard(ChessBoardInfoSO chessBoardInfoSO, PiecesPrefabsSO piecesPrefabsSO)
        {
            _chessBoardInfoSO = chessBoardInfoSO;
            _tileSet = new TileSet(_chessBoardInfoSO);
            _pieceArrangement = new PieceArrangement(_gameFen, _chessBoardInfoSO, piecesPrefabsSO);
        }

      

        private void DeepCopy()
        {
            
        }

        public void Initialize()
        {
            UnityEngine.Object.Instantiate(_chessBoardInfoSO.boardPrefab);
            _tileSet.Initialize();
            _pieceArrangement.Initialize();
        }
        
        public void OnTileHover(GameObject tile)
        {
            Vector2Int hitPosition = _tileSet.LookupTileIndex(tile);
            ChessPiece cp = _pieceArrangement[hitPosition.x, hitPosition.y];

            bool onTileSelected = Input.GetMouseButtonUp(0);
            if (onTileSelected)
            {
                if (ActivePiece != null)
                {
                    var move = new Movement(new Vector2Int(ActivePiece.CurrentX, ActivePiece.CurrentY),new Vector2Int(hitPosition.x, hitPosition.y),_pieceArrangement);
                    if (cp != null)
                    {
                        OnSelectPieceAndShowAvailableMoves(hitPosition);
                    }
                    else if(ContainsValidMove(_availableMoves, move))
                    {
                        Debug.Log("TRy move");
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
        private bool ContainsValidMove(List<Movement> moves, Movement move)
        {
            Debug.Log("TRy move");
            for (int i = 0; i < moves.Count; i++)
                if (moves[i]._from == move._from && moves[i]._to == move._to)
                    return true;
            
            return false;
        }
     
        
        protected void OnSelectPieceAndShowAvailableMoves(Vector2Int hitPosition)
        {
            ActivePiece = _pieceArrangement[hitPosition.x, hitPosition.y];
            _availableMoves = ActivePiece.GetAvailableMoves(_pieceArrangement, _chessBoardInfoSO.boardSizeX, _chessBoardInfoSO.boardSizeY);
          


        }
        
        
        protected void OnMoveTo(Movement move)
        {
            move.Process();
        }
        

    }

    public class PieceArrangement : IInitializable
    {
        private ChessPiece[,] _chessPieces;

        private readonly string _gameFen;
        private ChessBoardInfoSO _chessBoardInfoSO;
        private PiecesPrefabsSO _piecesPrefabsSO;
        
        private const string PiecesParent = "Pieces";

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
        
        private ChessPiece SpawnSinglePiece(ChessPieceType type, Team team, PiecesPrefabsSO piecesPrefabsSO, Transform piecesParent)
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
        
        private void PositionPieces()
        {
            for (int x = 0; x < _chessBoardInfoSO.boardSizeX; x++)
            for (int y = 0; y < _chessBoardInfoSO.boardSizeY; y++)
                if (this[x, y] != null)
                    this[x, y].PositionPiece(x, y, true);
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