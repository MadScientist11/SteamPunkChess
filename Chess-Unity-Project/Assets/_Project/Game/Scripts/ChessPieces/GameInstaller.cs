using System.Collections;
using System.Collections.Generic;
using SteampunkChess;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private ChessBoardInfoSO _chessBoardInfoSO;
    [SerializeField] private PiecesPrefabsSO _piecesPrefabsSO;
    public override void InstallBindings()
    {
        Container
            .Bind<ChessBoardInfoSO>()
            .FromInstance(_chessBoardInfoSO);
        Container
            .Bind<PiecesPrefabsSO>()
            .FromInstance(_piecesPrefabsSO);
        Container
            .Bind<ChessBoard>()
            .AsSingle();
        BoardFactory.DIContainer = Container; 
        Debug.Log("GameInstaller");
    }
}
