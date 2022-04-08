using System.Reflection;
using FluentAssertions;
using Zenject;
using NUnit.Framework;
using UnityEngine;
using Logger = SteampunkChess.Logger;

namespace SteampunkChess.Tests
{
    [TestFixture]
    public class ChessBoardTests : ZenjectUnitTestFixture
    {

        [SetUp]
        public void SetUp()
        {
            BindChessBoardData();
        }

        private void BindChessBoardData()
        {
            GameDataSO data = Resources.Load<GameDataSO>("DefaultGameData");
            Container
                .Bind<NotationString>()
                .To<FenNotationString>()
                .FromInstance(new FenNotationString(data.notationString))
                .AsSingle();

            Container
                .Bind<ChessBoardInfoSO>()
                .FromInstance(data.chessBoardInfoSO)
                .AsSingle();

            Container
                .Bind<PiecesPrefabsSO>()
                .FromInstance(data.piecesPrefabsSO)
                .AsSingle();

            Container
                .Bind<TileSelectionInfoSO>()
                .FromInstance(data.tileSelectionSO)
                .AsSingle();

            Container
                .Bind<ChessBoardData>()
                .AsSingle();
        }


        [Test]
        public void WhenSpawningChessPiece_AndPositionPieceXY55_ThenXYShouldBe55()
        {
            //Arrange
            var notationString = Container.Resolve<NotationString>();
            var chessBoardInfoSO = Container.Resolve<ChessBoardInfoSO>();
            var piecesPrefabsSO = Container.Resolve<PiecesPrefabsSO>();
            var pieceArrangement = new PieceArrangement(notationString, chessBoardInfoSO, piecesPrefabsSO);
            var tileSet = new TileSet(chessBoardInfoSO);
        
            //Act
            var king = pieceArrangement.SpawnSinglePiece(ChessPieceType.King, Team.White);
            king.PositionPiece(5, 5);
        
            //Assert
            king.CurrentX.Should().Be(5);
            king.CurrentY.Should().Be(5);
        }
    
        [Test]
        public void WhenCreatingTiles_AndLookupTileWithIndex25_ThenTileIndexShouldBe25()
        {
            //Arrange
            var notationString = Container.Resolve<NotationString>();
            var chessBoardInfoSO = Container.Resolve<ChessBoardInfoSO>();
            var piecesPrefabsSO = Container.Resolve<PiecesPrefabsSO>();
            var tileSet = new TileSet(chessBoardInfoSO);
            tileSet.Initialize();
        
            //Act
            var tileIndex = tileSet.LookupTileIndex(tileSet[2, 5]);


            //Assert
            tileIndex.Should().Be(new Vector2Int(2, 5));
        }
    
        [Test]
        public void WhenCreatingMoveWithKnightOnB1ToC3_AndGetMovePGN_ThenMovePGNShouldBeC3()
        {
            //Arrange
            var notationString = Container.Resolve<NotationString>();
            var chessBoardInfoSO = Container.Resolve<ChessBoardInfoSO>();
            var piecesPrefabsSO = Container.Resolve<PiecesPrefabsSO>();
            var pieceArrangement = new PieceArrangement(notationString, chessBoardInfoSO, piecesPrefabsSO);
            var tileSet = new TileSet(chessBoardInfoSO);
            tileSet.Initialize();
            pieceArrangement.Initialize();
            Movement move = new Movement(new Vector2Int(1, 0), new Vector2Int(2, 2), pieceArrangement);
        
        
            //Act
            var movePGN = move.GetMovePGN();


            //Assert
            movePGN.Should().Be("<size=30>♞</size>c3");
        }
    
    
    }
}