using Sirenix.OdinInspector;
using UnityEngine;

namespace SteampunkChess
{
    public class RoomPasswordPopUpMediator : MonoBehaviour
    {
        [SerializeField] private RoomPasswordPopUp _roomPasswordPopUp;

        [Button, DisableInEditorMode] public void OnClick_Join() => _roomPasswordPopUp.JoinRoom();
    }
}