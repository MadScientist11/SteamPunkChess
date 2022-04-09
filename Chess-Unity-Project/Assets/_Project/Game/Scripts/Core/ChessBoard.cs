using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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
    [Serializable]
    public abstract class NotationString
    {
        protected readonly string _notationString;

        protected NotationString(string notationString)
        {
            _notationString = notationString;
        }

        public abstract PieceArrangementData GameDataFromNotationString();
    }
    [Serializable]
    public class FenNotationString : NotationString
    {
        public FenNotationString(string notationString) : base(notationString)
        {
        }

        public override PieceArrangementData GameDataFromNotationString()
        {
            return FenUtility.GameDataFromStringFen(_notationString);
        }
    }

    public class MoveListing
    {
        private readonly IReadOnlyList<Movement> _moveHistory;
        private readonly List<MoveListingEntry> _moveListingEntries;
        private readonly MoveListingData _moveListingData;
        

        public MoveListing(MoveListingData moveListingData, List<Movement> moveHistory)
        {
            _moveHistory = moveHistory;
            _moveListingEntries = new List<MoveListingEntry>();
            _moveListingData = moveListingData;
        }
        
        public void UpdateMoveListing()
        {
            var lastMove = _moveHistory[_moveHistory.Count - 1];
            if (_moveListingEntries.Count == 0 || _moveListingEntries[_moveListingEntries.Count - 1].IsFilled)
            {
                var listingEntry = CreateMoveListingEntry();
                _moveListingEntries.Add(listingEntry);
                listingEntry.AddMove(lastMove);
            }
            else
            {
                _moveListingEntries[_moveListingEntries.Count - 1].AddMove(lastMove);
            }
        }

        private MoveListingEntry CreateMoveListingEntry()
        {
            GameObject entryPrefab = (_moveHistory.Count & 1) == 0
                ? _moveListingData.moveListingDarker
                : _moveListingData.moveListingLighter;
            int fullMoveNumber = 1 + (_moveHistory.Count >> 1);
            
            MoveListingEntry listingEntry = new MoveListingEntry(entryPrefab, _moveListingData.content, fullMoveNumber);
            return listingEntry;
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

        private GameObject CreateVisual()
        {
            return Object.Instantiate(_entryPrefab, _entryParent);
        }

        public void AddMove(Movement move)
        {
            if (_moves.Count == 0)
            {
                GameObject listingEntryGO = CreateVisual();
                _listingEntryText = listingEntryGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                _listingEntryText.text = $"{_moveNumber}. {move.GetMovePGN()}";
            }
            else
            {
                _listingEntryText.text = $"{_listingEntryText.text} {move.GetMovePGN()}";
            }

            _moves.Add(move);
        }
    }

  

    public abstract class ChessBoard 
    {
        private readonly TileSet _tileSet;
        private readonly PieceArrangement _pieceArrangement;
        private readonly ChessBoardInfoSO _chessBoardInfoSO;
        private readonly List<Movement> _moveHistory;
        private readonly MoveListing _moveListing;
        private readonly TileSelection _tileSelection;

        protected ChessPiece ActivePiece;
        private ChessGame _chessGame;
        private List<Movement> _availableMoves;
        private bool _processingMove;


        protected ChessBoard(ChessBoardData chessBoardData, MoveListingData moveListingData)
        {
            _chessBoardInfoSO = chessBoardData.ChessBoardInfoSO;
            _moveHistory = new List<Movement>();
            _moveListing = new MoveListing(moveListingData, _moveHistory);
            _tileSelection = new TileSelection(chessBoardData.TileSelectionSO);
            _tileSet = new TileSet(_chessBoardInfoSO);
            _pieceArrangement = new PieceArrangement(chessBoardData.NotationString, _chessBoardInfoSO, chessBoardData.PiecesPrefabsSO);
        }

        public ChessPiece this[int x, int y] => _pieceArrangement[x, y];


        private void DeepCopy()
        {
        }

        public virtual void Initialize(ChessGame chessGame)
        {
            _chessGame = chessGame;
            InitializeBoardComponents();
        }

        private void InitializeBoardComponents()
        {
            _tileSet.Initialize();
            _pieceArrangement.Initialize();
            _tileSelection.Initialize();
        }
        
        protected abstract void MoveTo(Vector2 moveTo);
        protected abstract void SelectPieceAndShowAvailableMoves(Vector2 hitPosition);

        public void OnTileHover(GameObject tile)
        {
            if (_processingMove || !_chessGame.CanPerformMove())
                return; 
            
            Vector2Int hitPosition = _tileSet.LookupTileIndex(tile);
            ChessPiece cp = _pieceArrangement[hitPosition.x, hitPosition.y];

            bool onTileClicked = Input.GetMouseButtonUp(0);
            
            if (onTileClicked)
            {
                if (ActivePiece != null)
                {
                    if (cp != null && ActivePiece.IsFromSameTeam(cp))
                    {
                        SelectPieceAndShowAvailableMoves(hitPosition);
                    }
                    else if (SearchForMoveDestination(_availableMoves, hitPosition, out Movement move))
                    {
                        MoveTo(move.Destination);
                    }
                }
                else
                {
                    if (cp != null && _chessGame.IsTeamTurnActive(cp.Team))
                        SelectPieceAndShowAvailableMoves(hitPosition);
                }
            }
        }

        public bool SearchForMoveDestination(List<Movement> moves, Vector2Int moveDestination, out Movement move)
        {
            for (int i = 0; i < moves.Count; i++)
            {
                if (moves[i].Destination == moveDestination)
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
                _chessBoardInfoSO.boardSizeY, _moveHistory);
            RemoveMovesToPreventCheck();
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


        protected async void OnMoveTo(Vector2Int moveTo)
        {
            if (SearchForMoveDestination(_availableMoves, moveTo, out Movement move))
            {
                _processingMove = true;
                _tileSelection.ClearSelection();
                _moveHistory.Add(move);
                _moveListing.UpdateMoveListing();
                await move.Process();

                if (IsCheckmated())
                {
                    _chessGame.EndOfGame(move.MovePiece.Team);
                }
                
                ActivePiece = null;
                _chessGame.ChangeActiveTeam();
                _processingMove = false;
            }
        }
        
        private void RemoveMovesToPreventCheck()
        {
            ChessPiece targetKing = _chessGame.ActivePlayer.GetPiecesOfType<King>().First();
            SimulateForSinglePiece(ActivePiece, _availableMoves, targetKing);
        }

        private void SimulateForSinglePiece(ChessPiece cp, List<Movement> moves, ChessPiece king)
        {
            
            List<Movement> movesToRemove = new List<Movement>();
            Team attackingTeam = cp.Team == Team.White ? Team.Black : Team.White;
            

            for (int i = 0; i < moves.Count; i++)
            {
                ChessPiece simulationPiece = cp.ShallowCopy();
                int simX = moves[i].Destination.x;
                int simY = moves[i].Destination.y;

                Vector2Int kingPositionThisSim = new Vector2Int(king.CurrentX, king.CurrentY);
                
                if (cp.ChessType == ChessPieceType.King)
                    kingPositionThisSim = new Vector2Int(simX, simY);

                
                PieceArrangement simulation = _pieceArrangement.DeepCopy();
                var possibleAttackingPieces = new List<ChessPiece>(_chessGame.ChessPlayers[(int) attackingTeam].ActivePieces);
               

                // Simulate that move
                simulation[simulationPiece.CurrentX, simulationPiece.CurrentY] = null;
                simulationPiece.CurrentX = simX;
                simulationPiece.CurrentY = simY;
                simulation[simX, simY] = simulationPiece;
                
                
                List<Movement> simulatedMoveHistory = new List<Movement>(_moveHistory);
                simulatedMoveHistory.Add(new Movement(new Vector2Int(simulationPiece.CurrentX, simulationPiece.CurrentY), new Vector2Int(simX, simY), simulation));

                // Did one of the pieces got taken down during  simulation
                var deadPiece = possibleAttackingPieces.Find(x => x.CurrentX == simX && x.CurrentY == simY);

                if (deadPiece != null)
                    possibleAttackingPieces.Remove(deadPiece);
                
                // Get all attacking pieces available moves
                List<Movement> simMoves = new List<Movement>();
                for (int a = 0; a < possibleAttackingPieces.Count; a++)
                {
                    var pieceMoves = possibleAttackingPieces[a].GetAvailableMoves(simulation, _chessBoardInfoSO.boardSizeX,
                        _chessBoardInfoSO.boardSizeY, simulatedMoveHistory);
                    for (int b = 0; b < pieceMoves.Count; b++)
                    {
                        simMoves.Add(pieceMoves[b]);
                    }
                }

                //Is that spot safe for king?
                if (SearchForMoveDestination(simMoves, kingPositionThisSim, out Movement move))
                {
                    movesToRemove.Add(moves[i]);
                }
            }

            //Remove  some moves from availableMoves
            for (int i = 0; i < movesToRemove.Count; i++)
            {
                moves.Remove(movesToRemove[i]);
            }
        }
        
        private bool IsCheckmated()
        {
            var lastMove = _moveHistory[_moveHistory.Count - 1];
            Team attackingTeam = _pieceArrangement[lastMove.Destination.x, lastMove.Destination.y].Team;
            Team targetTeam = (attackingTeam == Team.White) ? Team.Black : Team.White;
            
            IReadOnlyList<ChessPiece> attackingPieces = _chessGame.ChessPlayers[(int) attackingTeam].ActivePieces;
            IReadOnlyList<ChessPiece> defendingPieces = _chessGame.ChessPlayers[(int) targetTeam].ActivePieces;
            ChessPiece targetKing = _chessGame.ChessPlayers[(int) targetTeam].GetPiecesOfType<King>().First();


           
            List<Movement> attackingMoves = new List<Movement>();
            for (int a = 0; a < attackingPieces.Count; a++)
            {
                var pieceMoves = attackingPieces[a].GetAvailableMoves(_pieceArrangement, _chessBoardInfoSO.boardSizeX, _chessBoardInfoSO.boardSizeY, _moveHistory);
                for (int b = 0; b < pieceMoves.Count; b++)
                {
                    attackingMoves.Add(pieceMoves[b]);
                }
            }
           
            if (SearchForMoveDestination(attackingMoves, new Vector2Int(targetKing.CurrentX, targetKing.CurrentY), out _))
            {
                //King is under attack, can we defend him?
                for (int i = 0; i < defendingPieces.Count; i++)
                {
                    var defendingMoves = defendingPieces[i].GetAvailableMoves(_pieceArrangement, _chessBoardInfoSO.boardSizeX, _chessBoardInfoSO.boardSizeY, _moveHistory);
                    //Delete moves that give us check
                    SimulateForSinglePiece(defendingPieces[i], defendingMoves, targetKing);
                    
                    if (defendingMoves.Count != 0)
                        return false;
                }
                return true;
            }
            return false;
        }
    }

    public class Rules
    {
        public bool SearchForMoveDestination(List<Movement> moves, Vector2Int moveDestination, out Movement move)
        {
            for (int i = 0; i < moves.Count; i++)
            {
                if (moves[i].Destination == moveDestination)
                {
                    move = moves[i];
                    return true;
                }
            }

            move = null;
            return false;
        }
        
        private void SimulateForSinglePiece(ChessPiece cp, List<Movement> moves, ChessPiece king, List<Movement> moveHistory, PieceArrangement pieceArrangement, IReadOnlyList<ChessPiece> attackingTeamPieces, ChessBoardInfoSO chessBoardInfoSO)
        {
            //Save values to reset them in the end
            ChessPiece simulationPiece = cp.ShallowCopy();
            List<Movement> movesToRemove = new List<Movement>();
            Team attackingTeam = cp.Team == Team.White ? Team.Black : Team.White;

            for (int i = 0; i < moves.Count; i++)
            {
                int simX = moves[i].Destination.x;
                int simY = moves[i].Destination.y;

                Vector2Int kingPositionThisSim = new Vector2Int(king.CurrentX, king.CurrentY);
                
                if (cp.ChessType == ChessPieceType.King)
                    kingPositionThisSim = new Vector2Int(simX, simY);

                
                PieceArrangement simulation = pieceArrangement.DeepCopy();
                var possibleAttackingPieces = new List<ChessPiece>(attackingTeamPieces);
               

                // Simulate that move
                simulation[simulationPiece.CurrentX, simulationPiece.CurrentY] = null;
                simulationPiece.CurrentX = simX;
                simulationPiece.CurrentY = simY;
                simulation[simX, simY] = simulationPiece;

                List<Movement> simulatedMoveHistory = new List<Movement>(moveHistory);;
                simulatedMoveHistory.Add(new Movement(new Vector2Int(simulationPiece.CurrentX, simulationPiece.CurrentY), new Vector2Int(simX, simY), simulation));

                // Did one of the pieces got taken down during  simulation
                var deadPiece = possibleAttackingPieces.Find(x => x.CurrentX == simX && x.CurrentY == simY);

                if (deadPiece != null)
                    possibleAttackingPieces.Remove(deadPiece);

                // Get all attacking pieces available moves
                List<Movement> simMoves = new List<Movement>();
                for (int a = 0; a < possibleAttackingPieces.Count; a++)
                {
                    var pieceMoves = possibleAttackingPieces[a].GetAvailableMoves(simulation, chessBoardInfoSO.boardSizeX,
                        chessBoardInfoSO.boardSizeY, simulatedMoveHistory);
                    for (int b = 0; b < pieceMoves.Count; b++)
                    {
                        simMoves.Add(pieceMoves[b]);
                    }
                }

                //Is that spot safe for king?
                if (SearchForMoveDestination(simMoves, kingPositionThisSim, out Movement move))
                {
                    Logger.DebugError($"Remove move {move.MovePiece.ChessType}");
                    movesToRemove.Add(moves[i]);
                }
            }

            //Remove  some moves from availableMoves
            for (int i = 0; i < movesToRemove.Count; i++)
            {
                moves.Remove(movesToRemove[i]);
            }
        }
        private bool IsCheckmated(List<Movement> moveHistory, PieceArrangement pieceArrangement, ChessGame chessGame, ChessBoardInfoSO chessBoardInfoSO)
        {
            var lastMove = moveHistory[moveHistory.Count - 1];
            Team attackingTeam = pieceArrangement[lastMove.Destination.x, lastMove.Destination.y].Team;
            Team targetTeam = (attackingTeam == Team.White) ? Team.Black : Team.White;
            
            IReadOnlyList<ChessPiece> attackingPieces = chessGame.ChessPlayers[(int) attackingTeam].ActivePieces;
            IReadOnlyList<ChessPiece> defendingPieces = chessGame.ChessPlayers[(int) targetTeam].ActivePieces;
            ChessPiece targetKing = chessGame.ChessPlayers[(int) targetTeam].GetPiecesOfType<King>().First();


            // Is king attacked right now
            List<Movement> attackingMoves = new List<Movement>();
            for (int a = 0; a < attackingPieces.Count; a++)
            {
                var pieceMoves = attackingPieces[a].GetAvailableMoves(pieceArrangement, chessBoardInfoSO.boardSizeX, chessBoardInfoSO.boardSizeY, moveHistory);
                for (int b = 0; b < pieceMoves.Count; b++)
                {
                    attackingMoves.Add(pieceMoves[b]);
                }
            }
           
            if (SearchForMoveDestination(attackingMoves, new Vector2Int(targetKing.CurrentX, targetKing.CurrentY), out Movement move))
            {
                //King is under attack, can we defend him?
                for (int i = 0; i < defendingPieces.Count; i++)
                {
                    var defendingMoves = defendingPieces[i].GetAvailableMoves(pieceArrangement, chessBoardInfoSO.boardSizeX, chessBoardInfoSO.boardSizeY, moveHistory);
                    //Delete moves that still give us check
                    //SimulateForSinglePiece(defendingPieces[i], defendingMoves, targetKing, moveHistory, pieceArrangement,);
                    
                    if (defendingMoves.Count != 0)
                        return false;
                }
                return true;
            }
            return false;
        }
    }
}