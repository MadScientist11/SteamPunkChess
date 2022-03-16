using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace SteampunkChess
{
    public class TileSelection : IInitializable 
    {
        private readonly List<GameObject> _activeSelections;
        private readonly TileSelectionInfoSO _tileSelectionInfoSO;
        private ObjectPool<GameObject> _pool;
       

        public TileSelection(TileSelectionInfoSO tileSelectionInfoSO)
        {
            _activeSelections = new List<GameObject>();
            _tileSelectionInfoSO = tileSelectionInfoSO;
        }
        
        public void Initialize()
        {
            _pool = new ObjectPool<GameObject>(CreatePoolObject,
                actionOnGet: (obj) => obj.gameObject.SetActive(true), actionOnRelease: (obj) => obj.gameObject.SetActive(false),
                actionOnDestroy: Object.Destroy, false, 10, 27);
        }

        private GameObject CreatePoolObject()
        {
            return Object.Instantiate(_tileSelectionInfoSO.selectionPrefab);
        }

        public void ShowSelection(IEnumerable<(Vector3 tileCenter, bool isAttackMove)> tilesData)
        {
            ClearSelection();
            foreach (var tileData in tilesData)
            {
                GameObject selector = _pool.Get();
                selector.transform.SetPositionAndRotation(tileData.tileCenter, Quaternion.identity);
                _activeSelections.Add(selector);
                selector.GetComponent<MeshRenderer>().material = tileData.isAttackMove ? _tileSelectionInfoSO.enemySquareMaterial : _tileSelectionInfoSO.freeSquareMaterial;
            }
        }

        public void ClearSelection()
        {
            for (int i = 0; i < _activeSelections.Count; i++)
            {
                _pool.Release(_activeSelections[i]);
            }
            _activeSelections.Clear();
        }

    
    }
}