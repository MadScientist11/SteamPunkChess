using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace SteampunkChess
{
    public class PhotonPrefabPool : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _prefabs;
 
        public void InitializePrefabPool()
        {
            DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
            if (pool != null && _prefabs != null)
            {
                foreach (GameObject prefab in _prefabs)
                {
                    pool.ResourceCache.Add(prefab.name, prefab);
                }
            }
        }
    }
}
