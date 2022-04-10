using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SteampunkChess.PopUpService;
using UnityEngine;
using Zenject;


namespace SteampunkChess
{
    public static class TaskEx
    {
        /// <summary>
        /// Blocks while condition is true or timeout occurs.
        /// </summary>
        /// <param name="condition">The condition that will perpetuate the block.</param>
        /// <param name="frequency">The frequency at which the condition will be check, in milliseconds.</param>
        /// <param name="timeout">Timeout in milliseconds.</param>
        /// <exception cref="TimeoutException"></exception>
        /// <returns></returns>
        public static async Task WaitWhile(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (condition()) await Task.Delay(frequency);
            });

            if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)))
                throw new TimeoutException();
        }

        /// <summary>
        /// Blocks until condition is true or timeout occurs.
        /// </summary>
        /// <param name="condition">The break condition.</param>
        /// <param name="frequency">The frequency at which the condition will be checked.</param>
        /// <param name="timeout">The timeout in milliseconds.</param>
        /// <returns></returns>
        public static async Task WaitUntil(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (!condition()) await Task.Delay(frequency);
            });

            if (waitTask != await Task.WhenAny(waitTask,
                    Task.Delay(timeout)))
                throw new TimeoutException();
        }
    }

    public enum SpecialMoveType
    {
        EnPassant,
        Castling,
        Promotion
    }

    public class SpecialMoveFactory : ISpecialMoveFactory
    {
        private readonly IInstantiator _instantiator;

        public SpecialMoveFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public ISpecialMove CreateSpecialMove(SpecialMoveType specialMoveType, List<Movement> moveList,
            PieceArrangement pieceArrangement)
        {
            return specialMoveType switch
            {
                SpecialMoveType.EnPassant => _instantiator.Instantiate<EnPassant>(new object[]
                    {moveList, pieceArrangement}),
                SpecialMoveType.Castling => _instantiator.Instantiate<Castling>(new object[]
                    {moveList, pieceArrangement}),
                SpecialMoveType.Promotion => _instantiator.Instantiate<Promotion>(new object[]
                    {moveList, pieceArrangement}),
                _ => throw new ArgumentOutOfRangeException(nameof(specialMoveType), specialMoveType, null)
            };
        }
    }

    public interface ISpecialMoveFactory
    {
        ISpecialMove CreateSpecialMove(SpecialMoveType specialMoveType, List<Movement> moveList,
            PieceArrangement pieceArrangement);
    }

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