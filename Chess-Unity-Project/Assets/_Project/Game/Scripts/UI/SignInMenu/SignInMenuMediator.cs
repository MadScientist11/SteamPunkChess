using Sirenix.OdinInspector;
using UnityEngine;

namespace SteampunkChess
{
    public class SignInMenuMediator : MonoBehaviour
    {
        [SerializeField] private SignInMenu signInMenu;

        [Button, HideInEditorMode] public void OnClick_ContinueAsGuest() => signInMenu.ContinueAsGuest();
    }
}
