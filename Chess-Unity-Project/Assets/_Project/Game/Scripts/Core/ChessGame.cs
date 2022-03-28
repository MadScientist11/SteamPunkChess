using SteampunkChess.NetworkService;

namespace SteampunkChess
{
    public class ChessGame : IInitializable
    {
        private readonly NotationString _notationString;
        private readonly IBoardFactory _boardFactory;
        private readonly INetworkService _networkService;
        public ChessPlayer ActivePlayer { get; private set; }
        
        public ChessPlayer[] ChessPlayers { get; }

        private ChessPlayer _localPlayer;
        
        public PieceArrangementData InitialPieceArrangementData { get; private set; }

        public bool IsActivePlayer => ActivePlayer == _localPlayer;
        
        
        public Team WhoseTurn { get; private set; }


        public ChessGame(NotationString notationString, IBoardFactory boardFactory, INetworkService networkService)
        {
            _notationString = notationString;
            _boardFactory = boardFactory;
            _networkService = networkService;
            ChessPlayers = new ChessPlayer[2];
        }


        public void Initialize()
        {
            InitialPieceArrangementData = _notationString.GameDataFromNotationString(); 
            ChessBoard chessBoard = _boardFactory.Create();
            chessBoard.Initialize(this);
            
            CreatePlayers(chessBoard);
            ActivePlayer = ChessPlayers[InitialPieceArrangementData.whoseTurn];
            _localPlayer = GetLocalPlayer();
            
            WhoseTurn = (Team) InitialPieceArrangementData.whoseTurn;
        }

        private ChessPlayer GetLocalPlayer()
        {
            int team = _networkService.LocalPlayer.PlayerTeam;
            Logger.DebugError($"Local player is from team {team}");
            return ChessPlayers[team];
        }
        
        public bool CanPerformMove()
        {
            if (IsActivePlayer)
                return true;
            return false;
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