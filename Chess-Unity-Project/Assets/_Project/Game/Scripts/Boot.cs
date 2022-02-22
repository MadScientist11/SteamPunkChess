using UnityEngine;
using UnityEngine.AddressableAssets;


namespace SteampunkChess
{
    public class Boot : MonoBehaviour
    {
        [SerializeField] private AssetReference _mainMenuScene;

        private void Start()
        {
            if (GameCommandLineArgs.Contains(GameConstants.GameCLIArgs.SkipUserValidation))
                _mainMenuScene.LoadSceneAsync();
        }
    }
}