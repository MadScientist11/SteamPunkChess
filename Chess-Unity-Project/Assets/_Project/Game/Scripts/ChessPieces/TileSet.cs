using UnityEngine;

namespace SteampunkChess
{
    public class TileSet
    {
        private const string TilesParent = "TilesParent";
        private readonly ChessBoardInfoSO _chessBoardInfoSO;
        private readonly TileInfoSO _tileInfoSO;
        private readonly GameObject[,] _tiles;
        private readonly Transform _tilesParentTransform;

        public TileSet(ChessBoardInfoSO chessBoardInfoSO)
        {
            _chessBoardInfoSO = chessBoardInfoSO;
            _tileInfoSO = chessBoardInfoSO.tileInfoSO;
            _tiles = new GameObject[chessBoardInfoSO.boardSizeX, chessBoardInfoSO.boardSizeY];
            _tilesParentTransform = new GameObject(TilesParent).transform;
            CreateTilesGO();
        }
        
        public GameObject this[int x, int y]
        {
            get => _tiles[x, y];
            set => _tiles[x, y] = value;
        }
        
        private void CreateTilesGO()
        {
            for (int x = 0; x < _chessBoardInfoSO.boardSizeX; x++)
            for (int y = 0; y < _chessBoardInfoSO.boardSizeY; y++)
                this[x, y] = CreateTile(x, y);
        }

        private GameObject CreateTile(int x, int y)
        {
            float tileSize = _tileInfoSO.tileDimensionSize * 2;
            GameObject tile = Object.Instantiate(
                _tileInfoSO.tilePrefab, 
                _tileInfoSO.startingSpawnPoint + new Vector3((x * tileSize) + (_tileInfoSO.zOffset * x), _tileInfoSO.yOffset, (y * tileSize) + (_tileInfoSO.zOffset * y)),
                Quaternion.identity, 
                _tilesParentTransform);

            return tile;
        }
        public Vector2Int LookupTileIndex(GameObject gameObject)
        {
            for (int x = 0; x < _chessBoardInfoSO.boardSizeX; x++)
            for (int y = 0; y < _chessBoardInfoSO.boardSizeY; y++)
                if (this[x, y] == gameObject)
                    return new Vector2Int(x, y);

            return -Vector2Int.one;
        }

        public Vector3 GetTileCenter(int x, int y)
        {
            return _tileInfoSO.startingSpawnPoint + new Vector3(x * (_tileInfoSO.tileDimensionSize * 2) + _tileInfoSO.zOffset * x,
                _tileInfoSO.yOffset, y * (_tileInfoSO.tileDimensionSize * 2) + _tileInfoSO.zOffset * y) + new Vector3(_tileInfoSO.tileDimensionSize, 0, _tileInfoSO.tileDimensionSize);
        }
        
    }
}