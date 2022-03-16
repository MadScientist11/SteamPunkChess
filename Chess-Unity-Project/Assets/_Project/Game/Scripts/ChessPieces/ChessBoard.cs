using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public bool IsAttackMove => 
            _pieceArrangement[Destination.x, Destination.y] != null || _specialMove is EnPassant;


        private readonly PieceArrangement _pieceArrangement;

        private readonly ISpecialMove _specialMove;

        public Movement(Vector2Int start, Vector2Int destination, PieceArrangement pieceArrangement)
        {
            Start = start;
            Destination = destination;
            _pieceArrangement = pieceArrangement;
        }
        
        public Movement(Vector2Int start, Vector2Int destination, PieceArrangement pieceArrangement, ISpecialMove specialMove)
        {
            Start = start;
            Destination = destination;
            _specialMove = specialMove;
            _pieceArrangement = pieceArrangement;
        }
     

        public async Task Process()
        {
           
            ChessPiece pieceToMove = _pieceArrangement[Start.x, Start.y];
            ChessPiece enemy = _pieceArrangement[Destination.x, Destination.y];
           
            _pieceArrangement[Start.x, Start.y] = null;
            _pieceArrangement[Destination.x, Destination.y] = pieceToMove;
            await pieceToMove.PositionPiece(Destination.x, Destination.y);
            _specialMove?.ProcessSpecialMove();
            enemy?.Dispose();
        }
        
        public async void ProcessBackwards()
        {
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
        private List<Movement> _moveHistory;
        private TileSelection _tileSelection;


        public ChessBoard(ChessBoardInfoSO chessBoardInfoSO, PiecesPrefabsSO piecesPrefabsSO, TileSelectionInfoSO tileSelectionInfoSO)
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
                if (ActivePiece != null)
                {
                    if (cp != null && ActivePiece.IsFromSameTeam(cp))
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
            _availableMoves = ActivePiece.GetAvailableMoves(_pieceArrangement, _chessBoardInfoSO.boardSizeX, _chessBoardInfoSO.boardSizeY, _moveHistory, _availableMoves);
            ShowAvailableMoves();

        }
        private void ShowAvailableMoves()
        {
            List<(Vector3, bool)> tileData = new List<(Vector3, bool)>();
            for (int i = 0; i < _availableMoves.Count; i++)
            {
                //if (availableSpecialMove is EnPassant && availableMoves[i].x != activePiece.currentX)
                //    isSquareFree = false;

                tileData.Add((TileSet.GetTileCenter(_availableMoves[i].Destination.x, _availableMoves[i].Destination.y), _availableMoves[i].IsAttackMove));
            }
            _tileSelection.ShowSelection(tileData);
        }
        
        
        protected async void OnMoveTo(Movement move)
        {
            _tileSelection.ClearSelection();
            await move.Process();
            _moveHistory.Add(move);
            ActivePiece = null;


        }
        

    }

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

    public class ChessPlayer
    {
        public ChessPiece ActivePiece;
    }
    
    public class ChessGame
    {
        public ChessPlayer CurrentPlayer;
    }
    
    
}