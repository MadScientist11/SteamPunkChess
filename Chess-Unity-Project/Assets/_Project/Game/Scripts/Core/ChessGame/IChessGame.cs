namespace SteampunkChess
{
    public interface IChessGame
    {
        ChessPlayer ActivePlayer { get; }
        
        ChessPlayer[] ChessPlayers { get; }

        bool CanPerformMove();

        bool IsTeamTurnActive(Team team);

        void EndOfGame(Team winTeam);

        void ChangeActiveTeam();
    }
}