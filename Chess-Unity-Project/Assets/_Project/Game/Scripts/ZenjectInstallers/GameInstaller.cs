using System;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class ChessPieceFactory
    {
        private readonly IInstantiator _instantiator;

        public ChessPieceFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }
        public ChessPiece CreatePiece(ChessPieceType pieceType)
        {
            return pieceType switch
            {
                ChessPieceType.Pawn => _instantiator.Instantiate<Pawn>(),
                ChessPieceType.Rook => _instantiator.Instantiate<Rook>(),
                ChessPieceType.Knight => _instantiator.Instantiate<Knight>(),
                ChessPieceType.Bishop => _instantiator.Instantiate<Bishop>(),
                ChessPieceType.Queen => _instantiator.Instantiate<Queen>(),
                ChessPieceType.King => _instantiator.Instantiate<King>(),
                _ => throw new ArgumentOutOfRangeException(nameof(pieceType), pieceType, null)
            };
        }
    }
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameDataSO _gameDataSO;
        [SerializeField] private GameCameraController _gameCameraController;
        [SerializeField] private TimerTextW _timerTextW;
        [SerializeField] private TimerTextB _timerTextB;
        [SerializeField] private MoveListingData _moveListingData;
        public override void InstallBindings()
        {
            BindChessBoardData();
            BindGameCameraController();
            BindTimer();
            BindMoveListingData();
           
            
            Container
                .Bind<IGameFactory>()
                .To<GameFactory>()
                .AsSingle();
            
            Container
                .Bind<IBoardFactory>()
                .To<BoardFactory>()
                .AsSingle();

            Container
                .Bind<ChessPieceFactory>()
                .AsSingle();

            

            Container
                .Bind<PlayerFactory>()
                .AsSingle();

            

            Container
                .Bind<ISpecialMoveFactory>()
                .To<SpecialMoveFactory>()
                .AsSingle();



        }

        private void BindMoveListingData()
        {
            Container
                .Bind<MoveListingData>()
                .FromInstance(_moveListingData)
                .AsSingle();
        }

        private void BindGameCameraController()
        {
            Container
                .Bind<GameCameraController>()
                .FromInstance(_gameCameraController)
                .AsSingle();
        }

        private void BindTimer()
        {
            Container
                .Bind<TimerTextW>()
                .FromInstance(_timerTextW)
                .AsSingle();

            Container
                .Bind<TimerTextB>()
                .FromInstance(_timerTextB)
                .AsSingle();

            Container
                .Bind<TimerFactory>()
                .AsSingle();
        }

        private void BindChessBoardData()
        {
            Container
                .Bind<NotationString>()
                .To<FenNotationString>()
                .FromInstance(new FenNotationString(_gameDataSO.notationString))
                .AsSingle();

            Container
                .Bind<ChessBoardInfoSO>()
                .FromInstance(_gameDataSO.chessBoardInfoSO)
                .AsSingle();

            Container
                .Bind<PiecesPrefabsSO>()
                .FromInstance(_gameDataSO.piecesPrefabsSO)
                .AsSingle();
            
            Container
                .Bind<TileSelectionInfoSO>()
                .FromInstance(_gameDataSO.tileSelectionSO)
                .AsSingle();
            
            Container
                .Bind<ChessBoardData>()
                .AsSingle();
        }

      
    }
}

