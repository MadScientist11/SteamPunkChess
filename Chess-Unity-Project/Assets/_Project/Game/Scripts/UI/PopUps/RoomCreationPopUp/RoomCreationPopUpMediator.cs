using Sirenix.OdinInspector;
using UnityEngine;

namespace SteampunkChess
{
    public class RoomCreationPopUpMediator : MonoBehaviour
    {
        [SerializeField] private RoomCreationPopUp _roomCreationPopUp;

        [Button, DisableInEditorMode] public void OnClick_CreateRoom() => _roomCreationPopUp.CreateRoom();

        //Rider lie about this
        public void OnValueChanged_TeamToggle(int team) => _roomCreationPopUp.ChangeLocalPlayerTeam(team);

        public void OnValueChanged_TimeDropdown(int timeOption) => _roomCreationPopUp.ChangeRoomTime();
    }
}
