using Sirenix.OdinInspector;
using UnityEngine;

namespace SteampunkChess.PopUps
{
    public class ForgotPasswordPopUpMediator : MonoBehaviour
    {
        [SerializeField] private ForgotPasswordPopUp _forgotPasswordPopUp;

        [Button, DisableInEditorMode] public void OnClick_Reset() => _forgotPasswordPopUp.SendAccountRecoveryEmail();

  
    }
}