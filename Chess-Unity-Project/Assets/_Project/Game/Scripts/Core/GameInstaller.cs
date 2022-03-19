using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameDataSO _gameDataSO;
        public override void InstallBindings()
        {
            BindGameData();
            
            
            Container
                .Bind<IGameFactory>()
                .To<GameFactory>()
                .AsSingle();
            
            Container
                .Bind<IBoardFactory>()
                .To<BoardFactory>()
                .AsSingle();

            Container
                .Bind<ChessGame>()
                .AsSingle();




        }

        private void BindGameData()
        {
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
                .Bind<NotationString>()
                .FromInstance(new FenNotationString(_gameDataSO.notationString))
                .AsSingle();
        }
    }
}

