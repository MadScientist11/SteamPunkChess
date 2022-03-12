using SteampunkChess;

namespace SteamPunkChess
{
    [System.Serializable]
    public class PieceInfo
    {
        public ChessPieceType type;
        public Team team;
        public PieceInfo(ChessPieceType type, Team team)
        {
            this.type = type;
            this.team = team;
        }
    }
}