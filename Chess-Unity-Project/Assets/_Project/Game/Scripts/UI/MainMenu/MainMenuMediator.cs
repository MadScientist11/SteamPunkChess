using UnityEngine;

namespace SteampunkChess
{
    public class MainMenuMediator : MonoBehaviour
    {
        [SerializeField] private MainMenu _mainMenu;

        public void OnClick_Play() => _mainMenu.SwitchToLobby();
        
        public void OnClick_Settings() => _mainMenu.SwitchToSettings();

        public void OnClick_Exit() => Application.Quit();
    }
}