using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine;
using Zenject;
using NSubstitute;
using SteampunkChess.PopUpService;

namespace SteampunkChess.Tests
{
    [TestFixture]
    public class ChessBoardTests : ZenjectUnitTestFixture
    {

        [SetUp]
        public void SetUp()
        {
            BindChessBoardData();
            var popUpService = ScriptableObject.CreateInstance<PopUpServiceSO>();


            Container
                .Bind<IGameFactory>()
                .To<GameFactory>()
                .AsSingle();
            
            Container
                .Bind<ChessPieceFactory>()
                .AsSingle();

            Container
                .Bind<ISpecialMoveFactory>()
                .To<SpecialMoveFactory>()
                .AsSingle();
            Container
                .Bind<IPopUpService>()
                .To<PopUpServiceSO>()
                .FromInstance(popUpService)
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
        
        //[Test]
        // public void WhenWhitePawnOnE5_AndBlackPawnOnF5IsPreviousMove_ThanAvailableMovesShouldIncludeEnPassant()
        //{
        //    //Arrange
        //    var chessBoardInfoSO = Container.Resolve<ChessBoardInfoSO>();
        //    var piecesPrefabsSO = Container.Resolve<PiecesPrefabsSO>();
        //    var tileSelectionInfoSO = Container.Resolve<TileSelectionInfoSO>();
        //    var chessBoardData = new ChessBoardData(new FenNotationString("rnbqkbnr/1pppp1pp/p7/4Pp2/8/8/PPPP1PPP/RNBQKBNR w KQkq f6 0 3"), chessBoardInfoSO, piecesPrefabsSO, tileSelectionInfoSO);
        //    var chessPieceFactory = Container.Resolve<ChessPieceFactory>();
        //    MockChessBoard board = new MockChessBoard(chessBoardData, null, chessPieceFactory, null);
        //    var chessGame = Substitute.For<IChessGame>();
        //    var whitePlayer = new ChessPlayer(Team.White, board, 10000f);
        //    var blackPlayer = new ChessPlayer(Team.Black, board, 10000f);
        //    whitePlayer.Initialize();
        //    blackPlayer.Initialize();
        //    chessGame.ActivePlayer.Returns(whitePlayer);
        //    chessGame.ChessPlayers.Returns(new[] {whitePlayer, blackPlayer});
        //    board.Initialize(chessGame);
        //    
        //    //Act
        //    board.SelectPieceAndShowAvailableMoves(new Vector2(4,4));
        //    
        //    
        //    //Assert
        //    board.SearchForMoveDestination(board.AvailableMoves, new Vector2Int(5, 5), out Movement move).Should()
        //        .BeTrue();
        //  
        //}

        [Test]
        public void WhenWhitePawnOnE5_AndBlackPawnOnF5IsPreviousMove_ThanAvailableMovesShouldIncludeEnPassant()
        {
            //Arrange
            var notationString = new FenNotationString("rnbqkbnr/1pppp1pp/p7/4Pp2/8/8/PPPP1PPP/RNBQKBNR w KQkq f6 0 3");
            var chessBoardInfoSO = Container.Resolve<ChessBoardInfoSO>();
            var piecesPrefabsSO = Container.Resolve<PiecesPrefabsSO>();
            var chessPieceFactory = Container.Resolve<ChessPieceFactory>();
            var pieceArrangement = new PieceArrangement(notationString, chessBoardInfoSO, piecesPrefabsSO, chessPieceFactory);
            var tileSet = new TileSet(chessBoardInfoSO);
            pieceArrangement.Initialize();
            tileSet.Initialize();
            var moveHistory = new List<Movement>()
            {
                new Movement(new Vector2Int(5,6), new Vector2Int(5, 4),pieceArrangement),
            };
            var whiteAttackingPawn = pieceArrangement[4, 4];
            
            //Act
            var availableMoves = whiteAttackingPawn.GetAvailableMoves(pieceArrangement, 8, 8, moveHistory);
            
            //Assert
            availableMoves.Should().ContainEquivalentOf(new Movement(new Vector2Int(4, 4), new Vector2Int(5, 5), pieceArrangement,
                new EnPassant(moveHistory, pieceArrangement)));
        }

        [Test]
        public void WhenKingAndRookH1DidntMove_AndThereIsNoObstacleToThem_ThenAvailableMovesShouldIncludeCastling()
        {
            var notationString = new FenNotationString("rnbqkbnr/pp1p1ppp/2p5/1B2p3/8/4PN2/PPPP1PPP/RNBQK2R b KQkq - 1 3");
            var chessBoardInfoSO = Container.Resolve<ChessBoardInfoSO>();
            var piecesPrefabsSO = Container.Resolve<PiecesPrefabsSO>();
            var chessPieceFactory = Container.Resolve<ChessPieceFactory>();
            var pieceArrangement = new PieceArrangement(notationString, chessBoardInfoSO, piecesPrefabsSO, chessPieceFactory);
            var tileSet = new TileSet(chessBoardInfoSO);
            pieceArrangement.Initialize();
            tileSet.Initialize();
            var moveHistory = new List<Movement>()
            {
                new Movement(new Vector2Int(5,6), new Vector2Int(5, 4),pieceArrangement),
            };
            var whiteKing = pieceArrangement[4, 0];
            var expectedCastlingMove = new Movement(new Vector2Int(4, 0),
                new Vector2Int(6, 0), 
                pieceArrangement, 
                new Castling(moveHistory, pieceArrangement));
            
            //Act
            var availableMoves = whiteKing.GetAvailableMoves(pieceArrangement, 8, 8, moveHistory);
            
            //Assert
            availableMoves.Should().ContainEquivalentOf(expectedCastlingMove);
        }

        [Test]
        public void WhenWhitePawnOnG7_AndCanMoveG8_ThanAvailableMovesShouldContainPromotion()
        {
            var notationString = new FenNotationString("rnbqkb1r/ppppppP1/8/3n4/7p/4P3/PPPP2PP/RNBQKBNR w KQkq - 0 6");
            var chessBoardInfoSO = Container.Resolve<ChessBoardInfoSO>();
            var piecesPrefabsSO = Container.Resolve<PiecesPrefabsSO>();
            var chessPieceFactory = Container.Resolve<ChessPieceFactory>();
            var pieceArrangement = new PieceArrangement(notationString, chessBoardInfoSO, piecesPrefabsSO, chessPieceFactory);
            var tileSet = new TileSet(chessBoardInfoSO);
            pieceArrangement.Initialize();
            tileSet.Initialize();
            var moveHistory = new List<Movement>();
            var whitePawn = pieceArrangement[6, 6];
            var expectedPromotionMove = new Movement(new Vector2Int(6, 6),
                new Vector2Int(6, 7), 
                pieceArrangement);
            
            //Act
            var availableMoves = whitePawn.GetAvailableMoves(pieceArrangement, 8, 8, moveHistory);
            
            //Assert
            availableMoves.Should().ContainEquivalentOf(expectedPromotionMove);
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
        
        [Test]
        public void WhenGivenStartPositionFen_AndParseToGameData_ThanPieceArrangementShouldBeAsExpected()
        {
            //Arrange
            var notationString = new FenNotationString("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            PieceInfo[,] expectedArrangementArray = {
                { new PieceInfo(ChessPieceType.Rook, Team.White),new PieceInfo(ChessPieceType.Pawn, Team.White), null, null, null, null, new PieceInfo(ChessPieceType.Pawn, Team.Black), new PieceInfo(ChessPieceType.Rook, Team.Black) },
                { new PieceInfo(ChessPieceType.Knight, Team.White),new PieceInfo(ChessPieceType.Pawn, Team.White), null, null, null, null, new PieceInfo(ChessPieceType.Pawn, Team.Black), new PieceInfo(ChessPieceType.Knight, Team.Black) },
                { new PieceInfo(ChessPieceType.Bishop, Team.White),new PieceInfo(ChessPieceType.Pawn, Team.White), null, null, null, null, new PieceInfo(ChessPieceType.Pawn, Team.Black), new PieceInfo(ChessPieceType.Bishop, Team.Black) },
                { new PieceInfo(ChessPieceType.Queen, Team.White),new PieceInfo(ChessPieceType.Pawn, Team.White), null, null, null, null, new PieceInfo(ChessPieceType.Pawn, Team.Black), new PieceInfo(ChessPieceType.Queen, Team.Black) },
                { new PieceInfo(ChessPieceType.King, Team.White),new PieceInfo(ChessPieceType.Pawn, Team.White), null, null, null, null, new PieceInfo(ChessPieceType.Pawn, Team.Black), new PieceInfo(ChessPieceType.King, Team.Black) },
                { new PieceInfo(ChessPieceType.Bishop, Team.White),new PieceInfo(ChessPieceType.Pawn, Team.White), null, null, null, null, new PieceInfo(ChessPieceType.Pawn, Team.Black), new PieceInfo(ChessPieceType.Bishop, Team.Black) },
                { new PieceInfo(ChessPieceType.Knight, Team.White),new PieceInfo(ChessPieceType.Pawn, Team.White), null, null, null, null, new PieceInfo(ChessPieceType.Pawn, Team.Black), new PieceInfo(ChessPieceType.Knight, Team.Black) },
                { new PieceInfo(ChessPieceType.Rook, Team.White),new PieceInfo(ChessPieceType.Pawn, Team.White), null, null, null, null, new PieceInfo(ChessPieceType.Pawn, Team.Black), new PieceInfo(ChessPieceType.Rook, Team.Black) },
            };
            
            //Act
            var gameData = notationString.GameDataFromNotationString();
            
            //Assert
            gameData.piecesInfo.Should().BeEquivalentTo(expectedArrangementArray);
        }

        [Test]
        public void WhenGivenFenAndParsedToGameData_AndParseBackToFen_ThanFenTextShouldBeTheSame()
        {
            //Arrange
            var notationString = new FenNotationString("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            var notationString1 = new FenNotationString("rnbqkbnr/pp1ppppp/8/2p5/4P3/5N2/PPPP1PPP/RNBQKB1R b KQkq - 1 2");
            var notationString2 = new FenNotationString("rn2k2r/ppp1bppp/8/8/8/3N4/PPP3PP/RNB2RK1 b kq - 2 13");

            //Act
            var gameData = notationString.GameDataFromNotationString();
            var fenNotationString = gameData.ToFen();
            
            var gameData1 = notationString1.GameDataFromNotationString();
            var fenNotationString1 = gameData1.ToFen();
            
            var gameData2 = notationString2.GameDataFromNotationString();
            var fenNotationString2 = gameData2.ToFen();

            //Assert
            fenNotationString.NotationStringText.Should().Be(notationString.NotationStringText);
            fenNotationString1.NotationStringText.Should().Be(notationString1.NotationStringText);
            fenNotationString2.NotationStringText.Should().Be(notationString2.NotationStringText);
        }
        
    }
}