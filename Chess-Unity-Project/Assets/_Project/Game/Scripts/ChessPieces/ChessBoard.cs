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
        public Vector2Int Start { get; }
        public Vector2Int Destination { get; }

        private readonly PieceArrangement _pieceArrangement;

        public ISpecialMove SpecialMove { get; set; }

        public Movement(Vector2Int start, Vector2Int destination, PieceArrangement pieceArrangement)
        {
            Start = start;
            Destination = destination;
            _pieceArrangement = pieceArrangement;
        }
     

        public async void Process()
        {
            Debug.Log("8");
            Debug.Log(SpecialMove);
            ChessPiece pieceToMove = _pieceArrangement[Start.x, Start.y];
           
            _pieceArrangement[Start.x, Start.y] = null;
            _pieceArrangement[Destination.x, Destination.y] = pieceToMove;
            await pieceToMove.PositionPiece(Destination.x, Destination.y);
            SpecialMove?.ProcessSpecialMove();
            Debug.Log("Move is done");
        }
        
        public async void ProcessBackwards()
        {
            //_pieceToMove.GetSpec
            ChessPiece pieceToMove = _pieceArrangement[Start.x, Start.y];
            _pieceArrangement[Start.x, Start.y] = null;
            _pieceArrangement[Destination.x, Destination.y] = pieceToMove;
            await pieceToMove.PositionPiece(Destination.x, Destination.y);
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
        private List<Movement> _moveHistory = new List<Movement>();


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
                    if (cp != null)
                    {
                        OnSelectPieceAndShowAvailableMoves(hitPosition);
                    }
                    else if(SearchForMove(_availableMoves, hitPosition, out Movement move))
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
            _availableMoves = ActivePiece.GetAvailableMoves(_pieceArrangement, _chessBoardInfoSO.boardSizeX, _chessBoardInfoSO.boardSizeY);
            ActivePiece.GetSpecialMove(_pieceArrangement, _moveHistory, _availableMoves);
            
            
        }
        
        
        protected void OnMoveTo(Movement move)
        {
            move.Process();
            _moveHistory.Add(move);
            
           
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