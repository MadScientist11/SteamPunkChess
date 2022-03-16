using UnityEngine;

namespace SteampunkChess
{
    [CreateAssetMenu(fileName = "TileSelectionInfo", menuName = "ScriptableObjects/TileSelectionInfo")]
    public class TileSelectionInfoSO : ScriptableObject
    {
        public Material freeSquareMaterial;
        public Material enemySquareMaterial;
        public GameObject selectionPrefab;
    }
}