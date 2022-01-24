using Sirenix.OdinInspector;
using UnityEngine;

namespace SteampunkChess.PopUps
{
    public class SignUpPopUpMediator : MonoBehaviour
    {
        [SerializeField] private SignUpPopUp _signUpPopUp;

        [Button, DisableInEditorMode] public void OnClick_SignUp() => _signUpPopUp.SignUp();

        [Button, DisableInEditorMode] public void OnClick_LogIn() => _signUpPopUp.SwitchToLogInPopUp();
    }
}