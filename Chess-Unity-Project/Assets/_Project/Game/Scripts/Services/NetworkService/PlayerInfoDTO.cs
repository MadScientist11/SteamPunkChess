using SteampunkChess;

public class PlayerInfoDTO
{
    public PlayerInfoDTO(int playerID, string playerName, int playerScore, Team playerTeam)
    {
        PlayerID = playerID;
        PlayerName = playerName;
        PlayerScore = playerScore;
        PlayerTeam = playerTeam;
    }

    public int PlayerID { get; }
    public string PlayerName { get; }
    public int PlayerScore { get; }

    public Team PlayerTeam { get; }
}