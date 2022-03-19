namespace SteampunkChess
{
    public class ChessGame : IInitializable
    {
        private readonly NotationString _notationString;
        private readonly IBoardFactory _boardFactory;
        public ChessPlayer CurrentPlayer { get; private set; }
        public PieceArrangementData InitialPieceArrangementData { get; private set; }

        private ChessPlayer[] _chessPlayers;

        public Team WhoseTurn { get; private set; }


        public ChessGame(NotationString notationString, IBoardFactory boardFactory)
        {
            _notationString = notationString;
            _boardFactory = boardFactory;
            _chessPlayers = new ChessPlayer[2];
        }


        public void Initialize()
        {
            InitialPieceArrangementData = _notationString.GameDataFromNotationString(); 
            ChessBoard chessBoard = _boardFactory.Create();
            chessBoard.Initialize(this);
            
            CreatePlayers(chessBoard);
            CurrentPlayer = _chessPlayers[InitialPieceArrangementData.whoseTurn];
            WhoseTurn = (Team) InitialPieceArrangementData.whoseTurn;
        }

        public void ChangeActiveTeam()
        {
            WhoseTurn = WhoseTurn == Team.White ? Team.Black : Team.White;
            CurrentPlayer = _chessPlayers[(int) WhoseTurn];
        }

        private void CreatePlayers(ChessBoard chessBoard)
        {
            _chessPlayers[0] = new ChessPlayer(Team.White, chessBoard);
            _chessPlayers[0].CanRightSideCastle = InitialPieceArrangementData.canWhiteCastleKingSide;
            _chessPlayers[0].CanLeftSideCastle = InitialPieceArrangementData.canWhiteCastleQueenSide;
            
            _chessPlayers[1] = new ChessPlayer(Team.Black, chessBoard);
            _chessPlayers[1].CanRightSideCastle = InitialPieceArrangementData.canBlackCastleKingSide;
            _chessPlayers[1].CanLeftSideCastle = InitialPieceArrangementData.canBlackCastleQueenSide;
        }

        public PieceArrangementData AssembleCurrentGameData()
        {
            PieceArrangementData pieceArrangementData = new PieceArrangementData();
            pieceArrangementData.whoseTurn = (int)WhoseTurn;
            pieceArrangementData.canBlackCastleKingSide = _chessPlayers[(int) Team.Black].CanRightSideCastle;
            pieceArrangementData.canBlackCastleQueenSide = _chessPlayers[(int) Team.Black].CanLeftSideCastle;
            pieceArrangementData.canWhiteCastleKingSide = _chessPlayers[(int) Team.White].CanRightSideCastle;
            pieceArrangementData.canWhiteCastleQueenSide = _chessPlayers[(int) Team.White].CanLeftSideCastle;
            return pieceArrangementData;
        }
    }
}