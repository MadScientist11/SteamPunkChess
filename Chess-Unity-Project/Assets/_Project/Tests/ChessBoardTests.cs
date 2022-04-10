using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace SteampunkChess.Tests
{
    [TestFixture]
    public class ChessBoardTests : ZenjectUnitTestFixture
    {

        [SetUp]
        public void SetUp()
        {
            BindChessBoardData();
            
            Container
                .Bind<ChessPieceFactory>()
                .AsSingle();

            Container
                .Bind<ISpecialMoveFactory>()
                .To<SpecialMoveFactory>()
                .AsSingle();
            
            
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
            var chessPieceFactory = Container.Resolve<ChessPieceFactory>();
            var pieceArrangement = new PieceArrangement(notationString, chessBoardInfoSO, piecesPrefabsSO, chessPieceFactory);
            var tileSet = new TileSet(chessBoardInfoSO);
            
            //Act
            var king = pieceArrangement.SpawnSinglePiece(ChessPieceType.King, Team.White);
            king.PositionPiece(5, 5);
            
            //Assert
            king.CurrentX.Should().Be(5);
            king.CurrentY.Should().Be(5);
        }
    
        [Test]
        public void WhenCreatingTiles_AndLookupTileIndex_ThenTileIndexShouldBeAsStated()
        {
            //Arrange
            var notationString = Container.Resolve<NotationString>();
            var chessBoardInfoSO = Container.Resolve<ChessBoardInfoSO>();
            var piecesPrefabsSO = Container.Resolve<PiecesPrefabsSO>();
            var tileSet = new TileSet(chessBoardInfoSO);
            var tile = Object.Instantiate(chessBoardInfoSO.tileInfoSO.tilePrefab);
            tileSet.Initialize();
        
            //Act
            var tileIndex = tileSet.LookupTileIndex(tileSet[2, 5]);
            var tileIndex2 = tileSet.LookupTileIndex(tileSet[5, 7]);
            var tileIndex3 = tileSet.LookupTileIndex(tile);


            //Assert
            tileIndex.Should().Be(new Vector2Int(2, 5));
            tileIndex2.Should().Be(new Vector2Int(5, 7));
            tileIndex3.Should().Be(new Vector2Int(-1, -1));
        }
    
        [Test]
        public void WhenCreatingMoveWithKnightOnB1ToC3_AndGetMovePGN_ThenMovePGNShouldBeC3()
        {
            //Arrange
            var notationString = Container.Resolve<NotationString>();
            var chessBoardInfoSO = Container.Resolve<ChessBoardInfoSO>();
            var piecesPrefabsSO = Container.Resolve<PiecesPrefabsSO>();
            var chessPieceFactory = Container.Resolve<ChessPieceFactory>();
            var pieceArrangement = new PieceArrangement(notationString, chessBoardInfoSO, piecesPrefabsSO, chessPieceFactory);
            var tileSet = new TileSet(chessBoardInfoSO);
            tileSet.Initialize();
            pieceArrangement.Initialize();
            Movement move = new Movement(new Vector2Int(1, 0), new Vector2Int(2, 2), pieceArrangement);
            
            
            //Act
            var movePGN = move.GetMovePGN();
            
            
            //Assert
            movePGN.Should().Be("<size=30>♞</size>c3");
        }
        
        [Test]
        public void FindAllIndexesListExtensionTest()
        {
            //Arrange
            var list = new List<int>
            {
                10, 15, 10, 20, 30, 10, 40, 60
            };
            
            //Act
            var findAllIndexes = list.FindAllIndexes(x => x == 10);
            var findAllIndexes2 = list.FindAllIndexes(x => x == 15);
            var findAllIndexes3 = list.FindAllIndexes(x => x == -110);


            //Assert
            findAllIndexes.Count.Should().Be(3);
            findAllIndexes2.Count.Should().Be(1);
            findAllIndexes3.Count.Should().Be(0);

        }
    }
}