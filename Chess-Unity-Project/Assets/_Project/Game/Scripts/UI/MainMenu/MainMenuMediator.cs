using Sirenix.OdinInspector;
using UnityEngine;

namespace SteampunkChess
{
    public class MainMenuMediator : MonoBehaviour
    {
        [SerializeField] private MainMenu _mainMenu;

        [DisableInEditorMode, Button] public void OnClick_Play() => _mainMenu.SwitchToLobby();
        [DisableInEditorMode, Button] public void OnClick_Settings() => _mainMenu.SwitchToSettings();
        [DisableInEditorMode, Button] public void OnClick_Exit() => Application.Quit();
    }
}