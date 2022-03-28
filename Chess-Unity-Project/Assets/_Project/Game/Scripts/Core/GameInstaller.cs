using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameDataSO _gameDataSO;
        [SerializeField] private CameraPivot cameraPivot;
        
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
                .FromInstance(cameraPivot)
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

