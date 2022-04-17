using UnityEngine;

namespace SteampunkChess
{
    [CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData")]
    public class GameDataSO : ScriptableObject
    {
        public string notationString;
        public GameType gameType;
        public ChessBoardInfoSO chessBoardInfoSO;
        public PiecesPrefabsSO piecesPrefabsSO;
        public TileSelectionInfoSO tileSelectionSO;
    }

    public enum GameType
    {
        Multiplayer,
        AI
    }
}