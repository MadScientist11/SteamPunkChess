using UnityEngine;

namespace SteampunkChess
{
    [CreateAssetMenu(fileName = "ChessPieceData", menuName = "ScriptableObjects/ChessPieceData")]
    public class ChessPieceDataSO : ScriptableObject
    {
        public GameObject piecePrefab;
        public GameObject material;
    }
}