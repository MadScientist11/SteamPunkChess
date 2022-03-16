using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private ChessBoardInfoSO _chessBoardInfoSO;
        [SerializeField] private PiecesPrefabsSO _piecesPrefabsSO;
        [SerializeField] private TileSelectionInfoSO _tileSelectionSO;
        public override void InstallBindings()
        {
            Container
                .Bind<ChessBoardInfoSO>()
                .FromInstance(_chessBoardInfoSO);
            Container
                .Bind<PiecesPrefabsSO>()
                .FromInstance(_piecesPrefabsSO);
            Container
                .Bind<TileSelectionInfoSO>()
                .FromInstance(_tileSelectionSO);
            Container
                .BindInterfacesAndSelfTo<ChessBoard>()
                .AsSingle();
            
            BoardFactory.DIContainer = Container; 
            Debug.Log("GameInstaller");
        }
    }
}

