using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SteampunkChess
{
    public abstract class ChessBoard 
    {
        private readonly TileSet _tileSet;
        private readonly PieceArrangement _pieceArrangement;
        private readonly ChessBoardInfoSO _chessBoardInfoSO;
        private readonly List<Movement> _moveHistory;
        private readonly MoveListing _moveListing;
        private readonly TileSelection _tileSelection;
        private readonly IAudioSystem _audioSystem;

        private ChessPiece _activePiece;
        private IChessGame _chessGame;
        private List<Movement> _availableMoves;
        private bool _processingMove;
        
        protected ChessBoard(ChessBoardData chessBoardData, MoveListingData moveListingData, ChessPieceFactory chessPieceFactory, IAudioSystem audioSystem)
        {
            _audioSystem = audioSystem;
            _chessBoardInfoSO = chessBoardData.ChessBoardInfoSO;
            _moveHistory = new List<Movement>();
            _moveListing = new MoveListing(moveListingData, _moveHistory);
            _tileSelection = new TileSelection(chessBoardData.TileSelectionSO);
            _tileSet = new TileSet(_chessBoardInfoSO);
            _pieceArrangement = new PieceArrangement(chessBoardData.NotationString, _chessBoardInfoSO, chessBoardData.PiecesPrefabsSO, chessPieceFactory);
        }

        public ChessPiece this[int x, int y] => _pieceArrangement[x, y];

        public virtual void Initialize(IChessGame chessGame)
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
                if (_activePiece != null)
                {
                    if (cp != null && _activePiece.IsFromSameTeam(cp))
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

        private bool SearchForMoveDestination(List<Movement> moves, Vector2Int moveDestination, out Movement move)
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
            _activePiece = _pieceArrangement[hitPosition.x, hitPosition.y];
            _availableMoves = _activePiece.GetAvailableMoves(_pieceArrangement, _chessBoardInfoSO.boardSizeX,
                _chessBoardInfoSO.boardSizeY, _moveHistory);
            RemoveMovesToPreventCheck();
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
                _audioSystem.PlaySound(Sounds.PieceMoveSound);

                if (IsCheckmated())
                {
                    _chessGame.EndOfGame(move.MovePiece.Team);
                }
                
                _activePiece = null;
                _chessGame.ChangeActiveTeam();
                _processingMove = false;
            }
        }
        
        private void RemoveMovesToPreventCheck()
        {
            ChessPiece targetKing = _chessGame.ActivePlayer.GetPiecesOfType<King>().First();
            SimulateForSinglePiece(_activePiece, _availableMoves, targetKing);
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
}