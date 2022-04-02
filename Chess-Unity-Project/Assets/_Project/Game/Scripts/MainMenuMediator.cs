using UnityEngine;

namespace SteampunkChess
{
    public class MainMenuMediator : MonoBehaviour
    {
        [SerializeField] private MainMenu _mainMenu;

        public void OnClick_Play() => _mainMenu.OnPlay();

        public void OnClick_Exit() => Application.Quit();
    }
}