using UnityEngine;

namespace SteampunkChess
{
    [CreateAssetMenu(fileName = "ChessBoardInfo", menuName = "ScriptableObjects/ChessBoardInfo")]
    public class ChessBoardInfoSO : ScriptableObject
    {
        public GameObject boardPrefab;
        public int boardSizeX = 8;
        public int boardSizeY = 8;
        public TileInfoSO tileInfoSO;
    }
}