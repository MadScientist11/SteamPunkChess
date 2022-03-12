using UnityEngine;

namespace SteampunkChess
{
    [CreateAssetMenu(fileName = "TileInfo", menuName = "ScriptableObjects/TileInfo")]
    public class TileInfoSO : ScriptableObject
    {
        public GameObject tilePrefab;
        public float tileDimensionSize = 0.115f;
        public float yOffset = 0.01f;
        public float zOffset = 0.02f;
        public Vector3 startingSpawnPoint;
    }
}