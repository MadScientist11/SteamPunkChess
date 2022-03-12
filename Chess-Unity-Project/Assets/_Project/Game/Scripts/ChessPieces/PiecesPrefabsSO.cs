using UnityEngine;

namespace SteampunkChess
{
    [CreateAssetMenu(fileName = "PiecesPrefabsSO", menuName = "ScriptableObjects/PiecesPrefabsSO")]
    public class PiecesPrefabsSO : ScriptableObject
    {
        public GameObject[] piecesPrefabs;
    }
}