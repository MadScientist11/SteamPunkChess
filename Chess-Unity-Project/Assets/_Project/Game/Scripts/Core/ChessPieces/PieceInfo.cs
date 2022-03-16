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
        
        protected bool Equals(PieceInfo other)
        {
            return type == other.type && team == other.team;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PieceInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) type * 397) ^ (int) team;
            }
        }
    }
}