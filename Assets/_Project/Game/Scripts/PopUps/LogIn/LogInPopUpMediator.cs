using Sirenix.OdinInspector;
using SteampunkChess.PopUps;
using UnityEngine;

public class LogInPopUpMediator : MonoBehaviour
{
    [SerializeField] private LogInPopUp _signUpPopUp;

    [Button, DisableInEditorMode] public void OnClick_LogIn() => _signUpPopUp.LogIn();

    [Button, DisableInEditorMode] public void OnClick_SignUp() => _signUpPopUp.SwitchToLogInPopUp();

    [Button, DisableInEditorMode] public void OnClick_ForgotPassword() => _signUpPopUp.SwitchToForgotPasswordPopUp();
}
