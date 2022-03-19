using UnityEngine;

namespace SteampunkChess
{
    [CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData")]
    public class GameDataSO : ScriptableObject
    {
        public string notationString;
        public ChessBoardInfoSO chessBoardInfoSO;
        public PiecesPrefabsSO piecesPrefabsSO;
        public TileSelectionInfoSO tileSelectionSO;
    }
}