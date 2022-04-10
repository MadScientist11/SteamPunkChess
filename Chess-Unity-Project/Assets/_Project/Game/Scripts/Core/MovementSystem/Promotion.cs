using System.Collections.Generic;
using System.Threading.Tasks;
using SteampunkChess.PopUpService;
using UnityEngine;


namespace SteampunkChess
{
    public class Promotion : ISpecialMove
    {
        private readonly IReadOnlyList<Movement> _moveList;
        private readonly PieceArrangement _pieceArrangement;
        private readonly IPopUpService _popUpService;
        private readonly IGameFactory _gameFactory;

        public Promotion(List<Movement> moveList, PieceArrangement pieceArrangement, IPopUpService popUpService,
            IGameFactory gameFactory)
        {
            _moveList = moveList;
            _pieceArrangement = pieceArrangement;
            _popUpService = popUpService;
            _gameFactory = gameFactory;
        }

        //TODO: Choose promotion peace
        public async Task ProcessSpecialMove()
        {
            Movement lastMove = _moveList[_moveList.Count - 1];
            ChessPiece targetPawn = _pieceArrangement[lastMove.Destination.x, lastMove.Destination.y];
            
            if (targetPawn.Team == Team.White && lastMove.Destination.y == 7 ||
                targetPawn.Team == Team.Black && lastMove.Destination.y == 0)
            {
                _gameFactory.CachedGame.WaitingForUserInput = true;
                Debug.Log("Promotion");
                _popUpService.ShowPopUp(GameConstants.PopUps.PromotionWindow, _gameFactory, _moveList, _pieceArrangement);

                await TaskEx.WaitUntil(() => _gameFactory.CachedGame.WaitingForUserInput == false);
            }
            
        }
    }

    
}