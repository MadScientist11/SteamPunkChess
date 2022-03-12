using UnityEngine;

namespace SteampunkChess
{
    [CreateAssetMenu(fileName = "PiecesPrefabs", menuName = "ScriptableObjects/PiecesPrefabs")]
    public class PiecesPrefabsSO : ScriptableObject
    {
        public GameObject[] piecesPrefabs;
    }
}