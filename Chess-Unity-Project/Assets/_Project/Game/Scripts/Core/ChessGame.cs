namespace SteampunkChess
{
    public class ChessGame : IInitializable
    {
        private readonly NotationString _notationString;
        private readonly IBoardFactory _boardFactory;
        public ChessPlayer ActivePlayer { get; private set; }
        
        public ChessPlayer[] ChessPlayers { get; }
        
        public PieceArrangementData InitialPieceArrangementData { get; private set; }
        
        
        public Team WhoseTurn { get; private set; }


        public ChessGame(NotationString notationString, IBoardFactory boardFactory)
        {
            _notationString = notationString;
            _boardFactory = boardFactory;
            ChessPlayers = new ChessPlayer[2];
        }


        public void Initialize()
        {
            InitialPieceArrangementData = _notationString.GameDataFromNotationString(); 
            ChessBoard chessBoard = _boardFactory.Create();
            chessBoard.Initialize(this);
            
            CreatePlayers(chessBoard);
            ActivePlayer = ChessPlayers[InitialPieceArrangementData.whoseTurn];
            
            WhoseTurn = (Team) InitialPieceArrangementData.whoseTurn;
        }

        public void ChangeActiveTeam()
        {
            WhoseTurn = WhoseTurn == Team.White ? Team.Black : Team.White;
            ActivePlayer = ChessPlayers[(int) WhoseTurn];
        }
        
        public bool IsTeamTurnActive(Team team)
        {
            return ActivePlayer.Team == team;
        }

        private void CreatePlayers(ChessBoard chessBoard)
        {
            ChessPlayers[0] = new ChessPlayer(Team.White, chessBoard);
            ChessPlayers[0].CanRightSideCastle = InitialPieceArrangementData.canWhiteCastleKingSide;
            ChessPlayers[0].CanLeftSideCastle = InitialPieceArrangementData.canWhiteCastleQueenSide;
            
            ChessPlayers[1] = new ChessPlayer(Team.Black, chessBoard);
            ChessPlayers[1].CanRightSideCastle = InitialPieceArrangementData.canBlackCastleKingSide;
            ChessPlayers[1].CanLeftSideCastle = InitialPieceArrangementData.canBlackCastleQueenSide;
            
            for(int i = 0; i < 2; i++)
                ChessPlayers[i].Initialize();
        }

        public PieceArrangementData AssembleCurrentGameData()
        {
            PieceArrangementData pieceArrangementData = new PieceArrangementData();
            pieceArrangementData.whoseTurn = (int)WhoseTurn;
            pieceArrangementData.canBlackCastleKingSide = ChessPlayers[(int) Team.Black].CanRightSideCastle;
            pieceArrangementData.canBlackCastleQueenSide = ChessPlayers[(int) Team.Black].CanLeftSideCastle;
            pieceArrangementData.canWhiteCastleKingSide = ChessPlayers[(int) Team.White].CanRightSideCastle;
            pieceArrangementData.canWhiteCastleQueenSide = ChessPlayers[(int) Team.White].CanLeftSideCastle;
            return pieceArrangementData;
        }
    }
}