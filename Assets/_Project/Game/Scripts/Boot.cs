using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SteampunkChess
{
    public class Boot : MonoBehaviour
    {
        [SerializeField] private AssetReference _mainMenuScene;

        private void Start()
        {
            _mainMenuScene.LoadSceneAsync();
        }

    }
}