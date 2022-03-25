using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SteampunkChess
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private AssetReference _lobbyScene;
        
        public void OnPlay()
        {
            _lobbyScene.LoadSceneAsync();
        }
    }

    public class MainMenuMediator : MonoBehaviour
    {
        [SerializeField] private MainMenu _mainMenu;

        public void OnClick_Play() => _mainMenu.OnPlay();
    }
}
