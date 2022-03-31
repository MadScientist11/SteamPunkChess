using FluentAssertions;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameDataSO _gameDataSO;
        [SerializeField] private CameraPivot _cameraPivot;
        [SerializeField] private TimerTextW _timerTextW;
        [SerializeField] private TimerTextB _timerTextB;
        public override void InstallBindings()
        {
            BindGameData();
            BindChessBoardInfo();
            
            Container
                .Bind<IGameFactory>()
                .To<GameFactory>()
                .AsSingle();
            
            Container
                .Bind<IBoardFactory>()
                .To<BoardFactory>()
                .AsSingle();

            Container
                .Bind<NotationString>()
                .To<FenNotationString>()
                .FromInstance(new FenNotationString("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"));
            
            Container
                .Bind<CameraPivot>()
                .FromInstance(_cameraPivot)
                .AsSingle();

            Container
                .Bind<GameTimer>()
                .AsSingle();

            Container
                .Bind<PlayerFactory>()
                .AsSingle();

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

        private void BindGameData()
        {
            Container
                .Bind<GameDataSO>()
                .FromInstance(_gameDataSO)
                .AsSingle();
        }

        private void BindChessBoardInfo()
        {
            Container
                .Bind<ChessBoardInfoSO>()
                .FromInstance(_gameDataSO.chessBoardInfoSO)
                .AsSingle();
        }
    }
}

