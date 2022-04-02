using SteampunkChess.NetworkService;
using SteampunkChess.PopUpService;
using UnityEngine;
using Zenject;

namespace SteampunkChess
{
    public class ChessGame : IInitializable
    {
        private readonly NotationString _notationString;
        private readonly IBoardFactory _boardFactory;
        private readonly INetworkService _networkService;
        private readonly GameCameraController _gameCameraController;
        private readonly PlayerFactory _playerFactory;
        private readonly TimerFactory _timerFactory;

        public ChessPlayer ActivePlayer { get; private set; }
        
        public ChessPlayer[] ChessPlayers { get; }

        private ChessPlayer _localPlayer;

        private GameTimer _timer;
        
        public PieceArrangementData InitialPieceArrangementData { get; private set; }

        public bool IsActivePlayer => ActivePlayer == _localPlayer;
        
        
        public Team WhoseTurn { get; private set; }


        public ChessGame(NotationString notationString, IBoardFactory boardFactory, INetworkService networkService, GameCameraController gameCameraController
        , PlayerFactory playerFactory, TimerFactory timerFactory)
        {
            _notationString = notationString;
            _boardFactory = boardFactory;
            _networkService = networkService;
            _gameCameraController = gameCameraController;
            _playerFactory = playerFactory;
            _timerFactory = timerFactory;
            ChessPlayers = new ChessPlayer[2];
        }


        public void Initialize()
        {
            InitialPieceArrangementData = _notationString.GameDataFromNotationString(); 
            
            ChessBoard chessBoard = _boardFactory.Create();
            chessBoard.Initialize(this);
            
            CreatePlayers(chessBoard);
            
            _timer = _timerFactory.Create();
            _timer.InitializeTimer(ChessPlayers[0],ChessPlayers[1], GameOver);
            
            
            WhoseTurn = (Team) InitialPieceArrangementData.whoseTurn;
            ActivePlayer = ChessPlayers[(int) WhoseTurn];
            _localPlayer = GetLocalPlayer();
           
            _gameCameraController.Initialize(_localPlayer.Team);
            
            _timer.Start();

            
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
            _timer.SwitchPlayer();
        }
        
        public bool IsTeamTurnActive(Team team)
        {
            return ActivePlayer.Team == team;
        }

        private void CreatePlayers(ChessBoard chessBoard)
        {
            for (int i = 0; i < 2; i++)
            {
                ChessPlayers[i] = _playerFactory.Create(chessBoard, (Team) i, InitialPieceArrangementData, 
                    _networkService.LocalPlayer.MatchTimeLimitInSeconds);
                ChessPlayers[i].Initialize();
            }
            
        }

        public void GameOver()
        {
            _timer.Stop();
            Debug.LogError("Game over");
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

    public class TimerFactory
    {
        private readonly IInstantiator _instantiator;

        public TimerFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }
        public GameTimer Create()
        {
            var playerTimer = _instantiator.Instantiate<GameTimer>();
            return playerTimer;
        }
    }

    public class PlayerFactory
    {
        public ChessPlayer Create(ChessBoard chessBoard, Team team, PieceArrangementData data, float matchTIme)
        {
            ChessPlayer player = new ChessPlayer(team, chessBoard, matchTIme)
            {
                CanRightSideCastle = team == Team.White ? data.canWhiteCastleKingSide : data.canBlackCastleKingSide,
                CanLeftSideCastle = team == Team.White ? data.canWhiteCastleQueenSide : data.canBlackCastleQueenSide
            };
            return player;
        }
    }
}