using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using SteampunkChess.PopUps;
using UnityEngine;

namespace SteampunkChess
{
    public class WinOrLosePopUpMediator : MonoBehaviour
    {
        [SerializeField] private WinOrLosePopUp _winOrLosePopUp;
        
        [Button, DisableInEditorMode] public void OnClick_Exit() => _winOrLosePopUp.ToLobby();
    }
}
