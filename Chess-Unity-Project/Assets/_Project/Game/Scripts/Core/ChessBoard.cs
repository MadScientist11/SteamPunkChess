using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.Serialization;
using SteamPunkChess;
using TMPro;
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
    public abstract class Notation
    {
        public abstract GameData GameDataFromNotationString(string notationString);
        public abstract string NotationStringFromGameData(GameData gameData);
    }

    public class FenNotation : Notation
    {
        public override GameData GameDataFromNotationString(string notationString)
        {
           return FenUtility.GameDataFromStringFen(notationString);
        }

        public override string NotationStringFromGameData(GameData gameData)
        {
            return FenUtility.FenStringFromGameData(gameData);
        }
    }
    public class MoveListingData : MonoBehaviour
    {
        public Transform content;
        public GameObject moveListingDarker;
        public GameObject moveListingLighter;
    }

    public class MoveListing : IInitializable
    {
        private readonly List<Movement> _moveHistory;
        private readonly List<MoveListingEntry> _moveListingEntries;
        private MoveListingData _moveListingData;
        private readonly GameObject _moveListingPrefab;

        public MoveListing(List<Movement> moveHistory, List<MoveListingEntry> moveListingEntries, GameObject moveListingPrefab)
        {
            _moveHistory = moveHistory;
            _moveListingEntries = moveListingEntries;
            _moveListingPrefab = moveListingPrefab;
        }

        public Movement this[int index]
        {
            get => _moveHistory[index];
        }
        
        public void Initialize()
        {
            _moveListingData = Object.Instantiate(_moveListingPrefab).GetComponent<MoveListingData>();
        }

        public void UpdateMoveHistory(Movement move)
        {
            if (_moveListingEntries[_moveListingEntries.Count - 1].IsFilled)
            {
                GameObject entryPrefab = _moveHistory.Count % 2 == 0
                    ? _moveListingData.moveListingDarker
                    : _moveListingData.moveListingLighter;
                int fullMoveNumber = 1 + Mathf.FloorToInt((_moveHistory.Count / 2));
                MoveListingEntry listingEntry = new MoveListingEntry(entryPrefab, _moveListingData.content, fullMoveNumber);
            }
            
        }

      
    }

    public class MoveListingEntry
    {
        private readonly List<Movement> _moves;
        private readonly GameObject _entryPrefab;
        private readonly Transform _entryParent;
        private readonly int _moveNumber;
        private TextMeshProUGUI _listingEntryText;

        public MoveListingEntry(GameObject entryPrefab, Transform entryParent, int moveNumber)
        {
            _moves = new List<Movement>(2);
            _entryPrefab = entryPrefab;
            _entryParent = entryParent;
            _moveNumber = moveNumber;
        }

        public bool IsFilled => _moves.Count >= 2;

        private void CreateVisual()
        {
            _listingEntryText = Object.Instantiate(_entryPrefab, _entryParent).GetComponent<TextMeshProUGUI>();
        }
        
        public void AddMove(Movement move)
        {
            if (_moves.Count == 0)
            {
                CreateVisual();
                _listingEntryText.text = $"{_moveNumber}. {move.GetMovePGN()}";
            }
            else
            {
                _listingEntryText.text = $"{_listingEntryText.text} {move.GetMovePGN()}";
            }

            _moves.Add(move);
            // = move.RepresentationPGN
            
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
                    else if (SearchForMoveDestination(_availableMoves, hitPosition, out Movement move))
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

        private bool SearchForMoveDestination(List<Movement> moves, Vector2Int moveDestination, out Movement move)
        {
            for (int i = 0; i < moves.Count; i++)
            {
                if (moves[i].Destination.x == moveDestination.x && moves[i].Destination.y == moveDestination.y)
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
            //byte[] bytes = SerializationUtility.SerializeValue(_availableMoves[0], DataFormat.JSON);
            //File.WriteAllBytes(@"D:\Genshin Impact Game/games_archive", bytes);
            //byte[] bytess = File.ReadAllBytes(@"D:\Genshin Impact Game/games_archive");
            //var move = SerializationUtility.DeserializeValue<Movement>(bytess, DataFormat.JSON);
            //new Movement(move.Start, move.Destination, _pieceArrangement).Process();
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