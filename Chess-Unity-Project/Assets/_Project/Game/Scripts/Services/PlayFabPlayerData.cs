using System;

namespace SteampunkChess
{
    public class PlayFabPlayerData
    {
        public event Action<PlayFabPlayerData> OnPlayerDataChanged;

        public string PlayerName
        {
            get => _playerName;
            set
            {
                _playerName = value; 
                Logger.DebugError($"Player name {_playerName}");
                OnPlayerDataChanged?.Invoke(this);
            }
        }

        public int PlayerScore
        {
            get => _playerScore;
            set
            {
                _playerScore = value;
                OnPlayerDataChanged?.Invoke(this);
            }
        }

        private int _playerScore;
        private string _playerName;
    }
    
    public static partial class GameConstants
    {
        public static class PlayerDataKeys
        {
            public const string PlayerNameKey = "PlayerName";
            public const string PlayerScoreKey = "Score";
        }
    }
    
    public static partial class GameConstants
    {
        public static class CustomProperties
        {
            public const string Team = "Te";
            public const string Score = "S";
            public const string MatchTime = "Ti";
            public const string RoomPassword = "P";
        }
    }
}