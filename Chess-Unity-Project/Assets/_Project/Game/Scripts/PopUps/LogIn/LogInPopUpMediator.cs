using Sirenix.OdinInspector;
using SteampunkChess.PopUps;
using UnityEngine;
using UnityEngine.Serialization;

public class LogInPopUpMediator : MonoBehaviour
{
    [FormerlySerializedAs("_signUpPopUp")] [SerializeField] private LogInPopUp _logInPopUp;

    [Button, DisableInEditorMode] public void OnClick_LogIn() => _logInPopUp.LogIn();

    [Button, DisableInEditorMode] public void OnClick_SignUp() => _logInPopUp.SwitchToLogInPopUp();

    [Button, DisableInEditorMode] public void OnClick_ForgotPassword() => _logInPopUp.SwitchToForgotPasswordPopUp();
}
