using Sirenix.OdinInspector;
using UnityEngine;

namespace SteampunkChess
{
    public class RegisterUserMenuMediator : MonoBehaviour
    {
        [SerializeField] private RegisterUserMenu _registerUserMenu;

        [Button, HideInEditorMode] public void OnClick_ContinueAsGuest() => _registerUserMenu.ContinueAsGuest();
    }
}
