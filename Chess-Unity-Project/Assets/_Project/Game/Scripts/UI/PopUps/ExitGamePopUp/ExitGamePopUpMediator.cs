using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SteampunkChess
{
    public class ExitGamePopUpMediator : MonoBehaviour
    {
        [SerializeField] private ExitGamePopUp _exitGamePopUp;

        [Button, DisableInEditorMode] public void OnClick_Yes() => _exitGamePopUp.ExitGame();
        
        [Button, DisableInEditorMode] public void OnClick_No() => _exitGamePopUp.StayInGame();
        
    }
}
